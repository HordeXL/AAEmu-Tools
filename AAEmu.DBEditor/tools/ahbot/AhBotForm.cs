using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AAEmu.DBEditor.data;
using AAEmu.DBEditor.data.enums;
using AAEmu.DBEditor.data.gamedb;
using AAEmu.DBEditor.forms.server;
using AAEmu.DBEditor.Models.aaemu.webapi;
using AAEmu.DBEditor.utils;
using Newtonsoft.Json;

namespace AAEmu.DBEditor.tools.ahbot
{
    public partial class AhBotForm : Form
    {
        private const string AhBotSettingsFileName = "ahbot.json";
        private const string ApiAuctionList = "/api/auction/list";
        private const string ApiAuctionGenerate = "/api/auction/generate";
        private const string ApiMailList = "/api/mail/list";
        private const string ApiMailDelete = "/api/mail/delete";
        private const int WebApiDefaultPort = 1280;

        private static AhBotForm _instance;
        public static AhBotForm Instance => _instance ??= new AhBotForm();

        private Dictionary<long, TreeNode> ItemNodes { get; set; } = [];
        private AhBotSettings Settings { get; set; } = new();
        private AhBotListingSetting ListingSettings { get; set; } = new();
        private AhBotEntry SelectedAhBotItemEntry { get; set; }
        private Item SelectedItem { get; set; }
        private List<JsonAuctionLot> ServerAhListingCache { get; set; } = [];
        private bool CloseWhenDone { get; set; }
        private bool SkipSaving { get; set; }

        public AhBotForm()
        {
            InitializeComponent();
        }

        private void Log(string s)
        {
            Invoke(() => { tLog.Text = $@"[{DateTime.Now:HH:mm:ss}] {s}{Environment.NewLine}" + tLog.Text; });
        }

        private void btnPickAhCharacter_Click(object sender, EventArgs e)
        {
            using var characterForm = new CharacterForm();
            characterForm.btnSelectionOK.Visible = true;
            if (characterForm.ShowDialog() == DialogResult.OK)
            {
                var botCharacter = characterForm.GetSelectCharacter();
                if ((botCharacter != null) && !string.IsNullOrWhiteSpace(botCharacter.Name))
                {
                    lAhBotName.Text = botCharacter.Name;

                    var botAccount = Data.MySqlDb.Login.Users.FirstOrDefault(x => x.Id == botCharacter.AccountId);
                    if ((botAccount != null) && (botAccount.Id == botCharacter.AccountId))
                    {
                        lAhBotAccount.Text = botAccount.Username;

                        Settings.CharacterName = botCharacter.Name;
                        Settings.AccountName = botAccount.Username;
                        SaveSettings();
                        return;
                    }
                }

                lAhBotName.Text = @"<错误>";
                lAhBotAccount.Text = @"<错误>";
            }
        }

        private void AhBotForm_Load(object sender, EventArgs e)
        {
            MainForm.Self.AddOwnedForm(this);

            SkipSaving = true;
            try
            {
                Log("获取服务器列表");
                // Populate Servers
                cbServers.Items.Clear();
                var serverList = Data.MySqlDb.Login.GameServers.Where(s => s.Hidden == false);
                foreach (var server in serverList)
                {
                    cbServers.Items.Add(server.Name);
                }

                Log("生成AH类别");
                // Populate Items List
                var cats = new Dictionary<long, TreeNodeForAhCategory>();

                tvAhList.Nodes.Clear();
                ItemNodes.Clear();
                var itemsWithAhCats = Data.Server.CompactSqlite.Items.Where(x => x.AuctionACategoryId > 0).OrderBy(x => x.AuctionACategoryId).ToList();

                foreach (var item in itemsWithAhCats)
                {
                    if (item.BindId == 2) // Bind on Pickup item can technically not be sold on AH
                        continue;

                    var targetKeyA = TreeNodeForAhCategory.MakeKey(item.AuctionACategoryId, 0, 0);
                    var targetKeyB = TreeNodeForAhCategory.MakeKey(item.AuctionACategoryId, item.AuctionBCategoryId, 0);
                    var targetKeyC = TreeNodeForAhCategory.MakeKey(item.AuctionACategoryId, item.AuctionBCategoryId, item.AuctionCCategoryId);

                    // TODO: Find where the category names are stored in later versions
                    // Make sure the categories exist
                    // Cat A
                    if (!cats.TryGetValue(targetKeyA, out var catANode))
                    {
                        catANode = new TreeNodeForAhCategory();
                        catANode.CategoryA = item.AuctionACategoryId ?? 0;
                        catANode.CategoryB = 0;//item.AuctionBCategoryId ?? 0;
                        catANode.CategoryC = 0;//item.AuctionCCategoryId ?? 0;
                        catANode.Text = $@"A{catANode.CategoryA}";
                        var uiKey = catANode.GetUiTextKey();
                        if (!string.IsNullOrWhiteSpace(uiKey))
                        {
                            catANode.Text = Data.Server.GetUiText(107, uiKey, catANode.Text, true);
                        }
                        tvAhList.Nodes.Add(catANode);
                        cats.Add(targetKeyA, catANode);
                    }

                    // Cat B
                    if (!cats.TryGetValue(targetKeyB, out var catBNode))
                    {
                        catBNode = new TreeNodeForAhCategory();
                        catBNode.CategoryA = item.AuctionACategoryId ?? 0;
                        catBNode.CategoryB = item.AuctionBCategoryId ?? 0;
                        catBNode.CategoryC = 0;//item.AuctionCCategoryId ?? 0;
                        catBNode.Text = $@"B{catBNode.CategoryB}";
                        var uiKey = catBNode.GetUiTextKey();
                        if (!string.IsNullOrWhiteSpace(uiKey))
                        {
                            catBNode.Text = Data.Server.GetUiText(107, uiKey, catBNode.Text, true);
                        }
                        catANode.Nodes.Add(catBNode);
                        cats.Add(targetKeyB, catBNode);
                    }

                    // Cat C
                    if (!cats.TryGetValue(targetKeyC, out var catCNode))
                    {
                        catCNode = new TreeNodeForAhCategory();
                        catCNode.CategoryA = item.AuctionACategoryId ?? 0;
                        catCNode.CategoryB = item.AuctionBCategoryId ?? 0;
                        catCNode.CategoryC = item.AuctionCCategoryId ?? 0;
                        catCNode.Text = $@"C{catCNode.CategoryC}";
                        var uiKey = catCNode.GetUiTextKey();
                        if (!string.IsNullOrWhiteSpace(uiKey))
                        {
                            catCNode.Text = Data.Server.GetUiText(107, uiKey, catCNode.Text, true);
                        }
                        catBNode.Nodes.Add(catCNode);
                        cats.Add(targetKeyC, catCNode);
                    }

                    /*
                    // Get actual cat (it should be the same as catCNode above)
                    if (!cats.TryGetValue(targetKeyC, out var thisCat))
                    {
                        // Failed to create categories?
                        continue;
                    }

                    // Add the item
                    // Localized name
                    var itemName = Data.Server.LocalizedText.GetValueOrDefault(("items", "name", item.Id)) ?? item.Name;
                    // Not translated?
                    if (string.IsNullOrWhiteSpace(itemName) || itemName.Contains("DO NOT TRANSLATE"))
                        itemName = item.Name;
                    // No name?
                    if (string.IsNullOrWhiteSpace(itemName))
                        itemName = $"<item {item.Id}>";

                    var itemNode = thisCat.Nodes.Add(itemName);
                    itemNode.Tag = (item.Id ?? 0);
                    itemNode.ForeColor = SystemColors.GrayText;
                    ItemNodes.Add(item.Id ?? 0, itemNode);
                    */
                }

                foreach (var item in itemsWithAhCats)
                {
                    if (item.BindId == 2) // Bind on Pickup item can technically not be sold on AH
                        continue;

                    var targetKeyC = TreeNodeForAhCategory.MakeKey(item.AuctionACategoryId, item.AuctionBCategoryId, item.AuctionCCategoryId);

                    // Get actual cat (it should be the same as catCNode above)
                    if (!cats.TryGetValue(targetKeyC, out var thisCat))
                    {
                        // Failed to create categories?
                        continue;
                    }

                    // Add the item
                    // Localized name
                    var itemName = Data.Server.LocalizedText.GetValueOrDefault(("items", "name", item.Id)) ?? item.Name;
                    // Not translated?
                    if (string.IsNullOrWhiteSpace(itemName) || itemName.Contains("DO NOT TRANSLATE"))
                        itemName = item.Name;
                    // No name?
                    if (string.IsNullOrWhiteSpace(itemName))
                        itemName = $"<item {item.Id}>";

                    var itemNode = thisCat.Nodes.Add(itemName);
                    itemNode.Tag = (item.Id ?? 0);
                    itemNode.ForeColor = SystemColors.GrayText;
                    ItemNodes.Add(item.Id ?? 0, itemNode);
                }

                Log("加载强化等级");
                // Populate Grades
                cbGrade.Items.Clear();
                foreach (var itemGrade in Data.Server.CompactSqlite.ItemGrades.OrderBy(g => g.Id))
                {
                    var gradeId = itemGrade.Id ?? 0;
                    var gradeName = Data.Server.LocalizedText.GetValueOrDefault(("item_grades", "name", gradeId)) ??
                                    $"<grade {gradeId}>";
                    cbGrade.Items.Add((gradeName, gradeId));
                }

                if (cbGrade.Items.Count > 0)
                    cbGrade.SelectedIndex = cbGrade.Items.Count - 1;

                /*
                Log("Populating AH Listing");
                // Populate Items
                foreach (var item in Data.Server.CompactSqlite.Items.Where(x =>
                             (x.Price > 1) && (x.Refund > 1) && (x.AuctionACategoryId > 0)))
                {
                    if (item.BindId == 2) // Bind on Pickup item can technically not be sold on AH
                        continue;
                    // Localized name
                    var itemName = Data.Server.LocalizedText.GetValueOrDefault(("items", "name", item.Id)) ?? "";
                    // Not translated?
                    if (string.IsNullOrWhiteSpace(itemName) || itemName.Contains("DO NOT TRANSLATE"))
                        itemName = item.Name;
                    // No name?
                    if (string.IsNullOrWhiteSpace(itemName))
                        itemName = $"<item {item.Id}>";
                    if (catC.TryGetValue(item.AuctionCCategoryId ?? 0, out var catCNode))
                    {
                        var itemNode = catCNode.Nodes.Add(itemName);
                        itemNode.Tag = (item.Id ?? 0);
                        itemNode.ForeColor = SystemColors.GrayText;
                        ItemNodes.Add(item.Id ?? 0, itemNode);
                    }
                    else if (catB.TryGetValue(item.AuctionBCategoryId ?? 0, out var catBNode))
                    {
                        var itemNode = catBNode.Nodes.Add(itemName);
                        itemNode.Tag = (item.Id ?? 0);
                        itemNode.ForeColor = SystemColors.GrayText;
                        ItemNodes.Add(item.Id ?? 0, itemNode);
                    }
                    else if (catA.TryGetValue(item.AuctionACategoryId ?? 0, out var catANode))
                    {
                        var itemNode = catANode.Nodes.Add(itemName);
                        itemNode.Tag = (item.Id ?? 0);
                        itemNode.ForeColor = SystemColors.GrayText;
                        ItemNodes.Add(item.Id ?? 0, itemNode);
                    }
                }
                */
                LoadSettings();
                UpdateFromSettings();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            SkipSaving = false;
        }

        private void AhBotForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        private static bool TryDeserializeObject<T>(string json, out T result, out Exception error)
        {
            result = default;

            if (string.IsNullOrWhiteSpace(json))
            {
                error = new ArgumentException(@"NullOrWhiteSpace", nameof(json));
                return false;
            }

            try
            {
                result = JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                result = default;
                error = e;
                return false;
            }

            error = null;
            return result != null;
        }

        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                var settingsFile = Path.Combine(Application.StartupPath, AhBotSettingsFileName);
                if (File.Exists(settingsFile))
                {
                    Log("正在加载拍卖机器人配置");
                    var settingsString = File.ReadAllText(settingsFile);
                    if (!TryDeserializeObject<AhBotSettings>(settingsString, out var newSettings, out var ex1))
                    {
                        MessageBox.Show(ex1.Message);
                        return;
                    }

                    Settings = newSettings;


                    if (!string.IsNullOrWhiteSpace(Settings.AccountName) &&
                        Data.MySqlDb.Login.Users.Any(x => x.Username == Settings.AccountName))
                    {
                        lAhBotAccount.Text = Settings.AccountName;
                    }
                    else
                    {
                        Settings.AccountName = string.Empty;
                        lAhBotAccount.Text = @"<未选择任何账户>";
                    }

                    if (!string.IsNullOrWhiteSpace(Settings.CharacterName) &&
                        Data.MySqlDb.Game.Characters.Any(x => x.Name == Settings.CharacterName))
                    {
                        lAhBotName.Text = Settings.CharacterName;
                    }
                    else
                    {
                        Settings.CharacterName = string.Empty;
                        lAhBotName.Text = @"<未选择任何角色>";
                    }

                    if (!string.IsNullOrWhiteSpace(Settings.ServerName) &&
                        Data.MySqlDb.Login.GameServers.Any(x => x.Name == Settings.ServerName))
                    {
                        cbServers.SelectedIndex = cbServers.Items.IndexOf(Settings.ServerName);
                    }
                    else
                    {
                        Settings.ServerName = string.Empty;
                        cbServers.SelectedIndex = -1;
                    }
                }
                else
                {
                    Log($"未找到 AHBot 的配置文件: {settingsFile}");
                }

                var settingsListingFile = Path.Combine(Application.StartupPath, Settings.ListingsFile);
                if (File.Exists(settingsListingFile))
                {
                    Log($"正在加载 AH 列表配置: {settingsListingFile}");
                    var settingsListingString = File.ReadAllText(settingsListingFile);
                    if (!TryDeserializeObject<AhBotListingSetting>(settingsListingString, out var resListings,
                            out var ex2))
                    {
                        MessageBox.Show(ex2.Message);
                        return;
                    }

                    ListingSettings = resListings;
                }
                else
                {
                    Log($"未找到定义的列表文件: {settingsListingFile}");
                }

                UpdateFromSettings();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            if (SkipSaving)
                return;
            var res = JsonConvert.SerializeObject(Settings);
            var settingsFile = Path.Combine(Application.StartupPath, AhBotSettingsFileName);
            try
            {
                File.WriteAllText(settingsFile, res);
                Log($"设置已保存至: {settingsFile}");
            }
            catch (Exception ex)
            {
                Log($"保存失败: {settingsFile}");
                MessageBox.Show(ex.Message);
            }

            var resListing = JsonConvert.SerializeObject(ListingSettings);
            var settingsListFile = Path.Combine(Application.StartupPath, Settings.ListingsFile);
            try
            {
                File.WriteAllText(settingsListFile, resListing);
                Log($"列表已保存至: {settingsListFile}");
            }
            catch (Exception ex)
            {
                Log($"保存失败: {settingsListFile}");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Updates the colors of the nodes in the AH TreeView
        /// </summary>
        private void UpdateFromSettings()
        {
            // Gray out everything first
            foreach (var (_, itemNode) in ItemNodes)
            {
                itemNode.ForeColor = Color.LightGray;
            }

            foreach (var auctionLot in ServerAhListingCache)
            {
                if (!ItemNodes.TryGetValue(auctionLot.Item.TemplateId, out var itemNode))
                    continue;
                itemNode.ForeColor = Color.DarkBlue;
            }

            foreach (var settingsItem in ListingSettings.Items)
            {
                if (!ItemNodes.TryGetValue(settingsItem.ItemId, out var itemNode))
                    continue;
                itemNode.ForeColor = Color.Green;
            }
        }

        private void cbGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectGrade = Data.Server.CompactSqlite.ItemGrades.FirstOrDefault(x => x.Id == cbGrade.SelectedIndex);
            if (selectGrade != null)
            {
                lGrade.ForeColor = int.TryParse(selectGrade.ColorArgb, NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture, out var hexColor)
                    ? Color.FromArgb(hexColor)
                    : SystemColors.GrayText;
            }
            else
            {
                lGrade.ForeColor = SystemColors.ControlText;
            }
            UpdateItemIcon();
        }

        private void tvAhList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectedAhBotItemEntry = null;
            SelectedItem = null;
            lbAhList.Items.Clear();
            if (e.Node?.Tag is long itemId)
            {
                btnUpdateAhItem.Enabled = false;
                btnRemoveItem.Enabled = false;
                var item = Data.Server.CompactSqlite.Items.FirstOrDefault(x => x.Id == itemId);
                if (item != null)
                {
                    SelectedItem = item;
                    lItemId.Text = item.Id.ToString();
                    lItemName.Text = Data.Server.LocalizedText.GetValueOrDefault(("items", "name", itemId)) ??
                                     "<错误>";
                    if (item.FixedGrade >= 0)
                    {
                        cbGrade.SelectedIndex = (int)(item.FixedGrade ?? 0);
                        cbGrade.Enabled = false;
                    }
                    else
                    {
                        cbGrade.SelectedIndex = 0;
                        cbGrade.Enabled = true;
                    }

                    tSaleQuantity.Text = item.MaxStackSize.ToString();
                    tListedCount.Text = @"1";
                    tBuyOutPrice.Text = (item.Price * item.MaxStackSize).ToString();
                    tStartBid.Text = (item.Refund * item.MaxStackSize).ToString();
                    lStackMax.Text = $@"/ {item.MaxStackSize}";
                    tComment.Text = string.Empty;
                    btnUpdateAhItem.Enabled = true;

                    var ahBotItems = ListingSettings.Items.Where(x => x.ItemId == itemId).ToList();
                    if (ahBotItems.Any())
                    {
                        lbAhList.Items.Clear();
                        foreach (var ahBotItem in ahBotItems)
                        {
                            lbAhList.Items.Add(ahBotItem);
                        }
                    }

                    lbAhLiveList.Items.Clear();
                    var liveItems = ServerAhListingCache.Where(x => x.Item.TemplateId == item.Id).ToList();
                    if (liveItems.Any())
                    {
                        foreach (var jsonAuctionLot in liveItems)
                        {
                            lbAhLiveList.Items.Add(jsonAuctionLot);
                        }

                    }
                }
            }
        }

        private void btnUpdateAhItem_Click(object sender, EventArgs e)
        {
            /*
            if (SelectedAhBotItemEntry == null)
            {
                MessageBox.Show("No item selected");
                return;
            }
            */

            if (SelectedItem == null)
            {
                MessageBox.Show(@"未选择物品");
                return;
            }

            var itemName = Data.Server.LocalizedText.GetValueOrDefault(("items", "name", SelectedItem.Id)) ?? "";
            // Not translated?
            if (string.IsNullOrWhiteSpace(itemName) || itemName.StartsWith("DO NOT TRANSLATE"))
                itemName = SelectedItem.Name;
            // No name?
            if (string.IsNullOrWhiteSpace(itemName))
                itemName = $"<item:{SelectedItem.Id}>";

            var newGrade = (byte)cbGrade.SelectedIndex;

            if (!int.TryParse(tSaleQuantity.Text, NumberStyles.Integer, CultureInfo.InvariantCulture,
                    out var newQuantity))
            {
                MessageBox.Show(@"数量字段无效");
                return;
            }

            if (!long.TryParse(tBuyOutPrice.Text, NumberStyles.Integer, CultureInfo.InvariantCulture,
                    out var newBuyOutPrice))
            {
                MessageBox.Show(@"一口价无效");
                return;
            }

            if (!long.TryParse(tStartBid.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var newStartBid))
            {
                MessageBox.Show(@"起拍价无效");
                return;
            }

            if (!int.TryParse(tListedCount.Text, NumberStyles.Integer, CultureInfo.InvariantCulture,
                    out var newListedCount))
            {
                MessageBox.Show(@"上架数量无效");
                return;
            }

            if ((newQuantity < 1) || (newQuantity > SelectedItem.MaxStackSize))
            {
                MessageBox.Show($@"{itemName}的数量必须介于1到{SelectedItem.MaxStackSize}之间");
                return;
            }

            if (newBuyOutPrice < SelectedItem.Refund)
            {
                MessageBox.Show(
                    $@"{itemName}的一口价不得低于回收价 {SelectedItem.Refund}");
                return;
            }

            if (newStartBid >= newBuyOutPrice)
            {
                MessageBox.Show(@"起拍价必须低于一口价");
                return;
            }

            if (newListedCount < 1)
            {
                MessageBox.Show(@"您至少需要上架一件商品");
                return;
            }

            if (newListedCount > 5)
            {
                if (MessageBox.Show(
                        $@"当前需要为该物品上架 {newListedCount} 条交易(建议上限为5条)",
                        @"添加拍卖行商品",
                        MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }

            var thisItem = (lbAhList.SelectedItem as AhBotEntry);
            var isNewEntry = thisItem == null || thisItem.ItemId != SelectedItem.Id || thisItem.GradeId != newGrade;
            if (isNewEntry)
            {
                thisItem = new AhBotEntry();
            }

            thisItem.ItemId = (int)(SelectedItem.Id ?? 0);
            thisItem.GradeId = newGrade;
            thisItem.Quantity = newQuantity;
            thisItem.Price = newBuyOutPrice;
            thisItem.StartBid = newStartBid;
            thisItem.ItemEntryCount = newListedCount;
            thisItem.Comment = tComment.Text;

            if (isNewEntry)
            {
                ListingSettings.Items.Add(thisItem);
                Log($"已增加物品编号为：{thisItem.ItemId} 的拍卖机器人数据");
            }
            else
            {
                Log($"已更新物品编号为：{thisItem.ItemId} 的拍卖机器人数据");
            }

            tvAhList_AfterSelect(tvAhList, new TreeViewEventArgs(tvAhList.SelectedNode));
            SaveSettings();
            UpdateFromSettings();
        }

        private void btnQueryServerAH_Click(object sender, EventArgs e)
        {
            try
            {
                var server = Data.MySqlDb.Login.GameServers.FirstOrDefault(x => x.Name == Settings.ServerName);
                if (server == null)
                {
                    Log($"无法找到服务器信息 {Settings.ServerName}");
                    return;
                }

                var queryUrl = $"http://{server.Host}:{WebApiDefaultPort}{ApiAuctionList}";
                UpdateLiveAhCache(queryUrl);
            }
            catch (Exception exception)
            {
                Log(exception.Message);
                MessageBox.Show(exception.Message);
            }
        }

        private bool UpdateLiveAhCache(string url)
        {
            try
            {
                var jsonResult = HttpHelper.SimpleGetUriAsString(url);
                var ahListResult = JsonConvert.DeserializeObject<JsonAuctionLotList>(jsonResult);
                ServerAhListingCache = ahListResult.Items;
                Log($"从实时服务器查询到 {ServerAhListingCache.Count} 条拍卖行数据");
            }
            catch (Exception ex)
            {
                ServerAhListingCache.Clear();
                Log($"实时服务器查询失败，正在清除缓存! : {ex.Message}");
                return false;
            }
            UpdateFromSettings();
            return true;
        }

        private bool CheckAndUpdateAhBotEntries()
        {
            Log("正在从实时服务器更新拍卖行列表");
            // TODO: Get this from settings
            string serverHostName;
            var serverPort = WebApiDefaultPort;
            try
            {
                var server = Data.MySqlDb.Login.GameServers.FirstOrDefault(x => x.Name == Settings.ServerName);
                if (server == null)
                {
                    Log($"无法找到服务器信息 {Settings.ServerName}");
                    return false;
                }

                serverHostName = server.Host;
            }
            catch (Exception exception)
            {
                Log(exception.Message);
                MessageBox.Show(exception.Message);
                return false;
            }

            var character = Data.MySqlDb.Game.Characters.FirstOrDefault(x => x.Name == Settings.CharacterName);
            if (character == null)
            {
                Log($"无法找到角色 {Settings.CharacterName}");
                return false;
            }

            var queryUrl = $"http://{serverHostName}:{serverPort}{ApiAuctionList}";
            var generateUrl = $"http://{serverHostName}:{serverPort}{ApiAuctionGenerate}";
            try
            {
                if (!UpdateLiveAhCache(queryUrl))
                    return false;

                // Generate list of items to add
                var toAdd = new List<JsonAhGenerateItemRequest>();
                foreach (var listingSettingsItem in ListingSettings.Items)
                {
                    // Generate a list of all possible requests (create multiple entries for those that require multiple listings)
                    for (var count = 0; count < listingSettingsItem.ItemEntryCount; count++)
                    {
                        toAdd.Add(new JsonAhGenerateItemRequest()
                        {
                            BuyNowPrice = listingSettingsItem.Price,
                            ClientId = character.Id,
                            ClientName = character.Name,
                            Duration = 3,
                            GradeId = listingSettingsItem.GradeId,
                            ItemTemplateId = listingSettingsItem.ItemId,
                            Quantity = listingSettingsItem.Quantity,
                            StartPrice = listingSettingsItem.StartBid,
                        });
                    }
                }

                // Remove the request that are still up from the list
                foreach (var liveLot in ServerAhListingCache)
                {
                    var toFind = toAdd.FirstOrDefault(x =>
                        x.ItemTemplateId == liveLot.Item.TemplateId &&
                        x.GradeId == liveLot.Item.Grade &&
                        x.ClientId == liveLot.ClientId &&
                        x.Quantity == liveLot.Item.Count &&
                        x.BuyNowPrice == liveLot.DirectMoney);
                    if (toFind != null)
                    {
                        _ = toAdd.Remove(toFind);
                    }
                }

                if (toAdd.Count > 0)
                    Log($"正在向实时服务器添加 {toAdd.Count} 条数据");

                foreach (var generateItem in toAdd)
                {
                    _ = HttpHelper.SimplePostJsonUriAsString(generateUrl, generateItem);
                }

            }
            catch (Exception ex)
            {
                var errorMsg = $"从服务器获取拍卖行数据失败.\n\nURL: {queryUrl}\n\n{ex.Message}";
                Log(errorMsg);
                MessageBox.Show(errorMsg);
                return false;
            }

            return true;
        }

        private void cbServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.ServerName = cbServers.Text;
            SaveSettings();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // Toggle background worker thread
            if (!bgwAhCheckLoop.IsBusy)
            {
                Log("拍卖行工作正在开始");
                btnConnect.Enabled = false;
                btnConnect.Text = @"正在开始";
                bgwAhCheckLoop.RunWorkerAsync();
                tcAhBot.SelectedTab = tpLogs;
            }
            else
            {
                if (!bgwAhCheckLoop.CancellationPending)
                {
                    Log("拍卖行工作正在停止");
                    btnConnect.Text = @"正在停止 ...";
                    btnConnect.Enabled = false;
                    bgwAhCheckLoop.CancelAsync();
                }
            }
        }

        private void bgwAhCheckLoop_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // Init connection
            Log("正在初始化检查循环");

            Invoke(() =>
            {
                btnConnect.Text = @"停止";
                btnConnect.Enabled = true;
                btnQueryServerAH.Enabled = false;
            });

            // Main loop
            try
            {
                while (bgwAhCheckLoop.IsBusy)
                {
                    // Check own listings
                    // Update listed items
                    if (!CheckAndUpdateAhBotEntries())
                        break;
                    if (bgwAhCheckLoop.CancellationPending)
                        break;

                    // Take rewards/cancellations
                    if (!TryCheckMails())
                        break;
                    if (bgwAhCheckLoop.CancellationPending)
                        break;

                    // Wait a bit
                    for (var i = 0; i < 30; i++)
                    {
                        Thread.Sleep(1000);
                        if (bgwAhCheckLoop.CancellationPending)
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                Log($"拍卖行循环异常: {exception.Message}");
            }

            // Clean up
            Log("检查循环已结束");
        }

        private void bgwAhCheckLoop_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //
        }

        private void bgwAhCheckLoop_RunWorkerCompleted(object sender,
            System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Invoke(() =>
            {
                btnConnect.Text = @"开始";
                btnConnect.Enabled = true;
                btnQueryServerAH.Enabled = true;
            });
            Log("拍卖行工作已停止");
            if (CloseWhenDone)
                Dispose();
        }

        private void AhBotForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bgwAhCheckLoop.IsBusy)
            {
                CloseWhenDone = true;
                if (!bgwAhCheckLoop.CancellationPending)
                    bgwAhCheckLoop.CancelAsync();
                e.Cancel = true;
            }
        }

        private void btnCleanMails_Click(object sender, EventArgs e)
        {
            TryCheckMails();
        }

        private bool TryCheckMails()
        {
            Log("查询邮件");
            if (string.IsNullOrWhiteSpace(Settings.CharacterName))
            {
                Log("未选择角色");
                return false;
            }

            var character = Data.MySqlDb.Game.Characters.FirstOrDefault(x => x.Name == Settings.CharacterName);
            if (character == null)
            {
                Log($"无法找到角色名称: {Settings.CharacterName}");
                return false;
            }

            string serverHostName;
            var serverPort = WebApiDefaultPort;
            try
            {
                var server = Data.MySqlDb.Login.GameServers.FirstOrDefault(x => x.Name == Settings.ServerName);
                if (server == null)
                {
                    Log($"无法找到服务器信息: {Settings.ServerName}");
                    return false;
                }

                serverHostName = server.Host;
            }
            catch (Exception exception)
            {
                Log(exception.Message);
                MessageBox.Show(exception.Message);
                return false;
            }

            var listMailUrl = $"http://{serverHostName}:{serverPort}{ApiMailList}";
            var deleteMailUrl = $"http://{serverHostName}:{serverPort}{ApiMailDelete}";

            var request = new JsonListMailRequest()
            {
                CharacterId = character.Id
            };

            try
            {
                // Get list of mails for this Ah Bot character
                var jsonRes = HttpHelper.SimplePostJsonUriAsString(listMailUrl, request);
                var res = JsonConvert.DeserializeObject<JsonListMailResponseItems>(jsonRes);
                if (res == null || res.MailItems == null)
                {
                    Log($"服务器为角色 {Settings.CharacterName} ({request.CharacterId})返回了无效数据");
                    return false;
                }

                // Check if any of the mails were AH related
                // Dictionary<mailId, copperCoins>
                var mailsRewards = new Dictionary<long, long>();
                var mailsFailed = new Dictionary<long, long>();
                foreach (var mail in res.MailItems)
                {

                    switch (mail.MailType)
                    {
                        case MailType.AucOffSuccess: // Item sold, yay!
                            // Order server to delete the mail
                            mailsRewards.Add(mail.Id, mail.CopperCoins);
                            Log($"邮件编号:{mail.Id}, 类型:{mail.MailType}, {mail.SenderName} ({mail.SenderId}) -> {mail.ReceiverName} ({mail.ReceiverId}), 标题: {mail.Title}");
                            break;
                        case MailType.AucOffFail: // Didn't sell this time T.T
                        case MailType.AucOffCancel: // Shouldn't be cancelled unless this is manually done by the server owner
                            // Order server to delete the mail
                            mailsFailed.Add(mail.Id, mail.CopperCoins);
                            // Log($"Mail Id:{mail.Id}, Type:{mail.MailType}, {mail.SenderName} ({mail.SenderId}) -> {mail.ReceiverName} ({mail.ReceiverId}), Title: {mail.Title}");
                            break;
                        default:
                            // For all other cases, we just keep the mail on the server.
                            break;
                    }
                }

                // If we found new AH result mails, process them
                if (mailsRewards.Count > 0)
                {
                    Log(
                        $"找到 {Settings.CharacterName} 的 {res.MailItems.Count} 封邮件，其中 {mailsRewards.Count} 封需要处理奖励");
                    var totalReward = mailsRewards.Values.Sum(x => x);
                    if (totalReward > 0)
                    {
                        Log($"本次结算获得金额: {AaTextHelper.CopperToString(totalReward)}");

                        // Delete relevant mails
                        foreach (var (mailId, coins) in mailsRewards)
                        {
                            var mailToDelete = res.MailItems.FirstOrDefault(x => x.Id == mailId);
                            if (mailToDelete == null)
                                continue; // Shouldn't happen
                            if (coins <= 0)
                                continue; // Ignore failed sales

                            // If it was a return mail (fail or cancel), destroy the items
                            var trashItems = mailToDelete.MailType is MailType.AucOffCancel or MailType.AucOffFail;
                            TryDeleteMail(deleteMailUrl, mailToDelete.Id, mailToDelete.SenderId, mailToDelete.ReceiverId, trashItems);
                            Settings.TotalSalesCount++;
                        }

                        Settings.TotalEarned += totalReward;
                        SaveSettings();
                    }
                }

                if (mailsFailed.Count > 0)
                {
                    Log($"正在尝试删除 {mailsFailed.Count} 封包含拍卖失败通知的邮件");
                    // Delete relevant mails when no coins earned (failed sales)
                    foreach (var (mailId, coins) in mailsFailed)
                    {
                        if (coins != 0)
                            continue;
                        var mailToDelete = res.MailItems.FirstOrDefault(x => x.Id == mailId);
                        if (mailToDelete == null)
                            continue; // Shouldn't happen

                        // If it was a return mail (fail or cancel), destroy the items
                        var trashItems = mailToDelete.MailType is MailType.AucOffCancel or MailType.AucOffFail;
                        TryDeleteMail(deleteMailUrl, mailToDelete.Id, mailToDelete.SenderId, mailToDelete.ReceiverId, trashItems);
                    }
                    Log($"完成删除 {mailsFailed.Count} 封邮件");
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                return false;
            }
            return true;
        }

        private void TryDeleteMail(string url, long mailId, uint senderId, uint receiverId, bool trashItemsAsWell)
        {
            var deleteRequest = new JsonDeleteMailRequest()
            {
                MailId = mailId,
                SenderId = senderId,
                ReceiverId = receiverId,
                TrashItems = trashItemsAsWell,
            };

            var res = HttpHelper.SimplePostJsonUriAsString(url, deleteRequest);
            Log(res);
            // TODO: Report errors if any
        }

        private void tBuyOutPrice_TextChanged(object sender, EventArgs e)
        {
            if (!long.TryParse(tSaleQuantity.Text, out var count))
                count = 1;

            if (long.TryParse(tBuyOutPrice.Text, out var val))
            {
                lBuyOutPreview.Text = AaTextHelper.CopperToString(val);

                if (long.TryParse(tStartBid.Text, out var sBid) && val > 0 && sBid > 0)
                {
                    var rate = (float)sBid / (float)val * 100f;
                    lListingInfo.Text = $@"起拍价定为一口价的 {rate:F1}% , (单价 {AaTextHelper.CopperToString(sBid / count)})"";";
                }
                else
                {
                    lListingInfo.Text = @"?";
                }
            }
            else
            {
                lBuyOutPreview.Text = @"Invalid";
                lListingInfo.Text = @"?";
            }
        }

        private void tStartBid_TextChanged(object sender, EventArgs e)
        {
            if (!long.TryParse(tSaleQuantity.Text, out var count))
                count = 1;

            if (long.TryParse(tStartBid.Text, out var val))
            {
                lStartBidPreview.Text = AaTextHelper.CopperToString(val);
                if (long.TryParse(tBuyOutPrice.Text, out var sBuy) && val > 0 && sBuy > 0)
                {
                    var rate = (float)val / (float)sBuy * 100f;
                    lListingInfo.Text = $@"起拍价定为一口价的 {rate:F1}% , (单价 {AaTextHelper.CopperToString(val / count)})";
                }
                else
                {
                    lListingInfo.Text = @"?";
                }
            }
            else
            {
                lStartBidPreview.Text = @"Invalid";
                lListingInfo.Text = @"?";
            }

        }

        private void lbAhList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedAhEntry = lbAhList.SelectedItem as AhBotEntry;
            if (selectedAhEntry == null)
            {
                return;
            }
            SelectedAhBotItemEntry = selectedAhEntry;
            btnUpdateAhItem.Enabled = false;
            btnRemoveItem.Enabled = false;
            var item = Data.Server.CompactSqlite.Items.FirstOrDefault(x => x.Id == SelectedAhBotItemEntry.ItemId);
            if (item == null)
                return;

            SelectedItem = item;
            lItemId.Text = item.Id.ToString();
            lItemName.Text = Data.Server.LocalizedText.GetValueOrDefault(("items", "name", item.Id)) ?? "<error>";
            if (item.FixedGrade >= 0)
            {
                cbGrade.SelectedIndex = (int)(item.FixedGrade ?? 0);
                cbGrade.Enabled = false;
            }
            else
            {
                cbGrade.SelectedIndex = SelectedAhBotItemEntry.GradeId;
                cbGrade.Enabled = true;
            }

            tSaleQuantity.Text = SelectedAhBotItemEntry.Quantity.ToString();
            tBuyOutPrice.Text = SelectedAhBotItemEntry.Price.ToString();
            tStartBid.Text = SelectedAhBotItemEntry.StartBid.ToString();
            tListedCount.Text = SelectedAhBotItemEntry.ItemEntryCount.ToString();
            tComment.Text = SelectedAhBotItemEntry.Comment;
            btnUpdateAhItem.Enabled = true;
            btnRemoveItem.Enabled = true;
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (SelectedAhBotItemEntry == null)
                return;

            btnRemoveItem.Enabled = false;
            ListingSettings.Items.Remove(SelectedAhBotItemEntry);
            SelectedAhBotItemEntry = null;
            btnUpdateAhItem.Enabled = false;
            tvAhList_AfterSelect(tvAhList, new TreeViewEventArgs(tvAhList.SelectedNode));
            SaveSettings();
            UpdateFromSettings();
        }

        private void lItemId_TextChanged(object sender, EventArgs e)
        {
            UpdateItemIcon();
        }

        private void UpdateItemIcon()
        {
            lItemIcon.ImageList = Data.Client.Icons;
            lItemIcon.ImageIndex = -1;
            lItemIcon.Text = @"???";

            var grade = Data.Server.CompactSqlite.ItemGrades.FirstOrDefault(x => x.Id == cbGrade.SelectedIndex);
            if (grade != null)
            {
                lItemIcon.BackColor = lGrade.ForeColor = int.TryParse(grade.ColorArgb, NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture, out var hexColor)
                    ? Color.FromArgb(hexColor)
                    : SystemColors.Control;
            }
            else
            {
                lItemIcon.BackColor = SystemColors.Control;
            }

            if (!long.TryParse(lItemId.Text, out var itemId))
                return;
            lItemIcon.ImageIndex = Data.Client.GetIconIndexByItemTemplateId(itemId);
            lItemIcon.Text = lItemIcon.ImageIndex > 0 ? "" : "未找到";
        }

        private void BtnClearLog_Click(object sender, EventArgs e)
        {
            tLog.Clear();
        }
    }
}

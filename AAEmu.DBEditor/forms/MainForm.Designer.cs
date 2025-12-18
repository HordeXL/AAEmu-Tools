
namespace AAEmu.DBEditor
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            MM = new System.Windows.Forms.MenuStrip();
            MMFile = new System.Windows.Forms.ToolStripMenuItem();
            MMFileOpenServer = new System.Windows.Forms.ToolStripMenuItem();
            MMFileOpenClient = new System.Windows.Forms.ToolStripMenuItem();
            MMFileOpenMySQL = new System.Windows.Forms.ToolStripMenuItem();
            MMFileS1 = new System.Windows.Forms.ToolStripSeparator();
            MMFileSettings = new System.Windows.Forms.ToolStripMenuItem();
            MMFileS3 = new System.Windows.Forms.ToolStripSeparator();
            MMFileReload = new System.Windows.Forms.ToolStripMenuItem();
            MMFileS2 = new System.Windows.Forms.ToolStripSeparator();
            MMFileExit = new System.Windows.Forms.ToolStripMenuItem();
            MMClient = new System.Windows.Forms.ToolStripMenuItem();
            MMClientMap = new System.Windows.Forms.ToolStripMenuItem();
            MMClientS1 = new System.Windows.Forms.ToolStripSeparator();
            MMClientItems = new System.Windows.Forms.ToolStripMenuItem();
            MMClientDoodads = new System.Windows.Forms.ToolStripMenuItem();
            MMClientNPCs = new System.Windows.Forms.ToolStripMenuItem();
            MMClientSkills = new System.Windows.Forms.ToolStripMenuItem();
            MMClientZones = new System.Windows.Forms.ToolStripMenuItem();
            MMClientBuffs = new System.Windows.Forms.ToolStripMenuItem();
            MMClientFactions = new System.Windows.Forms.ToolStripMenuItem();
            MMClientQuests = new System.Windows.Forms.ToolStripMenuItem();
            MMClientLoot = new System.Windows.Forms.ToolStripMenuItem();
            MMServer = new System.Windows.Forms.ToolStripMenuItem();
            MMServerAccounts = new System.Windows.Forms.ToolStripMenuItem();
            MMServerCharacters = new System.Windows.Forms.ToolStripMenuItem();
            MMServerGuilds = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            MMServerICS = new System.Windows.Forms.ToolStripMenuItem();
            MMVersion = new System.Windows.Forms.ToolStripMenuItem();
            MMTools = new System.Windows.Forms.ToolStripMenuItem();
            MMToolsAhBot = new System.Windows.Forms.ToolStripMenuItem();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            lMySQLServer = new System.Windows.Forms.Label();
            lClientPak = new System.Windows.Forms.Label();
            lServerDB = new System.Windows.Forms.Label();
            ofdServerDB = new System.Windows.Forms.OpenFileDialog();
            ofdClientPak = new System.Windows.Forms.OpenFileDialog();
            TestPanel = new System.Windows.Forms.Panel();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            sbL1 = new System.Windows.Forms.ToolStripStatusLabel();
            gbLocale = new System.Windows.Forms.GroupBox();
            rbLocaleFr = new System.Windows.Forms.RadioButton();
            rbLocaleDe = new System.Windows.Forms.RadioButton();
            rbLocaleJa = new System.Windows.Forms.RadioButton();
            rbLocaleZhTw = new System.Windows.Forms.RadioButton();
            rbLocaleZhCn = new System.Windows.Forms.RadioButton();
            rbLocaleRu = new System.Windows.Forms.RadioButton();
            rbLocaleKo = new System.Windows.Forms.RadioButton();
            rbLocaleEnUs = new System.Windows.Forms.RadioButton();
            gbLoadingProgress = new System.Windows.Forms.GroupBox();
            MM.SuspendLayout();
            statusStrip1.SuspendLayout();
            gbLocale.SuspendLayout();
            gbLoadingProgress.SuspendLayout();
            SuspendLayout();
            // 
            // MM
            // 
            MM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { MMFile, MMClient, MMServer, MMVersion, MMTools });
            MM.Location = new System.Drawing.Point(0, 0);
            MM.Name = "MM";
            MM.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            MM.Size = new System.Drawing.Size(856, 25);
            MM.TabIndex = 0;
            MM.Text = "菜单栏1";
            // 
            // MMFile
            // 
            MMFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MMFileOpenServer, MMFileOpenClient, MMFileOpenMySQL, MMFileS1, MMFileSettings, MMFileS3, MMFileReload, MMFileS2, MMFileExit });
            MMFile.Name = "MMFile";
            MMFile.Size = new System.Drawing.Size(44, 21);
            MMFile.Text = "&文件";
            // 
            // MMFileOpenServer
            // 
            MMFileOpenServer.Enabled = false;
            MMFileOpenServer.Name = "MMFileOpenServer";
            MMFileOpenServer.Size = new System.Drawing.Size(190, 22);
            MMFileOpenServer.Text = "打开服务器数据库 ...";
            MMFileOpenServer.Visible = false;
            MMFileOpenServer.Click += MMFileOpenServer_Click;
            // 
            // MMFileOpenClient
            // 
            MMFileOpenClient.Enabled = false;
            MMFileOpenClient.Name = "MMFileOpenClient";
            MMFileOpenClient.Size = new System.Drawing.Size(190, 22);
            MMFileOpenClient.Text = "打开游戏客户端 ...";
            MMFileOpenClient.Visible = false;
            MMFileOpenClient.Click += MMFileOpenClient_Click;
            // 
            // MMFileOpenMySQL
            // 
            MMFileOpenMySQL.Enabled = false;
            MMFileOpenMySQL.Name = "MMFileOpenMySQL";
            MMFileOpenMySQL.Size = new System.Drawing.Size(190, 22);
            MMFileOpenMySQL.Text = "打开MySQL数据库 ...";
            MMFileOpenMySQL.Visible = false;
            MMFileOpenMySQL.Click += MMFileOpenMySQL_Click;
            // 
            // MMFileS1
            // 
            MMFileS1.Name = "MMFileS1";
            MMFileS1.Size = new System.Drawing.Size(187, 6);
            MMFileS1.Visible = false;
            // 
            // MMFileSettings
            // 
            MMFileSettings.Name = "MMFileSettings";
            MMFileSettings.Size = new System.Drawing.Size(190, 22);
            MMFileSettings.Text = "设置 ...";
            MMFileSettings.Click += MMFileSettings_Click;
            // 
            // MMFileS3
            // 
            MMFileS3.Name = "MMFileS3";
            MMFileS3.Size = new System.Drawing.Size(187, 6);
            // 
            // MMFileReload
            // 
            MMFileReload.Name = "MMFileReload";
            MMFileReload.Size = new System.Drawing.Size(190, 22);
            MMFileReload.Text = "重新加载";
            MMFileReload.Click += MMFileReload_Click;
            // 
            // MMFileS2
            // 
            MMFileS2.Name = "MMFileS2";
            MMFileS2.Size = new System.Drawing.Size(187, 6);
            // 
            // MMFileExit
            // 
            MMFileExit.Name = "MMFileExit";
            MMFileExit.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4;
            MMFileExit.Size = new System.Drawing.Size(190, 22);
            MMFileExit.Text = "退&出";
            MMFileExit.Click += MMFileExit_Click;
            // 
            // MMClient
            // 
            MMClient.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MMClientMap, MMClientS1, MMClientItems, MMClientDoodads, MMClientNPCs, MMClientSkills, MMClientZones, MMClientBuffs, MMClientFactions, MMClientQuests, MMClientLoot });
            MMClient.Name = "MMClient";
            MMClient.Size = new System.Drawing.Size(56, 21);
            MMClient.Text = "客户端";
            MMClient.DropDownOpened += MMClient_DropDownOpened;
            // 
            // MMClientMap
            // 
            MMClientMap.Enabled = false;
            MMClientMap.Name = "MMClientMap";
            MMClientMap.Size = new System.Drawing.Size(112, 22);
            MMClientMap.Text = "地图";
            MMClientMap.Click += MMClientMap_Click;
            // 
            // MMClientS1
            // 
            MMClientS1.Name = "MMClientS1";
            MMClientS1.Size = new System.Drawing.Size(109, 6);
            // 
            // MMClientItems
            // 
            MMClientItems.Enabled = false;
            MMClientItems.Name = "MMClientItems";
            MMClientItems.Size = new System.Drawing.Size(112, 22);
            MMClientItems.Text = "物品";
            MMClientItems.Click += MMClientItems_Click;
            // 
            // MMClientDoodads
            // 
            MMClientDoodads.Enabled = false;
            MMClientDoodads.Name = "MMClientDoodads";
            MMClientDoodads.Size = new System.Drawing.Size(112, 22);
            MMClientDoodads.Text = "装饰物";
            // 
            // MMClientNPCs
            // 
            MMClientNPCs.Enabled = false;
            MMClientNPCs.Name = "MMClientNPCs";
            MMClientNPCs.Size = new System.Drawing.Size(112, 22);
            MMClientNPCs.Text = "NPC";
            // 
            // MMClientSkills
            // 
            MMClientSkills.Enabled = false;
            MMClientSkills.Name = "MMClientSkills";
            MMClientSkills.Size = new System.Drawing.Size(112, 22);
            MMClientSkills.Text = "技能";
            // 
            // MMClientZones
            // 
            MMClientZones.Enabled = false;
            MMClientZones.Name = "MMClientZones";
            MMClientZones.Size = new System.Drawing.Size(112, 22);
            MMClientZones.Text = "区域";
            // 
            // MMClientBuffs
            // 
            MMClientBuffs.Enabled = false;
            MMClientBuffs.Name = "MMClientBuffs";
            MMClientBuffs.Size = new System.Drawing.Size(112, 22);
            MMClientBuffs.Text = "BUFFs";
            // 
            // MMClientFactions
            // 
            MMClientFactions.Enabled = false;
            MMClientFactions.Name = "MMClientFactions";
            MMClientFactions.Size = new System.Drawing.Size(112, 22);
            MMClientFactions.Text = "阵营";
            // 
            // MMClientQuests
            // 
            MMClientQuests.Enabled = false;
            MMClientQuests.Name = "MMClientQuests";
            MMClientQuests.Size = new System.Drawing.Size(112, 22);
            MMClientQuests.Text = "任务";
            // 
            // MMClientLoot
            // 
            MMClientLoot.Enabled = false;
            MMClientLoot.Name = "MMClientLoot";
            MMClientLoot.Size = new System.Drawing.Size(112, 22);
            MMClientLoot.Text = "掉落";
            // 
            // MMServer
            // 
            MMServer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MMServerAccounts, MMServerCharacters, MMServerGuilds, toolStripMenuItem2, MMServerICS });
            MMServer.Name = "MMServer";
            MMServer.Size = new System.Drawing.Size(56, 21);
            MMServer.Text = "服务器";
            MMServer.DropDownOpened += serverToolStripMenuItem_DropDownOpened;
            // 
            // MMServerAccounts
            // 
            MMServerAccounts.Enabled = false;
            MMServerAccounts.Name = "MMServerAccounts";
            MMServerAccounts.Size = new System.Drawing.Size(100, 22);
            MMServerAccounts.Text = "账号";
            MMServerAccounts.Click += MMServerAccounts_Click;
            // 
            // MMServerCharacters
            // 
            MMServerCharacters.Enabled = false;
            MMServerCharacters.Name = "MMServerCharacters";
            MMServerCharacters.Size = new System.Drawing.Size(100, 22);
            MMServerCharacters.Text = "角色";
            MMServerCharacters.Click += MMServerCharacters_Click;
            // 
            // MMServerGuilds
            // 
            MMServerGuilds.Enabled = false;
            MMServerGuilds.Name = "MMServerGuilds";
            MMServerGuilds.Size = new System.Drawing.Size(100, 22);
            MMServerGuilds.Text = "公会";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(97, 6);
            // 
            // MMServerICS
            // 
            MMServerICS.Enabled = false;
            MMServerICS.Name = "MMServerICS";
            MMServerICS.Size = new System.Drawing.Size(100, 22);
            MMServerICS.Text = "商城";
            MMServerICS.Click += MMServerICS_Click;
            // 
            // MMVersion
            // 
            MMVersion.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            MMVersion.Name = "MMVersion";
            MMVersion.Size = new System.Drawing.Size(44, 21);
            MMVersion.Text = "版本";
            // 
            // MMTools
            // 
            MMTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MMToolsAhBot });
            MMTools.Name = "MMTools";
            MMTools.Size = new System.Drawing.Size(44, 21);
            MMTools.Text = "&工具";
            MMTools.DropDownOpened += MMTools_DropDownOpened;
            // 
            // MMToolsAhBot
            // 
            MMToolsAhBot.Name = "MMToolsAhBot";
            MMToolsAhBot.Size = new System.Drawing.Size(148, 22);
            MMToolsAhBot.Text = "拍卖行机器人";
            MMToolsAhBot.Click += MMToolsAhBot_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(7, 22);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(83, 17);
            label3.TabIndex = 3;
            label3.Text = "服务器数据库:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(7, 52);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(71, 17);
            label2.TabIndex = 4;
            label2.Text = "游戏客户端:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(7, 83);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(88, 17);
            label4.TabIndex = 5;
            label4.Text = "MySQL数据库:";
            // 
            // lMySQLServer
            // 
            lMySQLServer.AutoSize = true;
            lMySQLServer.Location = new System.Drawing.Point(97, 83);
            lMySQLServer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lMySQLServer.Name = "lMySQLServer";
            lMySQLServer.Size = new System.Drawing.Size(59, 17);
            lMySQLServer.TabIndex = 8;
            lMySQLServer.Text = "127.0.0.1";
            // 
            // lClientPak
            // 
            lClientPak.AutoSize = true;
            lClientPak.Location = new System.Drawing.Point(97, 52);
            lClientPak.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lClientPak.Name = "lClientPak";
            lClientPak.Size = new System.Drawing.Size(38, 17);
            lClientPak.TabIndex = 7;
            lClientPak.Text = "<无>";
            // 
            // lServerDB
            // 
            lServerDB.AutoSize = true;
            lServerDB.Location = new System.Drawing.Point(97, 22);
            lServerDB.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lServerDB.Name = "lServerDB";
            lServerDB.Size = new System.Drawing.Size(38, 17);
            lServerDB.TabIndex = 6;
            lServerDB.Text = "<无>";
            // 
            // ofdServerDB
            // 
            ofdServerDB.DefaultExt = "sqlite3";
            ofdServerDB.FileName = "compact.sqlite3";
            ofdServerDB.Filter = "SQLite 文件|*.sqlite*|所有文件|*.*";
            ofdServerDB.InitialDirectory = "Data";
            ofdServerDB.ReadOnlyChecked = true;
            ofdServerDB.Title = "打开服务器数据库文件";
            // 
            // ofdClientPak
            // 
            ofdClientPak.FileName = "game_pak";
            ofdClientPak.Filter = "Pak 文件|*pak*.*|所有文件|*.*";
            ofdClientPak.Title = "打开客户端 game_pak 文件";
            // 
            // TestPanel
            // 
            TestPanel.BackColor = System.Drawing.SystemColors.Control;
            TestPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            TestPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            TestPanel.Location = new System.Drawing.Point(16, 43);
            TestPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            TestPanel.Name = "TestPanel";
            TestPanel.Size = new System.Drawing.Size(70, 70);
            TestPanel.TabIndex = 11;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { sbL1 });
            statusStrip1.Location = new System.Drawing.Point(0, 259);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStrip1.Size = new System.Drawing.Size(856, 22);
            statusStrip1.TabIndex = 12;
            statusStrip1.Text = "状态栏";
            // 
            // sbL1
            // 
            sbL1.Name = "sbL1";
            sbL1.Size = new System.Drawing.Size(32, 17);
            sbL1.Text = "信息";
            // 
            // gbLocale
            // 
            gbLocale.Controls.Add(rbLocaleFr);
            gbLocale.Controls.Add(rbLocaleDe);
            gbLocale.Controls.Add(rbLocaleJa);
            gbLocale.Controls.Add(rbLocaleZhTw);
            gbLocale.Controls.Add(rbLocaleZhCn);
            gbLocale.Controls.Add(rbLocaleRu);
            gbLocale.Controls.Add(rbLocaleKo);
            gbLocale.Controls.Add(rbLocaleEnUs);
            gbLocale.Location = new System.Drawing.Point(91, 31);
            gbLocale.Name = "gbLocale";
            gbLocale.Size = new System.Drawing.Size(260, 88);
            gbLocale.TabIndex = 13;
            gbLocale.TabStop = false;
            gbLocale.Text = "切换语言";
            // 
            // rbLocaleFr
            // 
            rbLocaleFr.AutoSize = true;
            rbLocaleFr.Enabled = false;
            rbLocaleFr.Location = new System.Drawing.Point(200, 25);
            rbLocaleFr.Name = "rbLocaleFr";
            rbLocaleFr.Size = new System.Drawing.Size(50, 21);
            rbLocaleFr.TabIndex = 7;
            rbLocaleFr.Text = "法语";
            rbLocaleFr.UseVisualStyleBackColor = true;
            rbLocaleFr.CheckedChanged += rbLocale_CheckedChanged;
            // 
            // rbLocaleDe
            // 
            rbLocaleDe.AutoSize = true;
            rbLocaleDe.Enabled = false;
            rbLocaleDe.Location = new System.Drawing.Point(139, 25);
            rbLocaleDe.Name = "rbLocaleDe";
            rbLocaleDe.Size = new System.Drawing.Size(50, 21);
            rbLocaleDe.TabIndex = 6;
            rbLocaleDe.Text = "德语";
            rbLocaleDe.UseVisualStyleBackColor = true;
            rbLocaleDe.CheckedChanged += rbLocale_CheckedChanged;
            // 
            // rbLocaleJa
            // 
            rbLocaleJa.AutoSize = true;
            rbLocaleJa.Enabled = false;
            rbLocaleJa.Location = new System.Drawing.Point(200, 54);
            rbLocaleJa.Name = "rbLocaleJa";
            rbLocaleJa.Size = new System.Drawing.Size(50, 21);
            rbLocaleJa.TabIndex = 5;
            rbLocaleJa.Text = "日语";
            rbLocaleJa.UseVisualStyleBackColor = true;
            rbLocaleJa.CheckedChanged += rbLocale_CheckedChanged;
            // 
            // rbLocaleZhTw
            // 
            rbLocaleZhTw.AutoSize = true;
            rbLocaleZhTw.Enabled = false;
            rbLocaleZhTw.Location = new System.Drawing.Point(139, 54);
            rbLocaleZhTw.Name = "rbLocaleZhTw";
            rbLocaleZhTw.Size = new System.Drawing.Size(50, 21);
            rbLocaleZhTw.TabIndex = 4;
            rbLocaleZhTw.Text = "繁中";
            rbLocaleZhTw.UseVisualStyleBackColor = true;
            rbLocaleZhTw.CheckedChanged += rbLocale_CheckedChanged;
            // 
            // rbLocaleZhCn
            // 
            rbLocaleZhCn.AutoSize = true;
            rbLocaleZhCn.Enabled = false;
            rbLocaleZhCn.Location = new System.Drawing.Point(78, 54);
            rbLocaleZhCn.Name = "rbLocaleZhCn";
            rbLocaleZhCn.Size = new System.Drawing.Size(50, 21);
            rbLocaleZhCn.TabIndex = 3;
            rbLocaleZhCn.Text = "简中";
            rbLocaleZhCn.UseVisualStyleBackColor = true;
            rbLocaleZhCn.CheckedChanged += rbLocale_CheckedChanged;
            // 
            // rbLocaleRu
            // 
            rbLocaleRu.AutoSize = true;
            rbLocaleRu.Enabled = false;
            rbLocaleRu.Location = new System.Drawing.Point(78, 25);
            rbLocaleRu.Name = "rbLocaleRu";
            rbLocaleRu.Size = new System.Drawing.Size(50, 21);
            rbLocaleRu.TabIndex = 2;
            rbLocaleRu.Text = "俄语";
            rbLocaleRu.UseVisualStyleBackColor = true;
            rbLocaleRu.CheckedChanged += rbLocale_CheckedChanged;
            // 
            // rbLocaleKo
            // 
            rbLocaleKo.AutoSize = true;
            rbLocaleKo.Location = new System.Drawing.Point(6, 25);
            rbLocaleKo.Name = "rbLocaleKo";
            rbLocaleKo.Size = new System.Drawing.Size(50, 21);
            rbLocaleKo.TabIndex = 1;
            rbLocaleKo.Text = "韩语";
            rbLocaleKo.UseVisualStyleBackColor = true;
            rbLocaleKo.CheckedChanged += rbLocale_CheckedChanged;
            // 
            // rbLocaleEnUs
            // 
            rbLocaleEnUs.AutoSize = true;
            rbLocaleEnUs.Checked = true;
            rbLocaleEnUs.Enabled = false;
            rbLocaleEnUs.Location = new System.Drawing.Point(6, 54);
            rbLocaleEnUs.Name = "rbLocaleEnUs";
            rbLocaleEnUs.Size = new System.Drawing.Size(50, 21);
            rbLocaleEnUs.TabIndex = 0;
            rbLocaleEnUs.TabStop = true;
            rbLocaleEnUs.Text = "英语";
            rbLocaleEnUs.UseVisualStyleBackColor = true;
            rbLocaleEnUs.CheckedChanged += rbLocale_CheckedChanged;
            // 
            // gbLoadingProgress
            // 
            gbLoadingProgress.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            gbLoadingProgress.Controls.Add(label3);
            gbLoadingProgress.Controls.Add(label2);
            gbLoadingProgress.Controls.Add(label4);
            gbLoadingProgress.Controls.Add(lServerDB);
            gbLoadingProgress.Controls.Add(lClientPak);
            gbLoadingProgress.Controls.Add(lMySQLServer);
            gbLoadingProgress.Location = new System.Drawing.Point(14, 130);
            gbLoadingProgress.Name = "gbLoadingProgress";
            gbLoadingProgress.Size = new System.Drawing.Size(830, 114);
            gbLoadingProgress.TabIndex = 15;
            gbLoadingProgress.TabStop = false;
            gbLoadingProgress.Text = "状态";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(856, 281);
            Controls.Add(gbLoadingProgress);
            Controls.Add(gbLocale);
            Controls.Add(statusStrip1);
            Controls.Add(TestPanel);
            Controls.Add(MM);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = MM;
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "MainForm";
            Text = "AAEmu.Editor  ";
            Load += MainForm_Load;
            MM.ResumeLayout(false);
            MM.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            gbLocale.ResumeLayout(false);
            gbLocale.PerformLayout();
            gbLoadingProgress.ResumeLayout(false);
            gbLoadingProgress.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip MM;
        private System.Windows.Forms.ToolStripMenuItem MMFile;
        private System.Windows.Forms.ToolStripMenuItem MMFileOpenServer;
        private System.Windows.Forms.ToolStripMenuItem MMFileOpenClient;
        private System.Windows.Forms.ToolStripMenuItem MMFileOpenMySQL;
        private System.Windows.Forms.ToolStripSeparator MMFileS1;
        private System.Windows.Forms.ToolStripMenuItem MMFileSettings;
        private System.Windows.Forms.ToolStripSeparator MMFileS2;
        private System.Windows.Forms.ToolStripMenuItem MMFileExit;
        private System.Windows.Forms.ToolStripMenuItem MMClient;
        private System.Windows.Forms.ToolStripMenuItem MMClientItems;
        private System.Windows.Forms.ToolStripMenuItem MMClientMap;
        private System.Windows.Forms.ToolStripSeparator MMClientS1;
        private System.Windows.Forms.ToolStripMenuItem MMClientDoodads;
        private System.Windows.Forms.ToolStripMenuItem MMClientNPCs;
        private System.Windows.Forms.ToolStripMenuItem MMClientSkills;
        private System.Windows.Forms.ToolStripMenuItem MMClientZones;
        private System.Windows.Forms.ToolStripMenuItem MMClientBuffs;
        private System.Windows.Forms.ToolStripMenuItem MMClientFactions;
        private System.Windows.Forms.ToolStripMenuItem MMClientQuests;
        private System.Windows.Forms.ToolStripMenuItem MMClientLoot;
        private System.Windows.Forms.ToolStripMenuItem MMServer;
        private System.Windows.Forms.ToolStripMenuItem MMServerAccounts;
        private System.Windows.Forms.ToolStripMenuItem MMServerGuilds;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem MMServerICS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lMySQLServer;
        private System.Windows.Forms.Label lClientPak;
        private System.Windows.Forms.Label lServerDB;
        private System.Windows.Forms.ToolStripSeparator MMFileS3;
        private System.Windows.Forms.ToolStripMenuItem MMFileReload;
        private System.Windows.Forms.OpenFileDialog ofdServerDB;
        private System.Windows.Forms.OpenFileDialog ofdClientPak;
        private System.Windows.Forms.Panel TestPanel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel sbL1;
        private System.Windows.Forms.ToolStripMenuItem MMServerCharacters;
        private System.Windows.Forms.GroupBox gbLocale;
        private System.Windows.Forms.RadioButton rbLocaleEnUs;
        private System.Windows.Forms.RadioButton rbLocaleRu;
        private System.Windows.Forms.RadioButton rbLocaleKo;
        private System.Windows.Forms.RadioButton rbLocaleFr;
        private System.Windows.Forms.RadioButton rbLocaleDe;
        private System.Windows.Forms.RadioButton rbLocaleJa;
        private System.Windows.Forms.RadioButton rbLocaleZhTw;
        private System.Windows.Forms.RadioButton rbLocaleZhCn;
        private System.Windows.Forms.ToolStripMenuItem MMVersion;
        private System.Windows.Forms.ToolStripMenuItem MMTools;
        private System.Windows.Forms.ToolStripMenuItem MMToolsAhBot;
        private System.Windows.Forms.GroupBox gbLoadingProgress;
    }
}


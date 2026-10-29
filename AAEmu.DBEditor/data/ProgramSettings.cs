using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using AAEmu.DBEditor.utils;
using Newtonsoft.Json;

namespace AAEmu.DBEditor.data;


[JsonObject]
public class ProgramSettings
{
    [JsonIgnore]
    public static ProgramSettings Instance { get; set; } = new ProgramSettings();

    public static ProgramSettings LoadFromFile(string fileName)
    {
        if (!File.Exists(fileName))
            return new ProgramSettings();
        var jsonData = File.ReadAllText(fileName);
        if (!JsonHelper.TryDeserializeObject<ProgramSettings>(jsonData, out var newSettings, out var ex))
        {
            return null;
        }

        return newSettings;
    }

    public static bool SaveToFile(ProgramSettings settings, string fileName)
    {
        var res = JsonConvert.SerializeObject(settings);
        try
        {
            File.WriteAllText(fileName, res);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        return false;
    }

    /// <summary>
    /// Gets the path that we use to store settings and creates it if it doesn't exist yet
    /// </summary>
    /// <returns></returns>
    public static string GetSettingsFolder()
    {
        var targetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ZeromusXYZ", "AAEmu.DBEditor");
        if (!Directory.Exists(targetPath))
            Directory.CreateDirectory(targetPath);
        return targetPath;
    }

    /// <summary>
    /// Converts legacy/localized language labels to the stable codes used by the database.
    /// </summary>
    public static string NormalizeClientLanguage(string language)
    {
        if (string.IsNullOrWhiteSpace(language))
            return "en_us";

        switch (language.Trim().ToLowerInvariant())
        {
            case "en_us":
            case "en-us":
            case "英语":
                return "en_us";
            case "ko":
            case "ko_kr":
            case "韩语":
                return "ko";
            case "ru":
            case "俄语":
                return "ru";
            case "de":
            case "德语":
                return "de";
            case "fr":
            case "法语":
                return "fr";
            case "zh_cn":
            case "zh-cn":
            case "简中":
            case "简体中文":
                return "zh_cn";
            case "zh_tw":
            case "zh-tw":
            case "繁中":
            case "繁体中文":
                return "zh_tw";
            case "ja":
            case "日语":
                return "ja";
            default:
                return language.Trim().ToLowerInvariant();
        }
    }

    public string ClientPak { get; set; } = string.Empty;
    public string ServerDb { get; set; } = string.Empty;
    public string ClientDb { get; set; } = string.Empty; // Unused
    public string MySqlDb { get; set; } = "127.0.0.1:3306";
    public string MySqlUser { get; set; } = "root";
    public string MySqlPassword { get; set; } = string.Empty;
    public string MySqlLogin { get; set; } = "aaemu_login";
    public string MySqlGame { get; set; } = "aaemu_game";
    public string ClientLanguage { get; set; } = "zh_cn";
}
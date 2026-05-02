using System;
using System.IO;
using System.Text.Json;

namespace ScreenDimmer
{
    /// <summary>
    /// アプリ設定の永続化クラス。
    /// exe と同階層の AppData (%APPDATA%\ScreenDimmer\settings.json) に保存するため
    /// インストール不要・単体exe動作を維持したまま設定を永続化できる。
    /// </summary>
    public class AppSettings
    {
        private static readonly string SettingsDir =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ScreenDimmer");
        private static readonly string SettingsPath =
            Path.Combine(SettingsDir, "settings.json");

        // デフォルト値：0〜255 の中間よりやや薄め
        private const byte DefaultAlpha = 100;

        public byte Alpha { get; set; } = DefaultAlpha;
        public bool FilterEnabled { get; set; } = true;

        // ---- 静的ファクトリ ----

        public static AppSettings Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                }
            }
            catch
            {
                // 読み込み失敗時はデフォルト値で継続
            }
            return new AppSettings();
        }

        public void Save()
        {
            try
            {
                Directory.CreateDirectory(SettingsDir);
                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsPath, json);
            }
            catch
            {
                // 保存失敗は無視（ポータブル環境での権限問題等を考慮）
            }
        }
    }
}

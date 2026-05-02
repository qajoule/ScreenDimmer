using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Hardcodet.Wpf.TaskbarNotification;

namespace ScreenDimmer
{
    public partial class App : Application
    {
        // ---- フィールド ----
        private TaskbarIcon? _tbIcon;
        private static Mutex? _mutex;
        private readonly List<OverlayWindow> _overlays = new();
        private AppSettings _settings = new();

        // ---- バインディング用プロパティ ----
        // TaskbarIcon の DataContext = this (App) に設定するため public で公開する
        public ICommand ToggleFilterCommand { get; private set; } = null!;

        // ---- ライフサイクル ----

        protected override void OnStartup(StartupEventArgs e)
        {
            // 多重起動防止
            const string mutexName = "ScreenDimmerAppMutex";
            _mutex = new Mutex(true, mutexName, out bool createdNew);
            if (!createdNew)
            {
                Current.Shutdown();
                return;
            }

            base.OnStartup(e);

            // 設定を読み込む
            _settings = AppSettings.Load();

            // コマンドを初期化（DataContext より先に行う）
            ToggleFilterCommand = new RelayCommand(_ => ToggleFilter());

            // TaskbarIcon を取得し DataContext を App 自身に設定してバインディングを解決する
            _tbIcon = (TaskbarIcon)FindResource("TaskbarIcon");
            _tbIcon.DataContext = this;

            // 全モニター分のオーバーレイを生成
            CreateOverlay();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _settings.Save();
            _tbIcon?.Dispose();
            _mutex?.ReleaseMutex();
            base.OnExit(e);
        }

        // ---- オーバーレイ管理 ----

        /// <summary>
        /// 接続されている全モニターに対してオーバーレイウィンドウを生成する。
        /// </summary>
        private void CreateOverlay()
        {
            // WinForms の Screen クラスを使わず、WPF の SystemParameters + Win32 で取得する
            // マルチモニター対応: 仮想スクリーン全体を1枚のオーバーレイでカバーする方式
            // (モニターをまたいで1ウィンドウで覆う — DPI混在環境でも動作する)
            var overlay = new OverlayWindow();
            _overlays.Add(overlay);

            ApplySettings();
            overlay.Show();
        }

        /// <summary>
        /// フィルターの表示/非表示を切り替える。
        /// </summary>
        public void ToggleFilter()
        {
            _settings.FilterEnabled = !_settings.FilterEnabled;
            ApplyVisibility();
        }

        /// <summary>
        /// 不透明度を更新し、設定に保存する。
        /// </summary>
        public void UpdateOpacity(byte alpha)
        {
            _settings.Alpha = alpha;
            ApplyOpacity();
        }

        private void ApplySettings()
        {
            ApplyOpacity();
            ApplyVisibility();
        }

        private void ApplyOpacity()
        {
            var brush = new SolidColorBrush(Color.FromArgb(_settings.Alpha, 0, 0, 0));
            foreach (var overlay in _overlays)
                overlay.Background = brush;
        }

        private void ApplyVisibility()
        {
            var vis = _settings.FilterEnabled ? Visibility.Visible : Visibility.Hidden;
            foreach (var overlay in _overlays)
                overlay.Visibility = vis;
        }

        // ---- 設定値アクセサ（SettingsWindow への注入用） ----

        public byte CurrentAlpha => _settings.Alpha;

        // ---- メニューイベント ----

        private void MenuSettings_Click(object sender, RoutedEventArgs e)
        {
            // 現在値と更新コールバックを注入してstatic依存を排除する
            var settings = new SettingsWindow(_settings.Alpha, UpdateOpacity);
            settings.ShowDialog();
            // ダイアログを閉じたタイミングで設定を保存する
            _settings.Save();
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Current.Shutdown();
        }
    }
}

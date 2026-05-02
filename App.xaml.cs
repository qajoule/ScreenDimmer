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
        private TaskbarIcon? _tbIcon;
        private static Mutex? _mutex;
        private readonly List<OverlayWindow> _overlays = new();
        private AppSettings _settings = new();

        public ICommand ToggleFilterCommand { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            const string mutexName = "ScreenDimmerAppMutex";
            _mutex = new Mutex(true, mutexName, out bool createdNew);
            if (!createdNew)
            {
                Current.Shutdown();
                return;
            }

            base.OnStartup(e);

            _settings = AppSettings.Load();
            ToggleFilterCommand = new RelayCommand(_ => ToggleFilter());

            _tbIcon = (TaskbarIcon)FindResource("TaskbarIcon");
            _tbIcon.DataContext = this;

            CreateOverlay();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _settings.Save();
            _tbIcon?.Dispose();
            _mutex?.ReleaseMutex();
            base.OnExit(e);
        }

        private void CreateOverlay()
        {
            var overlay = new OverlayWindow();
            _overlays.Add(overlay);

            ApplySettings();
            overlay.Show();
        }

        public void ToggleFilter()
        {
            _settings.FilterEnabled = !_settings.FilterEnabled;
            ApplyVisibility();
        }

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

        public byte CurrentAlpha => _settings.Alpha;

        private void MenuSettings_Click(object sender, RoutedEventArgs e)
        {
            var settings = new SettingsWindow(_settings.Alpha, UpdateOpacity);
            settings.ShowDialog();
            _settings.Save();
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Current.Shutdown();
        }
    }
}
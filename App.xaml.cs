using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Hardcodet.Wpf.TaskbarNotification;

namespace ScreenDimmer
{
    public partial class App : Application
    {
        public static OverlayWindow? Overlay { get; private set; }
        private TaskbarIcon? tbIcon;
        public static byte CurrentAlpha = 100;

        // 多重起動防止用のMutex
        private static Mutex? _mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "ScreenDimmerAppMutex";
            _mutex = new Mutex(true, appName, out bool createdNew);

            if (!createdNew)
            {
                // 既に起動している場合はレスポンスなく静かに終了
                Current.Shutdown();
                return;
            }

            base.OnStartup(e);

            tbIcon = (TaskbarIcon)FindResource("TaskbarIcon");

            Overlay = new OverlayWindow();
            UpdateOpacity(CurrentAlpha);
            Overlay.Show();
        }

        public void ToggleFilter()
        {
            if (Overlay != null)
            {
                if (Overlay.Visibility == Visibility.Visible)
                {
                    Overlay.Visibility = Visibility.Hidden;
                }
                else
                {
                    Overlay.Visibility = Visibility.Visible;
                }
            }
        }

        public static void UpdateOpacity(byte alpha)
        {
            CurrentAlpha = alpha;
            if (Overlay != null)
            {
                Overlay.Background = new SolidColorBrush(Color.FromArgb(alpha, 0, 0, 0));
            }
        }

        private void MenuSettings_Click(object sender, RoutedEventArgs e)
        {
            var settings = new SettingsWindow();
            settings.ShowDialog();
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            if (tbIcon != null)
            {
                tbIcon.Dispose();
            }
            _mutex?.ReleaseMutex();
            Current.Shutdown();
        }
    }
}
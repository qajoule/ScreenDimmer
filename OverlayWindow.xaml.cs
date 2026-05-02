using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ScreenDimmer
{
    public partial class OverlayWindow : Window
    {
        // ---- Win32 P/Invoke ----
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_EXSTYLE      = -20;
        private const int WS_EX_TRANSPARENT = 0x00000020;
        private const int WS_EX_LAYERED    = 0x00080000;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        // ---- コンストラクタ ----

        public OverlayWindow()
        {
            InitializeComponent();

        }

        // ---- Win32 拡張スタイル適用 ----

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            int style = GetWindowLong(hwnd, GWL_EXSTYLE);
            // WS_EX_TRANSPARENT : マウスイベントを透過させる
            // WS_EX_LAYERED     : 透明度を有効にする
            // WS_EX_TOOLWINDOW  : Alt+Tab リストに表示しない
            SetWindowLong(hwnd, GWL_EXSTYLE, style | WS_EX_TRANSPARENT | WS_EX_LAYERED | WS_EX_TOOLWINDOW);
        }
    }
}

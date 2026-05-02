using System;
using System.Windows;

namespace ScreenDimmer
{
    public partial class SettingsWindow : Window
    {
        private readonly Action<byte> _onAlphaChanged;

        /// <summary>
        /// コンストラクタ。現在のアルファ値と更新コールバックを注入する。
        /// App クラスへの static 依存を排除し、単体テスト可能な設計にする。
        /// </summary>
        /// <param name="currentAlpha">スライダーの初期値（0〜255）</param>
        /// <param name="onAlphaChanged">スライダー変更時に呼ばれるコールバック</param>
        public SettingsWindow(byte currentAlpha, Action<byte> onAlphaChanged)
        {
            InitializeComponent();
            _onAlphaChanged = onAlphaChanged;

            // InitializeComponent() 完了後に値をセットすることで
            // ValueChanged イベントが意図せず発火しないようにする
            AlphaSlider.Value = currentAlpha;
            UpdateLabel(currentAlpha);
        }

        private void AlphaSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            byte alpha = (byte)e.NewValue;
            UpdateLabel(alpha);
            _onAlphaChanged(alpha);
        }

        private void UpdateLabel(byte alpha)
        {
            // AlphaLabel は InitializeComponent() 後に確実に存在する
            AlphaLabel.Text = $"Value: {alpha}";
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}

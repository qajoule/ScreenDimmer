using System.Windows;

namespace ScreenDimmer
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            AlphaSlider.Value = App.CurrentAlpha;
        }

        private void AlphaSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (AlphaSlider == null) return;
            App.UpdateOpacity((byte)AlphaSlider.Value);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
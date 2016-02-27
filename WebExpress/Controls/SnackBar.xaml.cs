using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WebExpress.Controls
{
    /// <summary>
    /// Interaction logic for SnackBar.xaml
    /// </summary>
    public partial class SnackBar : UserControl
    {
        public SnackBar()
        {
            InitializeComponent();
            OKBtn.Text("OK");
            OKBtn.ChangeTextColor("#1abc9c");
            OKBtn.RippleColor(Colors.White);
            Loaded += SnackBar_Loaded;
        }

        private async void SnackBar_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(6000);
            StaticFunctions.AnimateHeight(this.ActualHeight, 0, this, 0.1);
        }

        private void OKBtn_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            StaticFunctions.AnimateHeight(this.ActualHeight, 0, this, 0.1);
        }

        private async void SnackBar_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualHeight == 42)
            {
                await Task.Delay(6000);
                StaticFunctions.AnimateHeight(this.ActualHeight, 0, this, 0.1);
            }
        }

        public void Text(string text)
        {
            label.Content = text;
        }
    }
}

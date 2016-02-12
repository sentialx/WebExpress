using System.Windows.Controls;
using System.Windows.Media;

namespace WebExpress.Applets
{

    public partial class Settings : UserControl
    {
        MainWindow mainWindow;
        public Settings(MainWindow mw)
        {
            InitializeComponent();
            mainWindow = mw;
            Loaded += Settings_Loaded;

        }

        private void Settings_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var converter = new BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#1abc9c");
            mainWindow.TabBar.getTabFromForm(this).Background = brush;
        }

        private void Grid_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {

        }
    }
}

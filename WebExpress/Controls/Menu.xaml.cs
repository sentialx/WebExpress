using System.Windows.Controls;
using System.Windows.Media;
using WebExpress.Applets;

namespace WebExpress.Controls
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        public MainWindow mainWindow;
        public Menu()
        {
            InitializeComponent();
        }

        private void SettingsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var converter = new BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#1abc9c");
            mainWindow.TabBar.AddTab("Settings", mainWindow, new Settings(mainWindow), brush);
        }
    }
}

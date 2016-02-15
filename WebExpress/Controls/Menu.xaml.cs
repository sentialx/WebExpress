using System.Windows;
using UserControl = System.Windows.Controls.UserControl;
using WebExpress.Applets;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System;

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

        private void DownloadsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mainWindow.Downloads1.Visibility = Visibility.Visible;

            ExecuteStoryBoard();
        }

        private void HistoryButton_Click(object sender, EventArgs e)
        {
            var tv = new TabView(mainWindow, "file://" + TabView.Historylayoutpath);
            Console.WriteLine(TabView.Historylayoutpath);
            mainWindow.TabBar.AddTab("History", mainWindow, tv, new BrushConverter().ConvertFromString("#FFF9F9F9") as SolidColorBrush);

            ExecuteStoryBoard();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ExecuteStoryBoard();
        }
        private void SettingsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Settings tv = new Settings(mainWindow);

            var converter = new BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#1abc9c");
            mainWindow.TabBar.AddTab("Settings", mainWindow, tv, brush);

            ExecuteStoryBoard();
        }

        private void AddonsButton_Click(object sender, RoutedEventArgs e)
        {
            var tv = new Extensions();
            var converter = new BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#1abc9c");
            mainWindow.TabBar.AddTab("Extensions", mainWindow, tv, brush);

            ExecuteStoryBoard();
        }

        private void BookmarksButton_Click(object sender, RoutedEventArgs e)
        {
            var tv = new TabView(mainWindow, "file://" + TabView.Bookmarkslayoutpath);
            Console.WriteLine(TabView.Historylayoutpath);
            mainWindow.TabBar.AddTab("History", mainWindow, tv, new BrushConverter().ConvertFromString("#FFF9F9F9") as SolidColorBrush);
            
            ExecuteStoryBoard();
        }

        private void ExecuteStoryBoard()
        {
            Storyboard sb = this.FindResource("sb") as Storyboard;
            Storyboard.SetTarget(sb, this);
            sb.Begin();
            sb.Completed += (o, e1) =>
            {
                Visibility = Visibility.Hidden;
            };
        }
    }
}

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

        private void SettingsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var converter = new BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#1abc9c");
            mainWindow.TabBar.AddTab("Settings", mainWindow, new Settings(mainWindow), brush);
            Storyboard sb = this.FindResource("sb") as Storyboard;
            Storyboard.SetTarget(sb, this);
            sb.Begin();
            sb.Completed +=
             (o, e1) =>
             {

                 Visibility = Visibility.Hidden;
             };
        }

        private void DownloadsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mainWindow.Downloads1.Visibility = Visibility.Visible;
            Storyboard sb = this.FindResource("sb") as Storyboard;
            Storyboard.SetTarget(sb, this);
            sb.Begin();
            sb.Completed +=
             (o, e1) =>
             {

                 Visibility = Visibility.Hidden;
             };
        }

        private void HistoryButton_Click(object sender, EventArgs e)
        {
            TabView tv = new TabView(mainWindow, "mw");
            tv = new TabView(mainWindow, "file://" + tv.Historylayoutpath);
            Console.WriteLine(tv.Historylayoutpath);
            mainWindow.TabBar.AddTab("History", mainWindow, tv, Brushes.White);
            Storyboard sb = this.FindResource("sb") as Storyboard;
            Storyboard.SetTarget(sb, this);
            sb.Begin();
            sb.Completed +=
             (o, e1) =>
             {

                 Visibility = Visibility.Hidden;
             };
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = this.FindResource("sb") as Storyboard;
            Storyboard.SetTarget(sb, this);
            sb.Begin();
            sb.Completed +=
             (o, e1) =>
             {

                 Visibility = Visibility.Hidden;
             };
        }

        private void AddonsButton_Click(object sender, RoutedEventArgs e)
        {
            Extensions tv = new Extensions();
            tv = new Extensions();
            var converter = new BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#1abc9c");
            mainWindow.TabBar.AddTab("Extensions", mainWindow, tv, brush);
            Storyboard sb = this.FindResource("sb") as Storyboard;
            Storyboard.SetTarget(sb, this);
            sb.Begin();
            sb.Completed +=
             (o, e1) =>
             {

                 Visibility = Visibility.Hidden;
             };
        }
    }
}

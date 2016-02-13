using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shell;


namespace WebExpress
{
    public partial class MainWindow : Window
    {
        public bool menuToggled;
        private bool maximized;
        private AddBookmark addbook;
        public MainWindow()
        {
            InitializeComponent();
            maximized = false;
            if (maximized)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
            
            Loaded += MainWindow_Loaded;
            WindowChrome wc = new WindowChrome();
            WindowChrome.SetWindowChrome(this, wc);
            Downloads1.Visibility = Visibility.Hidden;
            wc.CaptionHeight = 0;
            wc.UseAeroCaptionButtons = false;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TabBar.AddTab("New tab", this, new TabView(this, ""), Brushes.White);
            Menu.mainWindow = this;
            menuToggled = false;
            Menu.Visibility = Visibility.Hidden;
        }

        private void MainGrid_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                Grid.Margin = new Thickness(8);
                TabBar.CalcSizes();
            }
            if (WindowState == WindowState.Normal)
            {
                Grid.Margin = new Thickness(0);
                TabBar.CalcSizes();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            CefSharp.Cef.Shutdown();
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (!maximized)
            {
                WindowState = WindowState.Maximized;
                maximized = true;
            }
            else
            {
                WindowState = WindowState.Normal;
                maximized = false;
            }
        }


        private void MainGrid_PreviewMouseDown(object sender, EventArgs e)
        {
            Storyboard sb = this.FindResource("sb") as Storyboard;
            Storyboard.SetTarget(sb, this.Menu);
            sb.Begin();
            sb.Completed +=
             (o, e1) =>
             {

                 Menu.Visibility = Visibility.Hidden;
                 menuToggled = false;
             };
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }

}

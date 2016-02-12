using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;


namespace WebExpress
{
    public partial class MainWindow : Window
    {

        private bool maximized;
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
            wc.CaptionHeight = 0;
            wc.UseAeroCaptionButtons = false;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TabBar.AddTab("New tab", this, new TabView(this), Brushes.White);
            
        }

        private void TabBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
            TabBar.CalcSizes();
        }

        private void MainGrid_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                Grid.Margin = new Thickness(8);
            }
            if (WindowState == WindowState.Normal)
            {
                Grid.Margin = new Thickness(0);
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

       
    }

}

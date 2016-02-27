using System.Windows;
using UserControl = System.Windows.Controls.UserControl;
using WebExpress.Applets;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CefSharp;
using Control = System.Windows.Controls.Control;

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
            SettingsButton.Text("Settings");
            HistoryButton.Text("History");
            BookmarksButton.Text("Bookmarks");
            DownloadsButton.Text("Downloads");
            ExtensionsButton.Text("Extensions");
            WindowButton.Text("New window");
            FullscreenButton.Text("Fullscreen");
            ScreenButton.Text("Take screenshot");
            DevButton.Text("Developer tools");
            IncognitoButton.Text("Incognito");

            SettingsButton.ImageSource("settings.png");
            HistoryButton.ImageSource("history.png");
            BookmarksButton.ImageSource("bookmarks.png");
            DownloadsButton.ImageSource("download.png");
            ExtensionsButton.ImageSource("extension.png");
            WindowButton.ImageSource(("window.png"));
            FullscreenButton.ImageSource("fullscreen.png");
            ScreenButton.ImageSource("screen.png");
            DevButton.ImageSource("dev.png");
            IncognitoButton.ImageSource("privacy.png");

        }

        private void DownloadsButton_Click(object sender, MouseButtonEventArgs e)
        {
            double height = 0;
            if (!mainWindow.Downloads1.ItemsCount.Equals(0))
            {
                height = mainWindow.Downloads1.MarginTop + 50 + mainWindow.Downloads1.Items.Count * mainWindow.Downloads1.ItemHeight;
            }
            else
            {
                height = mainWindow.Downloads1.MarginTop + 50;
            }
            mainWindow.Downloads1.Visibility = Visibility.Visible;
            StaticFunctions.AnimateScale(0,0,250,height, mainWindow.Downloads1, 0.2);
            ExecuteStoryBoard();
        }

        private void HistoryButton_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ApplicationCommands.New.Execute(
                    new OpenTabCommandParameters(new History(), "History",
                        "#1abc9c"), this);

                ExecuteStoryBoard();
            }
            catch
            {
                
            }
        }

        private void SettingsButton_Click(object sender, MouseButtonEventArgs e)
        {
            ApplicationCommands.New.Execute(new OpenTabCommandParameters(new Settings(), "Settings", "#1abc9c"), this);

            ExecuteStoryBoard();
        }

        private void AddonsButton_Click(object sender, MouseButtonEventArgs e)
        {
            ApplicationCommands.New.Execute(new OpenTabCommandParameters(new Extensions(), "Extensions", "#1abc9c"), this);

            ExecuteStoryBoard();
        }

        private void BookmarksButton_Click(object sender, MouseButtonEventArgs e)
        {

            try
            {
                ApplicationCommands.New.Execute(
                    new OpenTabCommandParameters(new Bookmarks.Bookmarks(mainWindow), "Bookmarks",
                        "#1abc9c"), this);

                ExecuteStoryBoard();
            }
            catch
            {

            }
        }

        private void ExecuteStoryBoard()
        {
            StaticFunctions.AnimateScale(this.ActualWidth, this.ActualHeight, 0, 0, this, 0.2);
        }


        private void IncognitoButton_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ExecuteStoryBoard();
            if (StaticDeclarations.IsIncognito)
            {
                StaticDeclarations.IsIncognito = false;
                foreach (TabView page in mainWindow.Pages)
                {
                    page.SnackBar.Text("You turned off incognito mode");
                    page.SnackBar.Visibility = Visibility.Visible;
                    StaticFunctions.AnimateHeight(0, 42, page.SnackBar, 0.1);
                }
            }
            else
            {
                foreach (TabView page in mainWindow.Pages)
                {
                    StaticDeclarations.IsIncognito = true;
                    page.SnackBar.Text("You turned on incognito mode");
                    page.SnackBar.Visibility = Visibility.Visible;
                    StaticFunctions.AnimateHeight(0, 42, page.SnackBar, 0.1);
                }
            }
        }

        private void FullscreenButton_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ExecuteStoryBoard();
            if (StaticDeclarations.IsFullscreen)
            {
                StaticDeclarations.IsFullscreen = false;
                mainWindow.WindowState = WindowState.Maximized;
                mainWindow.ShowTabs();
                FullscreenButton.ImageSource("fullscreen.png");
                foreach (TabView page in mainWindow.Pages)
                {
                    page.WebView.Margin = new Thickness(0,42,0,0);
                    page.Panel.Height = 42;
                    page.SnackBar.Text("You turned off fullscreen mode");
                    page.SnackBar.Visibility = Visibility.Visible;
                    StaticFunctions.AnimateHeight(0, 42, page.SnackBar, 0.1);
                    StaticFunctions.AnimateHeight(3, 42, page.Panel, 0.15);
                    page.Refresh();
                }
            }
            else
            {
                mainWindow.HideTabs();
                mainWindow.WindowState = WindowState.Maximized;
                StaticDeclarations.IsFullscreen = true;
                FullscreenButton.ImageSource("fullscreen-exit.png");
                foreach (TabView page in mainWindow.Pages)
                {
                    page.WebView.Margin = new Thickness(0);
                    page.Panel.Height = 3;
                    page.SnackBar.Text("You turned on fullscreen mode");
                    page.SnackBar.Visibility = Visibility.Visible;
                    StaticFunctions.AnimateHeight(42, 3, page.Panel, 0.15);
                    StaticFunctions.AnimateHeight(0, 42, page.SnackBar, 0.1);
                    page.Refresh();
                }
            }
        }

private void GetBitmap(Control pCtrl)
        {
            RenderTargetBitmap renderTargetBitmap =
                new RenderTargetBitmap((int)pCtrl.ActualWidth, (int)pCtrl.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(pCtrl);
            PngBitmapEncoder pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            Height = 0;
            Width = 0;
            Visibility = Visibility.Hidden;
            
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.FileName = "image.png";
            saveFileDialog1.Filter = "Png image (*.png)|*.png";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        pngImage.Save(fs);
                        break;
                }
                fs.Close();
                Visibility = Visibility.Visible;

            }


        }
        private void WindowButton_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ExecuteStoryBoard();
            Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        private void DevButton_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ExecuteStoryBoard();
            if (mainWindow.TabBar.getSelectedTab().form.GetType() == typeof (TabView))
            {
                TabView tv = mainWindow.TabBar.getSelectedTab().form as TabView;
                tv.WebView.ShowDevTools();
            }
        }

        private void ScreenButton_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mainWindow.TabBar.getSelectedTab().form.GetType() == typeof (TabView))
            {
                TabView tv = mainWindow.TabBar.getSelectedTab().form as TabView;
                GetBitmap(tv.WebView);
            }
        }
    }
}

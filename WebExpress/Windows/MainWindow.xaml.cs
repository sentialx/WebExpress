using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;

namespace WebExpress
{
    public partial class MainWindow : Window
    {
        public bool Maximized;
        private AddBookmark Addbook;
        public List<TabView> Pages;
        private WebClient DLUpdate;
        private WebClient JsonDownload;
        

        public void CheckForUpdates()
        {
                Dispatcher.Invoke(() =>
                {
                    try
                    {
                        DLUpdate = new WebClient();
                        JsonDownload = new WebClient();
                        string actualVersion =
                            Convert.ToString(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
                        string link = DLUpdate.DownloadString("http://pastebin.com/raw.php?i=bfs0Jdci");
                        string newVersion = DLUpdate.DownloadString(link + "WebExpress/update.txt");
                        DLUpdate.DownloadFileCompleted += DLUpdate_DownloadFileCompleted;

                        var version1 = new Version(actualVersion);
                        var version2 = new Version(newVersion);

                        var result = version1.CompareTo(version2);
                        if (result > 0)
                            Console.WriteLine("version1 is greater");
                        else if (result < 0)
                        {
                            JsonDownload.DownloadFileAsync(
                                new Uri(link + "WebExpress/files.json"), "files.json");
                            this.Hide();

                            DLUpdate.DownloadFileAsync(new Uri(link + "WebExpress/Update.exe"),
                                "Update.exe");
                        }
                        else
                            Console.WriteLine("versions are equal");
                        return;
                    }
                    catch(Exception ex)
                    {
                        
                    }
                });
            
            }

        public void HideTabs()
        {
            container.Margin = new Thickness(0);

        }

        public void ShowTabs()
        {
            container.Margin = new Thickness(0,26,0,0);
        }
        private void DLUpdate_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (File.Exists("Update.exe"))
                {
                    Process.Start("Update.exe");
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update open: " + ex.Message);
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, OpenNewTab));

            Maximized = false;
            if (Maximized)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
            CheckForUpdates();

            Loaded += MainWindow_Loaded;

            Pages = new List<TabView>();
            Downloads1.Height = 0;
            WindowChrome wc = new WindowChrome();
            WindowChrome.SetWindowChrome(this, wc);
            wc.CaptionHeight = 0;
            wc.UseAeroCaptionButtons = false;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!System.IO.File.Exists("settings.json"))
            {
                ApplicationCommands.New.Execute(new OpenTabCommandParameters("", "New Tab", "#FFF9F9F9"), this);
                try
                {
                    Values values = new Values();
                    values.SE = "Google";
                    values.Start = "";
                    string output = JsonConvert.SerializeObject(values);
                    System.IO.File.WriteAllText("settings.json", output);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("On first load settings save error: " + ex.Message);
                }
            }
            else
            {
                dynamic dyn = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("settings.json"));
                ApplicationCommands.New.Execute(
                    new OpenTabCommandParameters(Convert.ToString(dyn.Start), "New Tab", "#FFF9F9F9"), this);
            }


            Menu.mainWindow = this;
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
            if (!Maximized)
            {
                WindowState = WindowState.Maximized;
                Maximized = true;
            }
            else
            {
                WindowState = WindowState.Normal;
                Maximized = false;
            }
        }


        private void MainGrid_PreviewMouseDown(object sender, EventArgs e)
        {

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
                if (e.ChangedButton == MouseButton.Left)
                    DragMove();

        }

        private void OpenNewTab(object sender, ExecutedRoutedEventArgs e)
        {
            var commandParams = (OpenTabCommandParameters) e.Parameter;

            TabBar.AddTab(commandParams, this);
        }

        private void Close_MouseEnter(object sender, MouseEventArgs e)
        {
            StaticFunctions.ChangeButtonImage("close_button_hover.png", CloseImage);
        }

        private void Close_MouseLeave(object sender, MouseEventArgs e)
        {
            StaticFunctions.ChangeButtonImage("close_button.png", CloseImage);
        }

        private void Maximize_MouseEnter(object sender, MouseEventArgs e)
        {
            StaticFunctions.ChangeButtonImage("maximize_button_hover.png", MaximizeImage);
        }

        private void Maximize_MouseLeave(object sender, MouseEventArgs e)
        {
            StaticFunctions.ChangeButtonImage("maximize_button.png", MaximizeImage);
        }

        private void Minimize_MouseEnter(object sender, MouseEventArgs e)
        {
            StaticFunctions.ChangeButtonImage("minimize_button_hover.png", MinimizeImage);
        }

        private void Minimize_MouseLeave(object sender, MouseEventArgs e)
        {
            StaticFunctions.ChangeButtonImage("minimize_button.png", MinimizeImage);
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
            
        }

        private void container_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            StaticFunctions.AnimateScale(Menu.ActualWidth, Menu.ActualHeight, 0, 0 , Menu, 0.2);
            StaticFunctions.AnimateFade(1, 0, Menu, 0.3);
        }
    }
}
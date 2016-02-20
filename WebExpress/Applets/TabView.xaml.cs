using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CefSharp;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using UserControl = System.Windows.Controls.UserControl;
using System.Windows.Media.Animation;
using System.Drawing;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using CefSharp.Wpf;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
namespace WebExpress
{
    public partial class TabView : UserControl, IDisplayHandler, IDownloadHandler, ILifeSpanHandler, IContextMenuHandler
    {

        //Declarations

        private bool alreadyFocused;
        private string urlToLoad;
        private string Title;
        private string LastWebsite;
        private readonly MainWindow mainWindow;
        private readonly char splitChar;
        private bool refreshing;
        private string target;
        private System.Drawing.Color _color;
        private AddBookmark addbook;
        private List<string> allItems1;

        private BitmapImage backBtn;
        private BitmapImage backBtnHover;
        private BitmapImage forwardBtn;
        private BitmapImage forwardBtnHover;
        private BitmapImage refreshBtn;
        private BitmapImage refreshBtnHover;
        private BitmapImage menuBtn;
        private BitmapImage menuBtnHover;
        private BitmapImage bookmarkBtn;
        private BitmapImage bookmarkBtnHover;
        private BitmapImage stopBtn;
        private BitmapImage closeBtn;

        public TabView(MainWindow mw, string url)
        {
            InitializeComponent();

            //Assignments

            mainWindow = mw;
            LastWebsite = "";
            target = "";
            WebView.LifeSpanHandler = this;
            splitChar = (char) 42;
            WebView.DownloadHandler = this;
            WebView.DisplayHandler = this;
            BrowserSettings s1 = new BrowserSettings();
            s1.LocalStorage = CefState.Enabled;
            s1.Databases = CefState.Enabled;
            s1.ApplicationCache = CefState.Enabled;
            WebView.BrowserSettings = s1;
            refreshing = false;
            allItems1 = new List<string>();
            WebView.MenuHandler = this;


            //Events

            WebView.FrameLoadEnd += WebView_FrameLoadEnd;
            WebView.TitleChanged += WebView_TitleChanged;
            Loaded += TabView_Loaded;
            WebView.IsBrowserInitializedChanged += WebView_IsBrowserInitializedChanged;
            WebView.FrameLoadStart += WebView_FrameLoadStart;
            WebView.StatusMessage += WebView_StatusMessage;

            //Method calls

            HideSuggestions();
            mw.Pages.Add(this);

            urlToLoad = url;
        }

        private void WebView_StatusMessage(object sender, StatusMessageEventArgs e)
        {
            target = e.Value;
        }

        private void WebView_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                RefreshImage.Source = stopBtn;
                refreshing = true;
                startPage.Visibility = Visibility.Hidden;
            }));
        }


        private void WhiteButtons()
        {
            //Buttons Images

            //Back button

            backBtn = new BitmapImage();
            backBtn.BeginInit();
            backBtn.UriSource = new Uri("pack://application:,,,/Resources/back_white.png");
            backBtn.EndInit();

            //Back button hover

            backBtnHover = new BitmapImage();
            backBtnHover.BeginInit();
            backBtnHover.UriSource = new Uri("pack://application:,,,/Resources/back_hover_white.png");
            backBtnHover.EndInit();

            //Forward button

            forwardBtn = new BitmapImage();
            forwardBtn.BeginInit();
            forwardBtn.UriSource = new Uri("pack://application:,,,/Resources/forward_white.png");
            forwardBtn.EndInit();

            //Forward button hover

            forwardBtnHover = new BitmapImage();
            forwardBtnHover.BeginInit();
            forwardBtnHover.UriSource = new Uri("pack://application:,,,/Resources/forward_white_hover.png");
            forwardBtnHover.EndInit();

            //Refresh button

            refreshBtn = new BitmapImage();
            refreshBtn.BeginInit();
            refreshBtn.UriSource = new Uri("pack://application:,,,/Resources/reload_white.png");
            refreshBtn.EndInit();

            //Refresh button hover

            refreshBtnHover = new BitmapImage();
            refreshBtnHover.BeginInit();
            refreshBtnHover.UriSource = new Uri("pack://application:,,,/Resources/reload_hover_white.png");
            refreshBtnHover.EndInit();

            //Menu button

            menuBtn = new BitmapImage();
            menuBtn.BeginInit();
            menuBtn.UriSource = new Uri("pack://application:,,,/Resources/menu_white.png");
            menuBtn.EndInit();

            //Menu button hover

            menuBtnHover = new BitmapImage();
            menuBtnHover.BeginInit();
            menuBtnHover.UriSource = new Uri("pack://application:,,,/Resources/menu_hover_white.png");
            menuBtnHover.EndInit();

            //Bookmark button

            bookmarkBtn = new BitmapImage();
            bookmarkBtn.BeginInit();
            bookmarkBtn.UriSource = new Uri("pack://application:,,,/Resources/bookmark_white.png");
            bookmarkBtn.EndInit();

            //Bookmark button hover

            bookmarkBtnHover = new BitmapImage();
            bookmarkBtnHover.BeginInit();
            bookmarkBtnHover.UriSource = new Uri("pack://application:,,,/Resources/bookmark_hover_White.png");
            bookmarkBtnHover.EndInit();

            //Stop button

            stopBtn = new BitmapImage();
            stopBtn.BeginInit();
            stopBtn.UriSource = new Uri("pack://application:,,,/Resources/stop_white.png");
            stopBtn.EndInit();

            //Close button

            closeBtn = new BitmapImage();
            closeBtn.BeginInit();
            closeBtn.UriSource = new Uri("pack://application:,,,/Resources/close_Tab_white.png");
            closeBtn.EndInit();

            //Image set for buttons

            BackImage.Source = backBtn;
            ForwardImage.Source = forwardBtn;
            RefreshImage.Source = refreshBtn;
            MenuImage.Source = menuBtn;
            BookmarkImage.Source = bookmarkBtn;
            mainWindow.TabBar.getTabFromForm(this).CloseImage.Source = closeBtn;
        }


        private void BlackButtons()
        {
            //Buttons Images

            //Back button

            backBtn = new BitmapImage();
            backBtn.BeginInit();
            backBtn.UriSource = new Uri("pack://application:,,,/Resources/back.png");
            backBtn.EndInit();

            //Back button hover

            backBtnHover = new BitmapImage();
            backBtnHover.BeginInit();
            backBtnHover.UriSource = new Uri("pack://application:,,,/Resources/back_hover.png");
            backBtnHover.EndInit();

            //Forward button

            forwardBtn = new BitmapImage();
            forwardBtn.BeginInit();
            forwardBtn.UriSource = new Uri("pack://application:,,,/Resources/forward.png");
            forwardBtn.EndInit();

            //Forward button hover

            forwardBtnHover = new BitmapImage();
            forwardBtnHover.BeginInit();
            forwardBtnHover.UriSource = new Uri("pack://application:,,,/Resources/forward_hover.png");
            forwardBtnHover.EndInit();

            //Refresh button

            refreshBtn = new BitmapImage();
            refreshBtn.BeginInit();
            refreshBtn.UriSource = new Uri("pack://application:,,,/Resources/reload.png");
            refreshBtn.EndInit();

            //Refresh button hover

            refreshBtnHover = new BitmapImage();
            refreshBtnHover.BeginInit();
            refreshBtnHover.UriSource = new Uri("pack://application:,,,/Resources/reload_hover.png");
            refreshBtnHover.EndInit();

            //Menu button

            menuBtn = new BitmapImage();
            menuBtn.BeginInit();
            menuBtn.UriSource = new Uri("pack://application:,,,/Resources/menu.png");
            menuBtn.EndInit();

            //Menu button hover

            menuBtnHover = new BitmapImage();
            menuBtnHover.BeginInit();
            menuBtnHover.UriSource = new Uri("pack://application:,,,/Resources/menu_hover.png");
            menuBtnHover.EndInit();

            //Bookmark button

            bookmarkBtn = new BitmapImage();
            bookmarkBtn.BeginInit();
            bookmarkBtn.UriSource = new Uri("pack://application:,,,/Resources/bookmark.png");
            bookmarkBtn.EndInit();

            //Bookmark button hover

            bookmarkBtnHover = new BitmapImage();
            bookmarkBtnHover.BeginInit();
            bookmarkBtnHover.UriSource = new Uri("pack://application:,,,/Resources/bookmark_hover.png");
            bookmarkBtnHover.EndInit();

            //Stop button

            stopBtn = new BitmapImage();
            stopBtn.BeginInit();
            stopBtn.UriSource = new Uri("pack://application:,,,/Resources/stop.png");
            stopBtn.EndInit();

            //Close button

            closeBtn = new BitmapImage();
            closeBtn.BeginInit();
            closeBtn.UriSource = new Uri("pack://application:,,,/Resources/close_Tab.png");
            closeBtn.EndInit();


            //Image set for buttons

            BackImage.Source = backBtn;
            ForwardImage.Source = forwardBtn;
            RefreshImage.Source = refreshBtn;
            MenuImage.Source = menuBtn;
            BookmarkImage.Source = bookmarkBtn;
            mainWindow.TabBar.getTabFromForm(this).CloseImage.Source = closeBtn;
        }

        private void WebView_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!WebView.IsInitialized)
            {
                Cef.Initialize();
            }
            if (WebView.IsInitialized)
            {
                WebView.Load(urlToLoad);
            }
        }

        private void TabView_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(LoadSuggestions);
            BlackButtons();
            startPage.LoadFavs(mainWindow);
            textBox.Focus();
        }

        private void LoadSuggestions()
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                try
                {

                    listBox.Items.Clear();
                    allItems1.Clear();

                    if (System.IO.File.Exists(StaticDeclarations.Historypath))
                    {
                        string readFile = System.IO.File.ReadAllText(StaticDeclarations.Historypath);
                        dynamic json = JsonConvert.DeserializeObject(readFile);
                        foreach (dynamic item in json)
                        {
                            if (Convert.ToString((string) item.Url).Contains("#q="))
                            {
                                listBox.Items.Add(Convert.ToString(item.Title));
                                allItems1.Add(Convert.ToString(item.Title));
                            }
                            else
                            {
                                listBox.Items.Add(Convert.ToString(item.Url) + " - " + Convert.ToString(item.Title));
                                allItems1.Add(Convert.ToString(item.Url) + " - " + Convert.ToString(item.Title));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Load suggestions error: " + ex.Message);
                }
            }));
        }

        public void WriteHistory()
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                try
                {
                    if (!LastWebsite.Equals(WebView.Address))
                    {
                        if (!System.IO.File.Exists(StaticDeclarations.Historypath))
                        {
                            var history = new HistItem();
                            history.Title = Title;
                            history.Url = WebView.Address;
                            var histories = new List<HistItem>();
                            histories.Add(history);
                            var newJson = JsonConvert.SerializeObject(histories);
                            File.WriteAllText(StaticDeclarations.Historypath, newJson);

                            LastWebsite = WebView.Address;
                        }
                        else
                        {
                            HistItem history = new HistItem();
                            history.Title = Title;
                            history.Url = WebView.Address;
                            string json = File.ReadAllText(StaticDeclarations.Historypath);
                            List<HistItem> histories = JsonConvert.DeserializeObject<List<HistItem>>(json);
                            histories.Add(history);
                            string newJson = JsonConvert.SerializeObject(histories);
                            File.WriteAllText(StaticDeclarations.Historypath, newJson);

                            LastWebsite = WebView.Address;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Write history error: " + ex.Message);
                }
                try
                {
                    foreach (TabView page in mainWindow.Pages)
                    {

                        Task.Factory.StartNew(page.LoadSuggestions);

                    }
                }
                catch
                {
                }
            }));
        }


        private void WebView_TitleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            mainWindow.TabBar.getTabFromForm(this).SetTitle(e.NewValue.ToString());
            Title = e.NewValue.ToString();
        }

        private void WebView_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (e.Frame.IsMain)
                {
                    Task.Factory.StartNew(() => SetAddress(e.Url));
                    RefreshImage.Source = stopBtn;
                    refreshing = false;

                    if (Directory.Exists("Extensions"))
                    {
                        foreach (string file in System.IO.Directory.GetFiles("Extensions", "*.js"))
                        {
                            WebView.ExecuteScriptAsync(System.IO.File.ReadAllText(file));
                            Console.WriteLine(System.IO.File.ReadAllText(file));
                        }
                    }
                }
                Task.Factory.StartNew(ChangeColor);
                Task.Factory.StartNew(WriteHistory);
            });
        }

        private void SetAddress(string text)
        {
            Dispatcher.BeginInvoke((Action) (() => { textBox.Text = text; }));
        }

        public void Shutdown()
        {
            WebView.Dispose();
        }

        private void HideSuggestions()
        {
            ListContainer.Visibility = Visibility.Hidden;
        }

        private void ShowSuggestions()
        {
            Panel.Effect = null;
            ListContainer.Visibility = Visibility.Visible;
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(StaticDeclarations.Historypath) && textBox.Text != null)
                {
                    ShowSuggestions();

                    if (string.IsNullOrEmpty(textBox.Text.Trim()) == false)
                    {
                        listBox.Items.Clear();
                        foreach (string str in allItems1)
                        {
                            if (str.Contains(textBox.Text.Trim()))
                            {
                                listBox.Items.Add(str);
                            }
                        }
                    }

                    else if (textBox.Text.Trim() == "")
                    {
                        listBox.Items.Clear();

                        foreach (string str in allItems1)
                        {
                            listBox.Items.Add(str);
                        }
                    }


                    ListContainer.Height = listBox.Items.Count*34;
                }
                else
                {
                    ListContainer.Height = 0;
                    HideSuggestions();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Listbox1 error: " + ex.Message);
            }


            if (e.Key == Key.Enter)
            {
                dynamic textArray = textBox.Text.Split();

                if ((textBox.Text.Contains(".") && !textBox.Text.Contains(" ") && !textBox.Text.Contains(" .") &&
                     !textBox.Text.Contains(". ")) || textArray[0].Contains(":/") || textArray[0].Contains(":\\"))
                {
                    WebView.Load(textBox.Text);
                }
                else
                {
                    try
                    {
                        dynamic dyn = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("settings.json"));
                        if (Convert.ToString(dyn.SE) == "Google")
                        {
                            textBox.Text = textBox.Text.Replace(textBox.Text, "http://google.com/#q=" + textBox.Text);
                            WebView.Load(textBox.Text);
                        }
                    }
                    catch
                    {
                        
                    }
                }
                HideSuggestions();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            WebView.Back();
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            WebView.Forward();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            WebView.Reload();
        }

        private void listBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string[] split = listBox.SelectedItem.ToString().Split(splitChar);
                if (!split[0].Contains("http://") | !split[0].Contains("https://"))
                {
                    string[] split1 = listBox.SelectedItem.ToString().Split(splitChar);
                    dynamic dyn = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("settings.json"));
                    if (dyn.SE == "Google")
                    {
                        WebView.Load("https://google.com/#q=" + split1[0]);
                    }
                }

                if (split[0].Contains("http://") | split[0].Contains("https://"))
                {
                    string[] split1 = listBox.SelectedItem.ToString()
                        .Split(new string[] {" - "}, StringSplitOptions.None);
                    WebView.Load(split1[0]);
                }
                HideSuggestions();
            }
            catch
            {
            }
        }

        private void WebView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            HideSuggestions();
        }

        public void OnAddressChanged(IWebBrowser browserControl, AddressChangedEventArgs addressChangedArgs)
        {
            Thread thread = new Thread(new ThreadStart(delegate()
            {
                Thread.Sleep(200); // this is important ...
                try
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Send,
                        new ThreadStart(async delegate()
                        {
                            await ChangeColor();
                            this.Refresh();
                        }));
                }
                catch
                {
                }
            }));
            thread.Name = "thread-UpdateText";
            thread.Start();

        }
    


        public void OnTitleChanged(IWebBrowser browserControl, TitleChangedEventArgs titleChangedArgs)
        {
        }


        public void OnFaviconUrlChange(IWebBrowser browserControl, IBrowser browser, IList<string> urls)
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                var bitmap2 = new BitmapImage();
                try
                {
                    bitmap2.BeginInit();
                    bitmap2.UriSource = new Uri(urls[0], UriKind.Absolute);

                    bitmap2.EndInit();

                    mainWindow.TabBar.getTabFromForm(this).SetIcon(bitmap2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Favicon: " + ex.Message);
                }
            }));
        }

        public void ContrastColor(System.Drawing.Color color)
        {
            int d = 0;


            double a = 1 - (0.299*color.R + 0.587*color.G + 0.114*color.B)/255;

            if (a < 0.4)
            {
                if (!mainWindow.TabBar.getTabFromForm(this).bgTab)
                    mainWindow.TabBar.getTabFromForm(this).label_TabTitle.Foreground =
                        System.Windows.Media.Brushes.Black;
                textBox.Foreground = System.Windows.Media.Brushes.Black;
                mainWindow.TabBar.getTabFromForm(this).actualForeground = System.Windows.Media.Brushes.Black;
                BlackButtons();
                SolidColorBrush scb = new SolidColorBrush();
                scb.Color = System.Windows.Media.Color.FromArgb(255, 255, 255, 255);
                textBox.Background = scb;
                listBox.Foreground = System.Windows.Media.Brushes.Black;
                mainWindow.TabBar.getTabFromForm(this).darkColor = false;
            }
            else
            {
                if (!mainWindow.TabBar.getTabFromForm(this).bgTab)
                    mainWindow.TabBar.getTabFromForm(this).label_TabTitle.Foreground =
                        System.Windows.Media.Brushes.White;
                textBox.Foreground = System.Windows.Media.Brushes.White;
                WhiteButtons();
                listBox.Foreground = System.Windows.Media.Brushes.White;
                SolidColorBrush scb = new SolidColorBrush();
                scb.Color = System.Windows.Media.Color.FromArgb(50, 255, 255, 255);
                textBox.Background = scb;
                mainWindow.TabBar.getTabFromForm(this).actualForeground = System.Windows.Media.Brushes.White;
                mainWindow.TabBar.getTabFromForm(this).darkColor = true;
            }
        }

        public void OnFullscreenModeChange(IWebBrowser browserControl, IBrowser browser, bool fullscreen)
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                if (fullscreen == true)
                {
                    Panel.Visibility = Visibility.Hidden;
                    WebView.Margin = new Thickness(0);
                }
                else
                {
                    Panel.Visibility = Visibility.Visible;
                    WebView.Margin = new Thickness(0, 42, 0, 0);
                }
            }));
        }

        public bool OnTooltipChanged(IWebBrowser browserControl, string text)
        {
            return true;
        }

        public void OnStatusMessage(IWebBrowser browserControl, StatusMessageEventArgs statusMessageArgs)
        {

        }

        public bool OnConsoleMessage(IWebBrowser browserControl, ConsoleMessageEventArgs consoleMessageArgs)
        {
            return true;
        }

        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    ShowSuggestions();
                    listBox.Focus();
                    listBox.SelectedIndex = 0;
                    break;

                case Key.Enter:
                    try
                    {
                        string[] split = listBox.SelectedItem.ToString().Split(splitChar);
                        WebView.Load(split[0]);
                        HideSuggestions();
                    }
                    catch
                    {
                    }
                    break;
            }
        }

        private void listBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    string[] split = listBox.SelectedItem.ToString().Split(splitChar);
                    if (!split[0].Contains("http://") | !split[0].Contains("https://"))
                    {
                        string[] split1 = listBox.SelectedItem.ToString().Split(splitChar);
                        dynamic dyn = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("settings.json"));
                        if (dyn.SE == "Google")
                        {
                            WebView.Load("https://google.com/#q=" + split1[0]);
                        }
                    }

                    if (split[0].Contains("http://") | split[0].Contains("https://"))
                    {
                        string[] split1 = listBox.SelectedItem.ToString()
                            .Split(new string[] {" - "}, StringSplitOptions.None);
                        WebView.Load(split1[0]);
                    }
                    HideSuggestions();
                    break;

                case Key.Up:
                    if (listBox.SelectedIndex == 0)
                    {
                        textBox.Focus();
                    }
                    listBox.Items.MoveCurrentToPrevious();
                    break;
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Menu.Height = 0;
            mainWindow.Menu.Visibility = Visibility.Visible;
            Storyboard sb = mainWindow.FindResource("sb2") as Storyboard;
            Storyboard.SetTarget(sb, mainWindow.Menu);
            sb.Begin();
        }


        public void OnBeforeDownload(IBrowser browser, CefSharp.DownloadItem downloadItem,
            IBeforeDownloadCallback callback)
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                mainWindow.Downloads1.AddDownload(downloadItem.Url,
                    Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    downloadItem.SuggestedFileName);
                mainWindow.Downloads1.Visibility = Visibility.Visible;
            }));
        }

        public void OnDownloadUpdated(IBrowser browser, CefSharp.DownloadItem downloadItem,
            IDownloadItemCallback callback)
        {
        }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl,
            string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IWindowInfo windowInfo,
            ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;

            Dispatcher.BeginInvoke(
                (Action)
                    (() =>
                    {
                        try
                        {
                            ApplicationCommands.New.Execute(
                                new OpenTabCommandParameters(targetUrl, "New tab", "#FFF9F9F9"), this);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Open popup in new tab error: " + ex.Message);
                        }
                    }));

            return true;
        }

        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
        }

        public bool DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            return false;
        }

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
        }

        private void Back_MouseEnter(object sender, MouseEventArgs e)
        {
            BackImage.Source = backBtnHover;
        }

        private void Back_MouseLeave(object sender, MouseEventArgs e)
        {
            BackImage.Source = backBtn;
        }

        private void Forward_MouseEnter(object sender, MouseEventArgs e)
        {
            ForwardImage.Source = forwardBtnHover;
        }

        private void Forward_MouseLeave(object sender, MouseEventArgs e)
        {
            ForwardImage.Source = forwardBtn;
        }

        private void Refresh_MouseEnter(object sender, MouseEventArgs e)
        {
            RefreshImage.Source = refreshBtnHover;
        }

        private void Refresh_MouseLeave(object sender, MouseEventArgs e)
        {
            RefreshImage.Source = refreshBtn;
        }

        private void MenuButton_MouseEnter(object sender, MouseEventArgs e)
        {
            MenuImage.Source = menuBtnHover;
        }

        private void MenuButton_MouseLeave(object sender, MouseEventArgs e)
        {
            MenuImage.Source = menuBtn;
        }

        private void BookmarkButton_MouseEnter(object sender, MouseEventArgs e)
        {
            BookmarkImage.Source = bookmarkBtnHover;
        }

        private void BookmarkButton_MouseLeave(object sender, MouseEventArgs e)
        {
            BookmarkImage.Source = bookmarkBtn;
        }

        private void BookmarkButton_Click(object sender, RoutedEventArgs e)
        {
            addbook = new AddBookmark(Title, WebView.Address, mainWindow);
            addbook.HorizontalAlignment = HorizontalAlignment.Right;
            addbook.VerticalAlignment = VerticalAlignment.Top;
            addbook.Margin = new Thickness(0, -20, 30, 0);
            addbook.Width = 300;
            addbook.Height = 100;
            addbook.Visibility = Visibility.Visible;
            container.Children.Add(addbook);
        }


        private void textBox_GotMouseCapture(object sender, MouseEventArgs e)
        {
            textBox.SelectAll();
        }

        public async Task ChangeColor()
        {
            try
            {
                await Dispatcher.BeginInvoke((Action) (() =>
                {
                    Thread thread = new Thread(new ThreadStart(delegate ()
                    {
                        Task.Delay(200); // this is important ...
                        try
                        {
                            this.Dispatcher.BeginInvoke(DispatcherPriority.Send,
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
                                new ThreadStart(async delegate ()
                                {
                                    var topLeftCorner = WebView.PointToScreen(new System.Windows.Point(0, 0));
                                    var topLeftGdiPoint = new System.Drawing.Point((int)topLeftCorner.X, (int)topLeftCorner.Y);
                                    var size = new System.Drawing.Size((int)WebView.ActualWidth, (int)WebView.ActualHeight);
                                    Bitmap screenShot = new Bitmap((int)WebView.ActualWidth, (int)WebView.ActualHeight);
                                    using (var graphics = Graphics.FromImage(screenShot))
                                    {
                                        graphics.CopyFromScreen(topLeftGdiPoint, new System.Drawing.Point(), size,
                                            CopyPixelOperation.SourceCopy);
                                    }
                                    SolidColorBrush brush = new SolidColorBrush(StaticFunctions.ToMediaColor(screenShot.GetPixel(1, 1)));
                                    mainWindow.TabBar.getTabFromForm(this).Color = brush;
                                    mainWindow.TabBar.getTabFromForm(this).refreshColor();

                                    textBox.Background = brush;
                                    Panel.Background = brush;
                                    ListContainer.Background = brush;
                                    listBox.Background = brush;


                                    ContrastColor(screenShot.GetPixel(1, 1));
                                    this.Refresh();
                                }));
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
                        }
                        catch
                        {
                        }
                    }));
                    thread.Name = "thread-UpdateText";
                    thread.Start();
                    

                }));
            }
            catch
            {
            }
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            await Task.Run(ChangeColor);
        }
        private static IEnumerable<Tuple<string, CefMenuCommand>> GetMenuItems(IMenuModel model)
        {
            var list = new List<Tuple<string, CefMenuCommand>>();
            for(var i = 0; i < model.Count; i++)
            {
                var header = model.GetLabelAt(i);
                var commandId = model.GetCommandIdAt(i);
                list.Add(new Tuple<string, CefMenuCommand>(header, commandId));
            }

            return list;
        }
        public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters,
            IMenuModel model)
        {
            Console.WriteLine("Context menu opened");
            Console.WriteLine(parameters.MisspelledWord);

            if (model.Count > 0)
            {
                model.AddSeparator();
            }

            model.AddItem((CefMenuCommand)26501, "Open link in new tab");
        }

        public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters,
            CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            
                if (commandId == (CefMenuCommand) 26501)
                {
                Dispatcher.Invoke(() => {
                    Console.WriteLine(parameters.UnfilteredLinkUrl);
                    ApplicationCommands.New.Execute(
                        new OpenTabCommandParameters(parameters.FrameUrl, "New tab", "#FFF9F9F9"), this);
                   
                });
                    return true;
                }
           
            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

            chromiumWebBrowser.Dispatcher.Invoke(() =>
            {
                chromiumWebBrowser.ContextMenu = null;
            });
        }

        public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters,
            IMenuModel model, IRunContextMenuCallback callback)
        {
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

            var menuItems = GetMenuItems(model);

            chromiumWebBrowser.Dispatcher.Invoke(() =>
            {
                var menu = new ContextMenu
                {
                    IsOpen = true
                };

                RoutedEventHandler handler = null;

                handler = (s, e) =>
                {
                    menu.Closed -= handler;

                   

                };

                menu.Closed += handler;

                foreach (var item in menuItems)
                {
                    menu.Items.Add(new MenuItem
                    {
                        Header = item.Item1,
                        Command = new RelayCommand(() => { callback.Continue(item.Item2, CefEventFlags.None); })
                    });
                }
                chromiumWebBrowser.ContextMenu = menu;
            });

            return true;
        }

    }

}
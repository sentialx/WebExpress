using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CefSharp;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using UserControl = System.Windows.Controls.UserControl;
using System.Windows.Media.Animation;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Net;
using Newtonsoft.Json;

namespace WebExpress
{
    public partial class TabView : UserControl, IDisplayHandler, IDownloadHandler, ILifeSpanHandler
    {

        //Declarations

        public static string Bookmarkslayoutpath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data\\bookmarks-layout.html");

        public static string Bookmarkspath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data\\bookmarks-data.html");

        public static string Bookspath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WebExpress\\user data\\bookmarks.txt");

        public static string Historylayoutpath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data\\history-layout.html");

        public static string Historypath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data\\history-data.html");

        public static string Suggestionspath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data\\suggestions.txt");

        private bool alreadyFocused;
        private string urlToLoad;
        private string Title;
        private string LastWebsite;
        private readonly MainWindow mainWindow;
        private readonly char splitChar;
        private bool refreshing;
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
            WebView.LifeSpanHandler = this;
            splitChar = (char)42;
            WebView.DownloadHandler = this;
            WebView.DisplayHandler = this;
            BrowserSettings s1 = new BrowserSettings();
            s1.LocalStorage = CefState.Enabled;
            s1.Databases = CefState.Enabled;
            s1.ApplicationCache = CefState.Enabled;
            s1.WindowlessFrameRate = 60;
            WebView.BrowserSettings = s1;
            refreshing = false;
            allItems1 = new List<string>();

            //Events

            WebView.FrameLoadEnd += WebView_FrameLoadEnd;
            WebView.TitleChanged += WebView_TitleChanged;
            Loaded += TabView_Loaded;
            WebView.IsBrowserInitializedChanged += WebView_IsBrowserInitializedChanged;
            WebView.FrameLoadStart += WebView_FrameLoadStart;

            HideSuggestions();

            urlToLoad = url;
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

        private void whiteButtons()
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


        private void blackButtons()
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
            if(!WebView.IsInitialized)
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
            blackButtons();
            startPage.loadFavs(mainWindow);
        }

        private void LoadSuggestions()
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                try
                {
                    listBox.Items.Clear();
                    allItems1.Clear();

                    string[] readText = File.ReadAllLines(Suggestionspath);
                    var arr = new ArrayList();
                    foreach (var sr in readText)
                    {
                        arr.Add(sr);
                    }

                    foreach (string s in arr)
                    {
                        if (!s.Contains("#q="))
                        {
                            string[] split = s.Split(splitChar);
                            listBox.Items.Add(split[0] + " - " + split[1]);
                            allItems1.Add(split[0] + " - " + split[1]);
                        }
                        else
                        {
                            string[] split = Regex.Split(s, "#q=");
                            string[] split1 = split[1].Split(splitChar);
                            allItems1.Add(split1[0]);
                            listBox.Items.Add(split1[0]);
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
                if (!File.Exists(Historypath))
                {
                    File.Create(Historypath);
                }
                    if (!File.Exists(Historylayoutpath))
                    {

                    var filePath = Historylayoutpath;
                    using (var sw = new StreamWriter(filePath, true))
                    {
                        sw.WriteLine(
                            "<!DOCTYPE><html><head><title>WebExpress - history</title><link href='https://fonts.googleapis.com/css?family=Lato:300' rel='stylesheet' type='text/css'></head> <body><style>body {overflow:hidden;}</style><div id='div1'> <h1 style='font-family: lato; padding-left: 50px; padding-top: 20px;'>WebExpress - history</h1><iframe name='iframe1' id='iframe1' src='history-data.html' frameborder='0' border='0' cellspacing='0' style='border-style: none;width: 100vmin; padding-left: 45px; height:100vmin;'></iframe></div> </body></html>");
                        sw.Close();
                    }
                }
                if (!LastWebsite.Equals(WebView.Address) && !Title.Contains("WebExpress - history") && !Title.Contains("WebExpress - bookmarks"))
                    try {
                        {
                            string strFileContents = "";
                            string strDataToAppend = "<style>p { font-family: 'Arial'; } a {color: #606060; text-decoration: none;} a:hover { color: #000; } a:visited { color:#606060; } </style><p><img src='http://www.google.com/s2/favicons?domain=" +
                                WebView.Address + "' style='width: 16; height:16;'/>" + " " + Title +
                                "  <a target='_blank' href=" + WebView.Address + ">  -  " + WebView.Address + "</a></p>";
                            StreamReader srReader = null;
                            StreamWriter swWriter = null;
                            srReader = new StreamReader(Historypath);
                            strFileContents = srReader.ReadToEnd();
                            srReader.Close();

                            strFileContents = strDataToAppend + Environment.NewLine + strFileContents;
                            swWriter = new StreamWriter(Historypath, false);
                            swWriter.Write(strFileContents);
                            swWriter.Flush();

                            var filePath1 = Suggestionspath;
                            using (var sw = new StreamWriter(filePath1, true))
                            {
                                sw.WriteLine(WebView.Address + splitChar + Title);
                                sw.Close();
                            }
                            LastWebsite = WebView.Address;
                        }
                    } catch
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
                    var extensionsPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Extensions";
                    if(Directory.Exists(extensionsPath))
                    { 
                        foreach (string file in System.IO.Directory.GetFiles(extensionsPath, "*.js"))
                        {
                            WebView.EvaluateScriptAsync(System.IO.File.ReadAllText(file));
                        }
                    }
                }

                    Task.Factory.StartNew(WriteHistory);
                Task.Factory.StartNew(LoadSuggestions);

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
                ShowSuggestions();
                
                listBox.Items.Clear();
                List<string> allitems = allItems1.Distinct().ToList();
                for (int i = 0; i <= allitems.Count - 1; i++)
                {
                    if (allitems[i].Contains(textBox.Text))
                    {
                        listBox.Items.Add(allitems[i]);
                      
                    }
                    ListContainer.Height = listBox.Items.Count * 34;
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
                    dynamic dyn = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("settings.json"));
                    if (Convert.ToString(dyn.SE) == "Google")
                    {
                        
                        textBox.Text = textBox.Text.Replace(textBox.Text, "http://google.com/#q=" + textBox.Text);
                        WebView.Load(textBox.Text);
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
            try {
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
                    string[] split1 = listBox.SelectedItem.ToString().Split(new string[] { " - " }, StringSplitOptions.None);
                    WebView.Load(split1[0]);
                }
                HideSuggestions();
            } catch
            {

            }

        }

        private void WebView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            HideSuggestions();
        }

        public void OnAddressChanged(IWebBrowser browserControl, AddressChangedEventArgs addressChangedArgs)
        {
            
        }

        public void OnTitleChanged(IWebBrowser browserControl, TitleChangedEventArgs titleChangedArgs)
        {
           
        }
        public static System.Drawing.Color getDominantColor(Bitmap bmp)
        {
            int r = 0;
            int g = 0;
            int b = 0;

            int total = 0;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    System.Drawing.Color clr = bmp.GetPixel(x, y);

                    r += clr.R;
                    g += clr.G;
                    b += clr.B;

                    total++;
                }
            }

            r /= total;
            g /= total;
            b /= total;

            return System.Drawing.Color.FromArgb(r, g, b);
        }
        public void OnFaviconUrlChange(IWebBrowser browserControl, IBrowser browser, IList<string> urls)
        {
            Dispatcher.BeginInvoke((Action)(async () =>
            {
                var bitmap2 = new BitmapImage();
                try
                {
                    bitmap2.BeginInit();
                    bitmap2.UriSource = new Uri(urls[0], UriKind.Absolute);
                    HttpWebRequest request =
                    (HttpWebRequest)HttpWebRequest.Create(urls[0]);
                    System.Net.WebResponse response = await request.GetResponseAsync();
                    System.IO.Stream responseStream =
                        response.GetResponseStream();
                    Bitmap bmp = new Bitmap(responseStream);
                    System.Drawing.Color cc = getDominantColor(bmp);
                    System.Drawing.Color c2 = System.Drawing.Color.FromArgb(cc.A, Convert.ToInt32(cc.R / 1), Convert.ToInt32(cc.G / 1), Convert.ToInt32(cc.B / 1));
                    _color = c2;
                    
                    bitmap2.EndInit();

                    System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(c2.A, c2.R, c2.G, c2.B);
                    SolidColorBrush brush = new SolidColorBrush(newColor);
                    mainWindow.TabBar.getTabFromForm(this).Color = brush;
                    mainWindow.TabBar.getTabFromForm(this).refreshColor();

                    textBox.Background = brush;
                    Panel.Background = brush;
                    ListContainer.Background = brush;
                    listBox.Background = brush;
                    mainWindow.TabBar.getTabFromForm(this).SetIcon(bitmap2);

                    ContrastColor(c2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Favicon: " + ex.Message);
                }
            }));
        }
        void ContrastColor(System.Drawing.Color color)
        {
            int d = 0;

             
            double a = 1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

            if (a < 0.5)
            {
                if (!mainWindow.TabBar.getTabFromForm(this).bgTab) 
                mainWindow.TabBar.getTabFromForm(this).label_TabTitle.Foreground = System.Windows.Media.Brushes.Black;
                textBox.Foreground = System.Windows.Media.Brushes.Black;
                mainWindow.TabBar.getTabFromForm(this).actualForeground = System.Windows.Media.Brushes.Black;
                blackButtons();
                SolidColorBrush scb = new SolidColorBrush();
                scb.Color = System.Windows.Media.Color.FromArgb(50, 255, 255, 255);
                textBox.Background = scb;
                listBox.Foreground = System.Windows.Media.Brushes.Black;
                mainWindow.TabBar.getTabFromForm(this).darkColor = false;
            }
            else {
                if (!mainWindow.TabBar.getTabFromForm(this).bgTab)
                    mainWindow.TabBar.getTabFromForm(this).label_TabTitle.Foreground = System.Windows.Media.Brushes.White;
                textBox.Foreground = System.Windows.Media.Brushes.White;
                whiteButtons();
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
            fullscreen = true;
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
                    try {
                        string[] split = listBox.SelectedItem.ToString().Split(splitChar);
                        WebView.Load(split[0]);
                        HideSuggestions();
                    } catch
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
                        string[] split1 = listBox.SelectedItem.ToString().Split(new string[] {" - "}, StringSplitOptions.None);
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
                mainWindow.menuToggled = true;
        }


        public void OnBeforeDownload(IBrowser browser, CefSharp.DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                mainWindow.Downloads1.AddDownload(downloadItem.Url, Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), downloadItem.SuggestedFileName);
                mainWindow.Downloads1.Visibility = Visibility.Visible;
            }));
        }

        public void OnDownloadUpdated(IBrowser browser, CefSharp.DownloadItem downloadItem, IDownloadItemCallback callback)
        {

        }

        public void LoadUrl(string url)
        {
            WebView.Load(url);
        }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IWindowInfo windowInfo, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;
            Dispatcher.BeginInvoke((Action)(() => {
                

                TabView tv = new TabView(mainWindow, targetUrl);
                mainWindow.TabBar.AddTab("New tab", mainWindow, tv, new BrushConverter().ConvertFromString("#FFF9F9F9") as SolidColorBrush);
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
                addbook.Margin = new Thickness(0, -35, 21, 0);
                addbook.Width = 300;
                addbook.Height = 175;
                container.Children.Add(addbook);
            
        }



        private void textBox_GotMouseCapture(object sender, MouseEventArgs e)
        {
            textBox.SelectAll();
        }

    }
}
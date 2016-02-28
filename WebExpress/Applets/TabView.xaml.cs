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
using System.Drawing;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;
using CefSharp.Wpf;
using Newtonsoft.Json;
using WebExpress.Classes;
using WebExpress.Controls;
using ContextMenu = System.Windows.Controls.ContextMenu;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using MenuItem = System.Windows.Controls.MenuItem;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

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

        private string StopButton;

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
            SnackBar.Visibility = Visibility.Hidden;
            allItems1 = new List<string>();
            WebView.MenuHandler = this;

            //Events

            WebView.FrameLoadEnd += WebView_FrameLoadEnd;
            WebView.TitleChanged += WebView_TitleChanged;
            Loaded += TabView_Loaded;
            WebView.IsBrowserInitializedChanged += WebView_IsBrowserInitializedChanged;
            WebView.FrameLoadStart += WebView_FrameLoadStart;

            //Method calls

            HideSuggestions();
            mw.Pages.Add(this);

            urlToLoad = url;
        }
        private async void WebView_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            await Dispatcher.BeginInvoke((Action) (() =>
            {
                try
                {
                    RefreshButton.ImageSource(StopButton);
                    refreshing = true;
                    startPage.Visibility = Visibility.Hidden;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("WebView framestart error: " + ex.Message + " " + ex.Data);
                }
            }));
        }


        private async Task WhiteButtons()
        {

            MenuButton.ImageSource("menu_white.png");
            MenuButton.RippleColor(Colors.White);
            Back.ImageSource("back_white.png");
            Back.RippleColor(Colors.White);
            Forward.ImageSource("forward_white.png");
            Forward.RippleColor(Colors.White);
            RefreshButton.ImageSource("reload_white.png");
            RefreshButton.RippleColor(Colors.White);
            BookmarkButton.ImageSource("bookmark_white.png");
            BookmarkButton.RippleColor(Colors.White);
            StopButton = "stop_white.png";
            mainWindow.TabBar.getTabFromForm(this).CloseTab.ImageSource("close_Tab_white.png");
        }


        private async Task BlackButtons()
        {
            MenuButton.ImageSource("menu.png");
            MenuButton.RippleColor(Colors.Black);
            Back.ImageSource("back.png");
            Back.RippleColor(Colors.Black);
            Forward.ImageSource("forward.png");
            Forward.RippleColor(Colors.Black);
            RefreshButton.ImageSource("reload.png");
            RefreshButton.RippleColor(Colors.Black);
            BookmarkButton.ImageSource("bookmark.png");
            BookmarkButton.RippleColor(Colors.Black);
            StopButton = "stop.png";
            mainWindow.TabBar.getTabFromForm(this).CloseTab.ImageSource("close_Tab.png");
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

        private async void TabView_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Factory.StartNew(LoadSuggestions);
            await BlackButtons();
            await startPage.LoadFavs(mainWindow);
            textBox.Focus();
        }

        private async Task LoadSuggestions()
        {
            await Dispatcher.BeginInvoke((Action) (() =>
            {
                try
                {

                    MagicBox.Clear();
                    allItems1.Clear();

                    if (System.IO.File.Exists(StaticDeclarations.Historypath))
                    {
                        string readFile = System.IO.File.ReadAllText(StaticDeclarations.Historypath);
                        dynamic json = JsonConvert.DeserializeObject(readFile);
                        foreach (dynamic item in json)
                        {
                                MagicBox.AddSuggestion(Convert.ToString(item.Title), Convert.ToString(item.Url), mainWindow);
                                allItems1.Add(Convert.ToString(item.Url) + "*" + Convert.ToString(item.Title));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Load suggestions error: " + ex.Message + " " + ex.Data);
                }
            }));
        }

        public async Task WriteHistory()
        {
           await Dispatcher.BeginInvoke((Action) (() =>
            {
                if (!StaticDeclarations.IsIncognito)
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
                        Console.WriteLine("Write history error: " + ex.Message + " " + ex.Data);
                    }
                    try
                    {
                        foreach (TabView page in mainWindow.Pages)
                        {

                            Task.Factory.StartNew(page.LoadSuggestions);

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Load suggestions in each TabView error: " + ex.Message + " " + ex.Data);
                    }
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
                    try
                    {
                        Task.Factory.StartNew(() => SetAddress(e.Url));
                        refreshing = false;

                        if (Directory.Exists("Extensions"))
                        {
                            foreach (string file in System.IO.Directory.GetFiles("Extensions", "*.js"))
                            {
                                WebView.ExecuteScriptAsync(System.IO.File.ReadAllText(file));
                            }
                        }
                        HideSuggestions();
                        Task.Factory.StartNew(WriteHistory);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("WebView loadend error: " + ex.Message + " " + ex.Data);
                    }
                }

                Task.Factory.StartNew(ChangeColor);

            });
        }

        private void SetAddress(string text) {Dispatcher.BeginInvoke((Action) (() => { textBox.Text = text; }));}
        public void Shutdown() {WebView.Dispose();}
        public void HideSuggestions() {ListContainer.Visibility = Visibility.Hidden;}
        public void ShowSuggestions()
        {Panel.Effect = null; ListContainer.Visibility = Visibility.Visible; }

        private async void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(StaticDeclarations.Historypath) && textBox.Text != null)
                {
                    ShowSuggestions();

                    if (string.IsNullOrEmpty(textBox.Text.Trim()) == false)
                    {
                        MagicBox.Clear();
                        foreach (string str in allItems1)
                        {
                            if (str.Contains(textBox.Text.Trim()))
                            {
                                string[] split = str.Split(splitChar);
                               await MagicBox.AddSuggestion(split[1], split[0], mainWindow);
                            }
                        }
                    }

                    else if (textBox.Text.Trim() == "")
                    {
                        MagicBox.Clear();

                        foreach (string str in allItems1)
                        {
                            string[] split = str.Split(splitChar);
                            await MagicBox.AddSuggestion(split[1], split[0], mainWindow);
                        }
                    }

                    StaticFunctions.AnimateHeight(ListContainer.Height, MagicBox.ItemsCount * MagicBox.ItemHeight, ListContainer, 0.2);
                }
                else
                {
                    ListContainer.Height = 0;
                    HideSuggestions();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Listbox find suggestions error: " + ex.Message + " " + ex.Data);
            }

            if (e.Key == Key.Down)
            {
                MagicBox.Focus();
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
                        if (Convert.ToString(dyn.SE) == "DuckDuckGo")
                        {
                            textBox.Text = textBox.Text.Replace(textBox.Text, "https://duckduckgo.com/?q=" + textBox.Text);
                            WebView.Load(textBox.Text);
                        }
                        if (Convert.ToString(dyn.SE) == "Bing")
                        {
                            textBox.Text = textBox.Text.Replace(textBox.Text, "http://www.bing.com/search?q=" + textBox.Text);
                            WebView.Load(textBox.Text);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("textBox KeyDown Key.Enter error: " + ex.Message + " " + ex.Data);
                    }
                }
                HideSuggestions();
            }

        }

        private void Back_Click(object sender, MouseButtonEventArgs e)
        {
            WebView.Back();
        }

        private void Forward_Click(object sender, MouseButtonEventArgs e)
        {
            WebView.Forward();
        }

        private void Refresh_Click(object sender, MouseButtonEventArgs e)
        {
            if (refreshing)
                WebView.Stop();
            else
                WebView.Reload();
        }

        private void WebView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            HideSuggestions();
        }

        public void OnAddressChanged(IWebBrowser browserControl, AddressChangedEventArgs addressChangedArgs)
        {
            RefreshColor();
            SetAddress(addressChangedArgs.Address);
        }

        public void RefreshColor()
        {
            Thread thread = new Thread(new ThreadStart(async delegate()
            {
                Thread.Sleep(200);
                try
                {
                    await this.Dispatcher.BeginInvoke(DispatcherPriority.Send,
                        new ThreadStart(async delegate()
                        {
                            await ChangeColor();
                            this.Refresh();
                        }));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Address Changed error: " + ex.Message + " " + ex.Data);
                }
            })) {Name = "thread-UpdateText"};
            thread.Start();
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
                    Console.WriteLine("Favicon load error: " + ex.Message + " " + ex.Data);
                }
            }));
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

        private void MenuButton_Click(object sender, MouseButtonEventArgs e)
        {
            mainWindow.Menu.Visibility = Visibility.Visible;
            StaticFunctions.AnimateScale(0,0, 225, 400, mainWindow.Menu, 0.2);
            StaticFunctions.AnimateFade(0, 1, mainWindow.Menu, 0.3);
        }

        public void OnBeforeDownload(IBrowser browser, CefSharp.DownloadItem downloadItem,
            IBeforeDownloadCallback callback)
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                mainWindow.Downloads1.AddDownload(downloadItem.Url,
                    Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    downloadItem.SuggestedFileName);
                if (mainWindow.Downloads1.ActualHeight.Equals(0))
                {
                    double height = 0;
                    if (!mainWindow.Downloads1.ItemsCount.Equals(0))
                    {
                        height = mainWindow.Downloads1.MarginTop + 50 +
                                 mainWindow.Downloads1.Items.Count*mainWindow.Downloads1.ItemHeight;
                    }
                    else
                    {
                        height = mainWindow.Downloads1.MarginTop + 50;
                    }
                    mainWindow.Downloads1.Visibility = Visibility.Visible;
                    StaticFunctions.AnimateScale(0, 0, 250, height, mainWindow.Downloads1, 0.2);
                    StaticFunctions.AnimateFade(0, 1, mainWindow.Downloads1, 0.2, mainWindow.Downloads1.RefreshDownloads);
                }
            }));
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
                            Console.WriteLine("Open popup in new tab error: " + ex.Message + " " + ex.Data);
                        }
                    }));

            return true;
        }
        public void OnTitleChanged(IWebBrowser browserControl, TitleChangedEventArgs titleChangedArgs) { }
        public bool OnTooltipChanged(IWebBrowser browserControl, string text) { return true; }
        public void OnStatusMessage(IWebBrowser browserControl, StatusMessageEventArgs statusMessageArgs){ }
        public bool OnConsoleMessage(IWebBrowser browserControl, ConsoleMessageEventArgs consoleMessageArgs) { return true; }
        public void OnDownloadUpdated(IBrowser browser, CefSharp.DownloadItem downloadItem, IDownloadItemCallback callback){ }
        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser) { }
        public bool DoClose(IWebBrowser browserControl, IBrowser browser){return false;}
        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser){ }
        private void textBox_GotMouseCapture(object sender, MouseEventArgs e) { textBox.SelectAll(); }

        private void BookmarkButton_Click(object sender, MouseButtonEventArgs e)
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
       
        public async Task ContrastColor(System.Drawing.Color color)
        {
            int d = 0;


            double a = 1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

            if (a < 0.4)
            {
                if (!mainWindow.TabBar.getTabFromForm(this).bgTab)
                    mainWindow.TabBar.getTabFromForm(this).label_TabTitle.Foreground =
                        System.Windows.Media.Brushes.Black;
                textBox.Foreground = System.Windows.Media.Brushes.Black;
                mainWindow.TabBar.getTabFromForm(this).actualForeground = System.Windows.Media.Brushes.Black;
                await BlackButtons();
                SolidColorBrush scb = new SolidColorBrush();
                scb.Color = System.Windows.Media.Color.FromArgb(255, 255, 255, 255);
                textBox.Background = scb;
                mainWindow.TabBar.getTabFromForm(this).CloseTab.ImageSource("close_Tab.png");
                mainWindow.TabBar.getTabFromForm(this).darkColor = false;
                MagicBox._isDark = false;
            }
            else
            {
                if (!mainWindow.TabBar.getTabFromForm(this).bgTab)
                    mainWindow.TabBar.getTabFromForm(this).label_TabTitle.Foreground =
                        System.Windows.Media.Brushes.White;
                textBox.Foreground = System.Windows.Media.Brushes.White;
                await WhiteButtons();
                SolidColorBrush scb = new SolidColorBrush();
                scb.Color = System.Windows.Media.Color.FromArgb(50, 255, 255, 255);
                textBox.Background = scb;
                mainWindow.TabBar.getTabFromForm(this).CloseTab.ImageSource("close_Tab_white.png");
                mainWindow.TabBar.getTabFromForm(this).actualForeground = System.Windows.Media.Brushes.White;
                mainWindow.TabBar.getTabFromForm(this).darkColor = true;
                MagicBox._isDark = true;
            }
        }
      
        public async Task ChangeColor()
        {
            try
            {
                await Dispatcher.BeginInvoke((Action) (() =>
                {
                    Thread thread = new Thread(new ThreadStart(delegate ()
                    {
                        System.Threading.Thread.Sleep(200); 
                            this.Dispatcher.BeginInvoke(DispatcherPriority.Send,
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
                                    SolidColorBrush oldBrush = (SolidColorBrush)Panel.Background;
                                    SolidColorBrush brush = new SolidColorBrush(StaticFunctions.ToMediaColor(screenShot.GetPixel(1, 1)));
                                    StaticFunctions.AnimateColor(oldBrush, StaticFunctions.ToMediaColor(screenShot.GetPixel(1, 1)), Panel, 0.1);
                                    mainWindow.TabBar.getTabFromForm(this).Color = brush;
                                    mainWindow.TabBar.getTabFromForm(this).refreshColor();

                                    StaticFunctions.AnimateColor(oldBrush, StaticFunctions.ToMediaColor(screenShot.GetPixel(1, 1)), textBox, 0.1);
                                    StaticFunctions.AnimateColor(oldBrush, StaticFunctions.ToMediaColor(screenShot.GetPixel(1, 1)), ListContainer, 0.1);
                                    StaticFunctions.AnimateColor(oldBrush, StaticFunctions.ToMediaColor(screenShot.GetPixel(1, 1)), MagicBox, 0.1);


                                   await ContrastColor(screenShot.GetPixel(1, 1));
                                    this.Refresh();
                                }));
                    }));
                    thread.Name = "thread-UpdateText";
                    thread.Start();
                    

                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Change color error: " + ex.Message + " " + ex.Data);
            }
        }

        private string url;
        private string img;
        void IContextMenuHandler.OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            Console.WriteLine("Context menu opened");
            Console.WriteLine(parameters.MisspelledWord);

            if (model.Count > 0)
            {
                model.AddSeparator();
            }
            url = parameters.LinkUrl;
           
            model.Clear();
            if (!url.Equals(""))
            model.AddItem((CefMenuCommand)26501, "Open link in new tab");
            
            if (parameters.MediaType == ContextMenuMediaType.Image)
            {
                img = parameters.SourceUrl;
                model.AddItem((CefMenuCommand) 26502, "Save image");
            }

        }

        bool IContextMenuHandler.OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            if (commandId == (CefMenuCommand)26501)
            {
                if (!url.Equals(""))
                {
                    Dispatcher.Invoke(() =>
                    {
                        Console.WriteLine(parameters.SelectionText);
                        ApplicationCommands.New.Execute(
                            new OpenTabCommandParameters(url, "New tab", "#FFF9F9F9"), this);

                    });
                    return true;
                }
            }
            if (commandId == (CefMenuCommand) 26502)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "image.png";
                dialog.Filter = "Png image (*.png)|*.png|Gif Image (*.gif)|*.gif|JPEG image (*.jpg)|*.jpg";

                var result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Console.WriteLine("writing to: " + dialog.FileName);

                    var wClient = new WebClient();
                    wClient.DownloadFile(img, dialog.FileName);
                }
            }

            return false;
        }

        void IContextMenuHandler.OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

            chromiumWebBrowser.Dispatcher.Invoke(() =>
            {
                chromiumWebBrowser.ContextMenu = null;
            });
        }

        bool IContextMenuHandler.RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

            //IMenuModel is only valid in the context of this method, so need to read the values before invoking on the UI thread
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

                    //If the callback has been disposed then it's already been executed
                    //so don't call Cancel
                    if (!callback.IsDisposed)
                    {
                        callback.Cancel();
                    }
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

        private static IEnumerable<Tuple<string, CefMenuCommand>> GetMenuItems(IMenuModel model)
        {
            var list = new List<Tuple<string, CefMenuCommand>>();
            for (var i = 0; i < model.Count; i++)
            {
                var header = model.GetLabelAt(i);
                var commandId = model.GetCommandIdAt(i);
                list.Add(new Tuple<string, CefMenuCommand>(header, commandId));
            }

            return list;
        }

       
        private void RefreshButton_OnPreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            SnackBar.Text("Easter egg :)");
            SnackBar.Visibility = Visibility.Visible;
            StaticFunctions.AnimateHeight(0, 42, SnackBar, 0.1);
        }

        private void Panel_MouseEnter(object sender, MouseEventArgs e)
        {
            if (StaticDeclarations.IsFullscreen)
            {
                StaticFunctions.AnimateHeight(4, 42, Panel, 0.2);
                
            }
        }

        private void Panel_MouseLeave(object sender, MouseEventArgs e)
        {
            if (StaticDeclarations.IsFullscreen)
            {
                StaticFunctions.AnimateHeight(42, 3, Panel, 0.2);
                StaticFunctions.AnimateHeight(ListContainer.Height, 0, ListContainer, 0.2);
            }
        }
    }

}
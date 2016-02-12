using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CefSharp;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using UserControl = System.Windows.Controls.UserControl;
using System.Windows.Media.Animation;
using System.Drawing;
using System.Windows.Media;

namespace WebExpress
{
    /// <summary>
    ///     Interaction logic for TabView.xaml
    /// </summary>
    public partial class TabView : UserControl, IFocusHandler, IDisplayHandler
    {
        private bool menuToggled;
        private readonly MainWindow mainWindow;
        private readonly string splitChar;
        private System.Drawing.Color _color;

        private List<string> allItems1;

        public string Bookmarkslayoutpath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data\\bookmarks-layout.html");

        public string Bookmarkspath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data\\bookmarks-data.html");

        public string Bookspath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WebExpress\\user data\\bookmarks.txt");

        public string Historylayoutpath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data\\history-layout.html");

        public string Historypath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data\\history-data.html");

        private string LastWebsite;

        public string Suggestionspath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data\\suggestions.txt");

        private string Title;

        public string Userdatapath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data");

        public string Webexpresspath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WebExpress");

        public TabView(MainWindow mw)
        {
            InitializeComponent();
            WebView.FrameLoadEnd += WebView_FrameLoadEnd;
            WebView.TitleChanged += WebView_TitleChanged;
            mainWindow = mw;
            Menu.mainWindow = mw;
            menuToggled = false;
            Menu.Visibility = Visibility.Hidden;
            allItems1 = new List<string>();
            Loaded += TabView_Loaded;
            LastWebsite = "";
            splitChar = " - ";
            WebView.DisplayHandler = this;
            WebView.IsBrowserInitializedChanged += WebView_IsBrowserInitializedChanged;
            ListContainer.Visibility = Visibility.Hidden;
            if (!Directory.Exists(Webexpresspath))
            {
                Directory.CreateDirectory(Webexpresspath);
                Directory.CreateDirectory(Userdatapath);
            }
            if (!Directory.Exists(Userdatapath))
            {
                Directory.CreateDirectory(Userdatapath);
            }
            
        }

        private void WebView_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(!WebView.IsInitialized)
            {
                Cef.Initialize();
            }
        }

        private void TabView_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(LoadSuggestions);
        }

        public void OnGotFocus()
        {
            ListContainer.Visibility = Visibility.Hidden;
        }

        public bool OnSetFocus(CefFocusSource source)
        {
            ListContainer.Visibility = Visibility.Hidden;
            return false;
        }

        public void OnTakeFocus(bool next)
        {
            ListContainer.Visibility = Visibility.Hidden;
        }

        private void LoadSuggestions()
        {
            Dispatcher.Invoke(() =>
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
                            string[] split = Regex.Split(s, splitChar);
                            listBox.Items.Add(split[0] + splitChar + split[1]);
                            allItems1.Add(split[0] + splitChar + split[1]);
                        }
                        else
                        {
                            string[] split = Regex.Split(s, "#q=");
                            string[] split1 = Regex.Split(split[1], splitChar);
                            allItems1.Add(split1[0] + splitChar + split1[1]);
                            listBox.Items.Add(split1[0] + splitChar + split1[1]);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Load suggestions error: " + ex.Message);
                }
            });
        }


        public void WriteHistory()
        {
            Dispatcher.Invoke(() =>
            {
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
                if (!LastWebsite.Equals(WebView.Address))
                    {
                        
                    using (var sw = new StreamWriter(Historypath, true))
                    {
                        sw.WriteLine(
                            "<style>p { font-family: 'Arial'; } a {color: #606060; text-decoration: none;} a:hover { color: #000; } a:visited { color:#606060; } </style><p><img src='http://www.google.com/s2/favicons?domain=" +
                            WebView.Address + "' style='width: 16; height:16;'/>" + " " + Title +
                            "  <a target='_blank' href=" + WebView.Address + ">  -  " + WebView.Address + "</a></p>");
                        sw.Close();
                    }
                    var filePath1 = Suggestionspath;
                        using (var sw = new StreamWriter(filePath1, true))
                        {
                            sw.WriteLine(WebView.Address + splitChar + Title);
                            sw.Close();
                        }
                        LastWebsite = WebView.Address;
                    }
                

            });

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
                   
                }
                Task.Factory.StartNew(WriteHistory);
                Panel.Height = 28;
                WebView.Margin = new Thickness(0, 28, 0, 0);
            });
        }

        private void SetAddress(string text)
        {
            Dispatcher.Invoke(() => { textBox.Text = text; });
        }

        public void Shutdown()
        {
            WebView.Dispose();
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {


            try
            {
                ListContainer.Visibility = Visibility.Visible;
                listBox.Items.Clear();
                List<string> allitems = allItems1.Distinct().ToList();
                for (int i = 0; i <= allitems.Count - 1; i++)
                {
                    if (allitems[i].Contains(textBox.Text))
                    {
                        listBox.Items.Add(allitems[i]);
                        ListContainer.Height = listBox.Items.Count*34;
                    }
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
                    textBox.Text = textBox.Text.Replace(textBox.Text, "http://google.com/#q=" + textBox.Text);
                    WebView.Load(textBox.Text);
                }
                ListContainer.Visibility = Visibility.Hidden;
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

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void listBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!listBox.SelectedItem.ToString().Equals(null))
                {
                    string[] split = listBox.SelectedItem.ToString().Split(new string[] {splitChar}, StringSplitOptions.None);
                    WebView.Load(split[0]);
                    ListContainer.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Listbox mouse click error: " + ex.Message);
            }
        }

        private void WebView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ListContainer.Visibility = Visibility.Hidden;
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

        public void OnAddressChanged(IWebBrowser browserControl, AddressChangedEventArgs addressChangedArgs)
        {
            
        }

        public void OnTitleChanged(IWebBrowser browserControl, TitleChangedEventArgs titleChangedArgs)
        {
           
        }

        public void OnFaviconUrlChange(IWebBrowser browserControl, IBrowser browser, IList<string> urls)
        {
            Dispatcher.Invoke(() =>
            {
                try
                {
                    var bitmap2 = new BitmapImage();

                    bitmap2.BeginInit();
                    bitmap2.UriSource = new Uri(urls[0], UriKind.Absolute);
                    bitmap2.EndInit();
                    Bitmap bmp = BitmapImage2Bitmap(bitmap2);
                    System.Drawing.Color color = bmp.GetPixel(10, 10);
                    _color = color;
                    System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                    SolidColorBrush brush = new SolidColorBrush(newColor);
                    mainWindow.TabBar.getTabFromForm(this).Color = brush;
                    mainWindow.TabBar.getTabFromForm(this).refreshColor();
                    
                    textBox.Background = brush;
                    Panel.Background = brush;
                    
                    mainWindow.TabBar.getTabFromForm(this).SetIcon(bitmap2);
                  
                    ContrastColor(color);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Get favicon error: " + ex.Message);
                }
            });
            Console.WriteLine("changed");
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
            }
            else {
                if (!mainWindow.TabBar.getTabFromForm(this).bgTab)
                    mainWindow.TabBar.getTabFromForm(this).label_TabTitle.Foreground = System.Windows.Media.Brushes.White;
                textBox.Foreground = System.Windows.Media.Brushes.White;
                mainWindow.TabBar.getTabFromForm(this).actualForeground = System.Windows.Media.Brushes.White;
            }
         
        }
        private System.Drawing.Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new System.Drawing.Bitmap(bitmap);
            }
        }
        public void OnFullscreenModeChange(IWebBrowser browserControl, IBrowser browser, bool fullscreen)
        {
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
                    ListContainer.Visibility = Visibility.Visible;
                    listBox.Focus();
                    listBox.SelectedIndex = 0;
                    break;

                case Key.Enter:
                    try {
                        string[] split = listBox.SelectedItem.ToString().Split(new string[] { splitChar }, StringSplitOptions.None);
                        WebView.Load(split[0]);
                        ListContainer.Visibility = Visibility.Hidden;
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
                    string[] split = listBox.SelectedItem.ToString().Split(new string[] { splitChar }, StringSplitOptions.None);
                    WebView.Load(split[0]);
                    ListContainer.Visibility = Visibility.Hidden;     
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

                Menu.Height = 0;
                Menu.Visibility = Visibility.Visible;
                Storyboard sb = this.FindResource("sb2") as Storyboard;
                Storyboard.SetTarget(sb, this.Menu);
                sb.Begin();
                menuToggled = true;

            
        }


        private void ListContainer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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

    }
}
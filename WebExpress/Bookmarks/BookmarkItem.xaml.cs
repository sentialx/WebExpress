using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WebExpress.Controls;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;

namespace WebExpress.Bookmarks
{
    /// <summary>
    ///     Interaction logic for BookmarkItem.xaml
    /// </summary>
    public partial class BookmarkItem : UserControl
    {
        private readonly TabView _tv;
        private readonly string _url;
        private readonly Bookmarks bookmarks;
        private readonly MainWindow mainWindow;

        public BookmarkItem(string url, string title, TabView tv, MainWindow mw, Bookmarks books)
        {
            InitializeComponent();
            _url = url;
            _tv = tv;
            label.Content = title;
            Loaded += BookmarkItem_Loaded;
            mainWindow = mw;
            bookmarks = books;
        }

        private async void BookmarkItem_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(LoadFavs);
            button.Visibility = Visibility.Hidden;
        }

        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                var bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        private Task LoadFavs()
        {
            return Task.Factory.StartNew(() =>
            {
                Dispatcher.BeginInvoke((Action) (async () =>
                {
                    var request =
                        (HttpWebRequest) WebRequest.Create("http://www.google.com/s2/favicons?domain=" + _url);
                    var response = await request.GetResponseAsync();
                    var responseStream = response.GetResponseStream();
                    var bmp = new Bitmap(responseStream);
                    var brush = new SolidColorBrush(StaticFunctions.ToMediaColor(bmp.GetPixel(11, 11)));
                    Grid.Background = brush;
                    image.Source = BitmapToImageSource(bmp);
                    var foreColor = PerceivedBrightness(StaticFunctions.ToMediaColor(bmp.GetPixel(11, 11))) > 130
                        ? Brushes.Black
                        : Brushes.White;
                    label.Foreground = foreColor;
                }));
            });
        }

        private int PerceivedBrightness(Color c)
        {
            return (int) Math.Sqrt(
                c.R*c.R*.241 +
                c.G*c.G*.691 +
                c.B*c.B*.068);
        }

        private void ContrastColor(System.Drawing.Color color)
        {
            var d = 0;


            var a = 1 - (0.299*color.R + 0.587*color.G + 0.114*color.B)/255;

            if (a < 0.5)
            {
                label.Foreground = Brushes.Black;
            }
            else
            {
                label.Foreground = Brushes.White;
            }
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _tv.WebView.Load(_url);
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void RemoveLines(string fileName, string[] linesToRemove)
        {
            var lines = File.ReadAllLines(fileName);
            using (var sw = new StreamWriter(fileName))
            {
                foreach (var line in lines)
                {
                    if (Array.IndexOf(linesToRemove, line) == -1)
                    {
                        sw.WriteLine(line);
                    }
                }
            }
            foreach (var page in mainWindow.Pages)
            {
                try
                {
                    Task.Factory.StartNew(() => page.startPage.refreshFavs(mainWindow));
                }
                catch
                {
                }
            }
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            button.Visibility = Visibility.Visible;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            button.Visibility = Visibility.Hidden;
        }

        public void loadFavs(MainWindow mw)
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                bookmarks.ItemsCount = 0;
                bookmarks.RowsCount = 0;
                bookmarks.mainCanvas.Children.Clear();

                try
                {
                    var tabView = bookmarks.FindParent<TabView>();
                    foreach (var s in File.ReadAllLines(StaticDeclarations.Bookspath))
                    {
                        var split = s.Split((char) 42);
                        bookmarks.AddBookmark(split[0], split[1], tabView, mw);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("LoadFavs error: " + ex.Message);
                }
            }));
        }

        private void close_click(object sender, RoutedEventArgs e)
        {
            var readText = File.ReadAllLines(StaticDeclarations.Bookspath);
            var arr = new ArrayList();
            foreach (var sr in readText)
            {
                arr.Add(sr);
            }
            foreach (string line in arr)
            {
                if (line.Contains(_url))
                {
                    string[] lines = {line};
                    RemoveLines(StaticDeclarations.Bookspath, lines);
                }
            }

            var readText1 = File.ReadAllLines(StaticDeclarations.Bookmarkspath);
            var arr2 = new ArrayList();
            foreach (var sr in readText1)
            {
                arr.Add(sr);
            }
            foreach (string line in arr)
            {
                if (line.Contains(_url))
                {
                    string[] lines = {line};
                    RemoveLines(StaticDeclarations.Bookmarkspath, lines);
                }
            }
        }

        private void button_MouseEnter(object sender, MouseEventArgs e)
        {
            StaticFunctions.ChangeButtonImage("close_button_hover.png", CloseImage);
        }

        private void button_MouseLeave(object sender, MouseEventArgs e)
        {
            StaticFunctions.ChangeButtonImage("close_button.png", CloseImage);
        }
    }
}
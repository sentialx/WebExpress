using System;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;

namespace WebExpress.Bookmarks
{
    /// <summary>
    ///     Interaction logic for BookmarkItem.xaml
    /// </summary>
    public partial class BookmarkPanelItem : UserControl
    {
        private readonly TabView _tv;
        private readonly string _url;
        private readonly BookmarksPanel bookmarks;
        private readonly MainWindow mainWindow;

        public BookmarkPanelItem(string url, string title, TabView tv, MainWindow mw, BookmarksPanel books)
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
                    try
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
                    }
                    catch
                    {
                        
                    }
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


        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            button.Visibility = Visibility.Visible;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            button.Visibility = Visibility.Hidden;
        }


        private void close_click(object sender, RoutedEventArgs e)
        {

            string readFile = System.IO.File.ReadAllText(StaticDeclarations.Bookmarkspath);
            dynamic json = JsonConvert.DeserializeObject(readFile);
            try
            {
                foreach (dynamic item in json)
                {
                    if (_url.Equals(Convert.ToString(item.Url)))
                    {
                        JArray json2 = (json as JArray);
                        json2.Remove(item);
                        System.IO.File.WriteAllText(StaticDeclarations.Bookmarkspath, json2.ToString());
                        if (System.IO.File.Exists(StaticDeclarations.Bookmarkspath))
                        {
                            foreach (var page in mainWindow.Pages)
                            {
                                try
                                {
                                    Task.Factory.StartNew(() => page.startPage.RefreshFavs(mainWindow));
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
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
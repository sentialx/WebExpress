using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WebExpress.Bookmarks
{
    /// <summary>
    /// Interaction logic for BookmarkItem.xaml
    /// </summary>
    public partial class BookmarkItem : UserControl
    {
        private string _url;
        private TabView _tv;

        public BookmarkItem(string url, string title, TabView tv)
        {
            InitializeComponent();
            _url = url;
            _tv = tv;
            label.Content = title;
            Loaded += BookmarkItem_Loaded;

        }
        public static System.Windows.Media.Color ToMediaColor(System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        private  void BookmarkItem_Loaded(object sender, RoutedEventArgs e)
        {
           Task.Factory.StartNew(DoInBackground);
        }

        private void DoInBackground()
        {
            Dispatcher.Invoke(async () =>
            {
                HttpWebRequest request =
    (HttpWebRequest)HttpWebRequest.Create("http://www.google.com/s2/favicons?domain=" + _url);
                System.Net.WebResponse response = await request.GetResponseAsync();
                System.IO.Stream responseStream = response.GetResponseStream();
                Bitmap bmp = new Bitmap(responseStream);
                SolidColorBrush brush = new SolidColorBrush(ToMediaColor(bmp.GetPixel(11, 11)));
                Grid.Background = brush;
                var imgUrl = new Uri("http://www.google.com/s2/favicons?domain=" + _url);
                var bitmap2 = new BitmapImage();
                bitmap2.BeginInit();
                bitmap2.UriSource = imgUrl;
                bitmap2.EndInit();
                image.Source = bitmap2;
                var foreColor = (PerceivedBrightness(ToMediaColor(bmp.GetPixel(11, 11))) > 130 ? System.Windows.Media.Brushes.Black : System.Windows.Media.Brushes.White);
                label.Foreground = foreColor;
            });
       
        }

        private int PerceivedBrightness(System.Windows.Media.Color c)
        {
            return (int)Math.Sqrt(
            c.R * c.R * .241 +
            c.G * c.G * .691 +
            c.B * c.B * .068);
        }
        void ContrastColor(System.Drawing.Color color)
        {
            int d = 0;


            double a = 1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

            if (a < 0.5)
            {
                label.Foreground = System.Windows.Media.Brushes.Black;
            }
            else {
                label.Foreground = System.Windows.Media.Brushes.White;
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
    }
}

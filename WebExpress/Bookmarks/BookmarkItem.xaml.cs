using System;
using System.Collections;
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
        private MainWindow mainWindow;
        private TabView _tv;
        private Bookmarks bookmarks;
        public string Bookmarkspath =
    System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "WebExpress\\user data\\bookmarks-data.html");

        public string Bookspath = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WebExpress\\user data\\bookmarks.txt");

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
        public static System.Windows.Media.Color ToMediaColor(System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        private  void BookmarkItem_Loaded(object sender, RoutedEventArgs e)
        {
           Task.Factory.StartNew(DoInBackground);
            button.Visibility = Visibility.Hidden;
        }

        private void DoInBackground()
        {
            Dispatcher.BeginInvoke((Action) (async () =>
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
            }));
       
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

        private void RemoveLines(string fileName, string[] linesToRemove)
        {
            string[] lines = System.IO.File.ReadAllLines(fileName);
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName))
            {
                foreach (string line in lines)
                {
                    if (Array.IndexOf(linesToRemove, line) == -1)
                    {
                        sw.WriteLine(line);
                    }
                }
            }
            loadFavs(mainWindow);
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
                    string[] readText = System.IO.File.ReadAllLines(Bookspath);
                    ArrayList arr = new ArrayList();
                    foreach (var sr in readText)
                    {
                        arr.Add(sr);
                    }
                    
                    Grid parent = bookmarks.Parent as Grid;
                    Grid parent2 = parent.Parent as Grid;
                    StartPage parent3 = parent2.Parent as StartPage;
                    Grid parent4 = parent3.Parent as Grid;
                    TabView parent5 = parent4.Parent as TabView;
                    foreach (string s in arr)
                    {
                        string[] split = s.Split((char)42);
                        bookmarks.AddBookmark(split[0], split[1], parent5, mw);
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
            
            string[] readText = File.ReadAllLines(Bookspath);
            ArrayList arr = new ArrayList();
            foreach (var sr in readText)
            {
                arr.Add(sr);
            }
            foreach (string line in arr)
            {
                if (line.Contains(_url))
                {
                    string[] lines = { line };
                    RemoveLines(Bookspath, lines);
                }
            }

            string[] readText1 = File.ReadAllLines(Bookmarkspath);
            ArrayList arr2 = new ArrayList();
            foreach (var sr in readText1)
            {
                arr.Add(sr);
            }
            foreach (string line in arr)
            {
                if (line.Contains(_url))
                {
                    string[] lines = { line };
                    RemoveLines(Bookmarkspath, lines);
                }
            }
            
        }
    }
}

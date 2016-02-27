using System;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace WebExpress.Bookmarks
{
    /// <summary>
    /// Interaction logic for Bookmarks.xaml
    /// </summary>
    public partial class Bookmarks : UserControl
    {
        public int ItemsCount;
        private int ItemHeight;
        private MainWindow mainWindow;
        public Bookmarks(MainWindow mw)
        {
            InitializeComponent();
            ItemsCount = 0;
            ItemHeight = 42;
            Loaded += Bookmarks_Loaded;
            mainWindow = mw;
        }

        private void Bookmarks_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(StaticDeclarations.Bookmarkspath))
                {
                    string fileRead = System.IO.File.ReadAllText(StaticDeclarations.Bookmarkspath);
                    dynamic json = JsonConvert.DeserializeObject(fileRead);
                    foreach (dynamic item in json)
                    {
                        AddBookmark(Convert.ToString(item.Title), Convert.ToString(item.Url), mainWindow);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bookmarks load error: " + ex.Message);
            }
        }

        public void AddBookmark(string title, string url, MainWindow mw)
        {
            BookmarkItem hi = new BookmarkItem(title, url, this, mw);
            content.Children.Add(hi);
            hi.Margin = new Thickness(0, ItemsCount * ItemHeight, 0, 0);
            ItemsCount += 1;
        }
    }
}

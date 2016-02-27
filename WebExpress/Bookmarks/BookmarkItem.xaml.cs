using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebExpress.Bookmarks
{
    /// <summary>
    /// Interaction logic for BookmarkItem.xaml
    /// </summary>
    public partial class BookmarkItem : UserControl
    {
        private Bookmarks _Bookmarks;
        private MainWindow mainWindow;
        public BookmarkItem(string title, string url, Bookmarks b, MainWindow mw)
        {
            InitializeComponent();
            Title.Text = title;
            Url.Text = url;
            _Bookmarks = b;
            mainWindow = mw;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string readFile = System.IO.File.ReadAllText(StaticDeclarations.Bookmarkspath);
            dynamic json = JsonConvert.DeserializeObject(readFile);
            try
            {
                foreach (dynamic item in json)
                {
                    if (Url.Text.Equals(Convert.ToString(item.Url)))
                    {
                        JArray json2 = (json as JArray);
                        json2.Remove(item);
                        _Bookmarks.ItemsCount = 0;
                        _Bookmarks.content.Children.Clear();
                        System.IO.File.WriteAllText(StaticDeclarations.Bookmarkspath, json2.ToString());
                        if (System.IO.File.Exists(StaticDeclarations.Bookmarkspath))
                        {
                            string fileRead = System.IO.File.ReadAllText(StaticDeclarations.Bookmarkspath);
                            dynamic json1 = JsonConvert.DeserializeObject(fileRead);
                            foreach (dynamic item2 in json1)
                            {
                                _Bookmarks.AddBookmark(Convert.ToString(item2.Title), Convert.ToString(item2.Url), mainWindow);
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
            }
            catch
            {
            }
        }
    }
}

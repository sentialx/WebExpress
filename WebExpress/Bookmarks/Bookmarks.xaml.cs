using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WebExpress.Bookmarks
{
    /// <summary>
    /// Interaction logic for Bookmarks.xaml
    /// </summary>
    public partial class Bookmarks : UserControl
    {
        private BookmarkItem bookmarkItem;

        private int ItemsCount;
        public Bookmarks()
        {
            InitializeComponent();
            ItemsCount = 0;
        }
        public void AddBookmark(string url, string title)
        {
            bookmarkItem = new BookmarkItem(url, title);
            Canvas.SetLeft(bookmarkItem, ItemsCount * 177);
            bookmarkItem.Width = 172;
            bookmarkItem.Height = 100;
            canvas.Children.Add(bookmarkItem);
            ItemsCount += 1;
        }
    }
}

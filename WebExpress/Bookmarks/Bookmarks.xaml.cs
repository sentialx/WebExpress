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
using System.Windows.Media.Animation;
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

        public int ItemsCount;
        public int bookmarkWidth;
        public int bookmarkHeight;
        private MainWindow mainWindow;
        public int RowsCount;
        public Bookmarks()
        {
            InitializeComponent();
            ItemsCount = 0;
            RowsCount = 0;
            bookmarkHeight = 100;
            bookmarkWidth = 172;
        }
        public void AddBookmark(string url, string title, TabView tv, MainWindow mw)
        {
                if (ItemsCount != 3)
                {

                    bookmarkItem = new BookmarkItem(url, title, tv, mw, this);
                    Canvas canvas1 = new Canvas();
                    mainCanvas.Children.Add(canvas1);
                    Canvas.SetTop(canvas1, RowsCount * 105);
                    Canvas.SetLeft(bookmarkItem, ItemsCount * 177);
                    bookmarkItem.Width = bookmarkWidth;
                    bookmarkItem.Height = bookmarkHeight;
                    canvas1.Children.Add(bookmarkItem);
                    ItemsCount += 1;
                    if (ItemsCount == 3) {
                        ItemsCount = 0;
                        RowsCount += 1;
                    }
                    if (RowsCount > 3)
                    {
                        mainCanvas.Height = (RowsCount + 1) * 110;
                    }

               
            }
           
        }
       
        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}

using System.Windows;
using System.Windows.Controls;

namespace WebExpress.Bookmarks
{
    /// <summary>
    /// Interaction logic for Bookmarks.xaml
    /// </summary>
    public partial class BookmarksPanel : UserControl
    {
        private BookmarkPanelItem bookmarkItem;

        public int ItemsCount;
        public int bookmarkWidth;
        private int bookmarkTop;
        private int bookmarkLeft;
        public int bookmarkHeight;
        private MainWindow mainWindow;
        public int RowsCount;
        public BookmarksPanel()
        {
            InitializeComponent();
            ItemsCount = 0;
            RowsCount = 0;
            
            bookmarkHeight = 130;
            bookmarkWidth = 225;
            bookmarkLeft = bookmarkWidth + 2;
            bookmarkTop = bookmarkHeight + 2;
        }
        public void AddBookmark(string url, string title, TabView tv, MainWindow mw)
        {
                if (ItemsCount != 3)
                {

                    bookmarkItem = new BookmarkPanelItem(url, title, tv, mw, this);
                    Canvas canvas1 = new Canvas();
                    mainCanvas.Children.Add(canvas1);
                    Canvas.SetTop(canvas1, RowsCount * bookmarkTop);
                    Canvas.SetLeft(bookmarkItem, ItemsCount * bookmarkLeft);
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
                        mainCanvas.Height = (RowsCount + 1) * bookmarkTop;
                    }

               
            }
           
        }
       
        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}

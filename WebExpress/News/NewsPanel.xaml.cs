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

namespace WebExpress
{
    /// <summary>
    /// Interaction logic for NewsPanel.xaml
    /// </summary>
    public partial class NewsPanel : UserControl
    {
        private int ItemWidth;
        private int ItemHeight;
        public int ItemsCount;
        private int ItemMargin;
        private NewsItem ni;
        public NewsPanel()
        {
            InitializeComponent();
            ItemWidth = 128;
            ItemHeight = 350;
            ItemsCount = 0;
            ItemMargin = 10;
            
        }

        public void AddNews(string title, string url, string desc, string image, string category)
        {
            ni = new NewsItem(title,url, desc, image, category);
            canvas.Children.Add(ni);

        }

        public void RefreshNews(NewsItem ni1)
        {
            Canvas.SetTop(ni1, (19 + (350) + ItemMargin)*ItemsCount);
            this.Height = (500) * ItemsCount;
            ItemsCount += 1;
        }
    }
}

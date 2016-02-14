using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CefSharp;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using UserControl = System.Windows.Controls.UserControl;
using System.Windows.Media.Animation;
using System.Drawing;
using System.Windows.Media;

namespace WebExpress
{
    public partial class Downloads : UserControl
    {
        private List<DownloadItem> Items;
        private DownloadItem Item;
        private int ItemsCount;
        private int ItemHeight;
        private int MarginTop;
        public Downloads()
        {
            InitializeComponent();
            ItemsCount = 0;
            ItemHeight = 64;
            MarginTop = 53;
            this.MaxHeight = MarginTop + (5 * ItemHeight);
            Items = new List<DownloadItem>();
        }

        public void AddDownload(string url, string filepath, string filename)
        {
            Dispatcher.BeginInvoke((Action) (() =>
           {
               Item = new DownloadItem(filepath, url, filename);
               Item.Width = Content.ActualWidth;
               Item.Height = ItemHeight;
               canvas.Children.Add(Item);
               Canvas.SetTop(Item, Item.Height * ItemsCount);
               Items.Add(Item);
               this.Height = MarginTop + Items.Count * ItemHeight;
               ItemsCount += 1;
           }));
        }

        private void button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        public void RemoveDownload(DownloadItem di)
        {
            Items.Remove(di);
            canvas.Children.Remove(di);
            ItemsCount = 0;

            Dispatcher.BeginInvoke((Action) (() =>
            {
                foreach (DownloadItem Item in Items)
                {
                    Item.Width = Content.ActualWidth;
                    Item.Height = ItemHeight;
                    Canvas.SetTop(Item, Item.Height * ItemsCount);
                    this.Height = MarginTop + Items.Count * ItemHeight;
                    ItemsCount += 1;
                }
            }));
        }
    }
}

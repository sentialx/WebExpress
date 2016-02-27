using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace WebExpress
{
    public partial class Downloads : UserControl
    {
        private DownloadItem Item;
        public readonly int ItemHeight;
        public readonly List<DownloadItem> Items;
        public int ItemsCount;
        public readonly int MarginTop;

        public Downloads()
        {
            InitializeComponent();
            ItemsCount = 0;
            ItemHeight = 64;
            MarginTop = 64;
            button.ImageSource("close_button.png");
            button.SetRippleMargin(1);
            MaxHeight = MarginTop + 5*ItemHeight;
            Items = new List<DownloadItem>();
        }

        public void AddDownload(string url, string filepath, string filename)
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                Item = new DownloadItem(filepath, url, filename);
                Item.Width = content.ActualWidth;
                Item.VerticalAlignment = VerticalAlignment.Top;
                Item.HorizontalAlignment = HorizontalAlignment.Stretch;
                Item.Height = ItemHeight;
                content.Children.Add(Item);
                Item.Margin = new Thickness(0, MarginTop + Item.Height*ItemsCount, 0, 0);
                Items.Add(Item);
                var fade = new DoubleAnimation
                {
                    From = ActualHeight,
                    To = 103 + Items.Count*ItemHeight,
                    Duration = TimeSpan.FromSeconds(0.1)
                };
                Storyboard.SetTarget(fade, this);
                Storyboard.SetTargetProperty(fade, new PropertyPath(HeightProperty));

                var sb = new Storyboard();
                sb.Children.Add(fade);
                sb.Begin();
                ItemsCount += 1;
            }));
        }

        private void button_Click(object sender, MouseButtonEventArgs e)
        {
            StaticFunctions.AnimateScale(ActualWidth,ActualHeight,0,0,this,0.2);

        }

        public void RefreshDownloads()
        {
            ItemsCount = 0;

            Dispatcher.BeginInvoke((Action)(() =>
            {
                foreach (var Item in Items)
                {
                    Item.Width = content.ActualWidth;
                    Item.Height = ItemHeight;
                    Item.Margin = new Thickness(0, MarginTop + Item.Height * ItemsCount, 0, 0);
                    ItemsCount += 1;
                    var fade = new DoubleAnimation
                    {
                        From = ActualHeight,
                        To = 103 + Items.Count * ItemHeight,
                        Duration = TimeSpan.FromSeconds(0.1)
                    };
                    Storyboard.SetTarget(fade, this);
                    Storyboard.SetTargetProperty(fade, new PropertyPath(HeightProperty));

                    var sb = new Storyboard();
                    sb.Children.Add(fade);
                    sb.Begin();
                }
            }));
        }
        public void RemoveDownload(DownloadItem di)
        {
            Items.Remove(di);
            content.Children.Remove(di);
            ItemsCount = 0;

            Dispatcher.BeginInvoke((Action) (() =>
            {
                foreach (var Item in Items)
                {
                    Item.Width = content.ActualWidth;
                    Item.Height = ItemHeight;
                    Item.Margin = new Thickness(0, MarginTop + Item.Height*ItemsCount, 0, 0);
                    ItemsCount += 1;
                    var fade = new DoubleAnimation
                    {
                        From = ActualHeight,
                        To = 103 + Items.Count*ItemHeight,
                        Duration = TimeSpan.FromSeconds(0.1)
                    };
                    Storyboard.SetTarget(fade, this);
                    Storyboard.SetTargetProperty(fade, new PropertyPath(HeightProperty));

                    var sb = new Storyboard();
                    sb.Children.Add(fade);
                    sb.Begin();
                }
            }));
        }

    }
}
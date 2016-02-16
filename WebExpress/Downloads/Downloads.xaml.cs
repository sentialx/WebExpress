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
        private readonly int ItemHeight;
        private readonly List<DownloadItem> Items;
        private int ItemsCount;
        private readonly int MarginTop;

        public Downloads()
        {
            InitializeComponent();
            ItemsCount = 0;
            ItemHeight = 64;
            MarginTop = 64;
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var fade = new DoubleAnimation
            {
                From = ActualHeight,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.25)
            };
            Storyboard.SetTarget(fade, this);
            Storyboard.SetTargetProperty(fade, new PropertyPath(HeightProperty));

            var sb = new Storyboard();
            sb.Children.Add(fade);
            sb.Completed +=
                (o, e1) => { Visibility = Visibility.Hidden; };
            sb.Begin();
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

        private void button_MouseEnter(object sender, MouseEventArgs e)
        {
            StaticFunctions.ChangeButtonImage("close_button_hover.png", CloseImage);
        }

        private void button_MouseLeave(object sender, MouseEventArgs e)
        {
            StaticFunctions.ChangeButtonImage("close_button.png", CloseImage);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                double height = 0;
                if (!ItemsCount.Equals(0))
                {
                    height = MarginTop + Items.Count*ItemHeight;
                }
                else
                {
                    height = MarginTop + 50;
                }
                var fade = new DoubleAnimation
                {
                    From = 0,
                    To = height,
                    Duration = TimeSpan.FromSeconds(0.25)
                };
                Storyboard.SetTarget(fade, this);
                Storyboard.SetTargetProperty(fade, new PropertyPath(HeightProperty));

                var sb = new Storyboard();
                sb.Children.Add(fade);
                sb.Begin();
            }
        }
    }
}
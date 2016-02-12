using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brushes = System.Windows.Media.Brushes;

namespace WebExpress
{
    public partial class Tab : UserControl
    {
        public UserControl form;
        private bool _bgTab;
        public MainWindow mainWindow;
        private int closeTabMargin;
        public Brush actualForeground;
        private Brush color;
        private int favIconMargin;

        public Brush Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                if (bgTab)
                {
                    bg.Background = color;
                }
            }
        }

        public void refreshColor()
        {
            if (!bgTab)
            {
                bg.Background = color;
            } else
            {
                bg.Background = Brushes.LightGray;
            }
        }

        public Tab(string title, MainWindow mw, UserControl uc, System.Windows.Media.Brush brush)
        {
            InitializeComponent();
            form = uc;
            color = brush;
            actualForeground = Brushes.Black;
            form.HorizontalAlignment = HorizontalAlignment.Stretch;
            form.VerticalAlignment = VerticalAlignment.Stretch;
            label_TabTitle.Content = title;
            Loaded += Tab_Loaded;
            mainWindow = mw;
            closeTabMargin = 8;
            favIconMargin = 6;
        }

        private void Tab_Loaded(object sender, RoutedEventArgs e)
        {
            

            Canvas parentForm = this.Parent as Canvas;
            Grid parentForm2 = parentForm.Parent as Grid;
            TabBar parentForm3 = parentForm2.Parent as TabBar;
            mainWindow.container.Children.Add(form);
            parentForm3.SelectTab(this);
            if (parentForm3.TabCollection.Count > 0)
            {
                for (var i = 0; i < parentForm3.TabCollection.Count - 1; i++)
                {
                    parentForm3.SelectTab(parentForm3.TabCollection[i + 1]);
                }
            }
            if (this.ActualWidth - (CloseTab.ActualWidth + closeTabMargin) > 0)
            {
                label_TabTitle.Width = this.ActualWidth - (CloseTab.ActualWidth + closeTabMargin);
            }
            label_TabTitle.Margin = new Thickness(6, 1, 0, 0);
        }

        public bool bgTab
        {
            get { return _bgTab; }
            set
            {
                _bgTab = value;
                if (value == true)
                {
                    
                    bg.Background = Brushes.LightGray;
                    label_TabTitle.Foreground = Brushes.Black;
                }
                else {
                    bg.Background = color;
                    label_TabTitle.Foreground = actualForeground;
                }
            }
        }
        delegate void GetFavDelegate(BitmapImage icon);
        public void SetIcon(BitmapImage icon)
        {
            this.Dispatcher.Invoke((Action) (() =>
            {
                label_TabTitle.Width = this.ActualWidth - (CloseTab.ActualWidth + closeTabMargin) - (FavIcon.Width + favIconMargin);
                label_TabTitle.Margin = new Thickness(FavIcon.Width + 6, 1, 0, 0);
                FavIcon.Source = icon;
            }));
        }

        public void SetTitle(string title)
        {
            label_TabTitle.Content = title;
        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
           Canvas parentForm = this.Parent as Canvas;
            Grid parentForm2 = parentForm.Parent as Grid;
           TabBar parentForm3 = parentForm2.Parent as TabBar;
            parentForm3.RemoveTab(this);
            
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas parentForm = this.Parent as Canvas;
            Grid parentForm2 = parentForm.Parent as Grid;
            TabBar parentForm3 = parentForm2.Parent as TabBar;
            parentForm3.SelectTab(this);
        }

        private void label_TabTitle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (label_TabTitle.Margin == new Thickness(6, 1, 0, 0))
            {
                if (this.ActualWidth - (CloseTab.ActualWidth + closeTabMargin) - (FavIcon.Width + favIconMargin) > 0)
                {
                    label_TabTitle.Width = this.ActualWidth - (CloseTab.ActualWidth + closeTabMargin);
                }
            }
            else
            {
                if (this.ActualWidth - (CloseTab.ActualWidth + closeTabMargin) - (FavIcon.Width + favIconMargin) > 0)
                {
                    label_TabTitle.Width = this.ActualWidth - (CloseTab.ActualWidth + closeTabMargin) - (FavIcon.Width + favIconMargin);
                }
            }
        }


        private void bg2_MouseEnter(object sender, MouseEventArgs e)
        {
            if (bgTab)
            {
                bg.Background = Brushes.Gainsboro;
            }
        }

        private void bg2_MouseLeave(object sender, MouseEventArgs e)
        {
            if (bgTab)
            {
                bg.Background = Brushes.LightGray;
            }
        }
    }
}

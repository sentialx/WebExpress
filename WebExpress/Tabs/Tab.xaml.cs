using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WebExpress.Controls;
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
        public bool darkColor;

        public Brush Color
        {
            get { return color; }
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
            }
            else
            {

                bg.Background = Brushes.Transparent;
                CloseTab.ImageSource("close_Tab.png");
            }
        }

        public Tab(string title, MainWindow mw, UserControl uc, System.Windows.Media.Brush brush)
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                darkColor = false;
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
                CloseTab.ImageSource("close_Tab.png");
                
                CloseTab.SetImageScale(8);
                CloseTab.SetRippleMargin(1);
            }));
        }

        private void Tab_Loaded(object sender, RoutedEventArgs e)
        {

            Dispatcher.BeginInvoke((Action) (() =>
            {
                TabBar tb = this.FindParent<TabBar>();
                mainWindow.container.Children.Add(form);
                tb.SelectTab(this);
                if (tb.TabCollection.Count > 0)
                {
                    for (var i = 0; i < tb.TabCollection.Count - 1; i++)
                    {
                        tb.SelectTab(tb.TabCollection[i + 1]);
                    }
                }
                if (this.ActualWidth - (CloseTab.ActualWidth + closeTabMargin) > 0)
                {
                    label_TabTitle.Width = this.ActualWidth - (CloseTab.ActualWidth + closeTabMargin);
                }
                label_TabTitle.Margin = new Thickness(6, 1, 0, 0);
            }));
        }


        public bool bgTab
        {
            get { return _bgTab; }
            set
            {
                _bgTab = value;
                if (value)
                {

                    bg.Background = Brushes.Transparent;
                    label_TabTitle.Foreground = Brushes.Black;
                    CloseTab.ImageSource("close_Tab.png");

                }
                else
                {

                    bg.Background = color;
                    label_TabTitle.Foreground = actualForeground;
                    CloseTab.ImageSource(darkColor
                        ? "close_Tab_white.png"
                        : "close_Tab.png");

                }
            }
        }


        public void SetIcon(BitmapImage icon)
        {
            this.Dispatcher.BeginInvoke((Action) (() =>
            {
                label_TabTitle.Width = this.ActualWidth - (CloseTab.ActualWidth + closeTabMargin) -
                                       (FavIcon.Width + favIconMargin);
                label_TabTitle.Margin = new Thickness(FavIcon.Width + 6, 1, 0, 0);
                FavIcon.Source = icon;
            }));
        }

        public void SetTitle(string title)
        {
            label_TabTitle.Content = title;
        }

        private void button_close_Click(object sender, MouseButtonEventArgs e)
        {
                if (form.GetType() == typeof(Applets.Settings))
                {
                    try {
                        Applets.Settings s = form as Applets.Settings;
                        s.SaveSettings();
                    } catch (Exception ex)
                    {
                        Console.WriteLine("Save settings error: " + ex.Message + " " + ex.Data + " ");
                    }
                }

            TabBar tb = this.FindParent<TabBar>();
            tb.RemoveTab(this);

        }

       

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

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
                    label_TabTitle.Width = this.ActualWidth - (CloseTab.ActualWidth + closeTabMargin) -
                                           (FavIcon.Width + favIconMargin);
                }
            }
        }


        private void bg2_MouseEnter(object sender, MouseEventArgs e)
        {
            if (bgTab)
            {
                StaticFunctions.AnimateColor(Colors.Transparent, Colors.Gainsboro, bg, 0.2);

            }
        }

        private void bg2_MouseLeave(object sender, MouseEventArgs e)
        {
            if (bgTab)
            {
                StaticFunctions.AnimateColor(Colors.Gainsboro, Colors.Transparent, bg, 0.2);
            }
        }

        private async void Me_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TabBar tb = this.FindParent<TabBar>();
            tb.SelectTab(this);

            if (form.GetType() == typeof(TabView))
            {
                TabView tv = (form as TabView);
                await tv.ChangeColor();

            }
        }

        private void Me_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ActualWidth < 40)
            {
                try
                {
                    label_TabTitle.Width = ActualWidth - (CloseTab.ActualWidth + closeTabMargin);
                }
                catch
                {
                    
                }
            } 
        }
    }
    public static class ExtensionMethods
    {
        private static Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
}

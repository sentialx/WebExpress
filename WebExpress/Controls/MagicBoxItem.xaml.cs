using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WebExpress.Controls
{
    /// <summary>
    /// Interaction logic for MagicBoxItem.xaml
    /// </summary>
    public partial class MagicBoxItem : UserControl
    {
        private bool _isDark;
        private MainWindow mainWindow;
        public MagicBox parent;
        public MagicBoxItem(string title, string url, MainWindow mw)
        {
            InitializeComponent();
            Title.Content = title;
            Url.Content = url;
            Loaded += MagicBoxItem_Loaded;
            mainWindow = mw;
        }

        private void MagicBoxItem_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Title.Margin = new Thickness(30 + Url.ActualWidth,0,30,0);
        }
        public void RippleColor(Color color)
        {
            SolidColorBrush scb = new SolidColorBrush(color);
            Ripple.Fill = scb;
        }
        public bool IsDark
        {
            get { return _isDark; }
            set
            {
                _isDark = value;
                if (value)
                {
                    Title.Foreground = Brushes.White;
                    Url.Foreground = Brushes.White;
                    RippleColor(Colors.White);
                }
                else
                {
                    Title.Foreground = Brushes.Black;
                    RippleColor(Colors.Black);
                    Url.Foreground = Brushes.Black;
                }
            }
        }

        private void MagicBoxItem_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var targetWidth = Math.Max(ActualHeight, ActualWidth * 2);
            StaticFunctions.AnimateFade(0, 0.1, Ripple, 0.1);
            Ripple.Height = 0;
            Ripple.Width = 0;
            StaticFunctions.AnimateRipple(0, targetWidth, Ripple, 0.6);
        }

        private void MagicBoxItem_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Url.MaxWidth = this.ActualWidth / 1.7;
            Title.MaxWidth = this.ActualWidth / 1.3;
            Title.Margin = new Thickness(30 + Url.ActualWidth, 0, 30, 0);
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (IsDark)
            {
                StaticFunctions.AnimateColor(Colors.Transparent, Color.FromArgb(50,255,255,255), this, 0.2);
            }
            else
            {
                StaticFunctions.AnimateColor(Colors.Transparent, Colors.Gainsboro, this, 0.2);
            }
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (IsDark)
            {
                StaticFunctions.AnimateColor(Color.FromArgb(50, 255, 255, 255), Colors.Transparent, this, 0.2);
            }
            else
            {
                StaticFunctions.AnimateColor(Colors.Gainsboro, Colors.Transparent, this, 0.2);
            }
        }

        private void UIElement_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mainWindow.TabBar.getSelectedTab().form.GetType() == typeof (TabView))
            {
                TabView tv = mainWindow.TabBar.getSelectedTab().form as TabView;
                tv.WebView.Load(Url.Content.ToString());
                tv.HideSuggestions();
            }   
        }
    }
}

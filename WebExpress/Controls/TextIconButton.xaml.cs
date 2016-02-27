using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WebExpress.Controls
{
    /// <summary>
    /// Interaction logic for IconButton.xaml
    /// </summary>
    public partial class TextIconButton : UserControl
    {
        public TextIconButton()
        {
            InitializeComponent();
            Loaded += IconButton_Loaded;
        }

        private void IconButton_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void Text(string text)
        {
            textBlock.Content = text;
        }
        public void ImageSource(string uri)
        {
            try
            {
                var bmp = new BitmapImage(new Uri("pack://application:,,,/Resources/" + uri));
                image.Source = bmp;
            } catch{}
        }

        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var targetWidth = Math.Max(ActualHeight, ActualWidth * 2);
            StaticFunctions.AnimateFade(0, 0.1, Ripple, 0.1);
            Ripple.Height = 0;
            Ripple.Width = 0;
            StaticFunctions.AnimateRipple(0, targetWidth, Ripple, 0.6);

        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            StaticFunctions.AnimateColor(Colors.White, Colors.Gainsboro, bg, 0.2);
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            StaticFunctions.AnimateColor(Colors.Gainsboro, Colors.White, bg, 0.2);
        }
    }
}

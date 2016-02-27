using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WebExpress.Controls
{
    /// <summary>
    /// Interaction logic for IconButton.xaml
    /// </summary>
    public partial class IconButton : UserControl
    {
        private double _margin;
        public IconButton()
        {
            InitializeComponent();
            _margin = 10;
        }

        public void ImageSource(string uri)
        {

                var bmp = new BitmapImage(new Uri("pack://application:,,,/Resources/" + uri));
                image.Source = bmp;
        }

        public void RippleColor(Color color)
        {
            SolidColorBrush scb = new SolidColorBrush(color);
            Ripple.Fill = scb;
        }

        public void SetImageScale(double scale)
        {
            image.Height = scale;
            image.Width = scale;
        }

        public void SetRippleMargin(double margin)
        {
            _margin = margin;
        }
        private void UserControl_PreviewMouseDown( object sender, MouseButtonEventArgs e)
        {
            StaticFunctions.AnimateFade(0, 0.1, Ripple, 0.1);
            Ripple.Height = 0;
            Ripple.Width = 0;
            StaticFunctions.AnimateRipple(0, this.ActualWidth - _margin, Ripple, 0.2);
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            image.Opacity = 1;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            image.Opacity = 0.70;
        }
    }
}

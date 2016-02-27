using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WebExpress.Controls
{

    public partial class FlatButton : UserControl
    {
        private double _opacity;
        public FlatButton()
        {
            InitializeComponent();
            _opacity = 0.3;
        }
        private static readonly BrushConverter Converter = new BrushConverter();

        public void Text(string text)
        {
            textBlock.Content = text;
        }

        public void ChangeTextColor(string hex)
        {
            SolidColorBrush scb = (SolidColorBrush)Converter.ConvertFromString(hex);
            textBlock.Foreground = scb;
        }

        public void RippleColor(Color color)
        {
            SolidColorBrush scb = new SolidColorBrush(color);
            Ripple.Fill = scb;
        }
        public void RippleColor(string hex)
        {
            SolidColorBrush scb = (SolidColorBrush)Converter.ConvertFromString(hex);
            Ripple.Fill = scb;
        }
        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var targetWidth = Math.Max(ActualHeight, ActualWidth * 2);
            StaticFunctions.AnimateFade(0, _opacity, Ripple, 0);
            Ripple.Height = 0;
            Ripple.Width = 0;
            StaticFunctions.AnimateRipple(0, targetWidth, Ripple, 0.6);
        }
        public void SetRippleOpacity(double opacity) { Ripple.Opacity = opacity; _opacity = opacity; }
    }
}

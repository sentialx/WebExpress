using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace WebExpress
{
    class StaticFunctions
    {
        public static Color GetDominantColor(Bitmap bmp)
        {
            int r = 0;
            int g = 0;
            int b = 0;

            int total = 0;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color clr = bmp.GetPixel(x, y);

                    r += clr.R;
                    g += clr.G;
                    b += clr.B;

                    total++;
                }
            }

            r /= total;
            g /= total;
            b /= total;

            return Color.FromArgb(r, g, b);
        }

        public static System.Windows.Media.Color ToMediaColor(System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        public static double ConvertBytesToMegabytes(long bytes)
        {
            return Math.Round((bytes / 1024f) / 1024f, 2);
        }

        public static void ChangeButtonImage(string filename, System.Windows.Controls.Image image)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri("pack://application:,,,/Resources/" + filename);
            bi.EndInit();
            image.Source = bi;
        }

        public static void AnimateScale(double fromX,double fromY, double toX, double toY, UIElement control, double duration)
        {
            var fade = new DoubleAnimation
            {
                From = fromY,
                To = toY,
                Duration = TimeSpan.FromSeconds(duration)
            };
            Storyboard.SetTarget(fade, control);
            Storyboard.SetTargetProperty(fade, new PropertyPath(FrameworkElement.HeightProperty));

            var fade2 = new DoubleAnimation
            {
                From = fromX,
                To = toX,
                Duration = TimeSpan.FromSeconds(duration)
            };
            Storyboard.SetTarget(fade2, control);
            Storyboard.SetTargetProperty(fade2, new PropertyPath(FrameworkElement.WidthProperty));


            var sb = new Storyboard();
            sb.Children.Add(fade2);
            sb.Children.Add(fade);
            sb.Begin();


        }

        public static void AnimateHeight(double from, double to, UIElement control, double duration)
        {
            var fade = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromSeconds(duration)
            };
            Storyboard.SetTarget(fade, control);
            Storyboard.SetTargetProperty(fade, new PropertyPath(FrameworkElement.HeightProperty));


            var sb = new Storyboard();
            sb.Children.Add(fade);
            sb.Begin();
        }
        public static void AnimateRipple(double from, double to, UIElement control, double duration)
        {
            var fade = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromSeconds(duration)
            };
            Storyboard.SetTarget(fade, control);
            Storyboard.SetTargetProperty(fade, new PropertyPath(FrameworkElement.HeightProperty));

            var fade2 = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromSeconds(duration)
            };
            Storyboard.SetTarget(fade2, control);
            Storyboard.SetTargetProperty(fade2, new PropertyPath(FrameworkElement.WidthProperty));


            var sb = new Storyboard();
            sb.Children.Add(fade2);
            sb.Children.Add(fade);
            sb.Completed +=
    (o, e1) => { AnimateFade(control.Opacity, 0, control, 0.4);  };
            sb.Begin();
        }
        public static void AnimateFade(double from, double to, UIElement control, double duration)
        {
            var fade = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromSeconds(duration)
            };
            Storyboard.SetTarget(fade, control);
            Storyboard.SetTargetProperty(fade, new PropertyPath(UIElement.OpacityProperty));

            var sb = new Storyboard();
            sb.Children.Add(fade);
            sb.Begin();
            
        }
        public static void AnimateFade(double from, double to, UIElement control, double duration, Action method)
        {
            var fade = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromSeconds(duration)
            };
            Storyboard.SetTarget(fade, control);
            Storyboard.SetTargetProperty(fade, new PropertyPath(UIElement.OpacityProperty));

            var sb = new Storyboard();
            sb.Children.Add(fade);
            sb.Completed +=
                (o, e) =>
                {
                    method();
                };
            sb.Begin();
        }

        public static void AnimateColor(System.Windows.Media.Color from, System.Windows.Media.Color to, UIElement control, double duration)
        {
            SolidColorBrush myBrush = new SolidColorBrush();
            myBrush.Color = from;
            ColorAnimation myColorAnimation = new ColorAnimation();
            myColorAnimation.From = from;
            myColorAnimation.To = to;
            myColorAnimation.Duration = new Duration(TimeSpan.FromSeconds(duration));
            myBrush.BeginAnimation(SolidColorBrush.ColorProperty, myColorAnimation);
            control.GetType().GetProperty("Background").SetValue(control, myBrush);
        }
        public static void AnimateColor(SolidColorBrush from, System.Windows.Media.Color to, UIElement control, double duration)
        {
            SolidColorBrush myBrush = new SolidColorBrush();
            myBrush = from;
            ColorAnimation myColorAnimation = new ColorAnimation();
            myColorAnimation.From = from.Color;
            myColorAnimation.To = to;
            myColorAnimation.Duration = new Duration(TimeSpan.FromSeconds(duration));
            myBrush.BeginAnimation(SolidColorBrush.ColorProperty, myColorAnimation);
            control.GetType().GetProperty("Background").SetValue(control, myBrush);
        }
        public static void AnimateColor(SolidColorBrush from, SolidColorBrush to, UIElement control, double duration)
        {
            SolidColorBrush myBrush = new SolidColorBrush();
            myBrush = from;
            ColorAnimation myColorAnimation = new ColorAnimation();
            myColorAnimation.From = from.Color;
            myColorAnimation.To = to.Color;
            myColorAnimation.Duration = new Duration(TimeSpan.FromSeconds(duration));
            myBrush.BeginAnimation(SolidColorBrush.ColorProperty, myColorAnimation);
            control.GetType().GetProperty("Background").SetValue(control, myBrush);
        }
    }
}

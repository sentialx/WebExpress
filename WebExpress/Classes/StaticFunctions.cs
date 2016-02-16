using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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
    }
}

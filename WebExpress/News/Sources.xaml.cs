using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WebExpress
{
    /// <summary>
    /// Interaction logic for Sources.xaml
    /// </summary>
    public partial class Sources : UserControl
    {
        public int ItemsCount;
        public Sources()
        {
            InitializeComponent();
            ItemsCount = 0;

        }

        public async Task AddSource(string dllPath)
        {
           await Dispatcher.BeginInvoke((Action) (() =>
            {
                Source source = new Source(dllPath);
                source.Height = 32;
                source.Width = this.ActualWidth;
                source.HorizontalAlignment = HorizontalAlignment.Stretch;
                source.VerticalAlignment = VerticalAlignment.Top;
                container.Children.Add(source);
                source.Margin = new Thickness(0, ItemsCount*32, 0, 0);
                ItemsCount += 1;
            }));
        }
    }
}

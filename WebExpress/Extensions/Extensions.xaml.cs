using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WebExpress
{
    /// <summary>
    /// Interaction logic for Extansions.xaml
    /// </summary>
    public partial class Extensions : UserControl
    {
        public int ItemsCount;
        private Extension extension;
        public List<Extension> extensions;
        public Extensions()
        {
            InitializeComponent();
            Loaded += Extensions_Loaded;
            ItemsCount = 0;
            extensions = new List<Extension>();
        }

        private void Extensions_Loaded(object sender, RoutedEventArgs e)
        {
            AddExtensions();
        }

        public void AddExtensions()
        {
            foreach (string file in System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Extensions"))
            {
                if (System.IO.Path.GetExtension(file) == ".json")
                {
                    string json = System.IO.File.ReadAllText(file);
                    dynamic dyn = JsonConvert.DeserializeObject(json);
                    extension = new Extension(file);
                    extension.Header.Content = dyn.id;
                    extension.Width = this.ActualWidth;
                    extension.Height = 125;
                    var path = System.IO.Path.Combine(Environment.CurrentDirectory, "Extensions", Convert.ToString(dyn.logo));
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.UriSource = new Uri(path);
                    bi.EndInit();
                    extension.image.Source = bi;
                    extension.VerticalAlignment = VerticalAlignment.Top;
                    extension.HorizontalAlignment = HorizontalAlignment.Stretch;
                    extension.Description.Content = dyn.description;
                    Canvas.SetTop(extension, ItemsCount * 125);
                    canvas.Children.Add(extension);
                    ItemsCount += 1;
                    extensions.Add(extension);
                }
            }
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (Extension ext in extensions)
            {
                ext.Width = this.ActualWidth;
            }
        }
    }
}

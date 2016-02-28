using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WebExpress.Controls;

namespace WebExpress
{
    /// <summary>
    /// Interaction logic for Source.xaml
    /// </summary>
    public partial class Source : UserControl
    {
        private string _dllPath;
        private AppDomain domain;
        public Source(string dllPath)
        {
            InitializeComponent();
            _dllPath = dllPath;
            int pos = dllPath.LastIndexOf("\\") + 1;
            string lol = dllPath.Substring(pos, dllPath.Length - pos);
            string replace = lol.Replace(".news", "");
          Title.Text = replace;
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(_dllPath))
                {

                    System.IO.File.Delete(_dllPath);
                    Sources sources = this.FindParent<Sources>();
                    sources.container.Children.Clear();
                    sources.ItemsCount = 0;
                    if (Directory.Exists("News"))
                    {
                        foreach (string file in System.IO.Directory.GetFiles("News", "*.news"))
                        {
                            await sources.AddSource(file);
                        }
                    }
                }
            }
            catch
            {
                
            }
        }
    }
}

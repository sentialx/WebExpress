using System;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace WebExpress
{
    /// <summary>
    /// Interaction logic for History.xaml
    /// </summary>
    public partial class History : UserControl
    {
        public int ItemsCount;
        private int ItemHeight;
        public History()
        {
            InitializeComponent();
            ItemsCount = 0;
            ItemHeight = 43;
            Loaded += History_Loaded;
        }

        private void History_Loaded(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists(StaticDeclarations.Historypath))
            {
                string fileRead = System.IO.File.ReadAllText(StaticDeclarations.Historypath);
                dynamic json = JsonConvert.DeserializeObject(fileRead);
                foreach (dynamic item in json)
                {
                    AddHistory(Convert.ToString(item.Title), Convert.ToString(item.Url));
                }
            }
        }

        public void AddHistory(string title, string url)
        {
            HistoryItem hi = new HistoryItem(title,url, this);
            content.Children.Add(hi);
            hi.Margin = new Thickness(0, ItemsCount * ItemHeight,0,0);
            ItemsCount += 1;
        }
    }
}

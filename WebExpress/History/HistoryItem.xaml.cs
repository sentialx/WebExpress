using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebExpress
{
    /// <summary>
    /// Interaction logic for HistoryItem.xaml
    /// </summary>
    public partial class HistoryItem : UserControl
    {
        private History _History;
        public HistoryItem(string title, string url, History h)
        {
            InitializeComponent();
            Title.Text = title;
            Url.Text = url;
            _History = h;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string readFile = System.IO.File.ReadAllText(StaticDeclarations.Historypath);
            dynamic json = JsonConvert.DeserializeObject(readFile);
            try
            {
                foreach (dynamic item in json)
                {
                    if (Url.Text.Equals(Convert.ToString(item.Url)))
                    {
                        Console.WriteLine("blah");
                        JArray json2 = (json as JArray);
                        json2.Remove(item);
                        _History.ItemsCount = 0;
                        _History.content.Children.Clear();
                        System.IO.File.WriteAllText(StaticDeclarations.Historypath, json2.ToString());
                        if (System.IO.File.Exists(StaticDeclarations.Historypath))
                        {
                            string fileRead = System.IO.File.ReadAllText(StaticDeclarations.Historypath);
                            dynamic json1 = JsonConvert.DeserializeObject(fileRead);
                            foreach (dynamic item2 in json1)
                            {
                                _History.AddHistory(Convert.ToString(item2.Title), Convert.ToString(item2.Url));
                            }
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
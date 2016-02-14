using Newtonsoft.Json;
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

namespace WebExpress
{
    /// <summary>
    /// Interaction logic for Extension.xaml
    /// </summary>
    public partial class Extension : UserControl
    {
        private string json;
        public Extension(string jsonFile)
        {
            InitializeComponent();
            json = jsonFile;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Canvas parent = this.Parent as Canvas;
            Grid parent2 = parent.Parent as Grid;
            Extensions parent3 = parent2.Parent as Extensions;
            foreach (var all in parent3.extensions)
            parent.Children.Remove(all);


                    this.image.Source = null;
                    parent3.ItemsCount = 0;
                    string json1 = System.IO.File.ReadAllText(System.IO.Path.Combine(Environment.CurrentDirectory, "Extensions", Convert.ToString(json)));
                    dynamic dyn = JsonConvert.DeserializeObject(json1);
                    var path = System.IO.Path.Combine(Environment.CurrentDirectory, "Extensions", Convert.ToString(dyn.logo));
                    var path2 = System.IO.Path.Combine(Environment.CurrentDirectory, "Extensions", json);
                    
                    System.IO.File.Delete(path);
                    System.IO.File.Delete(path2);
                    foreach (var script in dyn.scripts)
                    {
                        System.IO.File.Delete(System.IO.Path.Combine(Environment.CurrentDirectory, "Extensions", Convert.ToString(script.file)));

                    }
            parent3.AddExtensions();
        }
    }
}

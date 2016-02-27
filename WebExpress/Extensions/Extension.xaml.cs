using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void button_MouseEnter(object sender, MouseEventArgs e)
        {
            StaticFunctions.ChangeButtonImage("close_button_hover.png", CloseImage);
        }

        private void button_MouseLeave(object sender, MouseEventArgs e)
        {
            StaticFunctions.ChangeButtonImage("close_button.png", CloseImage);
        }
    }
}

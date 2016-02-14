
using System.Windows;
using UserControl = System.Windows.Controls.UserControl;
using System.Windows.Media;
using System;
using Newtonsoft.Json;

namespace WebExpress.Applets
{

    public partial class Settings : UserControl
    {
        MainWindow mainWindow;
        public Settings(MainWindow mw)
        {
            InitializeComponent();
            mainWindow = mw;
            Loaded += Settings_Loaded;

        }

        private void Settings_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var converter = new BrushConverter();
            var brush = (System.Windows.Media.Brush)converter.ConvertFromString("#1abc9c");
            mainWindow.TabBar.getTabFromForm(this).Background = brush;
            dynamic dyn = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("settings.json"));
            comboBox.Text = dyn.SE;
            textBox.Text = dyn.Start;
        }

        private void Grid_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {

        }
        public void SaveSettings()
        {
            try
            {
                Values values = new Values();
                values.SE = comboBox.Text;
                values.Start = textBox.Text;
                string output = JsonConvert.SerializeObject(values);
                System.IO.File.WriteAllText("settings.json", output);

            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveSettings error: " + ex.Message);
            }
        }

   
    }
    public class Values
    {
        public string SE { get; set; }
        public string Start { get; set; }
    }
}

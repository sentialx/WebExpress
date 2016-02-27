
using System.Windows;
using UserControl = System.Windows.Controls.UserControl;
using System;
using System.IO;
using Newtonsoft.Json;

namespace WebExpress.Applets
{

    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
            Loaded += Settings_Loaded;

        }

        private async void Settings_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try {
                dynamic dyn = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("settings.json"));
                comboBox.Text = dyn.SE;
                textBox.Text = dyn.Start;
                EnterCheck.IsChecked = Convert.ToBoolean(Convert.ToString(dyn.Entertainment));
                BusCheck.IsChecked = Convert.ToBoolean(Convert.ToString(dyn.Business));
                MotoCheck.IsChecked = Convert.ToBoolean(Convert.ToString(dyn.Moto));
                InfoCheck.IsChecked = Convert.ToBoolean(Convert.ToString(dyn.Information));
                SportCheck.IsChecked = Convert.ToBoolean(Convert.ToString(dyn.Sport));
                textBox2.Text = dyn.City;

            } catch (Exception ex)
            {
                Console.WriteLine("Load settings error: " + ex.Message + " " + ex.Data);
            }
            if (Directory.Exists("News"))
            {
                foreach (string file in System.IO.Directory.GetFiles("News", "*.news"))
                {

                    await Sources.AddSource(file);
                }
            }
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
                values.Entertainment = Convert.ToString(EnterCheck.IsChecked);
                values.Information = Convert.ToString(InfoCheck.IsChecked);
                values.Business = Convert.ToString(BusCheck.IsChecked);
                values.Sport = Convert.ToString(SportCheck.IsChecked);
                values.City = textBox2.Text;
                values.Moto = Convert.ToString(MotoCheck.IsChecked);
                string output = JsonConvert.SerializeObject(values);
                System.IO.File.WriteAllText("settings.json", output);

            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveSettings error: " + ex.Message);
            }
        }

        private void ClearCookiesBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private async void ClearHistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(StaticDeclarations.Historypath))
                {
                    System.IO.File.Delete(StaticDeclarations.Historypath);
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Clear history error: " + ex.Message);
            }
        }
    }

}

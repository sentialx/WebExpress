using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WebExpress
{
    /// <summary>
    /// Interaction logic for NewsItem.xaml
    /// </summary>
    public partial class NewsItem : UserControl
    {
        private string _url;
        public double ThisHeight;
        private int ImageHeight;
        private string _description;
        private string _title;
        private static BrushConverter converter = new BrushConverter();
        public NewsItem(string title, string url, string description, string image, string category)
        {
            InitializeComponent();
            Img.Height = 150;
            ImageHeight = 150;
            ReadMore.Text("READ MORE");
            ReadMore.ChangeTextColor("#FFF");
            ReadMore.RippleColor(Colors.White);
            try
            {
                var bitmap = new BitmapImage(new Uri(image));
                Img.Source = bitmap;
            }
            catch
            {

            }
            CategoryLabel.Content = category;
            _title = title;

            _url = url;
            _description = description;
            label.Text = _title;
            if (_description.Contains("&#8226; "))
            {
                string replace = _description.Replace("&#8226; ", "");
                if (replace.Contains("&#34"))
                {
                    string replace2 = replace.Replace("&#34", "");
                    DescriptionText.Text = replace2;
                } else
                {
                    DescriptionText.Text = replace;
                }

            }
            else
            {
                if (_description.Contains("&#34"))
                {
                    string replace2 = _description.Replace("&#34", "");
                    DescriptionText.Text = replace2;
                }
                else
                {
                    DescriptionText.Text = _description;
                }

            }
            this.Width = 244;
            
            Loaded += NewsItem_Loaded;
        }

        private async void NewsItem_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            CategoryPanel.Width = CategoryLabel.ActualWidth + 10;
            label.Margin = new Thickness(15, 10 + ImageHeight, 0, 0);
            if (label.ActualHeight >= label.MaxHeight)
            {
                DescriptionText.Margin = new Thickness(15, 20 + (ImageHeight + label.MaxHeight), 0, 0);
            } else
            {
                DescriptionText.Margin = new Thickness(15, 20 + (ImageHeight + label.ActualHeight), 0, 0);
            }

            if (CategoryLabel.Content.Equals("Entertainment"))
            {
                CategoryPanel.Background = (SolidColorBrush)converter.ConvertFromString("#33ce75");
                ReadMore.Background = (SolidColorBrush)converter.ConvertFromString("#33ce75");
            }
            if (CategoryLabel.Content.Equals("Business"))
            {
                CategoryPanel.Background = (SolidColorBrush)converter.ConvertFromString("#f39c12");
                ReadMore.Background = (SolidColorBrush)converter.ConvertFromString("#f39c12");
            }
            if (CategoryLabel.Content.Equals("Information"))
            {
                CategoryPanel.Background = (SolidColorBrush)converter.ConvertFromString("#e74c3c");
                ReadMore.Background = (SolidColorBrush)converter.ConvertFromString("#e74c3c");
            }
            if (CategoryLabel.Content.Equals("Automotive"))
            {
                CategoryPanel.Background = (SolidColorBrush)converter.ConvertFromString("#3498db");
                ReadMore.Background = (SolidColorBrush)converter.ConvertFromString("#3498db");
            }
            if (CategoryLabel.Content.Equals("Sport"))
            {
                CategoryPanel.Background = (SolidColorBrush)converter.ConvertFromString("#9b59b6");
                ReadMore.Background = (SolidColorBrush)converter.ConvertFromString("#9b59b6");
            }

        }

        private void ClearHistoryBtn_Click(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.BeginInvoke(
    (Action)
        (() =>
        {
            try
            {
                ApplicationCommands.New.Execute(
                    new OpenTabCommandParameters(_url, "New tab", "#FFF9F9F9"), this);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Open news in new tab error: " + ex.Message + " " + ex.Data);
            }
        }));
        }

    }
}

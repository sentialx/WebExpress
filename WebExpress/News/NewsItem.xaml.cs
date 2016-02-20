using System;
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
        private static BrushConverter converter = new BrushConverter();
        public NewsItem(string title, string url, string description, string image, string category)
        {
            InitializeComponent();
            if (description.Contains("&#8226; "))
            {
                string replace = description.Replace("&#8226; ", "");
                DescriptionText.Text = replace;
            }
            else
            {
                DescriptionText.Text = description;
            }
            
            var bitmap = new BitmapImage(new Uri(image));
            Img.Source = bitmap;
            CategoryLabel.Content = category;
            label.Text = title;
            _url = url;
            Loaded += NewsItem_Loaded;
            this.Height = 350;
        }

        private void NewsItem_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (CategoryLabel.Content.Equals("Entertainment"))
            {
                CategoryPanel.Background = (SolidColorBrush)converter.ConvertFromString("#33ce75");
            }
            if (CategoryLabel.Content.Equals("Business"))
            {
                CategoryPanel.Background = (SolidColorBrush)converter.ConvertFromString("#f39c12");
            }
            if (CategoryLabel.Content.Equals("Information"))
            {
                CategoryPanel.Background = (SolidColorBrush)converter.ConvertFromString("#e74c3c");
            }
            if (CategoryLabel.Content.Equals("Automotive"))
            {
                CategoryPanel.Background = (SolidColorBrush)converter.ConvertFromString("#3498db");
            }
            if (CategoryLabel.Content.Equals("Sport"))
            {
                CategoryPanel.Background = (SolidColorBrush)converter.ConvertFromString("#9b59b6");
            }
            CategoryPanel.Width = 10 + CategoryLabel.Width;
            Canvas parent = this.Parent as Canvas;
            Grid parent2 = parent.Parent as Grid;
            NewsPanel parent3 = parent2.Parent as NewsPanel;
            parent3.RefreshNews(this);
        }

        private void ClearHistoryBtn_Click(object sender, System.Windows.RoutedEventArgs e)
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
                Console.WriteLine("Open news in new tab error: " + ex.Message);
            }
        }));
        }
    }
}

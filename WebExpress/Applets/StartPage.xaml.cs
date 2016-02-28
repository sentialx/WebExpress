using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using WebExpress.Controls;

namespace WebExpress
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : UserControl
    {
        private int ItemsInRowCount;
        private int RowsCount;
        private int ItemHeight;
        private int ItemLeft;
        private int RowTop;
        
        private List<NewsItem> list;

        public StartPage()
        {
            InitializeComponent();
            Loaded += StartPage_Loaded;
            ItemHeight = 400;
            RowsCount = 0;
            RowTop = 370;
            ItemLeft = 254;
            ItemsInRowCount = 0;
            list = new List<NewsItem>();


        }

        public void RefreshFavs(MainWindow mw)
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                try
                {
                    bookmarks.ItemsCount = 0;
                    bookmarks.RowsCount = 0;
                    bookmarks.mainCanvas.Children.Clear();
                    if (System.IO.File.Exists(StaticDeclarations.Bookmarkspath))
                    {
                        string readFile = System.IO.File.ReadAllText(StaticDeclarations.Bookmarkspath);
                        dynamic json = JsonConvert.DeserializeObject(readFile);
                        foreach (dynamic item in json)
                        {
                            TabView tabView = bookmarks.FindParent<TabView>();
                            bookmarks.AddBookmark(Convert.ToString(item.Url), Convert.ToString(item.Title), tabView, mw);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Refresh settings error: " + ex.Message + " " + ex.Data);
                }
            }));
        }

        public async Task LoadFavs(MainWindow mw)
        {
           await Dispatcher.BeginInvoke((Action) (() =>
            {
                try
                {
                    if (System.IO.File.Exists(StaticDeclarations.Bookmarkspath))
                    {
                        string readFile = System.IO.File.ReadAllText(StaticDeclarations.Bookmarkspath);
                        dynamic json = JsonConvert.DeserializeObject(readFile);
                        foreach (dynamic item in json)
                        {
                            TabView tabView = bookmarks.FindParent<TabView>();
                            bookmarks.AddBookmark(Convert.ToString(item.Url), Convert.ToString(item.Title), tabView, mw);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Load bookmarks error: " + ex.Message + " " + ex.Data);
                }
            }));
        }

        private async void StartPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTab();
            
        }

        private async Task LoadTab()
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;

            string readSettings = System.IO.File.ReadAllText("settings.json");


            dynamic settings = JsonConvert.DeserializeObject(readSettings);


            //Weather


            try
            {
                string weatherRead =
                    await wc.DownloadStringTaskAsync(
                        new Uri(
                            "http://api.openweathermap.org/data/2.5/weather?q=" + settings.City +
                            "&appid=44db6a862fba0b067b1930da0d769e98"));
                dynamic json3 = JsonConvert.DeserializeObject(weatherRead);

                if (settings.City != null)
                    City.Content = Convert.ToString(json3.name);
                else
                {
                    City.Content = "To configure news and weather";
                    WeatherHeader.Content = "Hello!";
                }

                foreach (dynamic info in json3.weather)
                    State.Content = Convert.ToString(info.main);
                if (State.Content.Equals("Clouds"))
                {
                    var bmp = new BitmapImage(new Uri("pack://application:,,,/Resources/cloud.png"));
                    WeatherImage.Source = bmp;
                }
                if (State.Content.Equals("Rain"))
                {
                    var bmp = new BitmapImage(new Uri("pack://application:,,,/Resources/rain.png"));
                    WeatherImage.Source = bmp;
                }
                if (State.Content.Equals("Snow"))
                {
                    var bmp = new BitmapImage(new Uri("pack://application:,,,/Resources/snow.png"));
                    WeatherImage.Source = bmp;
                }
                if (State.Content.Equals("Clear"))
                {
                    var bmp = new BitmapImage(new Uri("pack://application:,,,/Resources/sun.png"));
                    WeatherImage.Source = bmp;
                }


                Temp.Content = Math.Round(Convert.ToDouble(json3.main.temp) - 273.15, 1) + "°C";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Load weather error: " + ex.Message + " " + ex.Data);
                City.Content = "To configure news and weather";
                WeatherHeader.Content = "Hello!";
            }


            //News
  

                if (Directory.Exists("News"))
                {
                    foreach (string file in System.IO.Directory.GetFiles("News", "*.news"))
                    {
                        string readContent = File.ReadAllText(file);
                        dynamic json = JsonConvert.DeserializeObject(readContent);


                        if (json.Category == "Information")
                        {
                            if (Convert.ToString(settings.Information) == "True")
                            {
                                await LoadNews(Convert.ToString(json.Category),
                                    Convert.ToString(json.Link));
                            }

                        }
                        if (json.Category == "Entertainment")
                        {
                            if (Convert.ToString(settings.Entertainment) == "True")
                            {
                            await LoadNews(Convert.ToString(json.Category),
                              Convert.ToString(json.Link));
                        }

                        }
                        if (json.Category == "Sport")
                        {
                            if (Convert.ToString(settings.Sport) == "True")
                            {
                            await LoadNews(Convert.ToString(json.Category),
                                Convert.ToString(json.Link));
                        }

                        }
                        if (json.Category == "Business")
                        {
                            if (Convert.ToString(settings.Business) == "True")
                            {
                            await LoadNews(Convert.ToString(json.Category),
                                Convert.ToString(json.Link));
                        }

                        }
                        if (json.Category == "Automotive")
                        {
                            if (Convert.ToString(settings.Moto) == "True")
                            {
                            await LoadNews(Convert.ToString(json.Category),
                                Convert.ToString(json.Link));
                        }


                        }
                    }
                }

            //Information

            if (Convert.ToString(settings.Information) == "True")
                {
                    await LoadNews("Information",
                        "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://wiadomosci.wp.pl/ver,rss,rss.xml");
                }

                //Moto

                if (Convert.ToString(settings.Moto) == "True")
                {
                    await LoadNews("Automotive",
                        "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://moto.wp.pl/rss.xml");
                }

                //Business

                if (Convert.ToString(settings.Business) == "True")
                {
                    await LoadNews("Business",
                        "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://finanse.wp.pl/rss.xml");
                }

                //Entertainment

                if (Convert.ToString(settings.Entertainment) == "True")
                {
                    await LoadNews("Entertainment",
                        "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://ksiazki.wp.pl/rss.xml");
                    await LoadNews("Entertainment",
                        "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://gry.wp.pl/rss/wiadomosci.xml");
                    await LoadNews("Entertainment",
                        "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://muzyka.wp.pl/rss.xml");
                    await LoadNews("Entertainment",
                        "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://kultura.wp.pl/rss.xml");
                    await LoadNews("Entertainment",
                        "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://film.wp.pl/rss.xml?ticaid=116841&_ticrsn=3");
                }

                //Sport

                if (Convert.ToString(settings.Sport) == "True")
                {
                    await LoadNews("Sport",
                        "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://interia.pl.feedsportal.com/c/34004/f/625088/index.rss");
                }

        }
     
        private async Task LoadNews(string category, string url)
        {
            await Dispatcher.BeginInvoke((Action)(async () =>
            {
                try { 
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                string readFile =
                   await wc.DownloadStringTaskAsync(
                        new Uri(
                            url));
                dynamic json = JsonConvert.DeserializeObject(readFile);
                    foreach (dynamic item in json.responseData.feed.entries)
                    {
                        if (ItemsInRowCount != 3)
                        {
                            if (!GetImage(Convert.ToString(item.content)).Equals(""))
                            {
                                NewsItem newsItem = new NewsItem(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), GetImage(Convert.ToString(item.content)),
                                category);
                                Canvas canvas1 = new Canvas();
                                mainCanvas.Children.Add(canvas1);
                                Canvas.SetTop(canvas1, RowsCount * RowTop);
                                Canvas.SetLeft(newsItem, ItemsInRowCount * ItemLeft);

                                newsItem.Height = 350;
                                canvas1.Children.Add(newsItem);
                                ItemsInRowCount += 1;
                                if (ItemsInRowCount == 3)
                                {
                                    ItemsInRowCount = 0;
                                    RowsCount += 1;
                                }
                                mainCanvas.Height = (RowsCount + 1) * RowTop;

                            }
                        }
                    }
                } catch (Exception ex)
                {
                    Console.WriteLine("Load news error: " + ex.Message + " " + ex.Data);
                }
            }));


        }
        private string GetImage(string content)
        {
           
            string img = Convert.ToString(content);
            if (img.Contains("<img src="))
            {
                string[] split = Regex.Split(img, "<img src=");

                Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                string replace = m.ToString().Replace("\"", "");

                    return replace;
                
            }
            return "";
        }

    }
}
using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebExpress.Controls;

namespace WebExpress
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : UserControl
    {
        private int ItemsCount = 0;
        private int ItemHeight;

        public StartPage()
        {
            InitializeComponent();
            Loaded += StartPage_Loaded;
            ItemHeight = 380;
        }

        public void RefreshFavs(MainWindow mw)
        {
            Dispatcher.BeginInvoke((Action) (() =>
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
            }));
        }

        public void LoadFavs(MainWindow mw)
        {
            Dispatcher.BeginInvoke((Action) (() =>
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
            catch
            {

            }


            //News


            double globalCount = 0;

            //Information

            if (Convert.ToString(settings.Information) == "True")
            {
                string readFile =
                    await wc.DownloadStringTaskAsync(
                        new Uri(
                            "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://wiadomosci.wp.pl/ver,rss,rss.xml"));
                dynamic json = JsonConvert.DeserializeObject(readFile);
                foreach (dynamic item in json.responseData.feed.entries)
                {
                    if (ItemsCount == globalCount / 2)
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");

                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");


                            News2.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Information");
                            globalCount += 1;
                            NewsContainer.Height = ItemHeight * (globalCount / 2);
                        }
                    }
                    else
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");

                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");

                            News.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Information");
                            globalCount += 1;
                            ItemsCount += 1;
                            NewsContainer.Height = ItemHeight * (globalCount / 2);
                        }
                    }
                }
            }

            //Moto


            if (Convert.ToString(settings.Moto) == "True")
            {
                string readFile =
                    await wc.DownloadStringTaskAsync(
                        new Uri(
                            "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://moto.wp.pl/rss.xml"));
                dynamic json = JsonConvert.DeserializeObject(readFile);
                foreach (dynamic item in json.responseData.feed.entries)
                {
                    if (ItemsCount == globalCount/2)
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");

                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");


                            News2.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Automotive");
                            globalCount += 1;
                            NewsContainer.Height = ItemHeight* (globalCount / 2);
                        }
                    }
                    else
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");

                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");

                            News.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Automotive");
                            globalCount += 1;
                            ItemsCount += 1;
                            NewsContainer.Height = ItemHeight* (globalCount / 2);
                        }
                    }
                }
            }

            //Business


            if (Convert.ToString(settings.Business) == "True")
            {
                string readFile =
                    await wc.DownloadStringTaskAsync(
                        new Uri(
                            "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://news.yahoo.com/rss/business"));
                dynamic json = JsonConvert.DeserializeObject(readFile);
                foreach (dynamic item in json.responseData.feed.entries)
                {
                    if (ItemsCount == globalCount/2)
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");

                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");


                            News2.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Business");
                            globalCount += 1;
                            NewsContainer.Height = ItemHeight* (globalCount / 2);
                        }
                    }
                    else
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");

                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");

                            News.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Business");
                            globalCount += 1;
                            ItemsCount += 1;
                            NewsContainer.Height = ItemHeight* (globalCount / 2);
                        }
                    }
                }
            }


            //Entertainment

            if (Convert.ToString(settings.Entertainment) == "True")
            {
                string readFile =
                    await wc.DownloadStringTaskAsync(
                        new Uri(
                            "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://gry.wp.pl/rss/wiadomosci.xml"));
                dynamic json = JsonConvert.DeserializeObject(readFile);
                foreach (dynamic item in json.responseData.feed.entries)
                {
                    if (ItemsCount == globalCount/2)
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");

                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");


                            News2.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Entertainment");
                            globalCount += 1;
                            NewsContainer.Height = ItemHeight* (globalCount / 2);
                        }
                    }

                    else
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");


                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");

                            News.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Entertainment");
                            globalCount += 1;
                            ItemsCount += 1;
                            NewsContainer.Height = ItemHeight* (globalCount / 2);
                        }
                    }
                }
            }

            if (Convert.ToString(settings.Entertainment) == "True")
            {
                string readFile =
                    await wc.DownloadStringTaskAsync(
                        new Uri(
                            "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://film.wp.pl/rss.xml?ticaid=116841&_ticrsn=3"));
                dynamic json = JsonConvert.DeserializeObject(readFile);
                foreach (dynamic item in json.responseData.feed.entries)
                {
                    if (ItemsCount == globalCount / 2)
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");

                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");


                            News2.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Entertainment");
                            globalCount += 1;
                            NewsContainer.Height = ItemHeight * (globalCount / 2);
                        }
                    }

                    else
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");


                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");

                            News.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Entertainment");
                            globalCount += 1;
                            ItemsCount += 1;
                            NewsContainer.Height = ItemHeight * (globalCount / 2);
                        }
                    }
                }
            }

            if (Convert.ToString(settings.Entertainment) == "True")
            {
                string readFile =
                    await wc.DownloadStringTaskAsync(
                        new Uri(
                            "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://kultura.wp.pl/rss.xml"));
                dynamic json = JsonConvert.DeserializeObject(readFile);
                foreach (dynamic item in json.responseData.feed.entries)
                {
                    if (ItemsCount == globalCount / 2)
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");

                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");


                            News2.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Entertainment");
                            globalCount += 1;
                            NewsContainer.Height = ItemHeight * (globalCount / 2);
                        }
                    }

                    else
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");


                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");

                            News.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Entertainment");
                            globalCount += 1;
                            ItemsCount += 1;
                            NewsContainer.Height = ItemHeight * (globalCount / 2);
                        }
                    }
                }
            }

            if (Convert.ToString(settings.Entertainment) == "True")
            {
                string readFile =
                    await wc.DownloadStringTaskAsync(
                        new Uri(
                            "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://muzyka.wp.pl/rss.xml"));
                dynamic json = JsonConvert.DeserializeObject(readFile);
                foreach (dynamic item in json.responseData.feed.entries)
                {
                    if (ItemsCount == globalCount / 2)
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");

                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");


                            News2.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Entertainment");
                            globalCount += 1;
                            NewsContainer.Height = ItemHeight * (globalCount / 2);
                        }
                    }

                    else
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");


                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");

                            News.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Entertainment");
                            globalCount += 1;
                            ItemsCount += 1;
                            NewsContainer.Height = ItemHeight * (globalCount / 2);
                        }
                    }
                }
            }

            if (Convert.ToString(settings.Entertainment) == "True")
            {
                string readFile =
                    await wc.DownloadStringTaskAsync(
                        new Uri(
                            "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://ksiazki.wp.pl/rss.xml"));
                dynamic json = JsonConvert.DeserializeObject(readFile);
                foreach (dynamic item in json.responseData.feed.entries)
                {
                    if (ItemsCount == globalCount / 2)
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");

                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");


                            News2.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Entertainment");
                            globalCount += 1;
                            NewsContainer.Height = ItemHeight * (globalCount / 2);
                        }
                    }

                    else
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");


                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");

                            News.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Entertainment");
                            globalCount += 1;
                            ItemsCount += 1;
                            NewsContainer.Height = ItemHeight * (globalCount / 2);
                        }
                    }
                }
            }

            if (Convert.ToString(settings.Sport) == "True")
            {
                string readFile =
                    await wc.DownloadStringTaskAsync(
                        new Uri(
                            "http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=http://interia.pl.feedsportal.com/c/34004/f/625088/index.rss"));
                dynamic json = JsonConvert.DeserializeObject(readFile);
                foreach (dynamic item in json.responseData.feed.entries)
                {
                    if (ItemsCount == globalCount / 2)
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");

                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");


                            News2.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Sport");
                            globalCount += 1;
                            NewsContainer.Height = ItemHeight * (globalCount / 2);
                        }
                    }

                    else
                    {
                        string img = Convert.ToString(item.content);
                        if (img.Contains("<img src="))
                        {
                            string[] split = Regex.Split(img, "<img src=");


                            Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                            string replace = m.ToString().Replace("\"", "");

                            News.AddNews(Convert.ToString(item.title),
                                Convert.ToString(item.link), Convert.ToString(item.contentSnippet), replace, "Sport");
                            globalCount += 1;
                            ItemsCount += 1;
                            NewsContainer.Height = ItemHeight * (globalCount / 2);
                        }
                    }
                }
            }


        }
    }
}
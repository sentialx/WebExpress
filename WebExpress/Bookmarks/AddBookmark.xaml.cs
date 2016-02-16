using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace WebExpress
{
    /// <summary>
    /// Interaction logic for AddBookmark.xaml
    /// </summary>
    public partial class AddBookmark : UserControl
    {
        private string _url;
        
        private MainWindow mainWindow;
        public AddBookmark(string title, string url, MainWindow mw)
        {
            InitializeComponent();
            TitleBox.Text = title;
            _url = url;
            mainWindow = mw;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                if (!System.IO.File.Exists(StaticDeclarations.Bookmarkslayoutpath))
                {
                    string filePath = StaticDeclarations.Bookmarkslayoutpath;
                    using (StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        sw.WriteLine("<!DOCTYPE><html><head><title>WebExpress - bookmarks</title><link href='https://fonts.googleapis.com/css?family=Lato:300' rel='stylesheet' type='text/css'></head> <body><style>body {overflow:hidden;}</style><div id='div1'> <h1 style='font-family: lato; padding-left: 50px; padding-top: 20px;'>WebExpress - bookmarks</h1><iframe name='iframe1' id='iframe1' src='bookmarks-data.html' frameborder='0' border='0' cellspacing='0' style='border-style: none;width: 100vmin; padding-left: 45px; height:100vmin;'></iframe></div> </body></html>");
                        sw.Close();
                    }
                }
                string filePath1 = StaticDeclarations.Bookmarkspath;
                using (StreamWriter sw = new StreamWriter(filePath1, true))
                {
                    sw.WriteLine("<style>p { font-family: 'Arial'; } a {color: #606060; text-decoration: none;} a:hover { color: #000; } a:visited { color:#606060; } </style><p><img src='http://www.google.com/s2/favicons?domain=" + _url + "' style='width: 16; height:16;'/>" + " " + TitleBox.Text + "  <a target='_blank' href=" + _url + ">  -  " + _url + "</a></p>");
                    sw.Close();
                }
                string filePath2 = StaticDeclarations.Bookspath;
                using (StreamWriter sw = new StreamWriter(filePath2, true))
                {
                    sw.WriteLine(_url + "*" + TitleBox.Text);
                    sw.Close();
                }
            }
            catch
            {
            }
            var fade = new DoubleAnimation
            {
                From = ActualHeight,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.20)
            };
            Storyboard.SetTarget(fade, this);
            Storyboard.SetTargetProperty(fade, new PropertyPath(HeightProperty));

            var sb = new Storyboard();
            sb.Children.Add(fade);
            sb.Completed +=
                (o, e1) =>
                {
                    Grid parent = this.Parent as Grid;
                    parent.Children.Remove(this);
                };
            sb.Begin();

            foreach (TabView tab in mainWindow.Pages)
            {
                tab.startPage.refreshFavs(mainWindow);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var fade = new DoubleAnimation
            {
                From = ActualHeight,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.20)
            };
            Storyboard.SetTarget(fade, this);
            Storyboard.SetTargetProperty(fade, new PropertyPath(HeightProperty));

            var sb = new Storyboard();
            sb.Children.Add(fade);
            sb.Completed +=
                (o, e1) =>
                {
                    Grid parent = this.Parent as Grid;
                    parent.Children.Remove(this);
                };
            sb.Begin();
        }

        private void closeButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            StaticFunctions.ChangeButtonImage("close_button_hover.png", CloseImage);
        }

        private void closeButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            StaticFunctions.ChangeButtonImage("close_button.png", CloseImage);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                var fade = new DoubleAnimation
                {
                    From = 0,
                    To = 175,
                    Duration = TimeSpan.FromSeconds(0.20)
                };
                Storyboard.SetTarget(fade, this);
                Storyboard.SetTargetProperty(fade, new PropertyPath(HeightProperty));

                var sb = new Storyboard();
                sb.Children.Add(fade);
                sb.Begin();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Newtonsoft.Json;

namespace WebExpress
{
    /// <summary>
    ///     Interaction logic for AddBookmark.xaml
    /// </summary>
    public partial class AddBookmark : UserControl
    {
        private readonly string _url;

        private readonly MainWindow mainWindow;

        public AddBookmark(string title, string url, MainWindow mw)
        {
            InitializeComponent();
            TitleBox.Text = title;
            _url = url;
            mainWindow = mw;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(StaticDeclarations.Bookmarkspath))
            {
                var bookmark = new BookItem();
                bookmark.Title = TitleBox.Text;
                bookmark.Url = _url;
                var bookmarks = new List<BookItem>();
                bookmarks.Add(bookmark);
                var newJson = JsonConvert.SerializeObject(bookmarks);
                File.WriteAllText(StaticDeclarations.Bookmarkspath, newJson);
            }
            else
            {
                var bookmark = new BookItem();
                bookmark.Title = TitleBox.Text;
                bookmark.Url = _url;
                var json = File.ReadAllText(StaticDeclarations.Bookmarkspath);
                var bookmarks = JsonConvert.DeserializeObject<List<BookItem>>(json);
                bookmarks.Add(bookmark);
                var newJson = JsonConvert.SerializeObject(bookmarks);
                File.WriteAllText(StaticDeclarations.Bookmarkspath, newJson);
            }


            ExecuteStoryboard();

            foreach (var tab in mainWindow.Pages)
            {
                tab.startPage.RefreshFavs(mainWindow);
            }
        }

        private void ExecuteStoryboard()
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
                    var parent = Parent as Grid;
                    parent.Children.Remove(this);
                };
            sb.Begin();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ExecuteStoryboard();
        }

        private void closeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            StaticFunctions.ChangeButtonImage("close_button_hover.png", CloseImage);
        }

        private void closeButton_MouseLeave(object sender, MouseEventArgs e)
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
                    To = 158,
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
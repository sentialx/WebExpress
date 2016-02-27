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
            DoneButton.Text("DONE");
            CloseButton.ImageSource("close_button.png");
            CloseButton.SetRippleMargin(1);
            CloseButton.SetImageScale(16);
            DoneButton.ChangeTextColor("#FF1ABC9C");
            mainWindow = mw;
        }

        private void button_Click(object sender, MouseButtonEventArgs e)
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

        public void ExecuteStoryboard()
        {
            try
            {
                StaticFunctions.AnimateScale(260, 178, 0, 0, this, 0.2);
                var fade = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.3)
                };
                Storyboard.SetTarget(fade, this);
                Storyboard.SetTargetProperty(fade, new PropertyPath(UIElement.OpacityProperty));

                var sb = new Storyboard();
                sb.Children.Add(fade);
                sb.Begin();


                sb.Completed +=
                    (o, e1) =>
                    {
                        var parent = Parent as Grid;
                        parent.Children.Remove(this);
                    };
                sb.Begin();
            } catch { }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                StaticFunctions.AnimateScale(0, 0, 260, 178, this, 0.2);
                StaticFunctions.AnimateFade(0, 1, this, 0.3);
            }
        }

        private void UIElement_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ExecuteStoryboard();
        }
    }
}
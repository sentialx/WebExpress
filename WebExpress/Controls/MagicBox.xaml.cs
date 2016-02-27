using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WebExpress.Controls
{
    /// <summary>
    /// Interaction logic for MagicBox.xaml
    /// </summary>
    public partial class MagicBox : UserControl
    {
        public int ItemsCount;
        public int ItemHeight;
        private int currentIndex;
        public List<MagicBoxItem> ItemList;
        public bool _isDark;
        public MagicBox()
        {
            InitializeComponent();
            ItemsCount = 0;
            ItemHeight = 36;
            ItemList = new List<MagicBoxItem>();
        }

        public async Task AddSuggestion(string title, string url, MainWindow mw)
        {
            if (ItemsCount*ItemHeight > 180)
            {

            }
            else
            {
                
                MagicBoxItem mbi = new MagicBoxItem(title, url, mw);
                mbi.VerticalAlignment = VerticalAlignment.Top;
                mbi.HorizontalAlignment = HorizontalAlignment.Stretch;
                mbi.Height = ItemHeight;
                mbi.Margin = new Thickness(0, ItemHeight*ItemsCount, 0, 0);
                container.Children.Add(mbi);
                mbi.Url.MaxWidth = this.ActualWidth/2;
                mbi.Title.MaxWidth = this.ActualWidth/3;
                ItemList.Add(mbi);
                mbi.parent = this;
                ItemsCount += 1;
                RefreshSizes();
                if (_isDark)
                {
                    mbi.IsDark = true;
                }
                else
                {
                    mbi.IsDark = false;
                }
            }
        }

        public void Clear()
        {
            container.Children.Clear();
            ItemsCount = 0;
            ItemList = new List<MagicBoxItem>();
            RefreshSizes();
        }

        private void MagicBox_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            RefreshSizes();
        }

        public void RefreshSizes()
        {
            foreach (MagicBoxItem mbi in ItemList)
            {
                mbi.Url.MaxWidth = this.ActualWidth / 1;
                mbi.Title.MaxWidth = this.ActualWidth / 3;
            }
        }

        private void UserControl_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (_isDark)
                {
                    StaticFunctions.AnimateColor(Colors.Transparent, Color.FromArgb(50, 255, 255, 255), ItemList[currentIndex + 1], 0.2);
                }
                else
                {
                    StaticFunctions.AnimateColor(Colors.Transparent, Colors.Gainsboro, ItemList[currentIndex + 1], 0.2);
                }
                if (currentIndex == ItemList.Count)
                {
                    currentIndex = 0;
                }
                else
                {
                    currentIndex += 1;
                }
            }
        }
    }
}

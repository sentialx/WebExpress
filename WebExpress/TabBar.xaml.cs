using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WebExpress
{
    public partial class TabBar : UserControl
    {
        private static object[] infoToPass;
        private int AddButtonCount;
        private MainWindow mainWindow;
        private int pixelsSoFar;
        private Tab tab;
        public List<Tab> TabCollection;
        private int TabCount;

        public TabBar()
        {
            InitializeComponent();
            TabCount = 0;
            pixelsSoFar = 0;
            AddButtonCount = 1;
            TabCollection = new List<Tab>();
        }

        public void AddTab(string title, MainWindow mw)
        {
            tab = new Tab(title, mw);


            tab.Width = 170;
            tab.Height = 26;
            canvas.Children.Add(tab);
            Canvas.SetLeft(tab, TabCount*170);
            Canvas.SetTop(tab, 0);
            Canvas.SetTop(AddButton, 0);
            if (AddButtonCount == 1)
            {
                AddButton.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(0, AddButtonCount*170, TimeSpan.FromMilliseconds(250)));
            }
            else
            {
                AddButton.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(Canvas.GetLeft(AddButton), AddButtonCount * 170, TimeSpan.FromMilliseconds(100)));
            }
            TabCount += 1;
            AddButtonCount += 1;
            TabCollection.Add(tab);
            

            
                var fade = new DoubleAnimation()
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.25),
                };
                Storyboard.SetTarget(fade, tab);
                Storyboard.SetTargetProperty(fade, new PropertyPath(Button.OpacityProperty));

                var sb = new Storyboard();
                sb.Children.Add(fade);
                sb.Begin();
            



            CalcSizes();
            mainWindow = mw;
        }
        public Tab getTabFromForm(TabView form)
        {

            Tab tempTab = TabCollection[0];
            foreach (Tab ctrl in TabCollection)
            {
                if (ctrl.form.Equals(form))
                {
                    tempTab = ctrl;
                }
            }
            return tempTab;
        }

        public void SelectTab(Tab tabSelect)
        {
            foreach (var tab in TabCollection)
            {
                if (tab == tabSelect)
                {
                    tab.bgTab = false;
                    tab.form.Visibility = Visibility.Visible;
                }
                else
                {
                    tab.bgTab = true;
                    tab.form.Visibility = Visibility.Hidden;
                }
            }
            CalcSizes();
        }

        public void RemoveTab(Tab tabToRemove)
        {
            TabCount = 0;
            AddButtonCount = 1;


            TabCollection.Remove(tabToRemove);
            canvas.Children.Remove(tabToRemove);
            foreach (var ctrl in TabCollection)
            {
                ctrl.Width = 170;
                Canvas.SetLeft(ctrl, TabCount*171);
                AddButton.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(Canvas.GetLeft(AddButton), AddButtonCount*170, TimeSpan.FromMilliseconds(100)));
                TabCount += 1;
                AddButtonCount += 1;
                CalcSizes();
                tabToRemove.mainWindow.container.Children.Remove(tabToRemove.form);
                SelectTab(TabCollection[TabCollection.Count - 1]);
            }
            
        }

        private void Sb_Completed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddTab("New tab", mainWindow);
            CalcSizes();
        }

        public void CalcSizes()
        {
            if ((TabCount == 2 & 171*TabCount > ActualWidth - AddButton.ActualWidth) |
                (TabCount > 2 & 171*TabCount > ActualWidth - AddButton.ActualWidth))
            {
                var TabCount1 = 0;
                var AddButtonCount1 = 1;
                foreach (var ctrl in TabCollection)
                {
                    try
                    {
                        ctrl.Width = (ActualWidth - AddButton.ActualWidth)/TabCount;

                        Canvas.SetLeft(ctrl, TabCount1*ctrl.Width);
                        AddButton.BeginAnimation(Canvas.LeftProperty,
    new DoubleAnimation(AddButtonCount1 * ctrl.Width, AddButtonCount1 * ctrl.Width, TimeSpan.FromMilliseconds(250)));
                        TabCount1 += 1;
                        AddButtonCount1 += 1;
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }
        }
        public double GetWidth()
        {
            double width = 0;

               
                foreach (var ctrl in TabCollection)
                {
                    try
                    {
                        width = (ActualWidth - AddButton.ActualWidth) / TabCount;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            return width;
        }
            
        

        private void TabBar1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CalcSizes();
        }
    }
}
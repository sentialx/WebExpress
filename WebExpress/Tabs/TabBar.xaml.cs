using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WebExpress
{
    public partial class TabBar : UserControl
    {
        private int AddButtonCount;
        private MainWindow mainWindow;
        private readonly int moveButtonDuration;
        private int pixelsSoFar;
        private Tab tab;
        public List<Tab> TabCollection;
        private int TabCount;
        private readonly int tabHeight;
        private readonly int tabWidth;

        public TabBar()
        {
            InitializeComponent();
            TabCount = 0;
            tabHeight = 26;
            moveButtonDuration = 100;
            pixelsSoFar = 0;
            tabWidth = 170;
            AddButtonCount = 1;
            TabCollection = new List<Tab>();
        }

        public void AddTab(OpenTabCommandParameters commandParams, MainWindow mw)
        {
            UserControl userControl;

            if (commandParams.Control == null)
            {
                userControl = new TabView(mw, commandParams.Url);
            }
            else
            {
                userControl = commandParams.Control;
            }

            Dispatcher.BeginInvoke((Action) (() =>
            {
                mainWindow = mw;
                tab = new Tab(commandParams.Title, mw, userControl, commandParams.Brush);

                tab.Width = tabWidth;
                tab.Height = tabHeight;
                canvas.Children.Add(tab);
                Canvas.SetLeft(tab, TabCount*tabWidth);
                Canvas.SetTop(tab, 0);
                Canvas.SetTop(AddButton, 0);
                if (AddButtonCount == 1)
                {
                    AddButton.BeginAnimation(Canvas.LeftProperty,
                        new DoubleAnimation(0, AddButtonCount*tabWidth, TimeSpan.FromMilliseconds(moveButtonDuration)));
                }
                else
                {
                    AddButton.BeginAnimation(Canvas.LeftProperty,
                        new DoubleAnimation(Canvas.GetLeft(AddButton), AddButtonCount*tabWidth,
                            TimeSpan.FromMilliseconds(moveButtonDuration)));
                }
                TabCount += 1;
                AddButtonCount += 1;
                TabCollection.Add(tab);

                var fade = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.25)
                };
                Storyboard.SetTarget(fade, tab);
                Storyboard.SetTargetProperty(fade, new PropertyPath(OpacityProperty));

                var sb = new Storyboard();
                sb.Children.Add(fade);
                sb.Completed += (o, e1) =>
                {
                    CalcSizes();
                };
                sb.Begin();

                
            }));
        }

        public Tab getTabFromForm(UserControl form)
        {
            foreach (var ctrl in TabCollection)
            {
                if (ctrl.form.Equals(form))
                {
                    return ctrl;
                }
            }
            return TabCollection[0];
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
                ctrl.Width = tabWidth;
                ctrl.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(Canvas.GetLeft(ctrl), TabCount*tabWidth,
                        TimeSpan.FromMilliseconds(moveButtonDuration)));
                AddButton.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(Canvas.GetLeft(AddButton), AddButtonCount*tabWidth,
                        TimeSpan.FromMilliseconds(moveButtonDuration)));
                TabCount += 1;
                AddButtonCount += 1;
                CalcSizes();
                tabToRemove.mainWindow.container.Children.Remove(tabToRemove.form);
                if (tabToRemove.form.GetType() == typeof (TabView))
                {
                    var tv = tabToRemove.form as TabView;
                    tv.Shutdown();
                }
                if (!tabToRemove.bgTab)
                {
                    SelectTab(TabCollection[TabCollection.Count - 1]);
                }
            }
            if (TabCount == 0)
            {
                Application.Current.Shutdown();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //Raise command to open a new tab
            ApplicationCommands.New.Execute(new OpenTabCommandParameters(string.Empty, "New tab", "#FFF9F9F9"), this);

        }

        public void CalcSizes()
        {
            if ((TabCount == 2 & tabWidth*TabCount > ActualWidth - AddButton.ActualWidth) |
                (TabCount > 2 & tabWidth*TabCount > ActualWidth - AddButton.ActualWidth))
            {
                var TabCount1 = 0;
                var AddButtonCount1 = 1;
                foreach (var ctrl in TabCollection)
                {
                    try
                    {
                        ctrl.Width = (ActualWidth - AddButton.ActualWidth)/TabCount;

                        AddButton.BeginAnimation(Canvas.LeftProperty,
                            new DoubleAnimation(AddButtonCount1*ctrl.Width, AddButtonCount1*ctrl.Width,
                                TimeSpan.FromMilliseconds(moveButtonDuration)));
                        ctrl.BeginAnimation(Canvas.LeftProperty,
                            new DoubleAnimation(Canvas.GetLeft(ctrl), TabCount1*ctrl.Width,
                                TimeSpan.FromMilliseconds(moveButtonDuration)));

                        TabCount1 += 1;
                        AddButtonCount1 += 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("CalcSizes error: " + ex.Message);
                    }
                }
            }
            else
            {
                var TabCount1 = 0;
                var AddButtonCount1 = 1;
                foreach (var ctrl in TabCollection)
                {
                    try
                    {
                        ctrl.Width = tabWidth;
                        AddButton.BeginAnimation(Canvas.LeftProperty,
                            new DoubleAnimation(Canvas.GetLeft(AddButton), AddButtonCount1 * tabWidth,
                                TimeSpan.FromMilliseconds(moveButtonDuration)));
                        ctrl.BeginAnimation(Canvas.LeftProperty,
                            new DoubleAnimation(Canvas.GetLeft(ctrl), TabCount1 * tabWidth,
                                TimeSpan.FromMilliseconds(moveButtonDuration)));

                        TabCount1 += 1;
                        AddButtonCount1 += 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("CalcSizes error: " + ex.Message);
                    }
                }
            }
            

        }

        private void TabBar1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CalcSizes();
        }
    }
}
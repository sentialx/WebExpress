using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WebExpress.Controls;

namespace WebExpress
{

    public partial class DownloadItem : UserControl
    {
        private WebClient webClient;
        private bool Downloaded;
        private string _filepath ;
        public DownloadItem(string filepath, string url, string filename)
        {
            Downloaded = false;
            InitializeComponent();
            webClient = new WebClient();
            webClient.DownloadFileAsync(new Uri(url), filepath + "\\" + filename);
            FileName.Text = filename;
            _filepath = filepath;
            button.SetRippleMargin(1);
           button.ImageSource("close_button.png");
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
        }


        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Downloaded = true;
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Size.Text = StaticFunctions.ConvertBytesToMegabytes(e.BytesReceived) + "MB / " + StaticFunctions.ConvertBytesToMegabytes(e.TotalBytesToReceive);
            ProgressBar.Width = e.ProgressPercentage * (this.ActualWidth / 100);
        }

        private void button_Click(object sender, MouseButtonEventArgs e)
        {
            webClient.Dispose();
            Downloads downloads = this.FindParent<Downloads>();
            
            downloads.RemoveDownload(this);

        }



        private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BrushConverter converter = new BrushConverter();

            bg.Background = (SolidColorBrush) converter.ConvertFromString("#FFE2E2E2");
            SolidColorBrush scb = (SolidColorBrush) converter.ConvertFromString("#FFE2E2E2");
            Color color = scb.Color;
            StaticFunctions.AnimateColor(Colors.White, color, bg, 0.2);
        }

        private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BrushConverter converter = new BrushConverter();

            bg.Background = (SolidColorBrush)converter.ConvertFromString("#FFF");
            SolidColorBrush scb = (SolidColorBrush)converter.ConvertFromString("#FFE2E2E2");
            Color color = scb.Color;
            StaticFunctions.AnimateColor(color, Colors.White, bg, 0.2);
        }

        private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Downloaded)
            {
                {
                    try
                    {
                        Process.Start(_filepath + "\\" + FileName.Text);
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void MenuItem_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {

            Process.Start(_filepath);
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Downloaded)
            {
                {
                    try
                    {
                        Process.Start(_filepath + "\\" + FileName.Text);
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}

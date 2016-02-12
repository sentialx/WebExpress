using System;
using System.Net;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace WebExpress
{

    public partial class DownloadItem : UserControl
    {
        private WebClient webClient;
        public DownloadItem(string filepath, string url, string filename)
        {

            InitializeComponent();
            webClient = new WebClient();
            webClient.DownloadFileAsync(new Uri(url), filepath + "\\" + filename);
            FileName.Text = filename;
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            return Math.Round((bytes / 1024f) / 1024f, 2);
        }
        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Size.Text = ConvertBytesToMegabytes(e.BytesReceived) + "MB / " + ConvertBytesToMegabytes(e.TotalBytesToReceive);
            ProgressBar.Width = e.ProgressPercentage * (this.ActualWidth / 100);
        }

        private void button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            webClient.Dispose();
            Canvas parent = this.Parent as Canvas;
            Grid parent1 = parent.Parent as Grid;
            Downloads downloads = parent1.Parent as Downloads;
            downloads.RemoveDownload(this);

        }
    }
}

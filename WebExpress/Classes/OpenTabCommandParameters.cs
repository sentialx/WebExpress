using System.Windows.Media;
using System.Windows.Controls;

namespace WebExpress
{
	public class OpenTabCommandParameters
	{
		private static BrushConverter converter = new BrushConverter();

		public string Url { get; private set; }
		public string Title { get; private set; }
		public SolidColorBrush Brush { get; private set; }
		public UserControl Control { get; private set;}

		public OpenTabCommandParameters(string url, string title, string brush)
		{
			Url = url;
			Title = title;
			Brush = (SolidColorBrush)converter.ConvertFromString(brush);
		}

		public OpenTabCommandParameters(UserControl control, string title, string brush)
		{
			Control = control;
			Title = title;
			Brush = (SolidColorBrush)converter.ConvertFromString(brush);
		}
        public OpenTabCommandParameters(UserControl control, string url, string title, string brush)
        {
            Control = control;
            Title = title;
            Url = url;
            Brush = (SolidColorBrush)converter.ConvertFromString(brush);
        }
    }
}

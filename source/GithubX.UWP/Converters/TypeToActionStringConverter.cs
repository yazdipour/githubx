using System;
using Windows.UI.Xaml.Data;

namespace GithubX.UWP.Converters
{
	internal class TypeToActionStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var str = (string)value;
			if (str.Contains("Watch"))
				str = "starred";
			else if (str.Contains("Fork"))
				str = "forked";
			else if (str.Contains("Create"))
				str = "created";
			return str;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}

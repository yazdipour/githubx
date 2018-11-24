using System;
using Windows.UI.Xaml.Data;

namespace GithubX.UWP.Converters
{
	internal class StringConcatConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var res = language.Equals("End")
					   ? (string)parameter + (string)value
					   : (string)value + (string)parameter;
			if (targetType.Name == "Uri") return new Uri(res);
			return res;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return "";
		}
	}
}

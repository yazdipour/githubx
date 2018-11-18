using System;
using Windows.UI.Xaml.Data;

namespace GithubX.UWP.Converters
{
	internal class StringConcatConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
			=> language.Equals("End", StringComparison.OrdinalIgnoreCase)
			? (string)parameter + (string)value
			: (string)value + (string)parameter;

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}

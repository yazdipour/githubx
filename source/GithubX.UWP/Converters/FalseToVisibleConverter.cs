using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace GithubX.UWP.Converters
{
	internal class FalseToVisibleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return value is bool collapsed ? collapsed ? Visibility.Collapsed : Visibility.Visible : (object)null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return value is Visibility visibility ? visibility == Visibility.Collapsed : (object)null;
		}
	}
}

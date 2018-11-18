using System;
using Windows.UI.Xaml.Data;

namespace GithubX.UWP.Converters
{
	internal class LanguageToColorConverter:IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
			=> Shared.Helpers.OctokitHelper.FindLanguageColor((string)value);

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}

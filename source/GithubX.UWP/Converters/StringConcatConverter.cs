using System;
using Windows.UI.Xaml.Data;

namespace GithubX.UWP.Converters
{
	internal class StringConcatConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			try
			{
				var res = language.Equals("End")
					   ? parameter.ToString() + value.ToString()
					   : value.ToString() + parameter.ToString();
				if (targetType.Name == "Uri") return new Uri(res);
				return res;
			}
			catch (Exception ex)
			{

				Shared.Services.Logger.E(ex);
				return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return "";
		}
	}
}

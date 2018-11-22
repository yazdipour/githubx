using Octokit;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace GithubX.UWP.Converters
{
	internal class ContentTypeToSymbolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			switch ((ContentType)value)
			{
				case ContentType.Submodule:
					return Symbol.MapDrive;
				case ContentType.Dir:
					return Symbol.Folder;
				default:
					return Symbol.Page;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}

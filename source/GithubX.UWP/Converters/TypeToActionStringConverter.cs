using System;
using Windows.UI.Xaml.Data;

namespace GithubX.UWP.Converters
{
	internal class TypeToActionStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var act = (Octokit.Activity)value;
			switch (act.Type)
			{
				case "WatchEvent":
					return "starred";
				case "ForkEvent":
					return "forked";
				case "CreateEvent":
					return "created" + (act.Payload.ToString().Contains("branch") ? " a branch" : "");
				case "PullRequestEvent":
					return "opened a PR";
				case "MemberEvent":
					return "added";
				case "PublicEvent":
					return "made public";
				default:
					return "";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}

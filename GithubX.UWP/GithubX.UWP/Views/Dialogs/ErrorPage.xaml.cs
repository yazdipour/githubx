using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class ErrorPage : Page
	{
		public ErrorPage()
		{
			this.InitializeComponent();
			if(App.Releasing) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Visit ErrorPage");
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (Frame.CanGoBack)
				Frame.GoBack();
		}
	}
}

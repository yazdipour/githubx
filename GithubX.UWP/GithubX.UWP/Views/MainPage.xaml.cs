using GithubX.UWP.Models;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP
{
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			this.InitializeComponent();
			var h = new Services.UI.UIHandler();
			h.TitleBarVisiblity(false, myTitleBar);
			h.TitleBarButton_TranparentBackground(false);
			var acc = Services.Api.ApiHandler.LoginFromCache();
			if (acc == null)
				iframe.Navigate(typeof(Views.LoginPage));
			else
				iframe.Navigate(typeof(Views.StarListPage), acc);
		}


	}
}

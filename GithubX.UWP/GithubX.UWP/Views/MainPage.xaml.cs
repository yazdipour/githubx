using Windows.UI.Xaml.Controls;

namespace GithubX.UWP
{
	public sealed partial class MainPage : Page
	{
		public static Microsoft.Toolkit.Uwp.UI.Controls.InAppNotification NotifyElement { get; set; }

		public MainPage()
		{
			this.InitializeComponent();
			NotifyElement = Notif;
			var h = new Services.UI.UIHandler();
			h.TitleBarVisiblity(false, myTitleBar);
			h.TitleBarButton_TranparentBackground(false);
			var acc = Services.Api.ApiHandler.LoginFromCache();
			if (acc == null)
			{
				if(Services.Api.HttpHandler.CheckConnection)
					iframe.Navigate(typeof(Views.LoginPage));
				else
					iframe.Navigate(typeof(Views.ErrorPage));
			}
			else
				iframe.Navigate(typeof(Views.StarListPage), acc);
		}
	}
}

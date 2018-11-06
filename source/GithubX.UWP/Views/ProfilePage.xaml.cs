using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class ProfilePage : Page
	{
		private readonly List<string> notifications = new List<string> { "Xxx", "Xxx" };

		public ProfilePage()
		{
			this.InitializeComponent();
			DataContext = notifications;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			//get user info
			//get notification
		}

		private void Notification_ItemClick(object sender, ItemClickEventArgs e)
		{
		}

		private void Logout_Clicked(object sender, RoutedEventArgs e)
		{

		}
	}
}

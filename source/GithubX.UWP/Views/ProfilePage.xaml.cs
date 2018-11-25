using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GithubX.Shared.Services;
using Windows.UI.Popups;
using System;

namespace GithubX.UWP.Views
{
	public sealed partial class ProfilePage : Page
	{
		private ObservableCollection<Octokit.Notification> _notifications = new ObservableCollection<Octokit.Notification>();
		private Octokit.User _user { get; set; }

		public ProfilePage() => InitializeComponent();

		private async void Page_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				if (_notifications.Count > 0) return;
				_user = await GithubService.UserService.GetUser();
				Bindings.Update();
				var temp = await GithubService.UserService.GetAllNotifications(new Octokit.ApiOptions() { PageSize = 10, PageCount = 1 });
				foreach (var t in temp) if (t.Unread) _notifications.Add(t);
			}
			catch (Exception ex)
			{
				Logger.E(ex);
			}
		}

		private void Notification_ItemClick(object sender, ItemClickEventArgs e)
		{
			Frame.Navigate(typeof(RepositoryPage));
		}

		private async void Logout_Clicked(object sender, RoutedEventArgs e)
		{
			var dialog = new MessageDialog("Are you sure you want to Logout?");
			dialog.Commands.Add(new UICommand("Logout", LogoutAction));
			dialog.Commands.Add(new UICommand("Cancel"));
			await dialog.ShowAsync();
		}

		private void LogoutAction(IUICommand command)
		{

		}
	}
}

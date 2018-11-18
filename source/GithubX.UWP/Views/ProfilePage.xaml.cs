using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;

namespace GithubX.UWP.Views
{
	public sealed partial class ProfilePage : Page
	{
		private readonly ObservableCollection<Octokit.Notification> notifications = new ObservableCollection<Octokit.Notification> { };
		private readonly Octokit.User user = new Octokit.User();

		public ProfilePage()
		{
			this.InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			//get user info
			//get notification
		}

		private void Notification_ItemClick(object sender, ItemClickEventArgs e)
		{
		}

		private async void Logout_Clicked(object sender, RoutedEventArgs e)
		{
			try
			{
				await new Dialogs.AddCategoryDialog().ShowAsync();
			}
			catch { }
		}
	}
}

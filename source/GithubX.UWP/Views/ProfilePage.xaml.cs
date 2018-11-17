using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;

namespace GithubX.UWP.Views
{
	public sealed partial class ProfilePage : Page
	{
		private readonly ObservableCollection<string> notifications = new ObservableCollection<string> { "Xxx", "Xxx" };

		public ProfilePage()
		{
			this.InitializeComponent();
			DataContext = notifications;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			//get user info
			//get notification
			notifications.Add("xx");
			notifications.Add("xx");
			notifications.Add("xx");
			notifications.Add("xx");
			notifications.Add("xx");
			notifications.Add("xx");
			notifications.Add("xx");
			notifications.Add("xx");
			notifications.Add("xx");
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

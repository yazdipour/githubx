using GithubX.Shared.Services;
using GithubX.UWP.Services.Api;
using Windows.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GithubX.UWP.Views
{
	public sealed partial class MotherPage : Page
	{
		public static Microsoft.Toolkit.Uwp.UI.Controls.InAppNotification NotifyElement { get; set; }

		private readonly Dictionary<NavigationViewItem, Type> dFrame = new Dictionary<NavigationViewItem, Type>()
		{
			{ new NavigationViewItem(){Content = "Profile"}, typeof(ProfilePage)},
			{ new NavigationViewItem(){Content = "Activities"}, typeof(ActivitiesPage)},
			{ new NavigationViewItem(){Content = "Categories"}, typeof(CategoriesPage)},
			{ new NavigationViewItem(){Content = "Stars"}, typeof(RepositoriesPage)},
			{ new NavigationViewItem(){Content = "Repositories"}, typeof(RepositoriesPage)},
			{ new NavigationViewItem(){Content = "Followers"}, typeof(FollowPage)},
			{ new NavigationViewItem(){Content = "Following"}, typeof(FollowPage)},
			{ new NavigationViewItem(){Content = "Gists"}, typeof(GistsPage)},
			{ new NavigationViewItem(){Content = "Help"}, typeof(MarkdownPage)}
		};

		public MotherPage()
		{
			this.InitializeComponent();
			NotifyElement = Notifer;
		}

		private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			foreach (var item in dFrame)
				navigationView.MenuItems.Add(item.Key);
			navigationView.SelectedItem = dFrame.First().Key;
			Shared.Helpers.CacheHandler.InitCache();
			Logger.Init(ApiKeys.AppCenter);
		}

		private async void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
		{
			if (args.SelectedItem == null) return;
			if (!args.IsSettingsSelected)
			{
				var nav = (args?.SelectedItem as NavigationViewItem);
				iframe.Navigate(dFrame.GetValueOrDefault(nav, dFrame.First().Value), nav.Content);
				navigationView.IsBackEnabled = iframe.CanGoBack;
			}
			else
			{
				await new SettingsDialog().ShowAsync();
				navigationView.SelectedItem = null;
			}
		}

		private void navigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
		{
			if (!iframe.CanGoBack) return;
			iframe.GoBack();
			navigationView.IsBackEnabled = iframe.CanGoBack;
			if (dFrame.ContainsValue(iframe.SourcePageType))
			{
				navigationView.SelectionChanged -= NavigationView_SelectionChanged;
				navigationView.SelectedItem = dFrame.FirstOrDefault(x => x.Value == iframe.SourcePageType).Key;
				navigationView.SelectionChanged += NavigationView_SelectionChanged;
			}
		}
	}
}

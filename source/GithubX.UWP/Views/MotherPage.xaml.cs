using GithubX.Shared.Services;
using GithubX.UWP.Services.Api;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class MotherPage : Page
	{
		public static Microsoft.Toolkit.Uwp.UI.Controls.InAppNotification NotifyElement { get; set; }
		public MotherPage()
		{
			this.InitializeComponent();
			NotifyElement = Notifer;
		}

		private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			Shared.Handlers.CacheHandler.InitCache();
			Logger.Init(ApiKeys.AppCenter);
			navigationView.SelectedItem = navigationView.MenuItems[0] as NavigationViewItem;
		}

		private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
		{
			if (args.IsSettingsSelected)
			{

			}
			else
			{
				System.Type pageType = null;
				string tag = (args?.SelectedItem as NavigationViewItem)?.Tag?.ToString() ?? "";
				switch (tag)
				{
					case "Profile":
						pageType = typeof(ProfilePage);
						break;
					case "Activities":
						pageType = typeof(ActivitiesPage);
						break;
					case "Categories":
						pageType = typeof(CategoriesPage);
						break;
					case "Stars":
					case "Repositories":
						pageType = typeof(RepositoriesPage);
						break;
					case "Followers":
					case "Following":
						pageType = typeof(FollowPage);
						break;
					case "Gists":
						pageType = typeof(GistsPage);
						break;
					case "Help":
						pageType = typeof(Controls.MarkdownPage);
						break;
				}
				if (pageType != null) iframe.Navigate(pageType, tag);
			}
		}
	}
}

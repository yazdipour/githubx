using GithubX.Shared.Services;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class ActivitiesPage : Page
	{
		private ObservableCollection<Octokit.Activity> activityRepositories
			= new ObservableCollection<Octokit.Activity>();
		private Octokit.ApiOptions options = new Octokit.ApiOptions() { PageSize = 40, PageCount = 1 };

		public ActivitiesPage() => InitializeComponent();

		private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var user = await GithubService.UserService.GetUser();
			var temp = await GithubService.UserService.GetUserActivity(options, user?.Login);
			foreach (var item in temp)
			{
				if (item.Type == "PublicEvent") continue;//>> X made X/Y public
				activityRepositories.Add(item);
			}
		}

		private void GridView_ItemClick(object sender, ItemClickEventArgs e)
			=> Frame.Navigate(typeof(RepositoryPage), (e.ClickedItem as Octokit.Activity)?.Repo);
	}
}

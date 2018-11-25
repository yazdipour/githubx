using GithubX.Shared.Services;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GithubX.UWP.Views
{
	public sealed partial class RepositoriesPage : Page
	{
		private ObservableCollection<Octokit.Repository> repositories = new ObservableCollection<Octokit.Repository>();
		private string Title = "Repositories", prevTitle;
		private Octokit.ApiOptions options = new Octokit.ApiOptions() { PageCount = 1, PageSize = 30 };

		public RepositoriesPage() => InitializeComponent();
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			Title = e.Parameter as string;
			if (prevTitle != Title) NavigationCacheMode = NavigationCacheMode.Disabled;
			else NavigationCacheMode = NavigationCacheMode.Enabled;
		}

		private void GridView_ItemClick(object sender, ItemClickEventArgs e)
			=> Frame.Navigate(typeof(RepositoryPage), e.ClickedItem as Octokit.Repository);

		private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			//if (repositories.Count > 0 && prevTitle == Title) return;
			prevTitle = Title;
			repositories.Clear();
			var temp = Title == "Repositories"
				? await GithubService.UserService.GetUserRepositories(options)
				: await GithubService.UserService.GetStarredRepositories(options);
			foreach (var t in temp) repositories.Add(t);
		}
	}
}

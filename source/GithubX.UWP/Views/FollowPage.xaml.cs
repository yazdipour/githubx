using GithubX.Shared.Services;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GithubX.UWP.Views
{
	public sealed partial class FollowPage : Page
	{
		private ObservableCollection<Octokit.User> users = new ObservableCollection<Octokit.User>();
		private string Title = "Followers";
		private string previousTitle = "Followers";
		private Octokit.ApiOptions options = new Octokit.ApiOptions() { PageCount = 1 };

		public FollowPage() => InitializeComponent();

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			Title = e.Parameter as string;
		}

		private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			if (users.Count > 0 && previousTitle == Title) return;
			previousTitle = Title;
			foreach (var t in (Title == "Followers")
				? await GithubService.UserService.GetAllFollowers(options)
				: await GithubService.UserService.GetAllFollowing(options))
				users?.Add(t);
		}
	}
}

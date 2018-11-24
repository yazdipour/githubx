using GithubX.Shared.Services;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GithubX.UWP.Views
{
	public sealed partial class GistsPage : Page
	{
		private ObservableCollection<Octokit.Gist> gists = new ObservableCollection<Octokit.Gist>();
		private Octokit.User previousUser;
		public GistsPage() => InitializeComponent();

		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			if (e.Parameter is Octokit.User user)
			{
				if (user == previousUser && gists.Count > 0)
					return;
				previousUser = user;
			}
			else
			{
				previousUser = null;
				if (gists.Count > 0) return;
			}
			var temp = await GithubService.UserService.GetUserGists(previousUser?.Login);
			gists.Clear();
			foreach (var t in temp) gists.Add(t);
		}

		private void GridView_ItemClick(object sender, ItemClickEventArgs e)
			=> Frame.Navigate(typeof(GistPage), e?.ClickedItem as Octokit.Gist);
	}
}

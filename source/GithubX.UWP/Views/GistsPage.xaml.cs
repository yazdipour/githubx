using GithubX.Shared.Services;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GithubX.UWP.Views
{
	public sealed partial class GistsPage : Page
	{
		private ObservableCollection<Octokit.Gist> gists = new ObservableCollection<Octokit.Gist>();

		public GistsPage()
		{
			this.InitializeComponent();
			Loaded += async (_, __) =>
			{
				var x = await GithubService.Client.Gist.GetAll();
				var temp = await GithubService.UserService.GetUserGists(new Octokit.ApiOptions() { PageSize = 10, StartPage = 0 }, "yazdipour");
				foreach (var t in x) gists.Add(t);
			};
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
		}

		private void GridView_ItemClick(object sender, ItemClickEventArgs e)
		{
			Frame.Navigate(typeof(RepositoryPage));
		}
	}
}

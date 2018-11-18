using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class GistsPage : Page
	{
		private ObservableCollection<Octokit.Gist> gists = new ObservableCollection<Octokit.Gist>();

		public GistsPage()
		{
			this.InitializeComponent();
		}

		private void GridView_ItemClick(object sender, ItemClickEventArgs e)
		{
			Frame.Navigate(typeof(RepositoryPage));
		}
	}
}

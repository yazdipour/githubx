using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class GistsPage : Page
	{
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

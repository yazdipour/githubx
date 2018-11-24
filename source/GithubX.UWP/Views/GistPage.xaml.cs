using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GithubX.UWP.Views
{
	public sealed partial class GistPage : Page
	{
		public Octokit.Gist gist { get; set; }
		public GistPage()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			gist = e.Parameter as Octokit.Gist;
		}
	}
}

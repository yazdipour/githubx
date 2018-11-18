using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class FollowPage : Page
	{
		private ObservableCollection<Octokit.User> users = new ObservableCollection<Octokit.User>();
		private string Title = "Followers";

		public FollowPage()
		{
			this.InitializeComponent();
		}
	}
}

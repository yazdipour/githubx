using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class ActivitiesPage : Page
	{
		private ObservableCollection<Octokit.Repository> activityRepositories = new ObservableCollection<Octokit.Repository>();

		public ActivitiesPage() => this.InitializeComponent();

		private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var oct=new Octokit.Repository();
		}
	}
}

using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class RepositoriesPage : Page
	{
		private ObservableCollection<Octokit.Repository> repositories = new ObservableCollection<Octokit.Repository>();
		private string Title = "Repositories";

		public RepositoriesPage() => InitializeComponent();
	}
}

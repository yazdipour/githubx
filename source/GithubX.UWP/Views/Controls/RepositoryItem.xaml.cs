using Octokit;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views.Controls
{
	public sealed partial class RepositoryItem : UserControl
	{
		public RepositoryItem() => InitializeComponent();

		public Repository Repo
		{
			get { return (Repository)GetValue(RepoProperty); }
			set { SetValue(RepoProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Repo.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty RepoProperty =
			DependencyProperty.Register("Repo", typeof(Repository), typeof(RepositoryItem), null);
	}
}
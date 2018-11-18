using Octokit;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views.Controls
{
	public sealed partial class ActivityItem : UserControl
    {
		public ActivityItem() => InitializeComponent();

		public string User
		{
			get { return (string)GetValue(UserProperty); }
			set { SetValue(UserProperty, value); }
		}

		public static readonly DependencyProperty UserProperty =
			DependencyProperty.Register("User", typeof(string), typeof(ActivityItem), null);

		public string Msg
		{
			get { return (string)GetValue(MsgProperty); }
			set { SetValue(MsgProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Msg2.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty MsgProperty =
			DependencyProperty.Register("Msg", typeof(string), typeof(ActivityItem), null);

		private Repository repository => null;
		public Repository Repository
		{
			get { return (Repository)GetValue(RepositoryProperty); }
			set { SetValue(RepositoryProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Repository.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty RepositoryProperty =
			DependencyProperty.Register("Repository", typeof(Repository), typeof(RepositoryItem), null);
	}
}
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Akavache;
using System;
using System.Reactive;
using System.Linq;
using System.Threading.Tasks;
using GithubX.Shared.Services;

namespace GithubX.UWP.Views
{
	public sealed partial class GistPage : Page
	{
		private Octokit.Gist gist;
		public GistPage()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			gist = e.Parameter as Octokit.Gist;
			loadGistFile(gist.Files.Values.First());
		}

		private void listView_ItemClick(object sender, ItemClickEventArgs e)
		{
			var file = (e.ClickedItem as Octokit.GistFile);
			loadGistFile(file);
		}

		private async void loadGistFile(Octokit.GistFile file)
		{
			try
			{
				var res = await GithubService.RepositoryService.GetMarkDownReadyAsync(file?.RawUrl, true);
				if (file.Filename.Contains(".md"))
					markdown.Text = res;
				else
					markdown.Text = $"``` {res} ```";
			}
			catch { }
		}
	}
}

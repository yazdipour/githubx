using GithubX.Shared.Services;
using GithubX.Shared.Services.Pocket;
using Octokit;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Security.Authentication.Web;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GithubX.UWP.Views
{
	public sealed partial class RepositoryPage : Windows.UI.Xaml.Controls.Page
	{
		private ObservableCollection<RepositoryContent> _contents = new ObservableCollection<RepositoryContent>();
		private string _markdownContent { get; set; } = "> Loading";
		private Repository _repository { get; set; }
		private string currentBranch { get; set; } = "master";

		public RepositoryPage()
		{
			InitializeComponent();
			DataTransferManager.GetForCurrentView().DataRequested += (_, args) =>
			{
				DataRequest request = args.Request;
				request.Data.SetText(_repository.HtmlUrl);
				request.Data.Properties.Title = "Shared by GithubX";
			};
		}

		private async void Page_Loading(FrameworkElement sender, object args)
		{
			if (_repository == null) return;
			var temp = await GithubService.RepositoryService.GetRepositoryContent(_repository.Id);
			foreach (var t in temp) _contents.Add(t);
			_markdownContent = (await GithubService.RepositoryService.GetRepositoryReadme(_repository.Id))?.Content;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			_repository = e.Parameter as Repository;
		}

		private async void Content_Click(object sender, ItemClickEventArgs e)
		{
			var content = e.ClickedItem as RepositoryContent;
			switch (content.Type.Value)
			{
				case ContentType.Dir:
					var temp = await GithubService.RepositoryService.GetRepositoryContent(_repository.Id, content.Path);
					if (temp.Count > 0) _contents.Clear();
					foreach (var t in temp) _contents.Add(t);
					break;
				case ContentType.File:
					try
					{
						_markdownContent = "> Loading";
						_markdownContent = await GithubService.RepositoryService.GetMarkDownReadyAsync(content);
					}
					catch
					{
						_markdownContent = "> Unsupported file ***OR*** Error happend while getting the file.";
					}
					break;
				case ContentType.Submodule:
					await Helpers.Utils.OpenUri(content.SubmoduleGitUrl);
					break;
				case ContentType.Symlink:
					await Helpers.Utils.OpenUri(content.Url);
					break;
			}
		}

		private void ToggleButton_Checked(object sender, RoutedEventArgs e)
		{
			var btn = e.OriginalSource as Control;
			switch (btn?.Tag?.ToString())
			{
				case "Star":
					break;
				case "Fork":
					break;
				case "Watch":
					break;
			}
		}

		private void ToggleButton_UnChecked(object sender, RoutedEventArgs e)
		{
			var btn = e.OriginalSource as Control;
			switch (btn?.Tag?.ToString())
			{
				case "Star":
					break;
				case "Fork":
					break;
				case "Watch":
					break;
			}
		}

		private async void AppBarButton_Click(object sender, RoutedEventArgs e)
		{
			var btn = e.OriginalSource as Control;
			switch (btn?.Tag?.ToString())
			{
				case "Pocket":
					await SaveInPocketAsync(_repository.Url);
					break;
				case "Share":
					DataTransferManager.ShowShareUI();
					break;
				case "Browser":
					await Helpers.Utils.OpenUri(_repository.Url);
					break;
				case "Download":
					//TODO: Download with app
					await Helpers.Utils.OpenUri($"{_repository.HtmlUrl}archive/{currentBranch}.zip");
					break;
				case "Git":
					var pkg = new DataPackage();
					pkg.SetText(_repository.CloneUrl);
					Clipboard.SetContent(pkg);
					MotherPage.NotifyElement.Show("Copied", 2000);
					break;
				case "Category":
					break;
				case "Commits":
					await Helpers.Utils.OpenUri($"{_repository.HtmlUrl}/commits/{currentBranch}");
					break;
				case "Issues":
					await Helpers.Utils.OpenUri($"{_repository.HtmlUrl}/issues");
					break;
			}
		}

		private async Task SaveInPocketAsync(string url)
		{
			Logger.D("Pocket", "Click");
			if (!Helpers.Utils.CheckConnection)
				MotherPage.NotifyElement.Show("✖ Error! No internet", 3000);
			var pocket = new PocketService(Shared.Keys.PocketToken);
			pocket.LoadFromCache();
			try
			{
				if (pocket.IsLoggedIn())
				{
					await pocket.Add(new Uri(url));
					MotherPage.NotifyElement.Show("✔ Saved in your Pocket", 3000);
				}
				else
				{
					var dialog = new MessageDialog("✔ Login to Pocket Then try Again");
					dialog.Commands.Add(new UICommand("OK", async (IUICommand _) =>
					{
						var (requestCode, uri) = await pocket.GenerateAuthUri();
						WebAuthenticationResult auth = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, uri, new Uri(pocket.FallBackUri));
						if (auth.ResponseStatus != WebAuthenticationStatus.Success) return;
						var res = auth.ResponseData;
						var token = await pocket.GetUserToken(requestCode);
						pocket.SaveInCache();
						MotherPage.NotifyElement.Show((token?.Length > 1) ? "✔ Logged in Pocket" : "✖ Failed to login", 3000);
					}));
					dialog.Commands.Add(new UICommand("Cancel"));
					await dialog.ShowAsync();
				}
			}
			catch { MotherPage.NotifyElement.Show("✖ Error!", 3000); }
		}

		private async void MarkdownTextBlock_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
		{
			try
			{
				if (e.Link.Contains("http"))
					await Windows.System.Launcher.LaunchUriAsync(new Uri(e.Link));
				else await Windows.System.Launcher.LaunchUriAsync(new Uri($"{_repository.HtmlUrl}/tree/{currentBranch}/{e.Link}"));
			}
			catch { MotherPage.NotifyElement.Show("Error in opening:" + e.Link, 3000); }
		}
	}
}

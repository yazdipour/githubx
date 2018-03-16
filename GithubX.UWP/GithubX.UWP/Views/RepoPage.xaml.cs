using System;
using GithubX.UWP.Models;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GithubX.UWP.Views
{
	public sealed partial class RepoPage : Page
	{
		RepoModel repo { get; set; }

		public RepoPage()
		{
			this.InitializeComponent();
			DataTransferManager.GetForCurrentView().DataRequested += (sender, args) =>
			{
				DataRequest request = args.Request;
				request.Data.SetText(repo.html_url);
				request.Data.Properties.Title = "Github Repo, Shared by GithubX";
			};
			SizeChanged += (sender, args) =>
			{
				headerGrid.Width = MarkdownText.Width = ActualWidth - buttonPanel.Margin.Left * 2;
				MarkdownScrollViewer.Height = ActualHeight - 92 - buttonPanel.ActualHeight - buttonPanel.Margin.Top - 32;
			};
			Loaded += async (sender, args) =>
			{
				string path = Services.Api.Api.RepoReadMeUrl(repo.full_name);
				var res = await Services.Api.ApiHandler.GetReadMeMdAsync(repo.id, path, true);
				MarkdownText.Text = res.Item2;
				if (res.Item1) MainPage.NotifyElement.Show("Loaded from Cached",3000);
			};
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			repo = e.Parameter as RepoModel;
		}

		private void Border_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			if (Frame.CanGoBack) Frame.GoBack();
		}

		private async void ButtonBar_Click(object sender, RoutedEventArgs e)
		{
			var btn = e.OriginalSource as Button;
			switch (btn.Tag.ToString())
			{
				case "0":
					DataTransferManager.ShowShareUI();
					break;
				case "1":
					await Windows.System.Launcher.LaunchUriAsync(new Uri(repo.html_url));
					break;
				//case "2":
				//	break;
				//case "3":
				//	//TODO :save pocket
				//	break;
				case "4":
					//MD reload
					try
					{
						MarkdownText.Text = "...";
						string path = Services.Api.Api.RepoReadMeUrl(repo.full_name);
						var res = await Services.Api.ApiHandler.GetReadMeMdAsync(repo.id, path, false);
						MarkdownText.Text = res.Item2;
					}
					catch { }
					break;
			}
		}
	}
}

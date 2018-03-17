using System;
using System.Collections.Generic;
using GithubX.UWP.Models;
using GithubX.UWP.Services.Api;
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
				if (res.Item1) MainPage.NotifyElement.Show("Loaded from Cached", 3000);
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
				case "2":
					MenuFlyout menu = new MenuFlyout();
					var tempCategoriesId = new List<int>(repo.CategoriesId);
					foreach (var item in ApiHandler.AllCategories)
					{
						if (item.Id == 0) continue;
						var el = new ToggleMenuFlyoutItem { Text = item.Text, Tag = item.Id.ToString(), IsChecked = tempCategoriesId.Contains(item.Id) };
						el.Click += El_Click;
						menu.Items.Add(el);
					}
					async void El_Click(object sen, RoutedEventArgs ee)
					{
						var tag = (sen as FrameworkElement).Tag;
						try
						{
							if (tag == null) throw new Exception();
							var inx = Convert.ToInt32(tag.ToString());
							if (tempCategoriesId.Contains(inx))
								//toggle off
								tempCategoriesId.Remove(inx);
							else
								//toggle on
								tempCategoriesId.Add(inx);
							repo.CategoriesId = tempCategoriesId.ToArray();
							await ApiHandler.UpdateRepoAsync(App.UserLoginAccountName, repo);
							MainPage.NotifyElement.Show("✔ Categories Updated", 3000);
						}
						catch { MainPage.NotifyElement.Show("Something is not right!!", 2000); }
					}
					menu.ShowAt(btn);
					break;
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

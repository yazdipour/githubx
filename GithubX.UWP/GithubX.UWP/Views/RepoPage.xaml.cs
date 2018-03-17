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
		List<ContentModel> ContentFiles { get; set; }
		string readmeUrl;

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
				// todo better way?!
				MarkdownScrollViewer.Height = ActualHeight - 92 - buttonPanel.ActualHeight - buttonPanel.Margin.Top - 32;
			};
			Loaded += async (sender, args) =>
			{
				ContentFiles = await ApiHandler.GetContentsAsync(repo.full_name);
				var readme = ContentFiles.Find(o => o.name.ToLower().Equals("readme.md"));
				if (readme != null)
				{
					readmeUrl = readme.download_url;
					var res = await ApiHandler.GetReadMeMdAsync(repo.id, readmeUrl, true);
					MarkdownText.Text = res.Item2;
					if (res.Item1) MainPage.NotifyElement.Show("Loaded from Cached", 3000);
				}
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
					if (App.Releasing) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Tap ReadMe.Share");
					DataTransferManager.ShowShareUI();
					break;
				case "1":
					if (App.Releasing) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Tap ReadMe.OpenBrowser");
					await Windows.System.Launcher.LaunchUriAsync(new Uri(repo.html_url));
					break;
				case "2":
					if (App.Releasing) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Tap ReadMe.ChangeCategory");
					MenuFlyout menu = new MenuFlyout();
					Style s = new Style { TargetType = typeof(MenuFlyoutPresenter) };
					s.Setters.Add(new Setter(RequestedThemeProperty, ElementTheme.Dark));
					menu.MenuFlyoutPresenterStyle = s;

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
					if (App.Releasing) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Tap ReadMe.Refresh");
					try
					{
						MarkdownText.Text = "...";
						var res = await ApiHandler.GetReadMeMdAsync(repo.id, readmeUrl, false);
						MarkdownText.Text = res.Item2;
					}
					catch { }
					break;
			}
		}

		private async void MarkdownText_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
		{
			if (e.Link != null)
			{
				try
				{
					await Windows.System.Launcher.LaunchUriAsync(new Uri(e.Link));
				}
				catch
				{
					try
					{
						var url = e.Link;
						if (url[0] != '/') url = '/' + e.Link;
						await Windows.System.Launcher.LaunchUriAsync(new Uri(repo.html_url + "/tree/master" + url));
					}
					catch
					{
						if (App.Releasing) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Error OpeningUrlInReadMe " + e.Link);

						MainPage.NotifyElement.Show("Error in opening:" + e.Link, 3000);
					}
				}
			}
		}
	}
}

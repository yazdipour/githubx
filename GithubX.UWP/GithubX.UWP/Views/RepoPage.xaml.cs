using System;
using System.Collections.Generic;
using GithubX.UWP.Models;
using GithubX.UWP.Services.Api;
using GithubX.UWP.Services.Cache;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GithubX.UWP.Views
{
	public sealed partial class RepoPage : Page
	{
		RepoModel repo { get; set; }
		List<ContentModel> ContentFiles { get; set; }
		private Style DarkFlyoutStyle
		{
			get
			{
				Style s = new Style { TargetType = typeof(MenuFlyoutPresenter) };
				s.Setters.Add(new Setter(RequestedThemeProperty, ElementTheme.Dark));
				return s;
			}
		}
		string Url { get; set; }
		string PFix { get; set; }
		MarkdownSetting md { get; set; }

		#region Markdown Setting
		private void SaveTheme(MarkdownSetting setting)
		{
			var wCache = new WindowsCacheHandler();
			wCache.Write(CacheKeys.ReadmeTheme, Newtonsoft.Json.JsonConvert.SerializeObject(setting));
		}
		private MarkdownSetting LoadTheme()
		{
			var wCache = new WindowsCacheHandler();
			try
			{
				var json = wCache.Read(CacheKeys.ReadmeTheme);
				return Newtonsoft.Json.JsonConvert.DeserializeObject<MarkdownSetting>(json);
			}
			catch
			{
				return new MarkdownSetting
				{
					BgColor = "#ffffff",
					Theme = ElementTheme.Light
				};
			}
		}
		#endregion

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
				MarkdownScrollViewer.Height = ActualHeight - 92 - buttonPanel.ActualHeight - buttonPanel.Margin.Top - 32;
				MarkdownText.Width = MarkdownScrollViewer.ActualWidth;
			};
			Loaded += async (sender, args) =>
			{
				PFix = string.Format("https://github.com/{0}/raw/master", repo.full_name);
				md = LoadTheme();
				ContentFiles = await ApiHandler.GetContentsAsync(repo.contents_url);
				var readme = ContentFiles.Find(o => o.name.ToLower().Equals("readme.md"));
				if (readme != null)
				{
					if (md == null) md = new MarkdownSetting();
					Url = readme.download_url;
					var res = await ApiHandler.GetReadMeMdAsync(repo.id, Url, true);
					MarkdownText.Text = res.Item2;
					if (res.Item1) MainPage.NotifyElement.Show("Loaded from Cached", 3000);
				}
				Bindings.Update();
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
					if (ApiKeys.AppCenter != null) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Tap ReadMe.Share");
					DataTransferManager.ShowShareUI();
					break;
				case "1":
					if (ApiKeys.AppCenter != null) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Tap ReadMe.OpenBrowser");
					await Windows.System.Launcher.LaunchUriAsync(new Uri(repo.html_url));
					break;
				case "2":
					if (ApiKeys.AppCenter != null) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Tap ReadMe.ChangeCategory");
					#region Flyout
					MenuFlyout menu = new MenuFlyout { MenuFlyoutPresenterStyle = DarkFlyoutStyle };
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
					#endregion
					break;
				case "3":
					if (!HttpHandler.CheckConnection) MainPage.NotifyElement.Show("Error! No internet", 3000);
					var pocket = new Services.Api.Pocket.PocketApi();
					if (pocket.CheckLogin())
					{
						var item = await pocket.Post(repo.html_url, new[] { "github", "github" });
						if (item == null) MainPage.NotifyElement.Show("Error! Something with the Pocket", 3000);
						else MainPage.NotifyElement.Show("Saved to Pocket", 3000);
					}
					else
					{
						var dialog = new MessageDialog("Login to Pocket then try Again");
						dialog.Commands.Add(new UICommand("OK", (IUICommand command) =>
						{
							//var uri = pocket.LoginUriAsync();
							//new WebAuthenticationBroker();
							//todo
						}));
						dialog.Commands.Add(new UICommand("Cancel"));
						await dialog.ShowAsync();
					}
					break;
				case "4":
					//MD reload
					if (ApiKeys.AppCenter != null) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Tap ReadMe.Refresh");
					try
					{
						MarkdownText.Text = "...";
						var res = await ApiHandler.GetReadMeMdAsync(repo.id, Url, false);
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
						if (ApiKeys.AppCenter != null) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Error OpeningUrlInReadMe " + e.Link);

						MainPage.NotifyElement.Show("Error in opening:" + e.Link, 3000);
					}
				}
			}
		}

		private void Flyout_Closed(object sender, object e)
		{
			SaveTheme(md);
			Bindings.Update();
		}
	}

	internal class MarkdownSetting
	{
		public string BgColor { get; set; }
		public ElementTheme Theme { get; set; }
		[Newtonsoft.Json.JsonIgnore]
		public bool DarkThemeBool
		{
			get => Theme == ElementTheme.Dark;
			set { Theme = value ? ElementTheme.Dark : ElementTheme.Light; }
		}
	}
}

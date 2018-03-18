using System;
using System.Collections.ObjectModel;
using System.Linq;
using GithubX.UWP.Models;
using GithubX.UWP.Services.Api;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GithubX.UWP.Views
{
	public sealed partial class StarListPage : Page
	{
		OwnerModel User = new OwnerModel();
		ObservableCollection<RepoModel> Repositories { get; set; }
		private string SharingUrl = null;
		private int pageInOnlineMode = 0, currentTabId = 0;

		#region OnLoad
		public StarListPage()
		{
			this.InitializeComponent();
			SizeChanged += (sender, e) =>
			{
				gridView.Height = ActualHeight - 92;
			};
			DataTransferManager.GetForCurrentView().DataRequested += (sen, args) =>
			{
				if (SharingUrl == null) return;
				DataRequest request = args.Request;
				request.Data.SetText(SharingUrl);
				request.Data.Properties.Title = "Github Repo, Shared by GithubX";
			};
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			User = e.Parameter as OwnerModel;
			App.UserLoginAccountName = User.login;
			if (gridView.Items.Count == 0)
			{
				await ApiHandler.PrepareAllCategories(User.login);
				tabList.ItemsSource = ApiHandler.AllCategories;
				if (ApiHandler.AllCategories != null)
					if (ApiHandler.AllCategories.Count > 0)
					{
						await ApiHandler.PrepareAllRepos(User.login);
						Repositories = new ObservableCollection<RepoModel>(ApiHandler.GetRepoOfCategory(0));
						if (Repositories.Count >= 30) LoadMoreButton.Visibility = Visibility.Visible;
						new Services.UI.UIHandler().ChangeHeaderTheme("HeaderAcrylic", ApiHandler.AllCategories[0].Color);
						Bindings.Update();
					}
			}
		}
		#endregion

		#region Events in Tab
		private async void CategorySetting_Click(object sender, RoutedEventArgs e)
		{
			var p = new CategoryPanel(currentTabId);
			await p.ShowAsync();
			//refresh if current category does not exist anymore
			if (!ApiHandler.AllCategories.ToList().Any(o=>o.Id== currentTabId)) RefreshPage();
		}

		private void RefreshPage()
		{
			NavigationCacheMode = NavigationCacheMode.Disabled;
			var type = Frame.CurrentSourcePageType;
			Frame.Navigate(type, User);
			Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
		}

		private void tabList_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (ApiKeys.Releasing) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Tap CategoriesTab");

			var item = e.ClickedItem as CategoryModel;
			if (item.Id == currentTabId) return;
			try
			{
				currentTabId = item.Id;
				new Services.UI.UIHandler().ChangeHeaderTheme("HeaderAcrylic", item.Color);
				Repositories = new ObservableCollection<RepoModel>(ApiHandler.GetRepoOfCategory(item.Id));
				LoadMoreButton.Visibility = (currentTabId == 0 && Repositories.Count % 30 == 0) ? Visibility.Visible : Visibility.Collapsed;
				Bindings.Update();
			}
			catch (Exception ex)
			{
				MainPage.NotifyElement.Show(ex.Message, 4000);
			}
		}

		private async void LoadMoreButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var res = await ApiHandler.GetNextPageReposAsync(User.login, ++pageInOnlineMode);
				res.ForEach(Repositories.Add);
				LoadMoreButton.Visibility = (res.Count % 30 == 0) ? Visibility.Visible : Visibility.Collapsed;
			}
			catch (Exception ex)
			{
				MainPage.NotifyElement.Show(ex.Message, 4000);
			}
		}
		#endregion

		#region GridView Events
		private void AdaptiveGridViewControl_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (e.ClickedItem is RepoModel repo) Frame.Navigate(typeof(RepoPage), repo);
		}

		private void gridView_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
		{
			if (ApiKeys.Releasing) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("RightClick Repo");

			var senderElement = sender as GridView;
			var repo = ((FrameworkElement)e.OriginalSource).DataContext as RepoModel;
			if (senderElement == null || repo == null) return;
			MenuFlyout myFlyout = new MenuFlyout();
			Style s = new Style { TargetType = typeof(MenuFlyoutPresenter) };
			s.Setters.Add(new Setter(RequestedThemeProperty, ElementTheme.Dark));
			myFlyout.MenuFlyoutPresenterStyle = s;

			var el = new MenuFlyoutItem { Text = "Share", Icon = new SymbolIcon(Symbol.Share) };
			el.Click += (sen, ee) => { SharingUrl = repo.html_url; DataTransferManager.ShowShareUI(); };
			myFlyout.Items.Add(el);
			el = new MenuFlyoutItem { Text = "Open in browser", Icon = new SymbolIcon(Symbol.World) };
			el.Click += async (sen, ee) => { await Windows.System.Launcher.LaunchUriAsync(new Uri(repo.html_url)); };
			myFlyout.Items.Add(el);

			if (ApiHandler.AllCategories.Count == 1)
			{
				myFlyout.ShowAt(senderElement, e.GetPosition(senderElement));
				return;
			}

			#region MoveTo Flyout
			var tempCategoriesId = new System.Collections.Generic.List<int>(repo.CategoriesId);
			var menu = new MenuFlyoutSubItem { Text = "Move to" };
			foreach (var item in ApiHandler.AllCategories)
			{
				if (item.Id == 0) continue;
				el = new ToggleMenuFlyoutItem { Text = item.Text, Tag = item.Id.ToString(), IsChecked = tempCategoriesId.Contains(item.Id) };
				el.Click += El_Click;
				menu.Items.Add(el);
			}
			myFlyout.Items.Add(menu);

			async void El_Click(object sen, RoutedEventArgs ee)
			{
				var tag = (sen as FrameworkElement).Tag;
				try
				{
					if (tag == null) throw new Exception();
					var inx = Convert.ToInt32(tag.ToString());
					if (tempCategoriesId.Contains(inx))
					{
						//toggle off
						if (currentTabId == inx) Repositories.Remove(repo);
						tempCategoriesId.Remove(inx);
					}
					else
						//toggle on
						tempCategoriesId.Add(inx);
					repo.CategoriesId = tempCategoriesId.ToArray();
					await ApiHandler.UpdateRepoAsync(User.login, repo);
					MainPage.NotifyElement.Show("✔ Categories Updated", 3000);
				}
				catch { MainPage.NotifyElement.Show("Something is not right!!", 2000); }
			}
			myFlyout.ShowAt(senderElement, e.GetPosition(senderElement));
			#endregion
		}

		#endregion


		private async void AvatarBtn_Click(object sender, RoutedEventArgs e)
		{
			if (ApiKeys.Releasing) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Tap Avatar.FlyoutButton");

			var el = sender as FrameworkElement;
			if (el == null) return;
			switch (el.Tag.ToString())
			{
				case "-1":
					await new AboutPanel().ShowAsync();
					break;
				case "0":
					// load with cache disable
					RefreshPage();
					break;
				case "1":
					//back
					//todo: get categories with repo id or all repo.json
					break;
				case "2":
					//logout
					ApiHandler.LogOut();
					Frame.BackStack.Clear();
					new Services.UI.UIHandler().ChangeHeaderTheme("HeaderAcrylic", Windows.UI.Colors.WhiteSmoke);
					Frame.Navigate(typeof(LoginPage));
					break;
			}
		}
	}
}

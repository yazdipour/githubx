using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GithubX.UWP.Models;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GithubX.UWP.Views
{
	public sealed partial class StarListPage : Page
	{
		OwnerModel User = new OwnerModel();
		ObservableCollection<CategoryModel> Categories { get; set; }
		ObservableCollection<RepoModel> Repositories { get; set; }

		private int currentPageInAllCat = 0;

		#region OnLoad
		public StarListPage()
		{
			this.InitializeComponent();
			SizeChanged += (sender, e) =>
			{
				gridView.Height = ActualHeight - 92;
			};

		}

		async void LoadCategories()
		{
			var ls = await Services.Api.ApiHandler.GetCategoriesAsync(User.login);
			Categories = new ObservableCollection<CategoryModel>(ls);
			if (Categories != null)
				if (Categories.Count > 0)
				{
					Repositories = new ObservableCollection<RepoModel>(Categories[0].RepoList);
					Bindings.Update();
					tabList.SelectedIndex = 0;
					LoadMoreButton.Visibility = Repositories.Count % 30 == 0 ? Visibility.Visible : Visibility.Collapsed;
				}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			User = e.Parameter as OwnerModel;
			App.UserLoginAccountName = User.login;
			if (gridView.Items.Count == 0) LoadCategories();
		}

		#endregion

		#region Events in Tab
		private async void CategorySetting_Click(object sender, RoutedEventArgs e)
		{
			var p = new CategoryPanel();
			await p.ShowAsync();
			RefreshPage();
		}

		private void RefreshPage()
		{
			NavigationCacheMode = NavigationCacheMode.Disabled;
			var type = Frame.CurrentSourcePageType;
			Frame.Navigate(type, User);
			Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
		}

		private async void tabList_ItemClick(object sender, ItemClickEventArgs e)
		{
			var item = e.ClickedItem as CategoryModel;
			new Services.UI.UIHandler().ChangeHeaderTheme("HeaderAcrylic", item.Color);
			await LoadingRepos(item);
		}
		#endregion

		private async Task LoadingRepos(CategoryModel cat, int page = 0)
		{
			try
			{
				var ls = await Services.Api.ApiHandler.GetReposAsync(User.login, cat, page);
				if (page == 0) Repositories = new ObservableCollection<RepoModel>(ls);
				else ls.ForEach(Repositories.Add);

				LoadMoreButton.Visibility = (cat.Id == 0 && ls.Count % 30 == 0) ? Visibility.Visible : Visibility.Collapsed;
			}
			catch (Exception ex)
			{
				MainPage.NotifyElement.Show(ex.Message, 2000);
			}
		}
		private async void LoadMoreButton_Click(object sender, RoutedEventArgs e)
		{
			await LoadingRepos(Categories[0], ++currentPageInAllCat);
		}

		#region GridView Events
		private void AdaptiveGridViewControl_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (e.ClickedItem is RepoModel repo) Frame.Navigate(typeof(RepoPage), repo);
		}

		private void gridView_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
		{
			var senderElement = sender as GridView;
			var repo = ((FrameworkElement)e.OriginalSource).DataContext as RepoModel;

			MenuFlyout myFlyout = new MenuFlyout();
			Style s = new Style { TargetType = typeof(MenuFlyoutPresenter) };
			s.Setters.Add(new Setter(RequestedThemeProperty, ElementTheme.Dark));
			myFlyout.MenuFlyoutPresenterStyle = s;

			MenuFlyoutItem el1 = new MenuFlyoutItem { Text = "Open in browser" };
			MenuFlyoutItem el2 = new MenuFlyoutItem { Text = "Move to" };
			MenuFlyoutItem el3 = new MenuFlyoutItem { Text = "Share" };
			el1.Click += async (sen, ee) =>
			{
				await Windows.System.Launcher.LaunchUriAsync(new Uri(repo.html_url));
			};
			el2.Click += async (sen, ee) =>
			{
				//todo : to sth
				await new CategoryPanel(repo.id).ShowAsync();
			};
			el3.Click += (sen, ee) =>
			{
				DataTransferManager.ShowShareUI();
			};
			DataTransferManager.GetForCurrentView().DataRequested += (sen, args) =>
			{
				DataRequest request = args.Request;
				request.Data.SetText(repo.html_url);
				request.Data.Properties.Title = "Github Repo, Shared by GithubX";
			};
			myFlyout.Items.Add(el1);
			myFlyout.Items.Add(el2);
			myFlyout.Items.Add(el3);
			//myFlyout.Placement = FlyoutPlacementMode.Left;
			myFlyout.ShowAt(senderElement, e.GetPosition(senderElement));
		}
		#endregion

		private async void AvatarBtn_Click(object sender, RoutedEventArgs e)
		{
			var el = sender as FrameworkElement;
			if (el == null) return;
			switch (el.Tag.ToString())
			{
				case "-1":
					await new AboutPanel().ShowAsync();
					break;
				case "0":
					RefreshPage();
					break;
				case "1":
					//back
					//todo: get categories with repo id or all repo.json
					break;
				case "2":
					//logout
					Services.Api.ApiHandler.LogOut();
					Frame.BackStack.Clear();
					Frame.Navigate(typeof(LoginPage));
					break;
			}
		}
	}
}

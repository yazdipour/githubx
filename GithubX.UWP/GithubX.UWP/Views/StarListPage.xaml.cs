using System;
using System.Collections.Generic;
using GithubX.UWP.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GithubX.UWP.Views
{
	public sealed partial class StarListPage : Page
	{
		OwnerModel User = new OwnerModel();
		List<CategoryModel> Categories = new List<CategoryModel>();
		private int currentPageInAllCat = 0;
		#region OnLoad
		public StarListPage()
		{
			this.InitializeComponent();
		}
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			Categories = Services.Api.ApiHandler.GetCategories(User.id);
			if (Categories.Count > 0)
			{
				tabList.SelectedIndex = 0;
				LoadingRepos(0);
			}
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			User = e.Parameter as OwnerModel;
		}

		#endregion

		#region Events in Tab
		private void CategorySetting_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			OpenCategoryDialogAsync();
		}

		private async void OpenCategoryDialogAsync()
		{
			CategoryPanel dialog = new CategoryPanel();
			await dialog.ShowAsync();
		}

		private void tabList_ItemClick(object sender, ItemClickEventArgs e)
		{
			var item = e.ClickedItem as CategoryModel;
			new Services.UI.UIHandler().ChangeHeaderTheme("HeaderAcrylic", item.Color);
			LoadingRepos(item.Id);
		}
		#endregion

		private void LoadingRepos(int catId = 0, int page = 0)
		{
			try
			{
				var cat = Categories.Find(obj => obj.Id == catId);
				if (cat == null) return;
				if (cat.RepoList == null)
					cat.RepoList = Services.Api.ApiHandler.GetRepos(userId: User.id, catId: catId, page: 0);
				else
					cat.RepoList.AddRange(Services.Api.ApiHandler.GetRepos(userId: User.id, catId: catId, page: page));
				LoadMoreButton.Visibility = (catId == 0 && cat.RepoList.Count % 30 == 0) ? Visibility.Visible : Visibility.Collapsed;
				gridView.ItemsSource = cat.RepoList;
			}
			catch { }
		}

		private void LoadMoreButton_Click(object sender, RoutedEventArgs e)
		{
			LoadingRepos(User.id, ++currentPageInAllCat);
		}

		private void AdaptiveGridViewControl_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (e.ClickedItem is RepoModel repo) Frame.Navigate(typeof(RepoPage), repo);
		}
	}
}

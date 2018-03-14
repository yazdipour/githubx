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
			Loaded += (sender, e) =>
			{
				LoadCategories();
			};
			SizeChanged += (sender, e) =>
			{
				gridView.Height = ActualHeight - 92;
			};
		}

		async void LoadCategories()
		{
			Categories = await Services.Api.ApiHandler.GetCategoriesAsync(User.login);
			if (Categories != null)
				if (Categories.Count > 0)
				{
					tabList.ItemsSource = Categories;
					tabList.SelectedIndex = 0;
					gridView.ItemsSource = Categories[0].RepoList;
				}
		}
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			User = e.Parameter as OwnerModel;
			App.UserLoginAccountName = User.login;
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
			LoadingRepos(item);
		}
		#endregion

		private void LoadingRepos(CategoryModel cat, int page = 0)
		{
			try
			{
				gridView.ItemsSource = Services.Api.ApiHandler.GetReposAsync(User.login, cat, page);
				LoadMoreButton.Visibility = (cat.Id == 0 && gridView.Items.Count % 30 == 0) ? Visibility.Visible : Visibility.Collapsed;
			}
			catch { }
		}

		private void LoadMoreButton_Click(object sender, RoutedEventArgs e)
		{
			LoadingRepos(Categories[0], ++currentPageInAllCat);
		}

		private void AdaptiveGridViewControl_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (e.ClickedItem is RepoModel repo) Frame.Navigate(typeof(RepoPage), repo);
		}
	}
}

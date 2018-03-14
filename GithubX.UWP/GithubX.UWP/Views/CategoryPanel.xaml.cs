using System.Collections.Generic;
using GithubX.UWP.Models;
using GithubX.UWP.Services.Api;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class CategoryPanel : ContentDialog
	{
		int RepoId = -1;
		private List<CategoryModel> categories;

		#region Useless section
		public CategoryPanel()
		{
			this.InitializeComponent();
		}

		public CategoryPanel(int rid)
		{
			this.InitializeComponent();
			RepoId = rid;
		}
		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			Hide();
		}
		#endregion

		private async void ContentDialog_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			categories = await ApiHandler.GetCategoriesAsync(App.UserLoginAccountName);
			if (CatList.Items.Count > 0)
			{
				(CatList.Items[0] as ListViewItem).CanDrag = false;
			}
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
//			ApiHandler.SaveCategoriesAsync(App.UserLoginAccountName, categories);
		}


		private void Color_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{

		}

		private void Add_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{

		}
	}
}

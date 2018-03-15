using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GithubX.UWP.Models;
using GithubX.UWP.Services.Api;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace GithubX.UWP.Views
{
	public sealed partial class CategoryPanel : ContentDialog
	{
		int RepoId = -1;
		private ObservableCollection<CategoryModel> categories;

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

			List<CategoryModel> myList = await ApiHandler.GetCategoriesAsync(App.UserLoginAccountName);
			categories = new ObservableCollection<CategoryModel>(myList);
			Bindings.Update();
		}

		private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			var x = CatList.ItemsSource as ObservableCollection<CategoryModel>;
			for (int i = 0; i < x.Count; i++) x[i].OrderId = i;
			await ApiHandler.SaveCategoriesAsync(App.UserLoginAccountName, categories.ToList());
		}

		private void Add_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var catName = flyoutTextBox.Text;
			flyoutTextBox.Text = "😍Fav";
			categories.Add(new CategoryModel()
			{
				Id = Api.UnixTimestamp,
				OrderId = categories[categories.Count - 1].OrderId + 1,
				Text = catName,
				Color = "#0000ff"
				// random !!
			});
			fl.Hide();
			Bindings.Update();
		}

		private void ListView1_OnDragItemsStarting(object sender, DragItemsStartingEventArgs e)
		{
			e.Cancel = e.Items.Any(obj =>
			{
				//ignore All
				return (obj is CategoryModel b && b.Id == 0);
			});
		}

		private void Flyout_Closed(object sender, object e)
		{
			var el = (sender as Flyout).Target.DataContext as CategoryModel;
			var inx = categories.IndexOf(el);
			categories.Insert(inx + 1, el);
			categories.RemoveAt(inx);
		}
	}
}

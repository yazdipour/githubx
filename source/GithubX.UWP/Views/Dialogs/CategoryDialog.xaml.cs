using System;
using System.Linq;
using GithubX.Shared.Models;
using GithubX.UWP.Helpers.Api;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace GithubX.UWP.Views
{
	public sealed partial class CategoryPanel : ContentDialog
	{
		public int CurrentCat { get; }

		public CategoryPanel(int currentCat)
		{
			this.InitializeComponent();
			CurrentCat = currentCat;
			Binding assetsVisibilityBinding = new Binding();
			assetsVisibilityBinding.Source = ApiHandler.AllCategories;
			assetsVisibilityBinding.Mode = BindingMode.TwoWay;
			CatList.SetBinding(ItemsControl.ItemsSourceProperty, assetsVisibilityBinding);

			// Making ALL invisible > to make ALL immutable :))
			Loaded += (s, e) =>
				(CatList.ContainerFromItem(ApiHandler.AllCategories[0]) as ListViewItem).Visibility = Visibility.Collapsed;
		}

		private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			for (int i = 0; i < ApiHandler.AllCategories.Count; i++)
				if (ApiHandler.AllCategories[i].Id != 0)
					ApiHandler.AllCategories[i].OrderId = i + 1; // +1 just to be sure never others can get 0
			await ApiHandler.SaveCategoriesAsync();
		}

		private void Ellipse_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			var el = sender as Windows.UI.Xaml.Shapes.Ellipse;
			el.ContextFlyout.ShowAt(el);
		}

		private void Create_Click(object sender, RoutedEventArgs e)
		{
			var colors = new[] { "#1abc9c", "#16a085", "#f1c40f", "#f39c12", "#2ecc71", "#27ae60", "#e67e22", "#d35400", "#3498db", "#2980b9", "#e74c3c", "#c0392b", "#9b59b6", "#8e44ad", "#34495e", "#2c3e50" };
			var catName = flyoutTextBox.Text.Trim();
			flyoutTextBox.Text = "😍Fav";
			ApiHandler.AllCategories.Add(new Category()
			{
				Id = Helpers.Utils.GetUnixTime(),
				Title = catName,
				Color = colors[new Random().Next(colors.Length)]
			});
			fl.Hide();
		}

		private void Del_Click(object sender, RoutedEventArgs e)
		{
			var btn = (e.OriginalSource as FrameworkElement);
			var cont = btn.DataContext as Category;
			var el = ApiHandler.AllCategories.ToList().Find(o => o.Id == cont.Id);
			if (el != null) ApiHandler.AllCategories.Remove(el);
			//cache still have some repo with ID==del
			//var oldCategoryRepos=ApiHandler.AllRepos.FindAll(o => o.CategoriesId.Contains(cont.Id));
			var oldCategoryRepos = ApiHandler.GetRepoOfCategory(cont.Id);
			foreach (var item in oldCategoryRepos)
			{
				var ls = item.CategoriesId.ToList();
				ls.Remove(cont.Id);
				item.CategoriesId = ls.ToArray();
			}
		}
		#region Flyout.Close
		//Color Picket
		private void Flyout_Closed(object sender, object e)
		{
			var el = (sender as Flyout).Target.DataContext as Category;
			var inx = ApiHandler.AllCategories.IndexOf(el);
			ApiHandler.AllCategories.Insert(inx + 1, el);
			ApiHandler.AllCategories.RemoveAt(inx);
			if (CurrentCat == inx) Helpers.Utils.ChangeHeaderTheme("HeaderAcrylic", ApiHandler.AllCategories[inx].Color);
		}

		//Editor
		private void Flyout_Closed_1(object sender, object e)
		{
			var el = (sender as Flyout).Target.DataContext as Category;
			var inx = ApiHandler.AllCategories.IndexOf(el);
			if (inx == -1) return;
			ApiHandler.AllCategories.Insert(inx + 1, el);
			ApiHandler.AllCategories.RemoveAt(inx);
		}
		#endregion
	}
}

// Archive:

// Cancel Dragging if SelectedItem==ALL
//DragItemsStarting="ListView1_OnDragItemsStarting"
//private void ListView1_OnDragItemsStarting(object sender, DragItemsStartingEventArgs e)
//{
//e.Cancel = e.Items.Any(obj =>
//{
//	return (obj is CategoryModel b && b.Id == 0);
//});
//}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GithubX.UWP.Models;
using GithubX.UWP.Services.Api;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class CategoryPanel : ContentDialog
	{
		private ObservableCollection<CategoryModel> categories;
		public bool NeedRefresh = true;

		#region OnLoad
		public CategoryPanel()
		{
			this.InitializeComponent();
		}

		private async void ContentDialog_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			List<CategoryModel> myList = await ApiHandler.GetCategoriesAsync(App.UserLoginAccountName);
			categories = new ObservableCollection<CategoryModel>(myList);
			Bindings.Update();
		}

		#endregion

		#region MainButtons
		private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			for (int i = 0; i < categories.Count; i++) categories[i].OrderId = i;
			await ApiHandler.SaveCategoriesAsync(App.UserLoginAccountName, categories.ToList());
		}
		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			NeedRefresh = false;
			Hide();
		}
		#endregion

		private void Add_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var colors = new []{ "#1abc9c", "#16a085", "#f1c40f", "#f39c12", "#2ecc71", "#27ae60", "#e67e22", "#d35400", "#3498db", "#2980b9", "#e74c3c", "#c0392b", "#9b59b6", "#8e44ad", "#34495e", "#2c3e50" };
			var catName = flyoutTextBox.Text;
			flyoutTextBox.Text = "😍Fav";
			categories.Add(new CategoryModel()
			{
				Id = Api.UnixTimestamp,
				OrderId = categories[categories.Count - 1].OrderId + 1,
				Text = catName,
				Color = colors[new System.Random().Next(colors.Length)]
			});
			fl.Hide();
			Bindings.Update();
		}

		private void ListView1_OnDragItemsStarting(object sender, DragItemsStartingEventArgs e)
		{
			e.Cancel = e.Items.Any(obj =>
			{
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

		private void Ellipse_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			var el = sender as Windows.UI.Xaml.Shapes.Ellipse;
			el.ContextFlyout.ShowAt(el);
		}
	}
}

using System.Linq;
using GithubX.UWP.Models;
using GithubX.UWP.Services.Api;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class CategoryPanel : ContentDialog
	{
		public CategoryPanel()
		{
			this.InitializeComponent();
			CatList.ItemsSource = ApiHandler.AllCategories;
		}

		private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			for (int i = 0; i < ApiHandler.AllCategories.Count; i++)
				if (ApiHandler.AllCategories[i].Id != 0) ApiHandler.AllCategories[i].OrderId = i;
			await ApiHandler.SaveCategoriesAsync(App.UserLoginAccountName);
		}

		private void Add_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var colors = new[] { "#1abc9c", "#16a085", "#f1c40f", "#f39c12", "#2ecc71", "#27ae60", "#e67e22", "#d35400", "#3498db", "#2980b9", "#e74c3c", "#c0392b", "#9b59b6", "#8e44ad", "#34495e", "#2c3e50" };
			var catName = flyoutTextBox.Text;
			flyoutTextBox.Text = "😍Fav";
			ApiHandler.AllCategories.Add(new CategoryModel()
			{
				Id = Api.UnixTimestamp,
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
			//var el = (sender as Flyout).Target.DataContext as CategoryModel;
			//var inx = ApiHandler.AllCategories.IndexOf(el);
			//ApiHandler.AllCategories.Insert(inx + 1, el);
			//ApiHandler.AllCategories.RemoveAt(inx);
			//new Services.UI.UIHandler().ChangeHeaderTheme("HeaderAcrylic", "");
		}

		private void Ellipse_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			var el = sender as Windows.UI.Xaml.Shapes.Ellipse;
			el.ContextFlyout.ShowAt(el);
		}
	}
}

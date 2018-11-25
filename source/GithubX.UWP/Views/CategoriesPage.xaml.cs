using GithubX.Shared.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class CategoriesPage : Page
	{
		private readonly ObservableCollection<Category> categories = new ObservableCollection<Category>();

		public CategoriesPage() => InitializeComponent();

		private void Add_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button btn) openCategoryDialog(btn?.DataContext as Category);
		}

		private void GridView_ItemClick(object sender, ItemClickEventArgs e)
		{

		}

		private async void openCategoryDialog(Category category = null)
		{
			try
			{
				var d = new AddCategoryDialog(category);
				await d.ShowAsync();
				if (d?.category?.Id != -1)
					categories.Add(d.category);
			}
			catch { }
		}
	}
}

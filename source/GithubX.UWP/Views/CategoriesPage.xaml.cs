using GithubX.Shared.Models;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class CategoriesPage : Page
	{
		private readonly ObservableCollection<Category> categories = new ObservableCollection<Category>();

		public CategoriesPage() => InitializeComponent();

		private async void Add_Click(object sender, RoutedEventArgs e)
		{
			try { await new Dialogs.AddCategoryDialog().ShowAsync(); } catch { }
		}
	}
}

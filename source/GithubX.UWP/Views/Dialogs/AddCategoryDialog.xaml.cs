using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using GithubX.Shared.Models;
using static GithubX.Shared.Helpers.CollectionExtension;

namespace GithubX.UWP.Views
{
	public sealed partial class AddCategoryDialog : ContentDialog
	{
		#region Variables
		public Category category = new Category();
		private readonly Button[] emojiTabs = new Button[6];
		private GradientColor[] ColorsList => Shared.Helpers.FontHelper.colorGroups;
		private readonly ObservableCollection<string> emojiKeyboardList = new ObservableCollection<string>();
		#endregion
		#region ctor
		public AddCategoryDialog(Category cat = null)
		{
			this.InitializeComponent();
			if (cat == null) return;
			category = cat;
			FindName("RemoveBtn");
		}
		#endregion

		private void ContentDialog_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			//set emoji buttons event
			emojiTabs[0] = (Button)FindName("DevButton");
			emojiTabs[1] = (Button)FindName("SmilesButton");
			emojiTabs[2] = (Button)FindName("BalloonButton");
			emojiTabs[3] = (Button)FindName("PizzaButton");
			emojiTabs[4] = (Button)FindName("CarButton");
			emojiTabs[5] = (Button)FindName("HeartButton");
			foreach (var button in emojiTabs)
				button.Click += ChangeCategoryClick;
			ChangeCategoryClick(emojiTabs[0], null);
		}

		private void ChangeCategoryClick(object sender, RoutedEventArgs e)
		{
			var button = (Button)sender;
			var tag = int.Parse(button.Tag.ToString());
			Grid.SetColumn(highlightBorder, tag + 1);
			emojiKeyboardList.RemoveAll();
			emojiKeyboardList.AddRange(Shared.Helpers.FontHelper.GetEmojiGroup(tag));
		}

		private void EmojiPresenter_ItemClick(object sender, ItemClickEventArgs e) => iconTextBlock.Text = e?.ClickedItem?.ToString();

		private void Color_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (e.ClickedItem is GradientColor color)
			{
				category.Background = color;
				Bindings.Update();
			}
		}

		#region return

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			category.Id = (category.Id == -1) ? Helpers.Utils.GetUnixTime() : category.Id;
			if (string.IsNullOrEmpty(category.Title)) category.Title = "new_category";
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
			=> category = null;
		#endregion

		private void RemoveBtn_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class MarkdownPage : Page
	{
		private string tag = "";
		public MarkdownPage() => this.InitializeComponent();

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
		}

		protected async override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			tag = e.Parameter as string;
			if (tag.Equals("Help", System.StringComparison.OrdinalIgnoreCase))
			{
				var fileContent = await Helpers.Utils.ReadFileFromAsset("ms-appx:///Assets/Files/HELP.md");
				markdownTextBlock.Text = $"# Github✘ v{App.Version}{fileContent}";
			}
		}

		private async void markdownTextBlock_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
		{
			await Windows.System.Launcher.LaunchUriAsync(new Uri(e?.Link));
		}
	}
}

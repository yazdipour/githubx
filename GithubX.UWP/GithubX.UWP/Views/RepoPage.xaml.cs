using GithubX.UWP.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GithubX.UWP.Views
{
	public sealed partial class RepoPage : Page
	{
		private RepoModel repo;

		public RepoPage()
		{
			this.InitializeComponent();
		}
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			repo = e.Parameter as RepoModel;
		}
		private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			bool failed = true;
			try
			{
				string path = Services.Api.Api.RepoReadMeUrl(repo.full_name);
				MarkdownText.Text = await Services.Api.ApiHandler.GetReadMeMdAsync(repo.id, path);
				if (MarkdownText.Text.Length > 2) failed = false;
			}
			catch { }
			finally
			{
				//TODO : Add Place holder if Failed
				if (failed)
				{

				}
			}
		}
		private void Page_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
		{
			headerGrid.Width = MarkdownText.Width = ActualWidth - buttonPanel.Margin.Left * 2;
			MarkdownScrollViewer.Height = ActualHeight - 92 - buttonPanel.ActualHeight - buttonPanel.Margin.Top - 32;
		}

		private void Border_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			if (Frame.CanGoBack) Frame.GoBack();
		}

		private void ButtonBar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var btn = e.OriginalSource as Button;
			switch (btn.Tag.ToString())
			{
				case "0":
					break;
				case "1":
					break;
				case "2":
					break;
				case "3":
					break;
			}
		}
	}
}

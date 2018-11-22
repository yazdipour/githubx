using GithubX.Shared.Services;
using Octokit;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace GithubX.UWP.Views
{
	public sealed partial class RepositoryPage : Windows.UI.Xaml.Controls.Page
	{
		private ObservableCollection<RepositoryContent> _contents { get; set; }
		private string _markdownContent { get; set; } = "> Loading";
		private Repository _repository { get; set; }

		public RepositoryPage()
		{
			this.InitializeComponent();
		}

		private async void Page_Loading(FrameworkElement sender, object args)
		{
			_contents = await GithubService.RepositoryService.GetRepositoryContent(_repository.Id);
			_markdownContent = (await GithubService.RepositoryService.GetRepositoryReadme(_repository.Id))?.Content;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			_repository = e.Parameter as Repository;
		}

		private async void AddToCategory_Click(object sender, RoutedEventArgs e)
		{
			await new AddCategoryDialog().ShowAsync();
		}

		private async void Content_Click(object sender, ItemClickEventArgs e)
		{
			var content = e.ClickedItem as RepositoryContent;
			switch (content.Type.Value)
			{
				case ContentType.Dir:
					_contents = await GithubService.RepositoryService.GetRepositoryContent(_repository.Id, content.Path);
					break;
				case ContentType.File:
					_markdownContent = await Helpers.Utils.GetMarkDownReadyAsync(content.Url, content.Sha);
					break;
				case ContentType.Submodule:
					await Helpers.Utils.OpenUri(content.SubmoduleGitUrl);
					break;
				case ContentType.Symlink:
					await Helpers.Utils.OpenUri(content.Url);
					break;
			}
		}

		private void ToggleButton_Checked(object sender, RoutedEventArgs e)
		{

		}

		private void ToggleButton_UnChecked(object sender, RoutedEventArgs e)
		{

		}

		private void AppBarButton_Click(object sender, RoutedEventArgs e)
		{
		}

	}
}

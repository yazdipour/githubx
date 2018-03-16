using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class AboutPanel : ContentDialog
	{
		public AboutPanel()
		{
			this.InitializeComponent();
		}
		

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}
	}
}

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace GithubX.UWP.Views
{
	public sealed partial class LoginPage : Page
	{
		public LoginPage()
		{
			this.InitializeComponent();
		}

		private async void Login_Click(object sender, RoutedEventArgs e)
		{
			LoadingControl.IsLoading = true;
			try
			{
				var user = await Services.Api.ApiHandler.LoginAsync(accTextBox.Text.Trim());
				if (user != null)
					Frame.Navigate(typeof(StarListPage), user);
				else MainPage.NotifyElement.Show("Error! Try Again", 2000);
			}
			catch { }
			LoadingControl.IsLoading = false;
		}
	}
}
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

		private void Login_Click(object sender, RoutedEventArgs e)
		{
			Login(accTextBox.Text.Trim());
			async void Login(string acc)
			{
				MainPage.LoadingLottie.IsLoading = true;
				var user = await Services.Api.ApiHandler.LoginAsync(acc);
				MainPage.LoadingLottie.IsLoading = false;
				if (user != null)
					Frame.Navigate(typeof(StarListPage), user);
				else
				{
					MainPage.NotifyElement.Content = "Error! Try Again";
					MainPage.NotifyElement.Show();
				}
			}
		}
	}
}
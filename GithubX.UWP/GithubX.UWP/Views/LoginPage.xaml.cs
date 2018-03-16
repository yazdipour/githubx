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
			if (!Services.Api.HttpHandler.CheckConnection) Frame.Navigate(typeof(ErrorPage));
			LoadingControl.IsLoading = true;
			await Login(accTextBox.Text.Trim());
			LoadingControl.IsLoading = false;
			async System.Threading.Tasks.Task Login(string acc)
			{
				var user = await Services.Api.ApiHandler.LoginAsync(acc);
				if (user != null)
					Frame.Navigate(typeof(StarListPage), user);
				else
					MainPage.NotifyElement.Show("Error! Try Again", 2000);
			}
		}
	}
}
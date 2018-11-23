using GithubX.Shared.Services;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP
{
	public sealed partial class App : Application
	{
		internal static string UserLoginAccountName { get; set; }
		internal static string Version = $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}";

		public App()
		{
			this.InitializeComponent();
			this.Suspending += OnSuspending;
		}

		protected override void OnLaunched(LaunchActivatedEventArgs args)
		{
			if (!(Window.Current.Content is Frame rootFrame))
			{
				rootFrame = new Frame();
				Window.Current.Content = rootFrame;
			}
			if (args?.PrelaunchActivated != true)
			{
				if (rootFrame.Content == null)
					rootFrame.Navigate(typeof(Views.LoginPage), args?.Arguments);
				Window.Current.Activate();
			}
		}

		private void OnSuspending(object sender, SuspendingEventArgs e) =>
			e.SuspendingOperation.GetDeferral().Complete();

		protected async override void OnActivated(IActivatedEventArgs args)
		{
			switch (args.Kind)
			{
				case ActivationKind.Protocol:
					string response = ((ProtocolActivatedEventArgs)args)?.Uri?.ToString();
					if (response.Contains(GithubService.FallBackUri))
						GithubService.Auth.SaveCredential(await GithubService.Auth.GetCredentialsAsync(response));
					break;
				case ActivationKind.CommandLineLaunch:
					OnLaunched(null);
					break;
			}
		}
	}
}

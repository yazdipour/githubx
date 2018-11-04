using GithubX.Shared.Services;
using GithubX.UWP.Services.Api;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP
{
	public sealed partial class App : Application
	{
		internal static string UserLoginAccountName { get; set; }
		internal static string PocketProtocol = "githubx://pocket";
		internal static string Version = $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}";

		public App()
		{
			this.InitializeComponent();
			this.Suspending += OnSuspending;
			Shared.Handlers.CacheHandler.InitCache();
			Logger.Init(ApiKeys.AppCenter);
		}

		protected override void OnLaunched(LaunchActivatedEventArgs args)
		{
#if DEBUG
			DebugSettings.EnableFrameRateCounter = System.Diagnostics.Debugger.IsAttached;
#endif
			if (!(Window.Current.Content is Frame rootFrame))
			{
				rootFrame = new Frame();
				Window.Current.Content = rootFrame;
			}
			if (args?.PrelaunchActivated != true)
			{
				if (rootFrame.Content == null)
					rootFrame.Navigate(typeof(MainPage), args?.Arguments);
				Window.Current.Activate();
			}
		}

		private void OnSuspending(object sender, SuspendingEventArgs e) =>
			e.SuspendingOperation.GetDeferral().Complete();

		protected override void OnActivated(IActivatedEventArgs args)
		{
			switch (args.Kind)
			{
				case ActivationKind.Protocol:
					string uri = ((ProtocolActivatedEventArgs)args)?.Uri?.ToString();
					break;
				case ActivationKind.CommandLineLaunch:
					OnLaunched(null);
					break;
			}
		}
	}
}

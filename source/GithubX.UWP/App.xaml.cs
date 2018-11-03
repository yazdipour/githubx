using System;
using GithubX.UWP.Services.Api;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GithubX.UWP
{
	sealed partial class App : Application
	{
		internal static string UserLoginAccountName { get; set; }
		internal static string PocketProtocol = "githubx://pocket";
		internal static string Version = $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}";

		public App()
		{
			this.InitializeComponent();
			this.Suspending += OnSuspending;
			if (ApiKeys.AppCenter != null) AppCenter.Start(ApiKeys.AppCenter, typeof(Analytics));
			Shared.Handlers.CacheHandler.InitCache();
		}

		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached)
				this.DebugSettings.EnableFrameRateCounter = true;
#endif

			if (!(Window.Current.Content is Frame rootFrame))
			{
				rootFrame = new Frame();
				Window.Current.Content = rootFrame;
			}

			if (e?.PrelaunchActivated != true)
			{
				if (rootFrame.Content == null)
					rootFrame.Navigate(typeof(MainPage), e?.Arguments);
				// Ensure the current window is active
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

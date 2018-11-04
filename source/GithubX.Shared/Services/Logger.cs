using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GithubX.Shared.Services
{
	public static class Logger
	{
		public static void Init(string token)
		{
			AppCenter.Start(token ?? "", typeof(Analytics), typeof(Crashes));
			AppCenter.LogLevel = LogLevel.Error; // Not sure
			CustomProperties properties = new CustomProperties();
			properties.Set("color", "red").Set("now", DateTime.UtcNow);
			AppCenter.SetCustomProperties(properties);
		}

		public static async System.Threading.Tasks.Task<string> didAppCrashed()
		{
			bool didAppCrash = await Crashes.HasCrashedInLastSessionAsync().ConfigureAwait(false);
			if (!didAppCrash) return null;
			ErrorReport crashReport = await Crashes.GetLastSessionCrashReportAsync().ConfigureAwait(false);
			Crashes.NotifyUserConfirmation(UserConfirmation.Send);
			return crashReport.ToString();
		}

		public static async System.Threading.Tasks.Task SystemGuIdAsync() =>
			await AppCenter.GetInstallIdAsync().ConfigureAwait(false);

		public static void E(Exception e, Dictionary<string, string> properties = null)
		{
			AppCenterLog.Error(properties?.Keys?.ElementAt(0) ?? "",
				properties?.Values?.ElementAt(0) ?? "", e);
			Crashes.TrackError(e, properties);
		}

		public static void T(string msg, Dictionary<string, string> properties = null)
			=> Analytics.TrackEvent(msg ?? "[NULL MSG]", properties);

		public static void D(string msg, string tag)
			=> AppCenterLog.Debug(tag, msg);

		public static void Enable(bool enable)
			=> AppCenter.SetEnabledAsync(enable);

		public static async System.Threading.Tasks.Task<bool> IsEnable()
			=> await AppCenter.IsEnabledAsync().ConfigureAwait(false);
	}
}

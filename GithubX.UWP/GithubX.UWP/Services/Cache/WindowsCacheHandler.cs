using System;
using Windows.Storage;

namespace GithubX.UWP.Services.Cache
{
	static class WindowsCacheHandler
	{
		public static bool Exists(string address, bool roam = false)
		{
			return Read(address, roam) != null;
		}

		public static string Read(string address, bool roam = false)
		{
			var ls = !roam ? ApplicationData.Current.LocalSettings : ApplicationData.Current.RoamingSettings;
			try
			{
				return ls.Values[address].ToString();
			}
			catch (Exception) { return null; }
		}

		public static bool Save(string address, string setting, bool roam = false)
		{
			var ls = !roam ? ApplicationData.Current.LocalSettings : ApplicationData.Current.RoamingSettings;
			try
			{
				ls.Values[address] = setting;
				return true;
			}
			catch (Exception) { return false; }
		}

		public static void Remove(string address, bool roam = false)
		{
			var ls = !roam ? ApplicationData.Current.LocalSettings : ApplicationData.Current.RoamingSettings;
			try
			{
				ls.Values.Remove(address);
			}
			catch (Exception) { }
		}
	}
}

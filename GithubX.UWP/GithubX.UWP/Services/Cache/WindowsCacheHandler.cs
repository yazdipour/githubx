using System;
using Windows.Storage;

namespace GithubX.UWP.Services.Cache
{
	internal class WindowsCacheHandler
	{
		ApplicationDataContainer ls = ApplicationData.Current.LocalSettings;

		public bool Exists(string address)
		{
			return Read(address) != null;
		}

		public string Read(string address)
		{
			try
			{
				return ls.Values[address].ToString();
			}
			catch (Exception) { return null; }
		}

		public bool Write(string address, string setting)
		{
			try
			{
				ls.Values[address] = setting;
				return true;
			}
			catch (Exception) { return false; }
		}

		public void Remove(string address)
		{
			try
			{
				ls.Values.Remove(address);
			}
			catch (Exception) { }
		}
	}
}

using System;
using System.Threading.Tasks;
using PocketSharp;
using PocketSharp.Models;

namespace GithubX.UWP.Services.Api
{
	class PocketApi
	{
		private Cache.WindowsCacheHandler CacheHandler = new Cache.WindowsCacheHandler();
		private PocketClient client;

		private PocketClient LoadCacheClient()
		{
			string cacheKey = Cache.CacheKeys.Pocket;
			if (!CacheHandler.Exists(cacheKey)) return null;
			return new PocketClient(ApiKeys.Pocket, CacheHandler.Read(cacheKey));
		}

		private void SaveCacheUser(PocketUser user) => CacheHandler.Write(Cache.CacheKeys.Pocket, user.Code);

		public async Task<bool> LoginUser()
		{
			var user = await client.GetUser();
			if (user == null) return false;
			SaveCacheUser(user);
			return true;
		}

		public bool CheckLogin()
		{
			client = LoadCacheClient();
			return (client != null);
		}

		public async Task<Uri> LoginUriAsync()
		{
			client = new PocketClient(ApiKeys.Pocket, callbackUri: App.PocketProtocol);
			string requestCode = await client.GetRequestCode();
			return client.GenerateAuthenticationUri();
		}

		public async Task<PocketItem> Post(string url, string[] tags) => await client.Add(new Uri(url), tags);
	}
}

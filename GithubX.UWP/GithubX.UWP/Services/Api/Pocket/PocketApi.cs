using System;
using System.Threading.Tasks;
using PocketSharp;
using PocketSharp.Models;

namespace GithubX.UWP.Services.Api.Pocket
{
	class PocketApi
	{
		private Cache.WindowsCacheHandler CacheHandler = new Cache.WindowsCacheHandler();

		public static PocketClient client;

		internal bool CheckLogin()
		{
			client = LoadCacheClient();
			return (client == null);
		}

		internal async Task<Uri> LoginUriAsync()
		{
			client = new PocketClient(ApiKeys.Pocket, callbackUri: App.PocketProtocol);
			string requestCode = await client.GetRequestCode();
			return client.GenerateAuthenticationUri();
		}

		private PocketClient LoadCacheClient()
		{
			string cacheKey = Cache.CacheKeys.Pocket;
			if (CacheHandler.Exists(cacheKey))
				return new PocketClient(ApiKeys.Pocket, CacheHandler.Read(cacheKey));
			return null;
		}

		private void SaveCacheUser(PocketUser user) 
			=> CacheHandler.Write(Cache.CacheKeys.Pocket, user.Code);

		internal async Task<PocketItem> Post(string url, string[] tags) 
			=> await client.Add(new Uri(url), tags);
	}
}

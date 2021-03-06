﻿using System.Reactive.Linq;
using Akavache;
using Refit;
using System;
using System.Threading.Tasks;

namespace GithubX.Shared.Services.Pocket
{
	public class PocketService
	{
		private readonly IPocketApi Api = RestService.For<IPocketApi>("https://getpocket.com/v3/");
		private string ApiKey { get; } //consumer_key
		private string AccessToken { get; set; }
		public string FallBackUri { get; } = "githubx://pocket";

		public PocketService(string apiKey, string accessToken = null)
		{
			ApiKey = apiKey;
			if (accessToken != null) AccessToken = accessToken;
		}

		public async Task<(string requestCode, Uri uri)> GenerateAuthUri()
		{
			RequestCode requestCode = await Api.GetRequestToken(FallBackUri, ApiKey);
			if (requestCode == null) throw new NullReferenceException("Null request_code");
			const string authentificationUri = "https://getpocket.com/auth/authorize?request_token={0}&redirect_uri={1}&mobile={2}&force={3}&webauthenticationbroker={4}";
			return (requestCode.Code, new Uri(string.Format(authentificationUri, requestCode.Code, FallBackUri, "1", "login", "1")));
		}

		public async Task<string> GetUserToken(string requestCode)
		{
			if (requestCode == null) throw new NullReferenceException("Call GetRequestCode() first to receive a request_code");
			var user = await Api.GetUserToken(requestCode, ApiKey);
			return AccessToken = user.Access_token;
		}

		public async Task Add(Uri uri)
			=> await Api.PostArticle(new AddParameters(uri?.AbsoluteUri, ApiKey, AccessToken));

		public async void LoadFromCache() => AccessToken = await BlobCache.UserAccount.GetObject<string>("pocket");

		public async void SaveInCache() => await BlobCache.UserAccount.InsertObject("pocket", AccessToken);

		public bool IsLoggedIn() => AccessToken.Length > 1;
	}
}

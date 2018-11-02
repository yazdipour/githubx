using System.Reactive.Linq;
using Akavache;
using Refit;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GithubX.Shared.Services.Pocket
{
	class PocketService
	{
		private readonly IPocketApi Api = RestService.For<IPocketApi>("https://getpocket.com/v3/");
		private string ApiKey { get; } //consumer_key
		private string AccessToken { get; set; }
		private string CallbackUri { get; }

		public PocketService(string apiKey, string callbackUri, string accessToken = null)
		{
			ApiKey = apiKey;
			CallbackUri = Uri.EscapeUriString(callbackUri);
			if (accessToken != null) AccessToken = accessToken;
		}

		public async Task<(string requestCode, Uri uri)> GenerateAuthUri()
		{
			RequestCode requestCode = await Api.GetRequestToken(CallbackUri, ApiKey).ConfigureAwait(false);
			if (requestCode == null) throw new NullReferenceException("Null request_code");
			const string authentificationUri = "https://getpocket.com/auth/authorize?request_token={0}&redirect_uri={1}&mobile={2}&force={3}&webauthenticationbroker={4}";
			return (requestCode.Code, new Uri(string.Format(authentificationUri, requestCode.Code, CallbackUri, "1", "login", "1")));
		}

		public async Task<string> GetUserToken(string requestCode)
		{
			if (requestCode == null) throw new NullReferenceException("Call GetRequestCode() first to receive a request_code");
			var user = await Api.GetUserToken(requestCode, ApiKey).ConfigureAwait(false);
			return AccessToken = user.Access_token;
		}

		public async Task Add(Uri uri)
			=> await Api.PostArticle(new AddParameters(uri?.AbsoluteUri, ApiKey, AccessToken)).ConfigureAwait(false);

		public async void LoadFromCache() => AccessToken = await BlobCache.UserAccount.GetObject<string>("pocket");

		public async void SaveInCache() => await BlobCache.UserAccount.InsertObject("pocket", AccessToken);
	}
}

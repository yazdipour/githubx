using Octokit;
using System;
using System.Threading.Tasks;
using Akavache;

namespace GithubX.Shared.Services
{
	internal class AuthService
	{
		public Uri GetOauthUrl(GitHubClient client, string ClientId, string FallBackUri)
		{
			var request = new OauthLoginRequest(ClientId)
			{
				Scopes = { "user", "repo", "gist" },
				RedirectUri = new Uri(FallBackUri),
			};
			return client?.Oauth?.GetGitHubLoginUrl(request);
		}

		public async Task<Credentials> GetCredentialsAsync
			(GitHubClient client, string response, string clientId, string clientSecret)
		{
			try
			{
				string responseData = response.Substring(response.IndexOf("code"));
				string[] keyValPairs = responseData.Split('=');
				string code = keyValPairs[1].Split('&')[0];
				var request = new OauthTokenRequest(clientId, clientSecret, code);
				var token = await client.Oauth.CreateAccessToken(request).ConfigureAwait(false);
				return new Credentials(token.AccessToken);
			}
			catch (Exception e)
			{
				//Log e
				return null;
			}
		}

		public void SaveCredential(Credentials credentials)
			=> BlobCache.Secure.SaveLogin(credentials.Login, credentials.GetToken());

		public IObservable<LoginInfo> ReadCredential() => BlobCache.Secure.GetLoginAsync();

		public bool IsLoadedIn(GitHubClient client) => client.Credentials != null;

		public void LogOut() => BlobCache.Secure.EraseLogin();
	}
}

using Octokit;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;

namespace GithubX.Shared.Services
{
	public class AuthService
	{
		public Uri GetOauthUrl()
		{
			var request = new OauthLoginRequest(GithubService.ClientId)
			{
				Scopes = { "user", "repo", "gist" },
				RedirectUri = new Uri(GithubService.FallBackUri),
			};
			return GithubService.Client.Oauth?.GetGitHubLoginUrl(request);
		}

		public async Task<Credentials> GetCredentialsAsync(string response)
		{
			try
			{
				string responseData = response.Substring(response.IndexOf("code"));
				string[] keyValPairs = responseData.Split('=');
				string code = keyValPairs[1].Split('&')[0];
				var request = new OauthTokenRequest(GithubService.ClientId, GithubService.ClientSecret, code);
				var token = await GithubService.Client.Oauth.CreateAccessToken(request);
				return new Credentials(token.AccessToken);
			}
			catch (Exception e)
			{
				Logger.E(e);
				return null;
			}
		}

		public void SaveCredential(Credentials credentials)
			=> BlobCache.UserAccount.InsertObject(GithubService.FallBackUri, credentials.GetToken());

		public async Task<Credentials> ReadCredential()
		{
			try
			{
				var token= await BlobCache.UserAccount.GetObject<string>(GithubService.FallBackUri);
				return new Credentials(token);
			}
			catch { return null; }
		}

		public bool IsLoggedIn() => GithubService.Client.Credentials != null;

		public void LogOut() => BlobCache.Secure.EraseLogin();
	}
}

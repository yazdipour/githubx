using Octokit;

namespace GithubX.Shared.Services
{
	public class GithubService
	{
		private const string AppName = "XGithub";
		private const string ClientId = "2e33e200f38ca1bb532b";
		private const string ClientSecret = "4162f549c706999244a32f035d658c94dc1a16dd";
		private const string FallBackUri = "githubx://auth";

		public GitHubClient Client
		{
			get { return Client ?? new GitHubClient(new ProductHeaderValue(AppName)); }
			set { Client = value; }
		}

		public void SetClient(string token) => Client = new GitHubClient(new ProductHeaderValue(AppName)) { Credentials = new Credentials(token) };

		
	}
}

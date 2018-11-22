using Octokit;

namespace GithubX.Shared.Services
{
	public static class GithubService
	{
		//TODO: revoke these keys and get new ones and Ignore them
		public const string AppName = "XGithub";
		public const string ClientId = "2e33e200f38ca1bb532b";
		public const string ClientSecret = "4162f549c706999244a32f035d658c94dc1a16dd";
		public const string FallBackUri = "githubx://auth";
		public static GitHubClient Client { get; set; } = new GitHubClient(new ProductHeaderValue(AppName));
		public static AuthService Auth = new AuthService();
		private static RepositoryService repositoryService;
		private static SearchService searchService;
		private static GistService gistService;
		private static UserService userService;
		private static GitHubClient client;

		public static void SetClient(string token)
			=> Client = new GitHubClient(new ProductHeaderValue(AppName)) { Credentials = new Credentials(token) };

		public static RepositoryService RepositoryService
		{
			get { return repositoryService ?? (repositoryService = new RepositoryService(ref client)); }
			set { repositoryService = value; }
		}

		public static GistService GistService
		{
			get { return gistService ?? (gistService = new GistService(ref client)); }
			set { gistService = value; }
		}

		public static SearchService SearchService
		{
			get { return searchService ?? (searchService = new SearchService(ref client)); }
			set { searchService = value; }
		}

		public static UserService UserService
		{
			get { return userService ?? (userService = new UserService(ref client)); }
			set { userService = value; }
		}
	}
}

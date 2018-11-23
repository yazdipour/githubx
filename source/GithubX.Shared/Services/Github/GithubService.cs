using Octokit;

namespace GithubX.Shared.Services
{
	public static class GithubService
	{
		//TODO: revoke these keys and get new ones and Ignore them
		public const string AppName = "XGithub";
		public const string ClientId = Keys.GithubClientId;
		public const string ClientSecret = Keys.GithubClientSecret;
		public const string FallBackUri = "githubx://auth";
		public static GitHubClient Client = new GitHubClient(new ProductHeaderValue(AppName));
		public static AuthService Auth = new AuthService();
		private static RepositoryService repositoryService;
		private static SearchService searchService;
		private static GistService gistService;
		private static UserService userService;

		public static void SetClient(string token)
			=> Client = new GitHubClient(new ProductHeaderValue(AppName)) { Credentials = new Credentials(token) };

		public static RepositoryService RepositoryService
		{
			get { return repositoryService ?? (repositoryService = new RepositoryService(ref Client)); }
			set { repositoryService = value; }
		}

		public static GistService GistService
		{
			get { return gistService ?? (gistService = new GistService(ref Client)); }
			set { gistService = value; }
		}

		public static SearchService SearchService
		{
			get { return searchService ?? (searchService = new SearchService(ref Client)); }
			set { searchService = value; }
		}

		public static UserService UserService
		{
			get { return userService ?? (userService = new UserService(ref Client)); }
			set { userService = value; }
		}
	}
}

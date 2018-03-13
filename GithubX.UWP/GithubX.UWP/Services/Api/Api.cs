namespace GithubX.UWP.Services.Api
{
	static class Api
	{
		public static string AccountInfoUrl(string acc)
		{
			return "https://api.github.com/users/" + acc;
		}

		public static string AccountStarsUrl(string acc)
		{
			return "https://api.github.com/users/" + acc + "/starred";
		}

		public static string RepoReadMeUrl(string fullName)
		{
			return "https://raw.githubusercontent.com/" + fullName + "/master/README.md";
		}
		public static string HttpUserAgent = "User-Agent: Github-X-App";
	}
}

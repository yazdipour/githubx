namespace GithubX.UWP.Services.Cache
{
	class CacheKeys
	{
		internal static string UserKey = "user-profile";

		internal static string ReadmeTheme = "user-theme";

		internal static string Pocket = "pocket-user";

		internal static string CategoriesKey(string user) => "cat-" + user;

		internal static string RepositoriesKey(string user) => "repo-" + user;

		internal static string Readme(int id) => "md-" + id;

		//public static string LastUpdate = "last-update";
	}
}

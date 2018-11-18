namespace GithubX.UWP.Helpers.Cache
{
	class CacheKeys
	{
		internal static string UserKey = "user-profile";

		internal static string ReadmeTheme = "user-theme";

		internal static string Pocket = "pocket-user";

		internal static string CategoriesKey => "cat-" + App.UserLoginAccountName;

		internal static string RepositoriesKey => "repo-" + App.UserLoginAccountName;

		internal static string ContentsKey(int id) => "content-repo-" + id;
	}
}

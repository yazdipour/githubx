namespace GithubX.UWP.Services.Cache
{
	class CacheKeys
	{
		public static string UserKey = "user-profile";
		public static string CategoriesKey(string user) => "cat-" + user;
		public static string RepositoriesKey(string user) => "repo-" + user;
		public static string Readme(int id) => "md-" + id;
		//public static string LastUpdate = "last-update";
	}
}

namespace GithubX.UWP.Services.Cache
{
	class CacheKeys
	{
		public static string UserKey = "user";
		public static string CategoriesKey(string user) => "user-cat-" + user;
		public static string CategoryKey(int catId) => "cat-" + catId;
		public static string Readme(int id) => "md-" + id;
		public static string LastUpdate = "last-update";
	}
}

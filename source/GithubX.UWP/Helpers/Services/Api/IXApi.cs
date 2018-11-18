using System.Collections.Generic;
using System.Threading.Tasks;
using GithubX.Shared.Models;
using Refit;

namespace GithubX.UWP.Helpers.Api
{
	[Headers("User-Agent: XGithub")]
	interface IXApi
	{
		[Get("/user.php?u={user}")]
		Task<User> GetUser(string user, [Header("Authorization")] string authorization);

		#region Repos
		[Get("/repos.php?u={user}")]
		Task<List<Repo>> GetRepos(string user, string token);

		[Post("/repos.php?u={user}")]
		Task PostRepos(string user, string token, [Body] List<Repo> repos);
		#endregion

		#region Category
		[Get("/categories.php?u={user}&t={token}")]
		Task<User> GetCategories(string user, string token);

		[Post("/categories.php?u={user}&t={token}")]
		Task<User> PostCategories(string user, string token, [Body] Category cat);
		#endregion
	}
}

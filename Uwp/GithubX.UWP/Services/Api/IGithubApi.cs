using System.Collections.Generic;
using System.Threading.Tasks;
using GithubX.UWP.Models;
using Refit;

namespace GithubX.UWP.Services.Api
{
	[Headers("User-Agent: XGithub")]
	interface IGithubApi
	{
		[Get("/users/{user}")]
		Task<User> GetUser(string user);

		[Get("/users/{user}/starred?page={page}")]
		Task<List<Repo>> GetUserStars(string user, int page = 1);

		[Get("/repos/{user}/{repoName}/contents/{subDir}")]
		Task<List<RepoContent>> GetRepoContent(string user, string repoName,string subDir);
	}
}

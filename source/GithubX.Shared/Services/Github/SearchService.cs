using Octokit;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GithubX.Shared.Services
{
	public class SearchService
	{
		private readonly GitHubClient client;
		public SearchService(ref GitHubClient _client) => client = _client;

		public async Task<SearchUsersResult> SearchUsers(GitHubClient client, string query, Language? language = null)
		{
			var request = new SearchUsersRequest(query);
			if (language != null) request.Language = language;
			return await client.Search.SearchUsers(request);
		}

		public async Task<SearchRepositoryResult> SearchRepos(string query, Language? language = null)
		{
			var request = new SearchRepositoriesRequest(query);
			if (language != null) request.Language = language;
			return await client.Search.SearchRepo(request);
		}
	}
}

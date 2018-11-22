using Octokit;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GithubX.Shared.Services
{
	public class SearchService
	{
		private readonly GitHubClient client;
		public SearchService(ref GitHubClient _client) => client = _client;

		public async Task<ObservableCollection<User>> SearchUsers(GitHubClient client, string query, Language? language = null)
		{
			var request = new SearchUsersRequest(query);
			if (language != null) request.Language = language;
			var result = await client.Search.SearchUsers(request).ConfigureAwait(false);
			return new ObservableCollection<User>(result.Items);
		}

		public async Task<ObservableCollection<Repository>> SearchRepos(string query, Language? language = null)
		{
			var request = new SearchRepositoriesRequest(query);
			if (language != null) request.Language = language;
			var result = await client.Search.SearchRepo(request);
			return new ObservableCollection<Repository>(result.Items);
		}
	}
}

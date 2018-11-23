using Octokit;
using System.Threading.Tasks;

namespace GithubX.Shared.Services
{
	public class GistService
	{
		private readonly GitHubClient client;
		public GistService(ref GitHubClient client) => this.client = client;

		public async Task<Gist> CreateGist(NewGist newGist)
			=> await client.Gist.Create(newGist);

		public async Task<Gist> GetGist(string gistId)
			=> await client.Gist.Get(gistId);

		public void UpdateGist(string gistId, (string fileId, string content)[] fileIds)
		{
			GistUpdate g = new GistUpdate();
			foreach (var (fileId, content) in fileIds) g.Files[fileId].Content = content;
			client.Gist.Edit(gistId, g);
		}
	}
}

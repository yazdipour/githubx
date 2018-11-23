using Akavache;
using Octokit;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System;

namespace GithubX.Shared.Services
{
	public class RepositoryService
	{
		private readonly GitHubClient client;
		public RepositoryService(ref GitHubClient _client) => client = _client;

		public async Task<Repository> GetRepository(string ownerName, string repoName)
			=> await client.Repository.Get(ownerName, repoName).ConfigureAwait(false);

		public async Task<Repository> GetRepository(long repoId)
			=> await client.Repository.Get(repoId).ConfigureAwait(false);

		#region Content
		public async Task<ObservableCollection<RepositoryContent>> GetRepositoryContentTextByPath(Repository repo, string path, string branch)
			=> new ObservableCollection<RepositoryContent>(await client.Repository.Content.GetAllContentsByRef(repo.Id, path, branch).ConfigureAwait(false));

		public async Task<string> GetReadmeHTMLForRepository(long repoId)
			=> await client.Repository.Content.GetReadmeHtml(repoId).ConfigureAwait(false);

		public async Task<Readme> GetRepositoryReadme(long repoId)
			=> await client.Repository.Content.GetReadme(repoId).ConfigureAwait(false);

		public async Task<ObservableCollection<RepositoryContent>> GetRepositoryContent(long repoId)
			=> new ObservableCollection<RepositoryContent>(await client.Repository.Content.GetAllContents(repoId));

		public async Task<ObservableCollection<RepositoryContent>> GetRepositoryContent(long repoId, string path)
			=> new ObservableCollection<RepositoryContent>(await client.Repository.Content.GetAllContents(repoId, path));

		private async Task<string> GetMarkDownReadyAsync(string url, bool fromCache = true)
		{
			var buffer = await BlobCache.LocalMachine.DownloadUrl(url, fetchAlways: !fromCache);
			string md = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
			try
			{
				// Html2Markdown needs HtmlAgilityPack.NugetPkg
				//md = new Html2Markdown.Converter().Convert(md).Trim();
				//md = md.Replace("[`", "[").Replace("`]", "]").Replace("<<", "");
				if (md == null || md.Length < 2) return "> 404 🤔";
				return md;
			}
			catch { return md; }
		}
		public async Task<string> GetMarkDownReadyAsync(RepositoryContent content)
		{
			var type = content.Name.Substring(content.Name.LastIndexOf("."));
			if (type.Equals("png", StringComparison.OrdinalIgnoreCase)
				|| type.Equals("jpg", StringComparison.OrdinalIgnoreCase)
				|| type.Equals("jpeg", StringComparison.OrdinalIgnoreCase)
				|| type.Equals("bmp", StringComparison.OrdinalIgnoreCase)
				|| type.Equals("tiff", StringComparison.OrdinalIgnoreCase))
				return $"![]({content.Url})";
			var oldSha = await BlobCache.LocalMachine.GetObject<string>("_" + content.Url) ?? "";
			await BlobCache.LocalMachine.InsertObject("_" + content.Url, content.Sha);
			var result = await GetMarkDownReadyAsync(content.Url, content.Sha.Equals(oldSha));
			return type.Equals("md", StringComparison.OrdinalIgnoreCase) ? result : $"``` {result} ```";
		}
		#endregion

		#region Branch
		public async Task<string> GetDefaultBranch(long repoId)
		{
			var repo = await client.Repository.Get(repoId);
			return repo.DefaultBranch;
		}

		public async Task<ObservableCollection<string>> GetAllBranches(Repository repo)
		{
			var branches = await client.Repository.Branch.GetAll(repo.Owner.Login, repo.Name).ConfigureAwait(false);
			ObservableCollection<string> branchList = new ObservableCollection<string>();
			foreach (Branch i in branches) branchList.Add(i.Name);
			return branchList;
		}
		#endregion

		#region Fork
		public async Task<Repository> ForkRepository(Repository repo)
			=> await client.Repository.Forks.Create(repo.Id, new NewRepositoryFork()).ConfigureAwait(false);
		#endregion

		#region Watch
		public async Task<bool> IsWatched(Repository repo)
			=> await client.Activity.Watching.CheckWatched(repo.Id).ConfigureAwait(false);

		public async Task<bool> WatchRepository(Repository repo)
			=> (await client.Activity.Watching.WatchRepo(repo.Id, new NewSubscription { Subscribed = true }).ConfigureAwait(false)).Subscribed;

		public async Task<bool> UnwatchRepository(Repository repo)
			=> await client.Activity.Watching.UnwatchRepo(repo.Id).ConfigureAwait(false);
		#endregion

		#region Star
		public async Task<bool> IsStarred(Repository repo)
			=> await client.Activity.Starring.CheckStarred(repo.Owner.Login, repo.Name).ConfigureAwait(false);

		public async Task<bool> StarRepository(Repository repo)
			=> await client.Activity.Starring.StarRepo(repo.Owner.Login, repo.Name).ConfigureAwait(false);

		public async Task<bool> UnstarRepository(Repository repo)
			=> await client.Activity.Starring.RemoveStarFromRepo(repo.Owner.Login, repo.Name).ConfigureAwait(false);
		#endregion
	}
}

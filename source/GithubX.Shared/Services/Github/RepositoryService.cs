using Akavache;
using Octokit;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System;
using System.Collections.Generic;

namespace GithubX.Shared.Services
{
	public class RepositoryService
	{
		private readonly GitHubClient client;
		public RepositoryService(ref GitHubClient _client) => client = _client;

		public async Task<Repository> GetRepository(string ownerName, string repoName)
			=> await client.Repository.Get(ownerName, repoName);

		public async Task<Repository> GetRepository(long repoId)
			=> await client.Repository.Get(repoId);

		#region Content
		public async Task<IReadOnlyList<RepositoryContent>> GetRepositoryContentTextByPath(Repository repo, string path, string branch)
			=> (await client.Repository.Content.GetAllContentsByRef(repo.Id, path, branch));

		public async Task<string> GetReadmeHTMLForRepository(long repoId)
			=> await client.Repository.Content.GetReadmeHtml(repoId);

		public async Task<Readme> GetRepositoryReadme(long repoId)
			=> await client.Repository.Content.GetReadme(repoId);

		public async Task<IReadOnlyList<RepositoryContent>> GetRepositoryContent(long repoId)
			=> await client.Repository.Content.GetAllContents(repoId);

		public async Task<IReadOnlyList<RepositoryContent>> GetRepositoryContent(long repoId, string path)
			=> await client.Repository.Content.GetAllContents(repoId, path);

		public async Task<string> GetMarkDownReadyAsync(string url, bool fromCache = true)
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
			var type = content.Name.Substring(content.Name.LastIndexOf(".") + 1);
			if (type.Equals("png", StringComparison.OrdinalIgnoreCase)
				|| type.Equals("jpg", StringComparison.OrdinalIgnoreCase)
				|| type.Equals("jpeg", StringComparison.OrdinalIgnoreCase)
				|| type.Equals("bmp", StringComparison.OrdinalIgnoreCase)
				|| type.Equals("tiff", StringComparison.OrdinalIgnoreCase))
				return $"![]({content.DownloadUrl})";
			else if (type.Equals("mp4", StringComparison.OrdinalIgnoreCase)
				|| type.Equals("mkv", StringComparison.OrdinalIgnoreCase)
				|| type.Equals("flv", StringComparison.OrdinalIgnoreCase)
				|| type.Equals("flv", StringComparison.OrdinalIgnoreCase)
				|| type.Equals("mp3", StringComparison.OrdinalIgnoreCase)){
				return $"Can't Open {type} file.\nDownload Link → {content.DownloadUrl}";
			}
			var oldSha = "";
			try
			{
				oldSha = await BlobCache.LocalMachine.GetObject<string>("_" + content.Url) ?? "";
			}
			catch { }
			var result = await GetMarkDownReadyAsync(content.DownloadUrl, content.Sha.Equals(oldSha));
			await BlobCache.LocalMachine.InsertObject("_" + content.Url, content.Sha);
			return type.Equals("md", StringComparison.OrdinalIgnoreCase) ? result : $"```{type} {result} ```";
		}
		#endregion

		#region Branch
		public async Task<IReadOnlyList<Branch>> GetAllBranches(Repository repo)
			=> await client.Repository.Branch.GetAll(repo.Owner.Login, repo.Name);
		#endregion

		#region Fork
		public async Task<Repository> ForkRepository(Repository repo)
			=> await client.Repository.Forks.Create(repo.Id, new NewRepositoryFork());
		#endregion

		#region Watch
		public async Task<bool> IsWatched(Repository repo)
			=> await client.Activity.Watching.CheckWatched(repo.Id);

		public async Task<bool> WatchRepository(Repository repo)
			=> (await client.Activity.Watching.WatchRepo(repo.Id, new NewSubscription { Subscribed = true })).Subscribed;

		public async Task<bool> UnwatchRepository(Repository repo)
			=> await client.Activity.Watching.UnwatchRepo(repo.Id);
		#endregion

		#region Star
		public async Task<bool> IsStarred(Repository repo)
			=> await client.Activity.Starring.CheckStarred(repo.Owner.Login, repo.Name);

		public async Task<bool> StarRepository(Repository repo)
			=> await client.Activity.Starring.StarRepo(repo.Owner.Login, repo.Name);

		public async Task<bool> UnstarRepository(Repository repo)
			=> await client.Activity.Starring.RemoveStarFromRepo(repo.Owner.Login, repo.Name);
		#endregion
	}
}

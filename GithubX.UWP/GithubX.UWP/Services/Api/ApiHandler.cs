using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GithubX.UWP.Models;
using GithubX.UWP.Services.Cache;
using Newtonsoft.Json;

namespace GithubX.UWP.Services.Api
{
	static class ApiHandler
	{
		static WindowsCacheHandler wCache = new WindowsCacheHandler();
		static LocalCacheHandler lCache = new LocalCacheHandler();
		public static List<RepoModel> AllRepos { get; set; }
		public static ObservableCollection<CategoryModel> AllCategories { get; set; }

		#region GetContent
		private static async Task<List<ContentModel>> GetContentsAsync(string contentUrl, string key)
		{
			try
			{
				var json = await HttpHandler.Get(contentUrl);
				if (json == null) throw new Exception();
				await lCache.SaveAsync(key, json);
				return JsonConvert.DeserializeObject<List<ContentModel>>(json);
			}
			catch { return null; }
		}
		public static async Task<List<ContentModel>> LoadContent(RepoModel repo, bool cache = true)
		{
			var key = CacheKeys.ContentsKey(repo.id);
			if (!HttpHandler.CheckConnection && !cache)
				throw new Exception("No internet, no candy for you!🤬");
			else if (cache)
				try { return JsonConvert.DeserializeObject<List<ContentModel>>(await lCache.ReadAsync(key)); }
				catch { return await GetContentsAsync(repo.contents_url, key); }
			else if (HttpHandler.CheckConnection)
			{
				return await GetContentsAsync(repo.contents_url, key);
			}
			else throw new Exception("No Cache, no Internet!!!🤬");
		}
		#endregion

		#region Category
		public static async Task SaveCategoriesAsync()
		{
			await lCache.SaveAsync(CacheKeys.CategoriesKey, JsonConvert.SerializeObject(AllCategories)).ConfigureAwait(false);
		}

		public static async Task PrepareAllCategories(string userId)
		{
			var keys = CacheKeys.CategoriesKey;
			try
			{
				var cats = new List<CategoryModel>();
				var json = await lCache.ReadAsync(keys); // if does not exist will return null and 
				cats = JsonConvert.DeserializeObject<List<CategoryModel>>(json); //then throw exception
				AllCategories = new ObservableCollection<CategoryModel>(cats.OrderBy(o => o.OrderId).ToList());
			}
			catch (Exception)
			{
				var cats = new List<CategoryModel>();
				cats.Add(new CategoryModel { Id = 0, Text = "All" });
				await SaveCategoriesAsync();
				AllCategories = new ObservableCollection<CategoryModel>(cats);
			}
		}
		#endregion

		#region Repositories

		public static List<RepoModel> GetRepoOfCategory(int catId) => AllRepos.FindAll(obj => obj.CategoriesId.Contains(catId));

		public static async Task<List<RepoModel>> GetNextPageReposAsync(string userAcc, int page)
		{
			if (!HttpHandler.CheckConnection) throw new Exception("No internet, no candy for you!🤬");
			var json = await HttpHandler.Get(Api.AccountStarsUrl(userAcc, page));
			if (json == null) throw new Exception("Oops!🤨🤔");
			var freshList = JsonConvert.DeserializeObject<List<RepoModel>>(json);
			if (page != 0) foreach (var r in freshList) r.CategoriesId = new int[] { };
			return freshList;
		}

		public static async Task PrepareAllRepos(string userAcc, bool cacheEnable = true)
		{
			if (!HttpHandler.CheckConnection && !cacheEnable) throw new Exception("No internet, no candy for you!🤬");
			if (HttpHandler.CheckConnection)
			{
				try
				{
					var freshList = await GetNextPageReposAsync(userAcc, 0);
					try
					{
						var cacheList = await LoadFromCache();
						AllRepos = MergeOldAndNew(cacheList, freshList);
					}
					catch { AllRepos = freshList; }
					await SaveCategoryReposAsync(userAcc);
					return;
				}
				catch { }
			}
			else AllRepos = await LoadFromCache();// only cache - if cache=ON or online failes

			List<RepoModel> MergeOldAndNew(List<RepoModel> old, List<RepoModel> fresh)
			{
				//TODO: dirty dirty code :( dont like it
				var temp = new List<RepoModel>();
				foreach (var item in old)
				{
					var ls = new List<int>(item.CategoriesId);
					//if [] goodbye cache
					if (ls.Count == 1) if (ls[0] == 0) continue;
					// cache.del only [0]
					if (ls.Contains(0)) ls.Remove(0);

					// cache.update [0....] if contain.fresh ?nothing else replace with [...]
					var newItem = fresh.Find(obj => obj.id == item.id);//>change
					if (newItem != null)
					{
						ls.Add(0);
						newItem.CategoriesId = ls.ToArray();
						continue;
					}
					temp.Add(item);
				}
				fresh.AddRange(temp);
				return fresh;
			}
			async Task<List<RepoModel>> LoadFromCache()
			{
				var json = await lCache.ReadAsync(CacheKeys.RepositoriesKey).ConfigureAwait(false);
				if (json == null) throw new Exception("Oops!🤨🤔");
				return JsonConvert.DeserializeObject<List<RepoModel>>(json);
			}
		}

		public static async Task UpdateRepoAsync(string login, RepoModel repo)
		{
			var item = AllRepos.Find(o => o.id == repo.id);
			if (item == null) AllRepos.Add(repo);
			item = repo;
			await SaveCategoryReposAsync(login);
		}
		public static async Task SaveCategoryReposAsync(string user)
		{
			var temp = AllRepos.FindAll(x => x.CategoriesId.Length != 0);
			await lCache.SaveAsync(CacheKeys.RepositoriesKey, JsonConvert.SerializeObject(temp)).ConfigureAwait(false);
		}
		#endregion

		#region Login
		public static OwnerModel LoginFromCache()
		{
			var key = CacheKeys.UserKey;
			if (wCache.Exists(key))
			{
				var json = wCache.Read(key);
				if (json.Length < 4) return null;
				return JsonConvert.DeserializeObject<OwnerModel>(json);
			}
			else return null;
		}

		public static async Task<OwnerModel> LoginAsync(string account)
		{
			var key = CacheKeys.UserKey;
			var json = await HttpHandler.Get(Api.AccountInfoUrl(account));
			if (json == null || json.Length < 4) return null;
			var user = JsonConvert.DeserializeObject<OwnerModel>(json);
			wCache.Write(key, json);
			return user;
		}

		public static void LogOut()
		{
			wCache.Remove(CacheKeys.UserKey);
		}
		#endregion

		#region ReadMe
		public static async Task<(bool, string)> GetReadMeMdAsync(int repoId, string url, bool fromCache = true)
		{
			string md = null;
			var key = CacheKeys.Readme(repoId);
			if (fromCache)
			{
				try
				{
					md = await lCache.ReadAsync(key);
					if (md != null)
					{
						removeUnSupported();
						await lCache.SaveAsync(key, md).ConfigureAwait(false);
						return (true, md);
					}
				}
				catch { }
			}
			return await LoadOnline();

			async Task<(bool, string)> LoadOnline()
			{
				try
				{
					md = await HttpHandler.GetString(url);
					if (md == null) return (false, "> No README.MD 🤔");
					md = new Html2Markdown.Converter().Convert(md);
					removeUnSupported();
					await lCache.SaveAsync(key, md).ConfigureAwait(false);
				}
				catch { }
				return (false, md);
			}
			void removeUnSupported()
			{
				md = md.Replace("[`", "[");
				md = md.Replace("`]", "]");
			}
		}
		#endregion
	}
}

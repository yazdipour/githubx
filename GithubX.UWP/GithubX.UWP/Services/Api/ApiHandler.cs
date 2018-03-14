using System.Collections.Generic;
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

		internal static async Task<List<CategoryModel>> GetCategoriesAsync(string userId)
		{
			try
			{
				var keys = CacheKeys.CategoriesKey(userId);
				var cats = new List<CategoryModel>();
				int[] catsId = { 0 };
				if (wCache.Exists(keys))
				{
					var json = wCache.Read(keys);
					catsId = JsonConvert.DeserializeObject<int[]>(json);
					foreach (var id in catsId)
					{
						try
						{
							json = await lCache.ReadAsync(CacheKeys.CategoryKey(id));
							cats.Add(JsonConvert.DeserializeObject<CategoryModel>(json));
						}
						catch { }
					}
				}
				else
				{
					var all = new CategoryModel { Id = 0, Text = "All" };
					all.RepoList = await GetReposAsync(userId, all, 0);
					cats.Add(all);
					await NewCategoryAsync(userId, all);
				}
				return cats.OrderBy(o => o.OrderId).ToList();
			}
			catch { return null; }
		}

		internal static async Task<List<RepoModel>> GetReposAsync(string userAcc, CategoryModel cat, int page)
		{
			if (HttpHandler.CheckConnection && cat.Id == 0)
			{
				var json = await HttpHandler.Get(Api.AccountStarsUrl(userAcc, page));
				if (json == null) return null;
				cat.RepoList = JsonConvert.DeserializeObject<List<RepoModel>>(json);
				if (page == 0)
					await lCache.SaveAsync(CacheKeys.CategoryKey(cat.Id), JsonConvert.SerializeObject(cat)).ConfigureAwait(false);
				wCache.Save(CacheKeys.LastUpdate, Api.UnixTimestamp.ToString());
				return cat.RepoList;
			}
			else if (page != 0 || cat.RepoList == null)
			{
				var json = await lCache.ReadAsync(CacheKeys.CategoryKey(cat.Id)).ConfigureAwait(false);
				if (json == null) return null;
				return JsonConvert.DeserializeObject<CategoryModel>(json).RepoList;
			}
			else
				return cat.RepoList;
		}

		#region Login
		internal static OwnerModel LoginFromCache()
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

		internal static async Task<OwnerModel> LoginAsync(string account)
		{
			var key = CacheKeys.UserKey;
			var json = await HttpHandler.Get(Api.AccountInfoUrl(account));
			if (json == null || json.Length < 4) return null;
			var user = JsonConvert.DeserializeObject<OwnerModel>(json);
			wCache.Save(key, json);
			return user;
		}
		#endregion

		#region Content
		public static async Task<string> GetReadMeMdAsync(int repoId, string url, bool fromCache = true)
		{
			string md = null;
			var key = CacheKeys.Readme(repoId);
			if (fromCache)
			{
				md = await lCache.ReadAsync(key);
				removeUnSupported();
				if (md != null) return md;
			}
			try
			{
				md = await HttpHandler.GetString(url);
				md = new Html2Markdown.Converter().Convert(md);
				removeUnSupported();
			}
			catch { }
			await lCache.SaveAsync(key, md).ConfigureAwait(false);
			return md;

			void removeUnSupported()
			{
				//md = System.Text.RegularExpressions.Regex.Replace(md, @"([')(\w)('])", "$2");
				md = md.Replace("[`", "[");
				md = md.Replace("`]", "]");
			}
		}
		#endregion

		internal static void AddRepo(CategoryModel category, RepoModel repo)
		{
			category.RepoList.Add(repo);
			var json = JsonConvert.SerializeObject(category);
			lCache.SaveAsync(CacheKeys.CategoryKey(category.Id), json).ConfigureAwait(false);
		}
		internal static async Task AddRepoAsync(int categoryId, RepoModel repo)
		{
			var json = await lCache.ReadAsync(CacheKeys.CategoryKey(categoryId)).ConfigureAwait(false);
			CategoryModel category = JsonConvert.DeserializeObject<CategoryModel>(json);
			AddRepo(category, repo);
		}
		internal static async Task NewCategoryAsync(string userId, CategoryModel cat)
		{
			var keys = CacheKeys.CategoriesKey(userId);
			var json = wCache.Read(keys);
			var catsId = new List<int>();
			if (json != null) catsId = JsonConvert.DeserializeObject<List<int>>(json);
			catsId.Add(cat.Id);
			wCache.Save(keys, JsonConvert.SerializeObject(catsId));
			await lCache.SaveAsync(CacheKeys.CategoryKey(cat.Id), JsonConvert.SerializeObject(cat)).ConfigureAwait(false);
		}
		internal static async Task ModifyCategoryAsync(CategoryModel newCat)
		{
			var json = await lCache.ReadAsync(CacheKeys.CategoryKey(newCat.Id)).ConfigureAwait(false);
			CategoryModel category = JsonConvert.DeserializeObject<CategoryModel>(json);
			category.Color = newCat.Color;
			category.OrderId = newCat.OrderId;
			category.Text = newCat.Text;
			await lCache.SaveAsync(CacheKeys.CategoryKey(category.Id), JsonConvert.SerializeObject(category)).ConfigureAwait(false);
		}
	}
}

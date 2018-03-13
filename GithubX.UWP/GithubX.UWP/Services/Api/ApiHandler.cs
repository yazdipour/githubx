using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GithubX.UWP.Models;

namespace GithubX.UWP.Services.Api
{
	static class ApiHandler
	{
		internal static List<CategoryModel> GetCategories(int id)
		{
			var cats = new List<CategoryModel>();
			//TODO : Load Caches or Live

			return cats;
		}

		internal static List<RepoModel> GetRepos(int userId, int catId, int page)
		{
			if (catId == 0 && page == 0)
			{
				//if online > save after load
				return null;
			}
			throw new NotImplementedException();
		}
		internal static void RepoToCategory(int categoryId, RepoModel repo)
		{

		}
		internal static void NewCategory(CategoryModel cat)
		{
			//save cache
		}
		internal static void ModifyCategory(CategoryModel cat)
		{
			//save base of cat.id
		}

		internal static OwnerModel LoginFromCache()
		{
			var key = "user";
			if (Cache.WindowsCacheHandler.Exists(key))
			{
				var json = Cache.WindowsCacheHandler.Read(key);
				if (json.Length < 4) return null;
				return Newtonsoft.Json.JsonConvert.DeserializeObject<OwnerModel>(json);
			}
			else return null;
		}

		internal static async Task<OwnerModel> LoginAsync(string account)
		{
			var json = await HttpHandler.Get(Api.AccountInfoUrl(account));
			if (json == null || json.Length < 4) return null;
			var user = Newtonsoft.Json.JsonConvert.DeserializeObject<OwnerModel>(json);
			var key = "user";
			Cache.WindowsCacheHandler.Save(key, json);
			return user;
		}

		public static async Task<string> GetReadMeMdAsync(int repoId, string url)
		{
			// TODO : Cache??!!
			var txt = await HttpHandler.GetString(url);
			try
			{
				txt = new Html2Markdown.Converter().Convert(txt);
			}
			catch { }
			return txt;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Akavache;
using GithubX.UWP.Models;
using GithubX.UWP.Services.Cache;
using Newtonsoft.Json;
using System.Reactive.Linq;
using Refit;
using System.Net.Http;

namespace GithubX.UWP.Services.Api
{
	static class ApiHandler
	{
		//private static OSCache wCache = new OSCache();
		static IGithubApi GApi = RestService.For<IGithubApi>("https://api.github.com");
		public static int UnixTimestamp => (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		public static string RateAppUri = "ms-windows-store:REVIEW?PFN=3783mindprojects.GithubX_6c8ydbw054cyy";
		public static string LanguagesColors = ",Mercury:#ff2b2b,TypeScript:#2b7489,PureBasic:#5a6986,Objective-C++:#6866fb,Self:#0579aa,NewLisp:#87AED7,Fortran:#4d41b1,JupyterNotebook:#DA5B0B,Rebol:#358a5b,Frege:#00cafe,AspectJ:#a957b0,Shell:#89e051,WebOntologyLanguage:#9cc9dd,AppleScript:#101F1F,Eiffel:#946d57,Nix:#7e7eff,RAML:#77d9fb,Nginx:#9469E9,MTML:#b7e1f4,Racket:#22228f,Elixir:#6e4a7e,SAS:#B34936,MQL4:#62A8D6,MQL5:#4A76B8,Agda:#315665,wisp:#7582D1,SRecodeTemplate:#348a34,D:#ba595e,PowerBuilder:#8f0f8d,Kotlin:#F18E33,Opal:#f7ede0,TIProgram:#A0AA87,Crystal:#776791,Perl6:#0000fb,Batchfile:#C1F12E,Oz:#fab738,Mirah:#c7a938,Objective-J:#ff0c5a,Ragel:#9d5200,Roff:#ecdebe,FreeMarker:#0050b2,Ruby:#701516,ComponentPascal:#B0CE4E,Arc:#aa2afe,Brainfuck:#2F2530,Nit:#009917,APL:#5A8164,Hack:#878787,Go:#375eab,VisualBasic:#945db7,PHP:#4F5D95,Cirru:#ccccff,SQF:#3F3F3F,Glyph:#e4cc98,1CEnterprise:#814CCC,Java:#b07219,MAXScript:#00a6a6,Scala:#c22d40,Makefile:#427819,ColdFusion:#ed2cd6,Perl:#0298c3,Lua:#000080,Vue:#2c3e50,Verilog:#b2b7f8,Factor:#636746,Haxe:#df7900,PureData:#91de79,Forth:#341708,Red:#f50000,Hy:#7790B2,Volt:#1F1F1F,LSL:#3d9970,eC:#913960,Terra:#00004c,CoffeeScript:#244776,HTML:#e34c26,Lex:#DBCA00,APIBlueprint:#2ACCA8,Swift:#ffac45,C:#555555,AutoHotkey:#6594b9,P4:#7055b5,Isabelle:#FEFE00,Metal:#8f14e9,Clarion:#db901e,JSONiq:#40d47e,Boo:#d4bec1,AutoIt:#1C3552,Genie:#fb855d,Clojure:#db5855,EQ:#a78649,Rust:#dea584,Prolog:#74283c,SourcePawn:#5c7611,AMPL:#E6EFBB,ANTLR:#9DC3FF,Harbour:#0e60e3,Yacc:#4B6C4B,Tcl:#e4cc98,BlitzMax:#cd6400,PigLatin:#fcd7de,xBase:#403a40,LLVM:#185619,Lasso:#999999,ECL:#8a1267,VHDL:#adb2cb,Elm:#60B5CC,PropellerSpin:#7fa2a7,Rascal:#fffaa0,X10:#4B6BEF,IDL:#a3522f,ATS:#1ac620,Ada:#02f88c,Nu:#c9df40,SuperCollider:#46390b,Oxygene:#cdd0e3,ASP:#6a40fd,Assembly:#6E4C13,Gnuplot:#f0a9f0,NetLinx:#0aa0ff,C#:#178600,Turing:#cf142b,Vala:#fbe5cd,Processing:#0096D8,Arduino:#bd79d1,FLUX:#88ccff,NetLogo:#ff6375,CSS:#563d7c,EmacsLisp:#c065db,Stan:#b2011d,SaltStack:#646464,Gherkin:#5B2063,QML:#44a51c,Pike:#005390,LOLCODE:#cc9900,ooc:#b0b77e,XSLT:#EB8CEB,XC:#99DA07,J:#9EEDFF,Mask:#f97732,EmberScript:#FFF4F3,TeX:#3D6117,Nemerle:#3d3c6e,Cuda:#3A4E3A,KRL:#28431f,Vimscript:#199f4b,Ren'Py:#ff7f7f,Golo:#88562A,PostScript:#da291c,Fancy:#7b9db4,OCaml:#3be133,Shen:#120F14,Pascal:#E3F171,F#:#b845fc,Puppet:#302B6D,ActionScript:#882B0F,Fantom:#dbded5,Zephir:#118f9e,Click:#E4E6F3,Smalltalk:#596706,DM:#447265,Ioke:#078193,PogoScript:#d80074,LiveScript:#499886,JavaScript:#f1e05a,PureScript:#1D222D,ABAP:#E8274B,Matlab:#bb92ac,Slash:#007eff,R:#198CE7,Erlang:#B83998,Pan:#cc0000,LookML:#652B81,Eagle:#814C05,Scheme:#1e4aec,Squirrel:#800000,Nim:#37775b,Python:#3572A5,Max:#c4a79c,CommonLisp:#3fb68b,Dart:#00B4AB,XQuery:#5232e7,Omgrofl:#cabbff,SystemVerilog:#DAE1C2,Chapel:#8dc63f,Groovy:#e69f56,Dylan:#6c616e,E:#ccce35,Parrot:#f3ca0a,GrammaticalFramework:#79aa7a,GameMakerLanguage:#8fb200,Papyrus:#6600cc,C++:#f34b7d,NetLinx+ERB:#747faa,Clean:#3F85AF,Alloy:#64C800,Gosu:#82937f,PLSQL:#dad8d8,PAWN:#dbb284,UnrealScript:#a54c4d,StandardML:#dc566d,Objective-C:#438eff,Julia:#a270ba,nesC:#94B0C7,Haskell:#29b544,NCL:#28431f,Io:#a9188d,Rouge:#cc0088,RUNOFF:#665a4e,AGSScript:#B9D9FF,Meson:#007800,Dogescript:#cca760,PowerShell:#012456";
		public static List<RepoModel> AllRepos { get; set; }
		public static ObservableCollection<CategoryModel> AllCategories { get; set; }

		#region GetContent
		private static async Task<List<ContentModel>> GetContentsAsync(string user, string repoName, string key)
		{
			try
			{
				List<ContentModel> temp = await GApi.GetRepoContent(user, repoName);
				await BlobCache.LocalMachine.InsertObject(key, temp);
				return temp;
			}
			catch { return null; }
		}
		public static async Task<List<ContentModel>> LoadContent(RepoModel repo, bool cache = true)
		{
			var key = CacheKeys.ContentsKey(repo.id);
			if (!Utils.CheckConnection && !cache)
				throw new Exception("No internet, no candy for you!🤬");
			else if (cache)
				try
				{
					return await BlobCache.LocalMachine.GetObject<List<ContentModel>>(key);
				}
				catch { return await GetContentsAsync(repo.owner.login, repo.name, key); }
			else if (Utils.CheckConnection)
				return await GetContentsAsync(repo.owner.login, repo.name, key);
			else throw new Exception("No Cache, no Internet!!!🤬");
		}
		#endregion

		#region Category
		public static async Task SaveCategoriesAsync()
		{
			await BlobCache.LocalMachine.InsertObject(CacheKeys.CategoriesKey, AllCategories);
		}

		public static async Task PrepareAllCategories(string userId)
		{
			var keys = CacheKeys.CategoriesKey;
			try
			{
				var cats = await BlobCache.LocalMachine.GetObject<List<CategoryModel>>(keys);
				AllCategories = new ObservableCollection<CategoryModel>(cats.OrderBy(o => o.OrderId).ToList());
			}
			catch (Exception)
			{
				var cats = new List<CategoryModel> { new CategoryModel { Id = 0, Text = "All" } };
				await SaveCategoriesAsync();
				AllCategories = new ObservableCollection<CategoryModel>(cats);
			}
		}
		#endregion

		#region Repositories

		public static List<RepoModel> GetRepoOfCategory(int catId) => AllRepos?.FindAll(obj => obj.CategoriesId.Contains(catId));

		public static async Task<List<RepoModel>> GetReposOfPage(string userAcc, int page)
		{
			if (!Utils.CheckConnection) throw new Exception("No internet, nothing for you!🤬");
			var freshList = await GApi.GetUserStars(userAcc, page + 1);
			if (page != 0) foreach (var r in freshList) r.CategoriesId = new int[] { };
			return freshList;
		}

		public static async Task PrepareAllRepos(string userAcc, bool cacheEnable = true)
		{
			if (!Utils.CheckConnection && !cacheEnable) throw new Exception("No internet, no candy for you!🤬");
			if (Utils.CheckConnection)
			{
				try
				{
					var freshList = await GetReposOfPage(userAcc, 0);
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

			async Task<List<RepoModel>> LoadFromCache() => await BlobCache.LocalMachine.GetObject<List<RepoModel>>(CacheKeys.RepositoriesKey);
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
			await BlobCache.LocalMachine.InsertObject<List<RepoModel>>(CacheKeys.RepositoriesKey, temp);
		}
		#endregion

		#region Login
		public static OwnerModel LoginFromCache()
		{
			var wCache = new OSCache();
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
			var user = await GApi.GetUser(account);
			new OSCache().Write(key, JsonConvert.SerializeObject(user));
			return user;
		}

		public static void LogOut() => new OSCache().Remove(CacheKeys.UserKey);
		#endregion

		#region ReadMe
		public static async Task<(bool, string)> GetReadMeMdAsync(int repoId, string url, bool fromCache = true)
		{
			var buffer = await BlobCache.LocalMachine.DownloadUrl(url, fetchAlways: !fromCache);
			string md = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
			try
			{
				// Html2Markdown needs HtmlAgilityPack.NugetPkg
				md = new Html2Markdown.Converter().Convert(md).Trim();
				removeUnSupported();
				if (md == null || md.Length < 2) return (false, "> No README.MD 🤔");
				return (true, md);
			}
			catch { return (false, md); }

			void removeUnSupported()
			{
				md = md.Replace("[`", "[");
				md = md.Replace("`]", "]");
			}
		}
		#endregion
	}
}

using System;
using GithubX.UWP.Services.Api;
using Newtonsoft.Json;

namespace GithubX.UWP.Models
{
	class RepoModel
	{
		//api
		public int id { get; set; }
		public string name { get; set; }
		public string full_name { get; set; }
		public OwnerModel owner { get; set; }
		public string html_url { get; set; }
		public string url { get; set; }
		public bool _private { get; set; }
		public string description { get; set; }
		public string homepage { get; set; }
		public int stargazers_count { get; set; }
		public string language { get; set; }
		public bool has_wiki { get; set; }
		public bool has_pages { get; set; }
		public int forks { get; set; }
		public string default_branch { get; set; }
		public int size { get; set; }
		public bool has_issues { get; set; }
		public bool has_projects { get; set; }
		public bool has_downloads { get; set; }
		public int open_issues_count { get; set; }
		public DateTime created_at { get; set; }
		public DateTime updated_at { get; set; }
		public DateTime pushed_at { get; set; }

		[JsonIgnore]
		public string clone_url => html_url + ".git";

		#region  inapp
		public int[] CategoriesId = { 0 };
		private string color = "#ffffff";

		[JsonIgnore]
		public string Color
		{
			get
			{
				if (color == null || color == "#ffffff") color = FindColor();
				return color;
			}
			set { color = value; }
		}

		[JsonIgnore]
		public string forks_label => forks + " 🔱";

		[JsonIgnore]
		public string stars_label => stargazers_count + " ⭐";

		private string FindColor()
		{
			//REF https://github.com/doda/github-language-colors/blob/master/colors.json
			if (language == "") return "#ffffff";
			var key = "," + language + ":";
			var a = ApiHandler.LanguagesColors.IndexOf(key);
			if (a == -1) return "#ffffff";
			a += key.Length;
			return ApiHandler.LanguagesColors.Substring(a, 7);
		}
		#endregion
	}
}

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
		public bool _private { get; set; }
		public string html_url { get; set; }
		public string description { get; set; }
		public bool fork { get; set; }
		public string url { get; set; }
		public int stargazers_count { get; set; }
		public string language { get; set; }
		public bool has_wiki { get; set; }
		public bool has_pages { get; set; }
		public int forks { get; set; }
		public string default_branch { get; set; }
		
		// inapp
		public int[] CategoriesId = { 0 };
		private string color = "#3c6382";

		[JsonIgnore]
		public string Color
		{
			get
			{
				if (color == null || color == "#3c6382") color = FindColor();
				return color;
			}
			set { color = value; }
		}

		[JsonIgnore]
		public string forks_label => forks + " ⭐";

		[JsonIgnore]
		public string stars_label => stargazers_count + " 🔱";

		private string FindColor()
		{
			//REF https://github.com/doda/github-language-colors/blob/master/colors.json
			if (language == "") return "#ffffff";
			var key = "," + language + ":";
			var a = Api.colors.IndexOf(key);
			if (a == -1) return "#3c6382";
			a += key.Length;
			return Api.colors.Substring(a, 7);
		}
	}
}

namespace GithubX.UWP.Models
{
	class RepoModel
	{
		private string color = "#e67e22";

		public string Color
		{
			get
			{
				if (color == null || color == "#e67e22")
					color = FindColor();
				return color;
			}
			set { color = value; }
		}


		public string forks_label
		{
			get { return forks + " ⭐"; }
		}

		public string stars_label
		{
			get { return stargazers_count + " 🍴"; }
		}


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
		public string homepage { get; set; }
		public int stargazers_count { get; set; }
		public int watchers_count { get; set; }
		public string language { get; set; }
		public bool has_issues { get; set; }
		public bool has_projects { get; set; }
		public bool has_downloads { get; set; }
		public bool has_wiki { get; set; }
		public bool has_pages { get; set; }
		public int forks { get; set; }
		public string default_branch { get; set; }

		public static string FindColor()
		{
			//language
			return "#ff00ff00";
		}
	}
}

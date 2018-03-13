namespace GithubX.UWP.Models
{
	class OwnerModel
	{
		public string login { get; set; }
		public int id { get; set; }
		public string avatar_url { get; set; }

		public string html_url { get; set; }
		public string starred_url { get; set; }
		public string type { get; set; }
		public bool site_admin { get; set; }
		public string name { get; set; }
		public string company { get; set; }
		public string blog { get; set; }
		public string location { get; set; }
		public object email { get; set; }
		public object hireable { get; set; }
		public string bio { get; set; }
		public int public_repos { get; set; }
		public int public_gists { get; set; }
		public int followers { get; set; }
		public int following { get; set; }
	}
}

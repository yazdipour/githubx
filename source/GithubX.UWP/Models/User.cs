namespace GithubX.UWP.Models
{
	class User
	{
		public string login { get; set; }
		public int id { get; set; }
		public string avatar_url { get; set; }

		public string type { get; set; }
		public string name { get; set; }
		public string location { get; set; }
		public int public_repos { get; set; }
		public int public_gists { get; set; }
		public int followers { get; set; }
		public int following { get; set; }
	}
}

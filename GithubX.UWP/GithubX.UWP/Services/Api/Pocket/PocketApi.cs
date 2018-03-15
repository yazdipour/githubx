namespace GithubX.UWP.Services.Api.Pocket
{
	class PocketModel
	{
		public string url { get; set; }
		public string title { get; set; }
		public int time { get; set; }
		public string consumer_key;
		public string access_token { get; set; }
	}
	class PocketApi
	{
		string addUrl = "https://getpocket.com/v3/add";
		string requestTokenUrl = "https://getpocket.com/v3/oauth/request";
	}
}

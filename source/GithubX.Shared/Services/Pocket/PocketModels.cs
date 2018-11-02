namespace GithubX.Shared.Services.Pocket
{
	internal class RequestCode
	{
		public string Code { get; set; }
		public string State { get; set; }
	}

	internal class AddParameters
	{
		public AddParameters(string url, string consumer_key, string access_token)
		{
			this.url = url;
			this.consumer_key = consumer_key;
			this.access_token = access_token;
		}

		public string url { get; set; }
		public string consumer_key { get; set; }
		public string access_token { get; set; }
	}

	internal class PocketUser
	{
		public string Access_token { get; set; }

		public string Username { get; set; }

		public object Account { get; set; }
	}
}

using Refit;
using System.Threading.Tasks;

namespace GithubX.Shared.Services.Pocket
{
	[Headers("User-Agent: XGithub", "X-Accept: application/json")]
	internal interface IPocketApi
	{
		[Post("oauth/request")]
		Task<RequestCode> GetRequestToken([Query("redirect_uri")] string redirect_uri, [Query("consumer_key")]string consumer_key);

		[Post("oauth/authorize")]
		//TODO: Not sure About the QUERY
		Task<PocketUser> GetUserToken([Query("code")] string request_token, [Query("consumer_key")]string consumer_key);

		[Post("add")]
		Task PostArticle([Body]AddParameters item);
	}
}

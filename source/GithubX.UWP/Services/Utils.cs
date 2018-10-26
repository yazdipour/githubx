using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Networking.Connectivity;
using Windows.Storage;

namespace GithubX.UWP.Services
{
	class Utils
	{
		public static bool CheckConnection => (NetworkInformation.GetInternetConnectionProfile() != null);

		public async Task DownloadFile(string url, string name)
		{
			try
			{
				CancellationToken token;
				Uri source;
				StorageFolder folder = KnownFolders.VideosLibrary;
				if (string.IsNullOrWhiteSpace(url) || !Uri.TryCreate(url, UriKind.Absolute, out source))
					throw new Exception("Invalid URI");
				StorageFile destinationFile;
				destinationFile = await folder.CreateFileAsync(name, CreationCollisionOption.GenerateUniqueName);
				var download = new BackgroundDownloader().CreateDownload(source, destinationFile);
				download.Priority = BackgroundTransferPriority.High;
				await download.StartAsync().AsTask(token);
			}
			catch (Exception e) { throw e; }
		}

		//internal static async Task<string> Get(string url)
		//{
		//	var httpClient = new Windows.Web.Http.HttpClient();
		//	var headers = httpClient.DefaultRequestHeaders;
		//	var httpResponse = new Windows.Web.Http.HttpResponseMessage();
		//	string httpResponseBody = null;
		//	try
		//	{
		//		httpResponse = await httpClient.GetAsync(new Uri(url));
		//		httpResponse.EnsureSuccessStatusCode();
		//		httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
		//	}
		//	catch (Exception ex)
		//	{
		//		throw new Exception();
		//	}
		//	return httpResponseBody;
		//}

	}
}

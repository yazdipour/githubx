using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace GithubX.UWP.Services.Api
{
	static class HttpHandler
	{
		//if(NetworkInformation.GetInternetConnectionProfile() == null)
		//    return "[ERROR!]";

		//var response = await http.GetAsync(new Uri(url), token);
		//response.EnsureSuccessStatusCode();
		//var htmlFile = await response.Content.ReadAsStringAsync();
		//await DownloadFile(url, name);
		internal static async Task<string> Get(string url)
		{
			// How to download .md content from the web and display it
			//using (var client = new HttpClient())
			//{
			string str_ReturnValue = "";
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.UserAgent = "User-Agent: XGithub";
				var webResponse = await request.GetResponseAsync();
				//HttpWebResponse
				//return myHttpWebResponse.GetResponseStream();
				using (Stream s = request.GetResponse().GetResponseStream())
				{
					using (StreamReader sr = new StreamReader(s))
					{
						var jsonData = sr.ReadToEnd();
						str_ReturnValue += jsonData.ToString();
					}
				}
				return str_ReturnValue;
				//var response = await client.GetAsync(url);
				//return await response.Content.ReadAsStringAsync();
			}
			catch { return null; }
			//}
		}
		internal static async Task<string> GetString(string url)
		{
			// How to download .md content from the web and display it
			using (var client = new HttpClient())
			{
				try
				{
					return await client.GetStringAsync(url);
				}
				catch { return null; }
			}
		}
		internal static async Task DownloadFile(string url, string name)
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
	}
}

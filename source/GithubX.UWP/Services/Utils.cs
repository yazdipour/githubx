using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Xaml.Media;

namespace GithubX.UWP.Services
{
	internal class Utils
	{
		public static bool CheckConnection => NetworkInformation.GetInternetConnectionProfile() != null;

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

		public static SolidColorBrush HexToSolidColor(string hex)
		{
			hex = hex.Replace("#", string.Empty);
			if (hex.Length < 8) hex += "FF";
			return new SolidColorBrush(Windows.UI.Color.FromArgb(
				(byte)Convert.ToUInt32(hex.Substring(0, 2), 16),
				(byte)Convert.ToUInt32(hex.Substring(2, 2), 16),
				(byte)Convert.ToUInt32(hex.Substring(4, 2), 16),
				(byte)Convert.ToUInt32(hex.Substring(6, 2), 16)));
		}

		public static string SolidColorToHex(SolidColorBrush solidColorBrush)
			=> string.Format("#{0:X2}{1:X2}{2:X2}", solidColorBrush.Color.R, solidColorBrush.Color.G, solidColorBrush.Color.B);

		public static int GetUnixTime() => (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

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

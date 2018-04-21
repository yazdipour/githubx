using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace GithubX.UWP.Services.Cache
{
	class FileCache : ICache
	{
		StorageFolder localFolder = ApplicationData.Current.LocalFolder;

		public bool Exists(string address)
		{
			return ReadAsync(address) != null;
		}

		public async Task<string> ReadAsync(string address)
		{
			return await ReadFile(address + ".cache");
		}

		public async Task<bool> SaveAsync(string address, string setting)
		{
			try
			{
				return await WriteFile(address + ".cache", setting).ConfigureAwait(false);
			}
			catch { return false; }
		}

		public async void Remove(string fileName)
		{
			StorageFile sampleFile = await localFolder.GetFileAsync(fileName);
			await sampleFile.DeleteAsync();
		}

		private async Task<bool> WriteFile(string fileName, string content)
		{
			try
			{
				StorageFile sampleFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
				await FileIO.WriteTextAsync(sampleFile, content);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private async Task<string> ReadFile(string fileName)
		{
			StorageFile sampleFile = await localFolder.GetFileAsync(fileName);
			return await FileIO.ReadTextAsync(sampleFile);
		}
	}
}

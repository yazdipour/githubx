using System.Threading.Tasks;

namespace GithubX.UWP.Services.Cache
{
	class ServerCacheHandler : ICache
	{
		public Task<string> ReadAsync(string address)
		{
			throw new System.NotImplementedException();
		}

		public Task<bool> SaveAsync(string address, string setting)
		{
			throw new System.NotImplementedException();
		}

		public void Remove(string fileName)
		{
			throw new System.NotImplementedException();
		}

		public bool Exists(string address)
		{
			throw new System.NotImplementedException();
		}
	}
}

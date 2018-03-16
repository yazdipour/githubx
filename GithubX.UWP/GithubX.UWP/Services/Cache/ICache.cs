using System.Threading.Tasks;

namespace GithubX.UWP.Services.Cache
{
	internal interface ICache
	{
		Task<string> ReadAsync(string address);
		Task<bool> SaveAsync(string address, string setting);
		void Remove(string fileName);
		bool Exists(string address);
	}
}

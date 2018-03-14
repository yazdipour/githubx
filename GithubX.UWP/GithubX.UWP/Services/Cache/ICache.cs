namespace GithubX.UWP.Services.Cache
{
	internal interface ICache
	{
		bool Exists(string address);

		string Read(string address);

		bool Save(string address, string setting);

		void Remove(string address);
	}
}

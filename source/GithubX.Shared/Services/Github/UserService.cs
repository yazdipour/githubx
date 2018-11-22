using Octokit;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GithubX.Shared.Services
{
	public class UserService
	{
		private readonly GitHubClient client;
		public UserService(ref GitHubClient client) => this.client = client;

		public Task<User> GetUser(string userName = null)
			=> userName == null ? client.User?.Current() : client.User?.Get(userName);

		#region Following/er
		public async Task<bool> FollowUserAsync(string user)
			=> await client.User.Followers.Follow(user).ConfigureAwait(false);

		public async Task UnFollowUserAsync(string user)
			=> await client.User.Followers.Unfollow(user).ConfigureAwait(false);

		public async Task<bool> IsFollowAsync(string user)
			=> await client.User.Followers.IsFollowingForCurrent(user).ConfigureAwait(false);

		public async Task<ObservableCollection<User>> GetAllFollowing(string user, ApiOptions options)
			=> new ObservableCollection<User>(await client.User.Followers.GetAllFollowing(user, options).ConfigureAwait(false));

		public async Task<ObservableCollection<User>> GetAllFollowers(string user, ApiOptions options)
			=> new ObservableCollection<User>(await client.User.Followers.GetAll(user, options).ConfigureAwait(false));
		#endregion

		#region Gist
		public async Task<ObservableCollection<Gist>> GetUserGists(ApiOptions options)
			=> new ObservableCollection<Gist>(await client.Gist.GetAll(options).ConfigureAwait(false));

		public async Task<ObservableCollection<Gist>> GetGistsForUser(string user, ApiOptions options)
			=> new ObservableCollection<Gist>(new List<Gist>(await client.Gist.GetAllForUser(user, options).ConfigureAwait(false)));
		#endregion

		#region Repos
		public async Task<ObservableCollection<Repository>> GetUserRepositories(ApiOptions options)
			=> new ObservableCollection<Repository>(await client.Repository.GetAllForCurrent(options).ConfigureAwait(false));

		public async Task<ObservableCollection<Repository>> GetRepositoriesForUser(string user, ApiOptions options)
			=> new ObservableCollection<Repository>(await client.Repository.GetAllForUser(user, options).ConfigureAwait(false));
		#endregion

		#region Star
		public async Task<ObservableCollection<Repository>> GetStarredRepositories(ApiOptions options)
			=> new ObservableCollection<Repository>(await client.Activity.Starring.GetAllForCurrent(options).ConfigureAwait(false));

		public async Task<ObservableCollection<Repository>> GetStarredRepositoriesForUser(string user, ApiOptions options)
			=> new ObservableCollection<Repository>(await client.Activity.Starring.GetAllForUser(user, options).ConfigureAwait(false));
		#endregion

		public async Task<ObservableCollection<Activity>> GetUserActivity(string user, ApiOptions options)
			=> new ObservableCollection<Activity>(await client.Activity.Events.GetAllUserReceivedPublic(user, options).ConfigureAwait(false));
	}
}

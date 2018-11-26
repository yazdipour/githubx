using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GithubX.Shared.Services
{
	public class UserService
	{
		private readonly GitHubClient client;
		public UserService(ref GitHubClient client) => this.client = client;

		public Task<User> GetUser(string userName = null)
			=> userName != null ? client?.User?.Get(userName) : client?.User?.Current();

		#region Following/er
		public async Task<bool> FollowUserAsync(string user)
			=> await client.User.Followers.Follow(user);

		public async Task UnFollowUserAsync(string user)
			=> await client.User.Followers.Unfollow(user);

		public async Task<bool> IsFollowAsync(string user)
			=> await client.User.Followers.IsFollowingForCurrent(user);

		public async Task<IReadOnlyList<User>> GetAllFollowing(ApiOptions options, string user = null)
			=> user == null
			? await client.User.Followers.GetAllFollowingForCurrent(options)
			: await client.User.Followers.GetAllFollowing(user, options);

		public async Task<IReadOnlyList<User>> GetAllFollowers(ApiOptions options, string user = null)
			=> user == null
			? await client.User.Followers.GetAllForCurrent(options)
			: await client.User.Followers.GetAll(user, options);
		#endregion

		public async Task<IReadOnlyList<Gist>> GetUserGists(string user = null)
			=> user == null
			? await client.Gist.GetAll()
			: await client.Gist.GetAllForUser(user);

		public async Task<IReadOnlyList<Repository>> GetUserRepositories(ApiOptions options, string user = null)
			=> user == null
			? (await client.Repository.GetAllForCurrent(options))
			: (await client.Repository.GetAllForUser(user, options));

		public async Task<IReadOnlyList<Repository>> GetStarredRepositories(ApiOptions options, string user = null)
			=> user == null
			? (await client.Activity.Starring.GetAllForCurrent(options))
			: (await client.Activity.Starring.GetAllForUser(user, options));

		public async Task<IReadOnlyList<Activity>> GetUserActivity(ApiOptions options, string user)
			=> await client.Activity.Events.GetAllUserReceived(user, options);

		#region Notification
		public async Task<IReadOnlyList<Notification>> GetAllNotifications(ApiOptions options)
			=> await client.Activity.Notifications.GetAllForCurrent(new NotificationsRequest { All = true }, options);

		public async Task MarkAllNotificationsAsRead() => await client.Activity.Notifications.MarkAsRead();

		public async Task MarkNotificationAsRead(string notificationId)
		{
			if (int.TryParse(notificationId, out int id))
				await client.Activity.Notifications.MarkAsRead(id);
		}
		#endregion
	}
}

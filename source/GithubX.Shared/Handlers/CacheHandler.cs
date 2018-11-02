using Akavache;
using System;

namespace GithubX.Shared.Handlers
{
	public static class CacheHandler
	{
		public static void InitCache()
		{
			BlobCache.ApplicationName = "GithubX";
			BlobCache.ForcedDateTimeKind = DateTimeKind.Utc;
		}
	}
}

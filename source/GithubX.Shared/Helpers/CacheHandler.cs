using Akavache;
using System;

namespace GithubX.Shared.Helpers
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

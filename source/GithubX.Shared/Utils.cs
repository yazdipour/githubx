using System;

namespace GithubX.Shared
{
	public class Utils
	{
		public int? GetUnixTimestamp(DateTime? dateTime)
			=> dateTime == null ? null : (int?)(int)((DateTime)dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
	}
}

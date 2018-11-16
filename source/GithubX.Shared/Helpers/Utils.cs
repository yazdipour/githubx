using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace GithubX.Shared.Helpers
{
	public static class Utils
	{
		public static int? GetUnixTimestamp(DateTime? dateTime)
			=> dateTime == null ? null : (int?)(int)((DateTime)dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
	}
}

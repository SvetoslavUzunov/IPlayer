using System.Text.RegularExpressions;

namespace Maui.Apps.Framework.Extensions;

public static class StringExtension
{
	public static string CleanCacheKey(this string url)
		=> Regex.Replace(new Regex("[\\~#%&*{}/:<>?|\"-]").Replace(url, " "), @"\s+", "_");
}

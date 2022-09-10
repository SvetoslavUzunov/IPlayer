using System.Text.RegularExpressions;

namespace Maui.Apps.Framework.Extensions;

public static class StringExtension
{
	public static string CleanCacheKey(this string url)
		=> Regex.Replace(new Regex("[\\~#%&*{}/:<>?|\"-]").Replace(url, " "), @"\s+", "_");

	public static string FormattedNumber(this string number)
		=> Convert.ToDouble(number).FormattedNumber();
}

using System.Collections.ObjectModel;

namespace Maui.Apps.Framework.Extensions;

public static class ListExtension
{
	public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> newItems, bool clearFirst = false)
	{
		if (clearFirst)
		{
			collection.Clear();
		}

		newItems.ForEach(newItem => collection.Add(newItem));
	}

	public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
		=> source.OrderBy((item) => new Random().Next());

	public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
	{
		foreach (var item in enumeration)
		{
			action(item);
		}
	}
}

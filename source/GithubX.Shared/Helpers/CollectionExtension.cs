using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GithubX.Shared.Helpers
{
	public static class CollectionExtension
	{
		public static int Remove<T>(this ObservableCollection<T> collection, Func<T, bool> condition)
		{
			var itemsToRemove = collection.Where(condition).ToList();
			foreach (var itemToRemove in itemsToRemove) collection.Remove(itemToRemove);
			return itemsToRemove.Count;
		}

		public static void RemoveAll<T>(this ObservableCollection<T> collection)
			=> collection.ToList().All(i => collection.Remove(i));

		public static void AddRange<T>(this ObservableCollection<T> collection, T[] items)
			=> AddRange(collection, items.ToList());

		public static void AddRange<T>(this ObservableCollection<T> collection, List<T> items)
		{
			foreach (var item in items) collection.Add(item);
		}
	}
}

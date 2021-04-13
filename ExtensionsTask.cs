using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public static class ExtensionsTask
	{
		/// <summary>
		/// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
		/// Медиана списка из четного количества элементов — это среднее арифметическое 
		/// двух серединных элементов списка после сортировки.
		/// </summary>
		/// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
		public static double Median(this IEnumerable<double> items)
		{
			var sortItems = items
				.OrderBy(number => number)
				.ToList();
			var countEnumerable = sortItems.Count;

			if (countEnumerable == 0)
				throw new InvalidOperationException();

			return countEnumerable % 2 == 0
				? ((sortItems[countEnumerable / 2] + sortItems[countEnumerable / 2 - 1]) / 2)
				: sortItems[countEnumerable / 2];
		}

		/// <returns>
		/// Возвращает последовательность, состоящую из пар соседних элементов.
		/// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
		/// </returns>
		public static IEnumerable<Tuple<T, T>> Bigrams<T>(this IEnumerable<T> items)
		{
			if (items == null)
				throw new InvalidOperationException();

			var prev = default(T);
			var flag = true;

			foreach (var e in items)
			{
				if (!flag)
					yield return new Tuple<T, T>(prev, e);
				prev = e;
				flag = false;
			}
		}
	}
}


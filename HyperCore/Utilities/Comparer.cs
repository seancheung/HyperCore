using HyperCore.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HyperCore.Utilities
{
	public static class Comparer
	{
		/// <summary>
		/// Distinct Class by specified properties
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="source"></param>
		/// <param name="keySelector"></param>
		/// <returns></returns>
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> seenKeys = new HashSet<TKey>();
			foreach (TSource element in source)
			{
				var elementValue = keySelector(element);
				if (seenKeys.Add(elementValue))
				{
					yield return element;
				}

			}
		}

		/// <summary>
		/// Compare Enum on their integer name
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public class EnumComparer<T> : IEqualityComparer<T> where T : struct
			{
				public bool Equals(T first, T second)
				{
					var firstParam = Expression.Parameter(typeof(T), "first");
					var secondParam = Expression.Parameter(typeof(T), "second");
					var equalExpression = Expression.Equal(firstParam, secondParam);

					return Expression.Lambda<Func<T, T, bool>>(equalExpression, new[] { firstParam, secondParam }).Compile().Invoke(first, second);
				}

				public int GetHashCode(T instance)
				{

					var parameter = Expression.Parameter(typeof(T), "instance");

					var convertExpression = Expression.Convert(parameter, typeof(int));

					return Expression.Lambda<Func<T, int>>

					       (convertExpression, new[] { parameter }).

					       Compile().Invoke(instance);

				}
			}

		/// <summary>
		/// Compare card on their ID property
		/// </summary>
		public class CardComparer : IEqualityComparer<Card>
		{
			public bool Equals(Card x, Card y)
			{
				return x.ID == y.ID;
			}

			public int GetHashCode(Card obj)
			{
				return obj.ID.GetHashCode();
			}
		}
	}
}

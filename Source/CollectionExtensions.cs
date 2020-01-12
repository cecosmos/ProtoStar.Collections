using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace ProtoStar.Collections
{
    public static class CollectionExtensions
    {
        public static bool TryFind<T>(this IEnumerable<T> source, System.Predicate<T> predicate, out T match)
        {
            foreach (T item in source) { if (predicate(item)) { match = item; return true; } }
            match = default(T); return false;
        }

        public static IEnumerable<T> Interleave<T>(
            this IEnumerable<T> source, 
            IEnumerable<T> second
        ) =>
            source.Zip(second, (l, r) => new[] { l, r }).SelectMany(x => x);

        public static IEnumerable<T> TakeWhile<T>(this IEnumerable<T> source, System.Predicate<T> predicate, bool inclusive)
        {
            foreach(var item in source)
            {
                if (predicate(item)) yield return item;
                else
                {
                    if (inclusive) yield return item;
                    yield break;
                }
            }
        }

        public static IReadOnlyCollection<T> AsReadOnlyCollection<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            return source as IReadOnlyCollection<T> ?? new CollectionAdapter<T>(()=>source);
        }

        public static IReadOnlyDictionary<TOut, double> ToNormalizedDictionary<TIn,TOut>(
            this IEnumerable<TIn> source,
            Func<TIn,TOut> keySelector,
            Func<TIn,double> valueSelector)
        {
            var dict = source.ToDictionary(keySelector,valueSelector);
            var valueSum = dict.Values.Sum();
            return dict.ToDictionary(keyValue => keyValue.Key, keyValue => keyValue.Value/valueSum);
        }

        public static IReadOnlyDictionary<T, double> ToNormalizedDictionary<T>(
            this IEnumerable<KeyValuePair<T,double>> source
        ) =>
            source.ToNormalizedDictionary(kv=> kv.Key,kv=>kv.Value);   

        public static IEnumerable<double> ToNormalized(
            this IEnumerable<double> source
        )
        {
            var valueSum = source.Sum();
            return source.Select(value => value/valueSum);
        }


    }
}
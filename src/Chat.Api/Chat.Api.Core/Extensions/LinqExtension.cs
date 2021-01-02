using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Api.Core.Extensions
{
    public static class LinqExtension
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> lhs, Action<T> func)
        {
            foreach (T x in lhs)
            {
                func(x);
                yield return x;
            }
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> lhs, Action<T, int> func)
        {
            int i = 0;
            foreach (T x in lhs)
            {
                func(x, i++);
                yield return x;
            }
        }

        public static void Evaluate<T>(this IEnumerable<T> lhs)
        {
            // ReSharper disable once UnusedVariable
            foreach (T x in lhs)
            {
            }
        }

        public static void Visit<T>(this IEnumerable<T> lhs, Action<T> func)
        {
            lhs
                .ForEach(func)
                .Evaluate();
        }

        public static void Visit<T>(this IEnumerable<T> lhs, Action<T, int> func)
        {
            lhs
                .ForEach(func)
                .Evaluate();
        }

        public static async Task<IEnumerable<T2>> VisitAsync<T1, T2>(this IEnumerable<T1> lhs, Func<T1, Task<T2>> func,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var list = new List<T2>();
            foreach (var x in lhs)
            {
                list.Add(await func(x).ConfigureAwait(false));
                cancellationToken.ThrowIfCancellationRequested();
            }

            return list;
        }

        public static IEnumerable<IEnumerable<T>> Chunkify<T>(
            this IEnumerable<T> source, int batchSize)
        {
            return source.Batch(batchSize);
        }

        public static IEnumerable<IEnumerable<T>> Batch<T>(
            this IEnumerable<T> source, int batchSize)
        {
            using (var enumerator = source.GetEnumerator())
                while (enumerator.MoveNext())
                    yield return YieldBatchElements(enumerator, batchSize - 1);
        }

        private static IEnumerable<T> YieldBatchElements<T>(
            IEnumerator<T> source, int batchSize)
        {
            yield return source.Current;
            for (int i = 0; i < batchSize && source.MoveNext(); i++)
                yield return source.Current;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
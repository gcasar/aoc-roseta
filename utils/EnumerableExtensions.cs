using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.utils;

public static class EnumerableExtensions
{
    public static IEnumerable<T> Sample<T>(this IEnumerable<T> enumerable, int start, int step, int end = int.MaxValue)
    {
        int tick = -1;
        int next = start;
        using var enumerator = enumerable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            tick++;
            if (tick == next)
            {
                yield return enumerator.Current;
                next += step;
            }
            if (tick >= end)
            {
                break;
            }
        }
    }

    public static IEnumerable<T> Iterate<T>(this IEnumerator<T> iterator)
    {
        while (iterator.MoveNext())
        {
            yield return iterator.Current;
        }
    }
 
    public static IEnumerable<IEnumerable<T>> Chunky<T>(this IEnumerable<T> enumerable, int chunkSize) =>
        enumerable
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value));
}
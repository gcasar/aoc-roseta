using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.utils;


public static class LineParsingExtensions
{
    public static IList<string> Parse(this string input, string pattern)
    {
        var regex = new Regex(pattern);
        var groups = regex.Match(input).Groups;
        return groups.Values.Skip(1).Select(it => it.Value).ToList();
    }

    public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest) {
        first = (list.Count > 0 ? list[0] : default(T))!; // or throw
        rest = list.Skip(1).ToList();
    }
    
    public static void Deconstruct<T>(this IList<T> list, out T first,  out T second, out IList<T> rest) {
        first = (list.Count > 0 ? list[0] : default(T))!; // or throw
        second = (list.Count > 0 ? list[1] : default(T))!; // or throw
        rest = list.Skip(2).ToList();
    }

    public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out T third, out IList<T> rest) {
        first = (list.Count > 0 ? list[0] : default(T))!; // or throw
        second = (list.Count > 0 ? list[1] : default(T))!; // or throw
        third = (list.Count > 0 ? list[2] : default(T))!; // or throw
        rest = list.Skip(3).ToList();
    }

}
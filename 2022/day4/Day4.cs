using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.utils;
using Xunit;

namespace AdventOfCode._2022.day4;

public class Day4
{
    [Theory]
    [FileLineTest("2022/day4/example1.in", 2)]
    [FileLineTest("2022/day4/input1.in", 542)]
    public void Star1(LineEnumerable lines, int expected)
    {
        var result = lines
            .Select(it => it.Trim())
            .Select(Range.ParseLine)
            .Count(it => Range.ContainsEither(it.a, it.b));

        Assert.Equal(expected, result);
    }

    [Theory]
    [FileLineTest("2022/day4/example1.in", 4)]
    [FileLineTest("2022/day4/input1.in", 900)]
    public void Star2(LineEnumerable lines, int expected)
    {
        var result = lines
            .Select(it => it.Trim())
            .Select(Range.ParseLine)
            .Count(it => Range.Overlap(it.a, it.b));

        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("2-8,3-7", true)]
    [InlineData("3-7,2-8", true)]
    [InlineData("1-1,1-1", true)]
    [InlineData("1-1,2-2", false)]
    public void ContainsEitherTest(string line, bool expected)
    {
        var (a, b) = Range.ParseLine(line);
        Assert.Equal(expected, Range.ContainsEither(a, b));
    }

    [Theory]
    [InlineData("2-8,3-7", true)]
    [InlineData("3-7,2-8", true)]
    [InlineData("2-3,3-4", true)]
    [InlineData("2-3,1-2", true)]
    [InlineData("2-4,6-8", false)]
    [InlineData("2-3,4-5", false)]
    [InlineData("1-1,2-2", false)]
    public void OverlapTest(string line, bool expected)
    {
        var (a, b) = Range.ParseLine(line);
        Assert.Equal(expected, Range.Overlap(a, b));
    }

    public readonly record struct Range(int Start, int End)
    {
        public static (Range a, Range b) ParseLine(string input)
        {
            var regex = new Regex(@"(\d+)-(\d+),(\d+)-(\d+)");
            var result = regex.Match(input).Groups;
            return (
                a: new Range(Start: Int32.Parse(result[1].Value), End: Int32.Parse(result[2].Value)),
                b: new Range(Start: Int32.Parse(result[3].Value), End: Int32.Parse(result[4].Value))
            );
        }

        public static bool Overlap(Range a, Range b) =>
            Math.Min(a.End, b.End) - Math.Max(a.Start, b.Start) >= 0;

        public static bool ContainsEither(Range a, Range b)
        {
            if (a.Start >= b.Start && a.End <= b.End)
            {
                return true;
            }

            if (b.Start >= a.Start && b.End <= a.End)
            {
                return true;
            }

            return false;
        }
    }
}
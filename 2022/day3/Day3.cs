using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.utils;
using Xunit;

namespace AdventOfCode._2022.day3;

public class Day3
{
    [Theory]
    [FileLineTest("2022/day3/example1.in", 157)]
    [FileLineTest("2022/day3/input1.in", 7428)]
    public void Star1(LineEnumerable lines, int expected)
    {
        var result = lines
            .Select(line => line.Trim())
            .Select(line =>
            (
                c1: new HashSet<char>(line[..(line.Length / 2)]),
                c2: new HashSet<char>(line[(line.Length / 2)..])
            ))
            .Select(it => it.c1.Intersect(it.c2))
            .Select(it => it.First())
            .Select( AssignScore )
            .Sum();
        Assert.Equal(expected, result);
    }

    [Theory]
    [FileLineTest("2022/day3/example1.in", 70)]
    [FileLineTest("2022/day3/input1.in", 2650)]
    public void Star2(LineEnumerable lines, int expected)
    {
        var result = lines
            .Select(line => line.Trim())
            .Chunk(3)
            .Select( (IEnumerable<string> chunk) => FindCommonCharacter(chunk) )
            .Select( AssignScore )
            .Sum();
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [FileLineTest("2022/day3/example1.in", 'r')]
    public void FindCommonCharacterTest(LineEnumerable lines, char expected)
    {
        var result = FindCommonCharacter( lines.Take(3).Select(it => it.Trim()) );
        Assert.Equal(expected, result);
    }
    
    public static char FindCommonCharacter(IEnumerable<string> lines) => lines
            .Aggregate( (aggregate, current) => String.Join("", aggregate.Intersect(current)) )
            .First();

    public static int AssignScore(char ch) => ch switch
    {
        >= 'a' and <= 'z' => ch - 'a' + 1,
        >= 'A' and <= 'Z' => ch - 'A' + 27,
        _ => throw new ArgumentException()
    };
}
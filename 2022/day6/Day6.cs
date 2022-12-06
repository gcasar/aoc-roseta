using System.Collections.Generic;
using System.Linq;
using AdventOfCode.utils;
using Xunit;

namespace AdventOfCode._2022.day6;

public class Day6
{
    [Theory]
    [FileLineTest("2022/day6/input1.in", 1080)]
    public void Star1(LineEnumerable lines, int expected)
    {
        SimpleSolve(lines.First(), 4, expected);
    }
    
    [Theory]
    [FileLineTest("2022/day6/input1.in", 3645)]
    public void Star2(LineEnumerable lines, int expected)
    {
        SimpleSolve(lines.First(), 14, expected);
    }
    
    [Theory]
    [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 4, 7)]
    [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 4, 5)]
    [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 4, 6)]
    [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 4, 10)]
    [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 4, 11)]
    [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 14, 19)]
    [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 14, 23)]
    [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 14, 23)]
    [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 14, 29)]
    [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 14, 26)]
    public void SimpleSolve(string input, int markerLength, int expected)
    {
        // Not doing a diff approach. Lets see if it pans out!
        // (diff is when you update your set with the new character and the character leaving)
        // (that approach is O(N) this one can be O(N*M) where M is the cost of creating the set)
        int i;
        for (i = markerLength; i < input.Length; i++)
        {
            var unique = new HashSet<char>(input.Substring(i - markerLength, markerLength));
            if (unique.Count == markerLength)
            {
                break;
            }
        }

        Assert.Equal(expected, i);
    }


}
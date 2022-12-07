using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdventOfCode.utils;
using Xunit;

namespace AdventOfCode._2022.day7;

public class Day7
{
    [Theory]
    [FileLineTest("2022/day7/example1.in", 95437)]
    [FileLineTest("2022/day7/input1.in", 1582412)]
    public void Star1(LineEnumerable lines, int expected)
    {
        const int directoryMaxSizeThreshold = 100_000;
        // seems like we can do DFS by the order of lines!
        // each subdirectory is navigated to in order of `ls`
        // once no more subdirectories exist we get neatly navigated up using `cd ..`
        var matching = IterateDirectories(lines).Where(size => size < directoryMaxSizeThreshold).Sum();
        Assert.Equal(expected, matching);
    }

    
    [Theory]
    [FileLineTest("2022/day7/example1.in", 24933642)]
    [FileLineTest("2022/day7/input1.in", 3696336)]
    public void Star2(LineEnumerable lines, int expected)
    {
        var directorySizes = IterateDirectories(lines).ToImmutableSortedSet();
        int unused = 70_000_000 - directorySizes.Max;
        int missing = 30_000_000 - unused;
        Assert.Equal(expected, directorySizes.First( size => size > missing ));
    }

    public IEnumerable<int> IterateDirectories(IEnumerable<string> stream)
    {
        var sizeStack = new Stack<int>();
        int currentSum = 0;
        foreach (string line in stream)
        {
            if (line == "$ ls") continue;
            if (line == "$ cd ..")
            {
                yield return currentSum;
                currentSum += sizeStack.Pop();
            }
            else if (line.StartsWith("$ cd "))
            {
                sizeStack.Push(currentSum);
                currentSum = 0;
            }
            else
            {
                int.TryParse(line.Split(" ")[0], out int fileSize);
                currentSum += fileSize;
            }
        }

        // manually close open directories (you can view this as calling $ cd ..)
        while (sizeStack.Count > 0)
        {
            yield return currentSum;
            currentSum += sizeStack.Pop();
        }
    }

}
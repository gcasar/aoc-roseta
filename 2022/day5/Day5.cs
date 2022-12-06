using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.utils;
using Xunit;

namespace AdventOfCode._2022.day5;

public class Day5
{
    [Theory]
    [FileLineTest("2022/day5/example1.in", "CMZ")]
    [FileLineTest("2022/day5/input1.in", "BSDMQFLSP")]
    public void Star1(LineEnumerable lines, string expected)
    {
        // use a buffer extension to first grab our stacks
        var iterable = lines.Publish();

        var crateStacks = iterable
            .TakeWhile(line => line[1] != '1')
            .Aggregate(
                new CrateStacks(12), 
                (prev, line) => prev.Add(line)
            );

        var crateMover = new CrateMover9000();
        iterable.Skip(1).ForEach(line => crateMover.Move(crateStacks, line));
        
        var result = string.Join(
            "", 
            crateStacks.Stacks.Select( it => it.Length > 0 ? $"{it[^1]}" : "")
        );
        Assert.Equal(expected, result);
    }

    [Theory]
    [FileLineTest("2022/day5/example1.in", "MCD")]
    [FileLineTest("2022/day5/input1.in", "PGSQBFLDP")]
    public void Star2(LineEnumerable lines, string expected)
    {
        // use a buffer extension to first grab our stacks
        var iterable = lines.Publish();

        var crateStacks = iterable
            .TakeWhile(line => line[1] != '1')
            .Aggregate(
                new CrateStacks(12), 
                (prev, line) => prev.Add(line)
            );

        var crateMover = new CrateMover9001();
        iterable.Skip(1).ForEach(line => crateMover.Move(crateStacks, line));
        
        var result = string.Join(
            "", 
            crateStacks.Stacks.Select( it => it.Length > 0 ? $"{it[^1]}" : "")
        );
        Assert.Equal(expected, result);
    }

    [Fact]
    public void MoveTest()
    {
        var crateStacks = new CrateStacks(3);
        crateStacks.Add("    [D]    ");
        crateStacks.Add("    [N] [C]");
        crateStacks.Add("[Z] [M] [P]");

        Assert.Equal("Z", crateStacks.Stacks[0]);
        Assert.Equal("MND", crateStacks.Stacks[1]);
        Assert.Equal("PC", crateStacks.Stacks[2]);

        var mover = new CrateMover9000();
        mover.Move(crateStacks, "move 3 from 2 to 1");

        Assert.Equal("ZDNM", crateStacks.Stacks[0]);
        Assert.Equal("", crateStacks.Stacks[1]);
    }

    public class CrateMover9000
    {

        public virtual CrateStacks Move(CrateStacks crateStacks, string line)
        {
            var action = ActionFromLine(line);

            crateStacks.Stacks[action.to] += string.Join("", crateStacks.Stacks[action.from][^action.count..].Reverse());
            crateStacks.Stacks[action.from] = crateStacks.Stacks[action.from][..^action.count];

            return crateStacks;
        }

        public static (int count, int from, int to) ActionFromLine(string line)
        {
            var regex = new Regex(@"move (\d+) from (\d+) to (\d+)");
            var groups = regex.Match(line).Groups;
            return (
                count: Int32.Parse(groups[1].Value), 
                from: Int32.Parse(groups[2].Value) - 1,
                to: Int32.Parse(groups[3].Value) - 1
            );
        }
    }

    public class CrateMover9001 : CrateMover9000
    {
        public override CrateStacks Move(CrateStacks crateStacks, string line)
        {
            var action = ActionFromLine(line);

            crateStacks.Stacks[action.to] +=crateStacks.Stacks[action.from][^action.count..];
            crateStacks.Stacks[action.from] = crateStacks.Stacks[action.from][..^action.count];

            return crateStacks;
        }
    }

    public class CrateStacks
    {
        public readonly string[] Stacks;

        public CrateStacks(int bufferSize)
        {
            Stacks = Enumerable.Range(0, bufferSize).Select(_ => "").ToArray();
        }

        public CrateStacks Add(string line)
        {
            line.Chunk(4)
                .Select((it, idx) => (ch: it[1], idx))
                .Where(it => it.ch != ' ')
                .ForEach(tuple => Stacks[tuple.idx] = tuple.ch + Stacks[tuple.idx]);
            return this;
        }


    }
   
}

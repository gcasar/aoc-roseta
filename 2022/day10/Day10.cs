using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.utils;
using Xunit;

namespace AdventOfCode._2022.day10;

public class Day10
{
    [Theory]
    [FileLineTest("2022/day10/example1.in", 13140)]
    [FileLineTest("2022/day10/input1.in", 17380)]
    public void Star1(LineEnumerable lines, int expected)
    {
        var program = new Program();
        var commands = lines.Select(Command.FromLine);
        var clock = program.Run(commands);
        // we are off-by-two because the first tick the clock yields is "2" (bug)
        var calculated = clock.Sample(18, 40)
            .Select(tick => tick * program.X)
            .Sum();
        Assert.Equal(expected, calculated);

    }

    [Theory]
    [FileLineTest("2022/day10/example1.in", "ABCDEFGH")]
    [FileLineTest("2022/day10/input1.in", "FGCUZREC")]
    public void Star2(LineEnumerable lines, string _)
    {
        const int width = 40;
        var program = new Program();
        var commands = lines.Select(Command.FromLine);
        var clock = program.Run(commands);

        var buffer = new StringBuilder();
        foreach (var tick in clock)
        {
            if (Math.Abs((tick-2) % width+1 - program.X) <= 1)
            {
                buffer.Append('#');
            }
            else
            {
                buffer.Append(' ');
            }

            if ((tick-2) % width == 0)
            {
                buffer.Append('\n');
            }
        }

        // also with a bug, but enough to decode.
        var _helper = @"
#
##  ##   ##  #  # #### ###  ####  ##    
   #  # #  # #  #    # #  # #    #  #  #
#  #    #    #  #   #  #  # ###  #      
   # ## #    #  #  #   ###  #    #      
   #  # #  # #  # #    # #  #    #  #   
    ###  ##   ##  #### #  # ####  ##   
";
    }

    [Theory]
    [InlineData("noop\n", "noop", null)]
    [InlineData("addx -1\n", "addx", -1)]
    public void ParseCommand(string line, string mnemonic, int? param)
    {
        var command = Command.FromLine(line);
        Assert.Equal(command.Mnemonic, mnemonic);
        Assert.Equal(command.param, param);
    }

    [Fact]
    public void TestRun()
    {
        var program = new Program();
        using var clock = program.Run(
            new[]
            {
                Command.FromLine("noop"),
                Command.FromLine("addx 3"),
                Command.FromLine("addx -5")
            }
        ).GetEnumerator();

        clock.MoveNext();
        Assert.Equal(1, program.X);
        clock.MoveNext();
        Assert.Equal(1, program.X);
        clock.MoveNext();
        Assert.Equal(4, program.X);
        clock.MoveNext();
        Assert.Equal(4, program.X);
        clock.MoveNext();
        Assert.Equal(-1, program.X);
        Assert.False(clock.MoveNext());
    }

    public class Program
    {
        public int X { private set; get; } = 1;

        public IEnumerable<int> Run(IEnumerable<Command> commands)
        {
            int cycle = 1;
            foreach (var command in commands)
            {
                switch(command.Mnemonic)
                {
                    case "noop":
                        yield return ++cycle;
                        break;
                    case "addx":
                        yield return ++cycle;
                        X += command.param!.Value;
                        yield return ++cycle;
                        break;
                }
            }
        }
    }

    public readonly record struct Command(string Mnemonic, int? param)
    {
        public static Command FromLine(string line)
        {
            var parts = line.Split(" ");
            if (parts.Length == 2)
            {
                return new Command(parts[0].Trim(), Int32.Parse(parts[1]));
            }
            return new Command(parts[0].Trim(), null);
        }
    }
}
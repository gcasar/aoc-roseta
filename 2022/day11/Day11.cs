using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.utils;
using Xunit;

namespace AdventOfCode._2022.day11;

public class Day11
{
    [Theory]
    [FileLineTest("2022/day11/example1.in", 10605)]
    [FileLineTest("2022/day11/input1.in", 58322)]
    public void Star1(LineEnumerable lines, int expected)
    {
        var monkeys = lines.ReadAll()
            .Split("Monkey", StringSplitOptions.RemoveEmptyEntries)
            .Select(it => it.Split("\n"))
            .Select(Monkey.FromLines)
            .ToArray();

        for (int i = 0; i < 20; i++)
        {
            // monkey round
            foreach (var (monkey, idx) in monkeys.Select((it, idx) => (it, idx)))
            {
                monkeys[idx].InspectCount += monkey.Items.Count;
                foreach (var action in monkey.PerformTurn( it => it / 3))
                {
                    monkeys[action.target].Items.Add(action.level);
                }
            }
        }

        var counts = monkeys.Select(it => it.InspectCount).OrderByDescending(it => it).Take(2).ToArray();

        Assert.Equal(expected, counts[0] * counts[1]);
    }

    [Theory]
    [FileLineTest("2022/day11/example1.in", 2713310158)]
    [FileLineTest("2022/day11/input1.in", 13937702909)]
    public void Star2(LineEnumerable lines, long expected)
    {
        var monkeys = lines.ReadAll()
            .Split("Monkey", StringSplitOptions.RemoveEmptyEntries)
            .Select(it => it.Split("\n"))
            .Select(Monkey.FromLines)
            .ToArray();

        var baseDivisors = monkeys.Select(it => it.TestDivisible).Distinct();
        var greatestCommonDivisor = baseDivisors.Aggregate(1L, (prev, curr) => prev * curr);

        for (int i = 0; i < 10_000; i++)
        {
            // monkey round
            foreach (var (monkey, idx) in monkeys.Select((it, idx) => (it, idx)))
            {
                monkeys[idx].InspectCount += monkey.Items.Count;
                foreach (var action in monkey.PerformTurn( it => it % greatestCommonDivisor))
                {
                    monkeys[action.target].Items.Add(action.level);
                }
            }
        }

        var counts = monkeys.Select(it => it.InspectCount).OrderByDescending(it => it).Take(2).ToArray();

        Assert.Equal(expected, counts[0] * counts[1]);
    }

    [Fact]
    public void MonkeyParseTest()
    {
        var monkeyDefinition = @"
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3
";

        var monkey = Monkey.FromLines(monkeyDefinition.Split("\n"));
        Assert.Equal(new List<int> {79, 98}, monkey.Items.Select( it => (int)it).ToList());
        Assert.Equal(19, monkey.Operation.Invoke(1));
        Assert.Equal(23, monkey.TestDivisible);
        Assert.Equal(2, monkey.Target1);
        Assert.Equal(3, monkey.Target2);
    }

    public record struct Monkey(
        List<long> Items,
        Func<long, long> Operation,
        long TestDivisible,
        int Target1,
        int Target2,
        long InspectCount = 0
    )
    {
        public static Monkey FromLines(IList<string> lines) =>
            new(
                Items: ParseItems(lines[1]),
                Operation: ParseOperation(lines[2]),
                TestDivisible: lines[3].ExtractInt(@".*divisible by (\d+)")!.Value,
                Target1: lines[4].ExtractInt(@".*monkey (\d+)")!.Value,
                Target2: lines[5].ExtractInt(@".*monkey (\d+)")!.Value
            );

        public static List<long> ParseItems(string line) =>
            line.ExtractString(@".*items: (.*)")
                .Split(",", StringSplitOptions.TrimEntries)
                .Select(long.Parse)
                // .Select( it => (long) it )
                .ToList();

        public static Func<long, long> ParseOperation(string line)
        {
            var (_, op, value, _) = line.Parse(@".*Operation: new = (\w+) (.) (\w+)");
            int? right = int.TryParse(value, out int rightInt) ? (int)rightInt : null;
            return op switch
            {
                "*" => old => old * (right ?? old),
                "+" => old => old + (right ?? old),
                _ => throw new ArgumentException()
            };
        }

        public IEnumerable<(int target, long level)> PerformTurn(Func<long, long> simplifier)
        {
            foreach (var item in Items)
            {
                var itemLevel = Operation(item);
                var warningLevel = simplifier.Invoke(itemLevel);
                var targetMonkey = warningLevel % TestDivisible == 0? Target1 : Target2;
                yield return (targetMonkey, warningLevel);
            }

            Items.Clear();
        }
    }
}
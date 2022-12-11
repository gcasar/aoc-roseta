using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
                // actionLog.Add($"Turn of Monkey {idx}");
                monkeys[idx].InspectCount += monkey.Items.Count;
                foreach (var action in monkey.PerformTurn( level => level))
                {
                    monkeys[action.target].Items.Add(action.level);
                }
            }
        }

        var counts = monkeys.Select(it => it.InspectCount).OrderByDescending(it => it).Take(2).ToArray();

        Assert.Equal(expected, counts[0] * counts[1]);
    }

    [Theory]
    [FileLineTest("2022/day11/example1.in", "ABCDEFGH")]
    [FileLineTest("2022/day11/input1.in", "FGCUZREC")]
    public void Star2(LineEnumerable lines, string _)
    {
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
        Assert.Equal(new List<int> {79, 14}, monkey.Items.Select( it => (int)it).ToList());
        Assert.Equal(19, (int)monkey.Operation.Invoke((NormalizedFactoredNumber)1));
        Assert.Equal(23, monkey.TestDivisible);
        Assert.Equal(2, monkey.Target1);
        Assert.Equal(3, monkey.Target2);
    }

    public record NormalizedFactoredNumber(HashSet<int> Factors)
    {
        // first 40 should be enough. If we don't get the correct result this is probably not the cause
        public static int[] PrimeNumbers = {
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71,
            73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173
        };
        
        public static NormalizedFactoredNumber operator * (NormalizedFactoredNumber a, NormalizedFactoredNumber b)
        {
            var factors = new HashSet<int>(a.Factors.Union(b.Factors));
            return new NormalizedFactoredNumber(factors);
        }

        public static NormalizedFactoredNumber operator +(NormalizedFactoredNumber a, NormalizedFactoredNumber b)
        {
            int expandedA = (int)a;
            int expandedB = (int)b;
            return (NormalizedFactoredNumber)(expandedA + expandedB);
        }

        // trying out some explicit operators ...
        public static explicit operator NormalizedFactoredNumber(int number)
        {
            var factors = new HashSet<int>();
            int remainder = number;
            for (int i = 0; i < PrimeNumbers.Length; i++)
            {
                while (remainder % PrimeNumbers[i] == 0)
                {
                    remainder /= PrimeNumbers[i];
                    factors.Add(PrimeNumbers[i]);
                }

                if (remainder == 1)
                    break;
            }

            return new NormalizedFactoredNumber(factors);
        }

        public static explicit operator int(NormalizedFactoredNumber a)
        {
            int result = 1;
            foreach (int factor in a.Factors)
            {
                result *= factor;
            }

            return result;
        }
    }

    public record struct Monkey(
        List<NormalizedFactoredNumber> Items,
        Func<NormalizedFactoredNumber, NormalizedFactoredNumber> Operation,
        int TestDivisible,
        int Target1,
        int Target2,
        int InspectCount = 0
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

        public static List<NormalizedFactoredNumber> ParseItems(string line) =>
            line.ExtractString(@".*items: (.*)")
                .Split(",", StringSplitOptions.TrimEntries)
                .Select(int.Parse)
                .Select( it => (NormalizedFactoredNumber) it )
                .ToList();

        public static Func<NormalizedFactoredNumber, NormalizedFactoredNumber> ParseOperation(string line)
        {
            var (_, op, value, _) = line.Parse(@".*Operation: new = (\w+) (.) (\w+)");
            NormalizedFactoredNumber? right = int.TryParse(value, out int rightInt) ? (NormalizedFactoredNumber)rightInt : null;
            return op switch
            {
                "*" => old => old * (right ?? old),
                "+" => old => old + (right ?? old),
                _ => throw new ArgumentException()
            };
        }

        public IEnumerable<(int target, NormalizedFactoredNumber level)> PerformTurn(Func<int, int> adjustor)
        {
            foreach (var item in Items)
            {
                // debugLog($"Inspecting item with level {item}");
                var itemLevel = Operation(item);
                // debugLog($"Item level is evaluated to {itemLevel}");
                var targetMonkey = itemLevel.Factors.Contains(TestDivisible) ? Target1 : Target2;
                // debugLog($"Item {worryLevel} thrown to {targetMonkey}");
                yield return (targetMonkey, itemLevel);
            }

            Items.Clear();
        }
    }
}
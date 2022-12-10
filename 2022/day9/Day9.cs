using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.utils;
using Xunit;

namespace AdventOfCode._2022.day9;

public class Day9
{
    [Theory]
    [FileLineTest("2022/day9/example1.in", 13)]
    [FileLineTest("2022/day9/input1.in", 5902)]
    public void Star1(LineEnumerable lines, int expected)
    {
        var visited = SimulateRopePull(lines, 1);
        Assert.Equal(expected, visited.Count);
    }

    [Theory]
    [FileLineTest("2022/day9/example1.in", 1)]
    [FileLineTest("2022/day9/example2.in", 36)]
    [FileLineTest("2022/day9/input1.in", 2445)]
    public void Star2(LineEnumerable lines, int expected)
    {
        
        var visited = SimulateRopePull(lines, 9);
        Assert.Equal(expected, visited.Count);
    }

    private static HashSet<Point> SimulateRopePull(LineEnumerable lines, int ropeLength)
    {
        var visited = new HashSet<Point>();
        var rope = Enumerable.Range(0, ropeLength + 1).Select(_ => new Point(0, 0)).ToArray();

        foreach (var line in lines)
        {
            var (direction, amountRaw, _) = line.Trim().Split(" ");
            var tailChanges = TugRope(Point.FromDirection(direction), int.Parse(amountRaw), rope);
            tailChanges.ForEach( it => visited.Add(it) );
        }

        return visited;
    }

    private static IEnumerable<Point> TugRope(Point dir, int amount, Point[] rope)
    {
        for (int i = 0; i < amount; i++)
        {
            // move head
            rope[0] += dir;
            for (int j = 1; j < rope.Length; j++)
            {
                var diff = rope[j-1] - rope[j];
                // shortcut, nothing will change past this point
                var magnitude = diff.Magnitude();
                if (magnitude <= 1)
                    break;
                rope[j] += diff.Direction();
            }
            // report tail position
            yield return rope[^1];
        }
    }

    [Theory]
    [InlineData(1, 1, 0, 0)]
    [InlineData(-1, 1, 0, 0)]
    [InlineData(1, -1, 0, 0)]
    [InlineData(-1, -1, 0, 0)]
    [InlineData(0, 0, 0, 0)]
    [InlineData(2, 0, 1, 0)]
    [InlineData(-2, 0, -1, 0)]
    [InlineData(0, 2, 0, 1)]
    [InlineData(0, -2, 0, -1)]
    [InlineData(-2, -2, -1, -1)]
    [InlineData(2, 2, 1, 1)]
    [InlineData(-2, 2, -1, 1)]
    [InlineData(2, -2, 1, -1)]
    [InlineData(-1, -2, -1, -1)]
    [InlineData(1, 2, 1, 1)]
    [InlineData(-1, 2, -1, 1)]
    [InlineData(1, -2, 1, -1)]
    public void DirectionTest(int x, int y, int ex, int ey)
    {
        var diff = new Point(x, y);
        var result = diff.Magnitude() > 1 ? diff.Direction() : new Point(0, 0);
        Assert.Equal(new Point(ex, ey), result);
    }

    public readonly record struct Point(int X, int Y)
    {
        public static Point operator +(Point a, Point b) => new (a.X + b.X, a.Y + b.Y);
        public static Point operator -(Point a, Point b) => new (a.X - b.X, a.Y - b.Y);

        public int Magnitude() => Math.Max(Math.Abs(X), Math.Abs(Y));

        public Point Direction() =>
            (X, Y) switch
            {
                { X: < 0, Y: < 0 } => new(-1, -1),
                { X: < 0, Y: > 0 } => new(-1, 1),
                { X: < 0, Y: 0 } => new(-1, 0),
                { X: > 0, Y: < 0 } => new(1, -1),
                { X: > 0, Y: > 0 } => new(1, 1),
                { X: > 0, Y: 0 } => new(1, 0),
                { X: 0, Y: < 0 } => new(0, -1),
                { X: 0, Y: > 0 } => new(0, 1),
                { X: 0, Y: 0 } => new (0, 0)
            };

        public static Point FromDirection(string direction) =>
            direction switch
            {
                "R" => new Point(1, 0),
                "L" => new Point(-1, 0),
                "U" => new Point(0, 1),
                "D" => new Point(0, -1),
                _ => throw new ArgumentException()
            };
    }
}
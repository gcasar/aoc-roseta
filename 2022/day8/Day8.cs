using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.utils;
using Xunit;

namespace AdventOfCode._2022.day8;

public class Day8
{
    [Theory]
    [FileLineTest("2022/day8/example1.in", 21)]
    [FileLineTest("2022/day8/input1.in", 1733)]
    public void Star1(LineEnumerable lines, int expected)
    {
        var forest = Matrix.FromCharStream(lines);
        var transposed = forest.Transpose();

        int visibleTrees = 0;
        for (int i = 0; i < forest.Height; i++)
        {
            for (int j = 0; j < forest.Width; j++)
            {
                var tree = forest.Data[i][j];
                var left = IsVisible(tree, forest.Data[i][..j]);
                var up = IsVisible(tree, transposed.Data[j][..i]);
                var right = IsVisible(tree, forest.Data[i][(j+1)..].Reverse());
                var down = IsVisible(tree,transposed.Data[j][(i+1)..].Reverse());
                if (left || up || right || down)
                {
                    visibleTrees++;
                }
            }
        }
        
        Assert.Equal(expected, visibleTrees);
    }
    
    [Theory]
    [FileLineTest("2022/day8/example1.in", 8)]
    [FileLineTest("2022/day8/input1.in", 284648)]
    public void Star2(LineEnumerable lines, int expected)
    {
        var forest = Matrix.FromCharStream(lines);
        var transposed = forest.Transpose();

        int bestSpot = 0;
        for (int i = 1; i < forest.Height - 1; i++)
        {
            for (int j = 1; j < forest.Width - 1; j++)
            {
                var tree = forest.Data[i][j];
                var left = ScenicView(tree, forest.Data[i][1..j].Reverse());
                var up = ScenicView(tree, transposed.Data[j][1..i].Reverse());
                var right = ScenicView(tree, forest.Data[i][(j+1)..^1]);
                var down = ScenicView(tree,transposed.Data[j][(i+1)..^1]);
                var score = left * up * right * down;
                bestSpot = Math.Max(score, bestSpot);
            }
        }
        
        Assert.Equal(expected, bestSpot);
    }

    public bool IsVisible(int height, IEnumerable<int> others)
    {
        return others.All(it => it < height);
    }

    public int ScenicView(int height, IEnumerable<int> others)
    {
        return others.TakeWhile(it => it < height).Count() + 1;
    }

    [Theory]
    [FileLineTest("2022/day8/example1.in", 5 * 5)]
    public void MatrixFromStreamTest(LineEnumerable lines, int expected)
    {
        var matrix = Matrix.FromCharStream(lines);
        Assert.Equal(expected, matrix.Width * matrix.Height);
    }

    [Fact]
    public void MatrixTransposeTest()
    {
        var source = new Matrix(new[] { new[] { 1, 2, 3 }, new[] { 4, 5, 6 } }, 3, 2);
        var transposed = source.Transpose();
        var identity = transposed.Transpose();
        Assert.Equal(source.Data, identity.Data);
    }
    
    public readonly record struct Matrix(int[][] Data, int Width, int Height)
    {
        public static Matrix FromCharStream(IEnumerable<string> lines)
        {
            int[][] data = lines
                .Select(line => line.Trim().Select(ch => (int)(ch-'0')).ToArray())
                .ToArray();
            return new(data, data[0].Length, data.Length);
        }

        public Matrix Transpose()
        {
            int[][] result = new int[Width][];

            for (int i = 0; i < Width; i++)
            {
                result[i] = new int[Height];
                for (int j = 0; j < Height; j++)
                {
                    result[i][j] = Data[j][i];
                }
            }

            return new(result, Height, Width);
        }

        public Matrix ElementMap(Func<int, int> mapper)
        {
            int[][] result = new int[Height][];

            for (int i = 0; i < Height; i++)
            {
                result[i] = new int[Width];
                for (int j = 0; j < Width; j++)
                {
                    result[i][j] = mapper.Invoke(Data[i][j]);
                }
            }

            return new(result, Width, Height);
        }

        public override string ToString()
        {
            return string.Join("\n", Data.Select(it => string.Join(",", it)));
        }
    }
}
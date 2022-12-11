using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.utils;

public class LineEnumerable : IEnumerable<string>
{
    private readonly string _filename;

    public LineEnumerable(string filename)
    {
        _filename = filename;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string> GetEnumerator()
    {
        var path = Path.GetRelativePath(Directory.GetCurrentDirectory(), _filename);

        if (!File.Exists(path))
        {
            throw new ArgumentException($"Could not find file at path: {path}");
        }

        return File.ReadLines(path).GetEnumerator();
    }

    public string ReadAll() => string.Join("\n", this);

    public override string ToString() => $"Lines from \"{_filename}\"";
}
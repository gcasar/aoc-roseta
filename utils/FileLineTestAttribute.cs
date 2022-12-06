using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace AdventOfCode.utils;

public class FileLineTestAttribute : DataAttribute
{
    private readonly string _filename;

    private readonly object _expectedResult;
    
    public FileLineTestAttribute(string filename, object expectedResult)
    {
        _filename = filename;
        _expectedResult = expectedResult;
    }

    /// <inheritDoc />
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        return new List<object[]>
        {
            new object[] { new LineEnumerable(_filename), _expectedResult }
        };
    }
}
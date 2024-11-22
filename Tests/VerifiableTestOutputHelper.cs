using System.Text;
using Xunit.Abstractions;

namespace Tests;

internal class VerifiableTestOutputHelper(ITestOutputHelper output) : ITestOutputHelper
{
    private readonly StringBuilder _stringBuilder = new();

    public void WriteLine(string message)
    {
        _stringBuilder.AppendLine(message);
        output.WriteLine(message);
    }

    public void WriteLine(string format, params object[] args)
    {
        _stringBuilder.AppendLine(string.Format(format, args));
        output.WriteLine(format, args);
    }

    public string GetOutput()
    {
        return _stringBuilder.ToString().Trim();
    }
}
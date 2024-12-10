using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;

namespace Benchmarks;

public class BenchmarkConfiguration : ManualConfig
{
    public BenchmarkConfiguration()
    {
        AddLogger(ConsoleLogger.Default);
        AddExporter(DefaultExporters.Html);
        AddColumnProvider(DefaultColumnProviders.Instance);
        Options = ConfigOptions.DisableLogFile;
    }
}
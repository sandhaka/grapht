using Benchmarks.PerfCase;
using BenchmarkDotNet.Running;
using Benchmarks;

var configuration = new BenchmarkConfiguration();

BenchmarkRunner.Run<DijkstraPathFinding>(configuration);
BenchmarkRunner.Run<BacktrackingPathFinding>(configuration);
using Benchmarks.PerfCase;
using BenchmarkDotNet.Running;
using Benchmarks;

BenchmarkRunner.Run<DijkstraPathFinding>(new BenchmarkConfiguration());
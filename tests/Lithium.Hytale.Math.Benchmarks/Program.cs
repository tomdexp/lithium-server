using BenchmarkDotNet.Running;
using Lithium.Hytale.Math.Benchmarks;

BenchmarkSwitcher.FromAssembly(typeof(Vec3fBenchmarks).Assembly).Run(args);

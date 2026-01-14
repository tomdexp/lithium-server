using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using Lithium.Hytale.Math;

namespace Lithium.Hytale.Math.Benchmarks;

/// <summary>
/// Benchmarks comparing SIMD-optimized Vec3f operations vs naive scalar implementations.
/// </summary>
[MemoryDiagnoser]
[DisassemblyDiagnoser(maxDepth: 2)]
public class Vec3fBenchmarks
{
    private Vec3f _a;
    private Vec3f _b;
    private Vec3f[] _vectors = null!;
    private const int BatchSize = 1000;

    [GlobalSetup]
    public void Setup()
    {
        _a = new Vec3f(1.5f, 2.5f, 3.5f);
        _b = new Vec3f(4.5f, 5.5f, 6.5f);

        _vectors = new Vec3f[BatchSize];
        var random = new Random(42);
        for (int i = 0; i < BatchSize; i++)
        {
            _vectors[i] = new Vec3f(
                (float)random.NextDouble() * 100,
                (float)random.NextDouble() * 100,
                (float)random.NextDouble() * 100);
        }
    }

    // --- Single Operation Benchmarks ---

    [Benchmark(Baseline = true)]
    public Vec3f Add_SIMD() => _a + _b;

    [Benchmark]
    public Vec3f Add_Naive() => AddNaive(_a, _b);

    [Benchmark]
    public float Dot_SIMD() => Vec3f.Dot(_a, _b);

    [Benchmark]
    public float Dot_Naive() => DotNaive(_a, _b);

    [Benchmark]
    public Vec3f Cross_SIMD() => Vec3f.Cross(_a, _b);

    [Benchmark]
    public Vec3f Cross_Naive() => CrossNaive(_a, _b);

    [Benchmark]
    public float Length_SIMD() => _a.Length();

    [Benchmark]
    public float Length_Naive() => LengthNaive(_a);

    [Benchmark]
    public Vec3f Normalize_SIMD() => _a.Normalized();

    [Benchmark]
    public Vec3f Normalize_Naive() => NormalizeNaive(_a);

    // --- Batch Operation Benchmarks ---

    [Benchmark]
    public float BatchDot_SIMD()
    {
        float sum = 0;
        for (int i = 0; i < _vectors.Length - 1; i++)
        {
            sum += Vec3f.Dot(_vectors[i], _vectors[i + 1]);
        }
        return sum;
    }

    [Benchmark]
    public float BatchDot_Naive()
    {
        float sum = 0;
        for (int i = 0; i < _vectors.Length - 1; i++)
        {
            sum += DotNaive(_vectors[i], _vectors[i + 1]);
        }
        return sum;
    }

    [Benchmark]
    public Vec3f BatchAdd_SIMD()
    {
        var result = Vec3f.Zero;
        foreach (var v in _vectors)
        {
            result = result + v;
        }
        return result;
    }

    [Benchmark]
    public Vec3f BatchAdd_Naive()
    {
        var result = Vec3f.Zero;
        foreach (var v in _vectors)
        {
            result = AddNaive(result, v);
        }
        return result;
    }

    // --- Naive (Scalar) Implementations ---

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Vec3f AddNaive(Vec3f a, Vec3f b)
        => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static float DotNaive(Vec3f a, Vec3f b)
        => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Vec3f CrossNaive(Vec3f a, Vec3f b)
        => new(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static float LengthNaive(Vec3f v)
        => MathF.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Vec3f NormalizeNaive(Vec3f v)
    {
        float len = LengthNaive(v);
        return new Vec3f(v.X / len, v.Y / len, v.Z / len);
    }
}

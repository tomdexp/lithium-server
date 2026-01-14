using System.Buffers.Binary;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using Lithium.Hytale.Math;

namespace Lithium.Hytale.Math.Benchmarks;

/// <summary>
/// Benchmarks comparing serialization methods: MemoryMarshal vs BinaryPrimitives.
/// </summary>
[MemoryDiagnoser]
public class SerializationBenchmarks
{
    private Vec3f _vec3f;
    private Vec4f _vec4f;
    private Quatf _quatf;
    private Mat4f _mat4f;
    private byte[] _buffer = null!;
    private byte[] _vec3fData = null!;
    private byte[] _vec4fData = null!;
    private byte[] _mat4fData = null!;

    [GlobalSetup]
    public void Setup()
    {
        _vec3f = new Vec3f(1.5f, 2.5f, 3.5f);
        _vec4f = new Vec4f(1.5f, 2.5f, 3.5f, 4.5f);
        _quatf = new Quatf(0.1f, 0.2f, 0.3f, 0.9f);
        _mat4f = Mat4f.Identity;
        _buffer = new byte[64];

        // Pre-serialize for deserialization tests
        _vec3fData = new byte[12];
        _vec3f.Serialize(_vec3fData);

        _vec4fData = new byte[16];
        _vec4f.Serialize(_vec4fData);

        _mat4fData = new byte[64];
        _mat4f.Serialize(_mat4fData);
    }

    // --- Vec3f Serialization ---

    [Benchmark(Baseline = true)]
    public void Vec3f_Serialize_MemoryMarshal()
    {
        _vec3f.Serialize(_buffer);
    }

    [Benchmark]
    public void Vec3f_Serialize_BinaryPrimitives()
    {
        BinaryPrimitives.WriteSingleLittleEndian(_buffer.AsSpan(0), _vec3f.X);
        BinaryPrimitives.WriteSingleLittleEndian(_buffer.AsSpan(4), _vec3f.Y);
        BinaryPrimitives.WriteSingleLittleEndian(_buffer.AsSpan(8), _vec3f.Z);
    }

    [Benchmark]
    public Vec3f Vec3f_Deserialize_MemoryMarshal()
    {
        return Vec3f.Deserialize(_vec3fData);
    }

    [Benchmark]
    public Vec3f Vec3f_Deserialize_BinaryPrimitives()
    {
        return new Vec3f(
            BinaryPrimitives.ReadSingleLittleEndian(_vec3fData.AsSpan(0)),
            BinaryPrimitives.ReadSingleLittleEndian(_vec3fData.AsSpan(4)),
            BinaryPrimitives.ReadSingleLittleEndian(_vec3fData.AsSpan(8)));
    }

    // --- Mat4f Serialization ---

    [Benchmark]
    public void Mat4f_Serialize_MemoryMarshal()
    {
        _mat4f.Serialize(_buffer);
    }

    [Benchmark]
    public void Mat4f_Serialize_BinaryPrimitives()
    {
        var span = _buffer.AsSpan();
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(0), _mat4f.M11);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(4), _mat4f.M12);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(8), _mat4f.M13);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(12), _mat4f.M14);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(16), _mat4f.M21);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(20), _mat4f.M22);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(24), _mat4f.M23);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(28), _mat4f.M24);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(32), _mat4f.M31);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(36), _mat4f.M32);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(40), _mat4f.M33);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(44), _mat4f.M34);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(48), _mat4f.M41);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(52), _mat4f.M42);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(56), _mat4f.M43);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(60), _mat4f.M44);
    }

    [Benchmark]
    public Mat4f Mat4f_Deserialize_MemoryMarshal()
    {
        return Mat4f.Deserialize(_mat4fData);
    }

    [Benchmark]
    public Mat4f Mat4f_Deserialize_BinaryPrimitives()
    {
        var span = _mat4fData.AsSpan();
        return new Mat4f(
            BinaryPrimitives.ReadSingleLittleEndian(span.Slice(0)),
            BinaryPrimitives.ReadSingleLittleEndian(span.Slice(4)),
            BinaryPrimitives.ReadSingleLittleEndian(span.Slice(8)),
            BinaryPrimitives.ReadSingleLittleEndian(span.Slice(12)),
            BinaryPrimitives.ReadSingleLittleEndian(span.Slice(16)),
            BinaryPrimitives.ReadSingleLittleEndian(span.Slice(20)),
            BinaryPrimitives.ReadSingleLittleEndian(span.Slice(24)),
            BinaryPrimitives.ReadSingleLittleEndian(span.Slice(28)),
            BinaryPrimitives.ReadSingleLittleEndian(span.Slice(32)),
            BinaryPrimitives.ReadSingleLittleEndian(span.Slice(36)),
            BinaryPrimitives.ReadSingleLittleEndian(span.Slice(40)),
            BinaryPrimitives.ReadSingleLittleEndian(span.Slice(44)),
            BinaryPrimitives.ReadSingleLittleEndian(span.Slice(48)),
            BinaryPrimitives.ReadSingleLittleEndian(span.Slice(52)),
            BinaryPrimitives.ReadSingleLittleEndian(span.Slice(56)),
            BinaryPrimitives.ReadSingleLittleEndian(span.Slice(60)));
    }
}

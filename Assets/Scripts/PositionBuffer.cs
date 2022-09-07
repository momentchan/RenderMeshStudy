using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PositionBuffer : IDisposable {
    public NativeArray<Vector3> Positions => arrays.p;
    public NativeArray<Matrix4x4> Matrices => arrays.m;

    private (NativeArray<Vector3> p, NativeArray<Matrix4x4> m) arrays;
    private (int x, int y) dims;

    public PositionBuffer(int xCount, int yCount) {
        dims = (xCount, yCount);
        arrays = (new NativeArray<Vector3>(dims.x * dims.y, Allocator.Persistent),
            new NativeArray<Matrix4x4>(dims.x * dims.y, Allocator.Persistent));
    }

    public void Dispose() {
        if (arrays.p.IsCreated) arrays.p.Dispose();
        if (arrays.m.IsCreated) arrays.m.Dispose();
    }
    public void Update(float t) {
        var offs = 0;

        for (var i = 0; i < dims.x; i++) {
            var x = i - dims.x * 0.5f + 0.5f;

            for(var j = 0; j < dims.y; j++) {
                var z = j - dims.y * 0.5f + 0.5f;
                var y = math.sin(math.sqrt(x * x + z * z) * 0.4f - t);
                var p = math.float3(x, y, z);
                arrays.p[offs] = p;
                arrays.m[offs] = float4x4.Translate(p);
                offs++;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

public class RenderMeshTest : MonoBehaviour {
    [SerializeField] protected Mesh mesh;
    [SerializeField] protected Material mat;
    [SerializeField] protected Vector2Int counts = new Vector2Int(10, 10);

    protected PositionBuffer buffer;

    void Start() {
        buffer = new PositionBuffer(counts.x, counts.y);
    }

    private void OnDestroy() {
        buffer.Dispose();
    }

    private void Update() {
        buffer.Update(Time.time);
        var matrices = buffer.Matrices;
        var rparams = new RenderParams(mat) {
            receiveShadows = true,
            shadowCastingMode = ShadowCastingMode.On
        };

        Profiler.BeginSample("Mass Mesh Update");
        Render(matrices, rparams);
        Profiler.EndSample();
    }

    protected virtual void Render(NativeArray<Matrix4x4> matrices, RenderParams rparams) {
        for (var i = 0; i < matrices.Length; i++)
            Graphics.RenderMesh(rparams, mesh, 0, matrices[i]);

    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Profiling;

public class StickShowEffect : MonoBehaviour {
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material mat;
    [SerializeField] private Audience audience = Audience.Default();

    private NativeArray<Matrix4x4> matrices;
    private NativeArray<Color> colors;
    private GraphicsBuffer colorBuffer;
    private MaterialPropertyBlock matProps;

    void Start() {
        matrices = new NativeArray<Matrix4x4>
            (audience.TotalSeatCount, Allocator.Persistent,
            NativeArrayOptions.UninitializedMemory);

        colors = new NativeArray<Color>
            (audience.TotalSeatCount, Allocator.Persistent,
            NativeArrayOptions.UninitializedMemory);

        colorBuffer = new GraphicsBuffer(
            GraphicsBuffer.Target.Structured,
            audience.TotalSeatCount, sizeof(float) * 4);

        matProps = new MaterialPropertyBlock();
    }

    private void OnDestroy() {
        matrices.Dispose();
        colors.Dispose();
        colorBuffer.Dispose();
    }

    void Update() {

        Profiler.BeginSample("Stick Update");
        var job = new AudienceAnimationJob() {
            config = audience,
            xform = transform.localToWorldMatrix,
            time = Time.time,
            matrices = matrices,
            colors = colors
        };
        job.Schedule(audience.TotalSeatCount, 64).Complete();
        Profiler.EndSample();

        colorBuffer.SetData(colors);
        mat.SetBuffer("_InstanceColorBuffer", colorBuffer);

        var rparams = new RenderParams(mat) { matProps = matProps };
        var (i, step) = (0, audience.BlockSeatCount);

        for (var sx = 0; sx < audience.blockCount.x; sx++) {
            for (var sy = 0; sy < audience.blockCount.y; sy++, i += step) {
                matProps.SetInt("_InstanceIDOffset", i);
                Graphics.RenderMeshInstanced
                    (rparams, mesh, 0, matrices, step, i);
            }
        }
    }
}

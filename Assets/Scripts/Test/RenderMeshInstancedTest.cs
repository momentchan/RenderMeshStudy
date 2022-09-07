using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class RenderMeshInstancedTest : RenderMeshTest {
    protected override void Render(NativeArray<Matrix4x4> matrices, RenderParams rparams) {
        for (var offs = 0; offs < matrices.Length; offs += 1023) {
            var count = Mathf.Min(1023, matrices.Length - offs);
            Graphics.RenderMeshInstanced(rparams, mesh, 0, matrices, count, offs);
        }
    }
}

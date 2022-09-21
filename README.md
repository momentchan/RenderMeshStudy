# RenderMeshStudy

## Render Mesh
Create render parameters for target material:
```
var rparams = new RenderParams(mat) { ... };
```
Render single mesh
```
Graphics.RenderMesh(rparams, mesh, 0, matrices[i]);
```

Render multiple meshes at the same time
```
Graphics.RenderMeshInstanced(rparams, mesh, 0, matrices, count, offs);
```

Note. remember enabling GPU instancing in material

## Job system
Define job structure
```
struct TestJob : IJobParallelFor {
    public float time;
    public void Execute(int i) {
      ...
    }
}
```
Create job object and execute by schedule/complete
```
var job = new TestJob() { time = Time.time };
job.Schedule(audience.TotalSeatCount, 64).Complete();
```

Note. using Unity.Mathematics for calculation

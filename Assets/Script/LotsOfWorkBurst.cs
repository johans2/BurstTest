using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class LotsOfWorkBurst : MonoBehaviour
{
    [BurstCompile(CompileSynchronously = false, FloatMode = FloatMode.Fast)]
    private struct LotsOfWorkJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Vector4> input;

        [WriteOnly]
        public NativeArray<Vector4> output;
        
        public void Execute(int index)
        {
            Vector4 newVector = input[index] + Vector4.one;
            newVector *= 2;
            newVector /= 5;

            output[index] = newVector;
        }
    }

    public int arraySize = 10000000;
    private Vector4[] vectors;
    private Vector4[] newVectors;

    private int jobBatchCount = 8;
    private LotsOfWorkJob job;
    
    private void Awake()
    {
        vectors = new Vector4[arraySize];
        newVectors = new Vector4[arraySize];

        job = new LotsOfWorkJob
        {
            input = new NativeArray<Vector4>(vectors, Allocator.Persistent),
            output = new NativeArray<Vector4>(newVectors, Allocator.Persistent)
        };
    }

    private void Update()
    {
        UnityEngine.Profiling.Profiler.BeginSample(" >>> Lots of Work Burst <<< ");

        JobHandle jobHandle = job.Schedule(vectors.Length, jobBatchCount);

        jobHandle.Complete();

        UnityEngine.Profiling.Profiler.EndSample();
    }

    private void OnDestroy()
    {
        job.input.Dispose();
        job.output.Dispose();
    }
}
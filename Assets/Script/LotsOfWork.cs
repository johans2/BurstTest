using UnityEngine;

public class LotsOfWork : MonoBehaviour
{
    public int arraySize = 1000000;

    private Vector4[] vectors;
    private Vector4[] calculatedVectors;

    private void Awake()
    {
        vectors = new Vector4[arraySize];
        calculatedVectors = new Vector4[arraySize];
    }

    private void Update()
    {
        Calculate();
    }

    public void Calculate()
    {
        UnityEngine.Profiling.Profiler.BeginSample(" >>> Lots of Work <<< ");

        for (int i = 0; i < vectors.Length; i++)
        {
            Vector4 newVector = vectors[i] + Vector4.one;
            newVector *= 2;
            newVector /= 5;
            calculatedVectors[i] = newVector;
        }

        UnityEngine.Profiling.Profiler.EndSample();

        Debug.Log($"Value = {calculatedVectors[10]}");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVectorGenerator: MonoBehaviour
{
    // You would seed this with a consistent value if you want reproducible results
    // or with something like System.DateTime.Now.Millisecond for more varied results.
    private int seed;

    public RandomVectorGenerator(int seed)
    {
        this.seed = seed;
    }

    // Call this method to get a new random vector
    public Vector3 GetRandomVector(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
    {
        // Initialize the random state with the seed to ensure reproducibility
        Random.InitState(seed);

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        float z = Random.Range(minZ, maxZ);

        // Optional: Increase the seed to ensure the next call gives a different result
        seed++;

        return new Vector3(x, y, z);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root: MonoBehaviour
{
    public int seed;
    public int seed1;
    public int seed2;
    public LineRenderer spline; // Assign the LineRenderer in the inspector
    public double subrootLength;
    public double subrootLengthDecreaseFactor;

    private Vector3 currentSubrootStart;
    private double currentSubrootLength;
    private Vector3 currentSubrootEnd;
    public int depth;
    public float slowness;
    public float thinness;
    public float acceleration;
    public float modelWidth;
    public bool isLeaf;
    public bool forceLeaf;
    public float minSizeVariation, maxSizeVariation;
    public float minRootDisplacement, maxRootDisplacement;
    public float sectionLength;
    private System.Random rand;
    public Vector3 Direction;

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void CreateDecorations()
    {
        int numPoints = spline.positionCount;
        double length = subrootLength;

        for (int i = 0; i < numPoints; i++)
        {
            if (i == 0 || length <= 0)
            {
                length = subrootLength;
                currentSubrootStart = spline.GetPosition(i);
                currentSubrootLength = length;
            }
            else
            {
                length -= subrootLengthDecreaseFactor;
                currentSubrootEnd = spline.GetPosition(i);
                UpdateSubroot(); // Update the subroot with the new length
                SetUpSplineScale();
                RenderSpline();
            }
        }
    }

    private void UpdateSubroot()
    {
        // Here you would handle the logic to update the subroot based on the current end position and length.
        // For example, setting new points on the LineRenderer or manipulating the spline in other ways.

        // Since this method is quite specific to your project and how you manage splines, you might have to adjust the details.
    }

    private void SetUpSplineScale()
    {
        int numPoints = spline.positionCount;

        AnimationCurve widthCurve = new AnimationCurve();

        for (int i = 0; i < numPoints - 1; i++)
        {
            int depthMinus = depth - numPoints;
            float DMPlus = depthMinus + i;
            float thinStuff = thinness + DMPlus;
            float depthStuff = Mathf.Pow(thinStuff, acceleration);
            float powerSlow = (float)slowness / depthStuff;
            float newModelWidth = modelWidth / 100;
            float clampResult = Mathf.Clamp(powerSlow, 0.1f, 1);
            float clampModelWidth = clampResult * newModelWidth;
            //Vector3 splineScale = new Vector3(clampModelWidth, clampModelWidth, clampModelWidth);
            // Add a key to the curve at the point index, with a value based on scaleValue
            // Note: Unity's width curve uses a normalized range from 0 to 1 for the time parameter
            float time = (float)i / (numPoints - 1); // Normalize index to [0, 1]
            widthCurve.AddKey(time, clampModelWidth); 
        }

        if(isLeaf || forceLeaf)
        {
            widthCurve.AddKey(1, 0);
        }

        // Apply the width curve to the spline
        spline.widthCurve = widthCurve;
    }

    private void RenderSpline()
    {
        int numPoints = spline.positionCount;

        if (numPoints > 1)
        {
            rand = new System.Random(depth);
            float randFloat = RandomFloatInRange(minSizeVariation, minSizeVariation);
            Vector3 randomVector = RandomVector(minRootDisplacement, maxRootDisplacement);
            float splineLength = GetSplineLength();
            int numSections = Mathf.CeilToInt(splineLength / sectionLength);

            for (int i = 0; i < numSections; i++)
            {
                float sizeVariation = RandomRange(minSizeVariation, maxSizeVariation);
                float start = i * sectionLength;
                float end = (i + 1) * sectionLength;
                Vector3 lastRandomVector = randomVector;
                Vector3 randomVector2 = RandomVector(minRootDisplacement, maxRootDisplacement);
                SetForwardAxis(Vector3.forward); 
                Vector3 displacement = RandomVector(minRootDisplacement, maxRootDisplacement);
                ApplyDisplacementAndSize(i, displacement, sizeVariation);
            }
        }
    }

    private float RandomRange(float min, float max)
    {
        return (float)rand.NextDouble() * (max - min) + min;
    }

    private Vector3 RandomVector(float min, float max)
    {
        float x = RandomRange(min, max);
        float y = RandomRange(min, max);
        float z = RandomRange(min, max);
        return new Vector3(x, y, z);
    }

    private void ApplyDisplacementAndSize(int sectionIndex, Vector3 displacement, float size)
    {
        // Implementation detail: Apply displacement and size to the specific section of the spline
    }

    private float GetSplineLength()
    {
        float totalLength = 0f;
        if (spline == null || spline.positionCount < 2)
        {
            return totalLength;
        }

        for (int i = 1; i < spline.positionCount; i++)
        {
            Vector3 startPoint = spline.GetPosition(i - 1);
            Vector3 endPoint = spline.GetPosition(i);

            float segmentLength = Vector3.Distance(startPoint, endPoint);

            totalLength += segmentLength;
        }

        return totalLength;
    }

    private void SetForwardAxis(Vector3 forward)
    {
        gameObject.transform.LookAt(forward);
    }

    // Method to reset the random stream to its initial state
    public void Reset(int seed)
    {
        rand = new System.Random(seed);
    }

    // Method to set a new seed for the random stream
    public void SetSeed(int newSeed)
    {
        rand = new System.Random(newSeed);
    }

    // Method to get a random float in a specified range
    public float RandomFloatInRange(float min, float max)
    {
        return (float)(rand.NextDouble() * (max - min) + min);
    }
}

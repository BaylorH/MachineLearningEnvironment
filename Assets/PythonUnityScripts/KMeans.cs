using System;
using System.Collections.Generic;
using UnityEngine;
using static KMeansPenguinDataVisualizer;

public class KMeans
{
    public int numClusters;
    public int maxIterations;
    public List<PenguinDataPoint> penguinData;
    public Vector3[] centroids;
    public int[] labels;

    // for keeping track of state across recalculations
    private bool initialized;
    private bool converged;
    private int currentIteration;

    public KMeans(int numClusters, int maxIterations, List<PenguinDataPoint> penguinData)
    {
        this.numClusters = numClusters;
        this.maxIterations = maxIterations;
        this.penguinData = penguinData;
        centroids = new Vector3[numClusters];
        labels = new int[penguinData.Count];
        initialized = false;
        converged = false;
        currentIteration = 0;
    }

    // initialize centroids (randomized)
    public void InitializeCentroids()
    {
        HashSet<int> selected = new HashSet<int>();
        System.Random random = new System.Random();

        for (int i = 0; i < numClusters; i++)
        {
            int index;
            do
            {
                index = random.Next(penguinData.Count);
            } while (selected.Contains(index));

            selected.Add(index);
            centroids[i] = penguinData[index].ToVector3(); // set the initial centroid
        }

        initialized = true;
        converged = false;
        currentIteration = 0;
    }

    // perform one iteration of the K-Means algorithm
    public bool PerformIteration()
    {
        if (!initialized || converged)
        {
            return false; // don't perform if not initialized or already converged
        }

        bool centroidsChanged = false;

        // step 1: assign each point to nearest centroid
        for (int i = 0; i < penguinData.Count; i++)
        {
            int nearestCentroid = GetNearestCentroid(penguinData[i].ToVector3());
            if (nearestCentroid != labels[i])
            {
                labels[i] = nearestCentroid;
                centroidsChanged = true;
            }
        }

        // step 2: recompute centroids
        UpdateCentroids();

        // step 3: check for convergence
        if (!centroidsChanged || currentIteration >= maxIterations)
        {
            converged = true; // stop if centroids have not changed or max iterations reached
        }

        currentIteration++;
        return centroidsChanged;
    }

    private int GetNearestCentroid(Vector3 point)
    {
        float minDistance = float.MaxValue;
        int nearestCentroid = 0;

        for (int i = 0; i < centroids.Length; i++)
        {
            float distance = Vector3.Distance(point, centroids[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCentroid = i;
            }
        }

        return nearestCentroid;
    }

    private void UpdateCentroids()
    {
        Vector3[] newCentroids = new Vector3[numClusters];
        int[] count = new int[numClusters];

        // reset centroids
        for (int i = 0; i < numClusters; i++)
        {
            newCentroids[i] = Vector3.zero;
            count[i] = 0;
        }

        // sum up all points in each cluster
        for (int i = 0; i < penguinData.Count; i++)
        {
            newCentroids[labels[i]] += penguinData[i].ToVector3();
            count[labels[i]]++;
        }

        // recompute centroids by taking the average
        for (int i = 0; i < numClusters; i++)
        {
            if (count[i] > 0)
            {
                centroids[i] = newCentroids[i] / count[i];
            }
        }
    }

    public Vector3[] GetCentroids() => centroids;
    public int[] GetLabels() => labels;
    public bool IsConverged() => converged;
}

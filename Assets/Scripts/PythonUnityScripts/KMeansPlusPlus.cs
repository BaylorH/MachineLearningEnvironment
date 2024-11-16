using System;
using System.Collections.Generic;
using UnityEngine;
using static KMeansPlusPlusPenguinDataVisualizer;

public class KMeansPlusPlus
{
    public int numClusters;
    public int maxIterations;
    public List<KMeansPlusPlusPenguinDataVisualizer.PenguinDataPoint> penguinData;
    public Vector3[] centroids;
    public HashSet<int> selected;
    public int[] labels;
    private bool converged;
    private int currentIteration;

    public KMeansPlusPlus(int numClusters, int maxIterations, List<KMeansPlusPlusPenguinDataVisualizer.PenguinDataPoint> penguinData)
    {
        this.numClusters = numClusters;
        this.maxIterations = maxIterations;
        this.penguinData = penguinData;
        centroids = new Vector3[numClusters];
        selected = new HashSet<int>();
        labels = new int[penguinData.Count];
        converged = false;
        currentIteration = 0;
    }

    // initialize first centroid (chosen)
    public void SetFirstCentroid(List<Vector3> initialCentroids)
    {
        centroids[0] = initialCentroids[0];
        converged = false;
        currentIteration = 0;
    }

    // initialize first centroid (randomized)
    public void InitializeFirstCentroid()
    {
        System.Random random = new System.Random();

        // random first centroid
        int firstCentroidIndex = random.Next(penguinData.Count);
        centroids[0] = penguinData[firstCentroidIndex].ToVector3();
        selected.Add(firstCentroidIndex);
    }

    // calculate max dist
    public void InitializeRestOfCentroidsMaxDist()
    {
        for (int i = 1; i < numClusters; i++)
        {
            float maxMinDistance = -1;
            int selectedIndex = -1;

            for (int j = 0; j < penguinData.Count; j++)
            {
                if (selected.Contains(j)) continue; // skip already selected points

                Vector3 point = penguinData[j].ToVector3();
                float minDistanceToCentroids = float.MaxValue;

                // calculate the min distance of this point to any of the selected centroids
                for (int k = 0; k < i; k++)
                {
                    float distanceToCentroid = Vector3.Distance(point, centroids[k]);
                    if (distanceToCentroid < minDistanceToCentroids)
                    {
                        minDistanceToCentroids = distanceToCentroid;
                    }
                }

                // find the point that maximizes the minimum distance to any centroid
                if (minDistanceToCentroids > maxMinDistance)
                {
                    maxMinDistance = minDistanceToCentroids;
                    selectedIndex = j;
                }
            }

            // if valid index found, select this point as next centroid
            if (selectedIndex != -1)
            {
                selected.Add(selectedIndex);
                centroids[i] = penguinData[selectedIndex].ToVector3();
            }
        }

        converged = false;
        currentIteration = 0;
    }


    public bool AssignPoints()
    {
        bool centroidsChanged = false;

        // assign each point to the nearest centroid
        for (int i = 0; i < penguinData.Count; i++)
        {
            int nearestCentroid = GetNearestCentroid(penguinData[i].ToVector3());
            if (nearestCentroid != labels[i])
            {
                labels[i] = nearestCentroid;
                centroidsChanged = true;
            }
        }

        if (!centroidsChanged)
        {
            converged = true;  // set convergence to true if no centroids changed
        }

        currentIteration++;  // increase iteration count
        return centroidsChanged;
    }

    public void RecalculateCentroids()
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

    public bool CheckConvergence()
    {
        return currentIteration >= maxIterations;
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

    public Vector3[] GetCentroids() => centroids;
    public int[] GetLabels() => labels;
    public bool IsConverged()
    {
        return converged || currentIteration >= maxIterations;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DiabetesDataVisualizer;

public class KNNDiabetesPredictionClient : MonoBehaviour
{
    private float[][] trainingData;
    private string[] trainingLabels;
    private DiabetesDataPoint[] diabetesDataPoints = new DiabetesDataPoint[] {
        
    };

    private void Start()
    {
        InitializeData();
    }

    private void InitializeData()
    {
        List<float[]> dataArrayList = new List<float[]>();
        List<string> labelList = new List<string>();

        foreach (DiabetesDataVisualizer.DiabetesDataPoint point in diabetesDataPoints)
        {
            dataArrayList.Add(new float[] { point.glucose, point.blood_pressure, point.bmi, point.age });
            labelList.Add(point.prediction);
        }

        trainingData = dataArrayList.ToArray();
        trainingLabels = labelList.ToArray();
    }

    public void Predict(float[] input, Action<string> onOutputReceived, Action<Exception> fallback)
    {
        try
        {
            string prediction = KNNPredict(input, 3); // k-value = 3
            onOutputReceived(prediction);
        }
        catch (Exception ex)
        {
            fallback(ex);
        }
    }

    private string KNNPredict(float[] input, int k)
    {
        var distances = new List<(string label, float distance)>();

        for (int i = 0; i < trainingData.Length; i++)
        {
            float distance = EuclideanDistance(input, trainingData[i]);
            distances.Add((trainingLabels[i], distance));
        }

        var sorted = distances.OrderBy(d => d.distance).Take(k).ToList();
        var prediction = sorted
            .GroupBy(d => d.label)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Min(x => x.distance)) // in case of tie, take class with closer points
            .First().Key;

        return prediction;
    }

    private float EuclideanDistance(float[] a, float[] b)
    {
        float sum = 0f;
        for (int i = 0; i < a.Length; i++)
        {
            sum += Mathf.Pow(a[i] - b[i], 2);
        }
        return Mathf.Sqrt(sum);
    }
}

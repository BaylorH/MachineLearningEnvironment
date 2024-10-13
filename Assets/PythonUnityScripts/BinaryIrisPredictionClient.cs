//using System;
//using UnityEngine;

//public class PredictionClient : MonoBehaviour
//{
//    private PredictionRequester predictionRequester;

//    private void Start() => InitializeServer();

//    public void InitializeServer() {
//        predictionRequester = new PredictionRequester();
//        predictionRequester.Start();
//    }

//    public void Predict(float[] input, Action<string> onOutputReceived, Action<Exception> fallback) {
//        predictionRequester.SetOnTextReceivedListener(onOutputReceived, fallback);
//        predictionRequester.SendInput(input);
//    }

//    private void OnDestroy() {
//        predictionRequester.Stop();
//    }
//}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static KNNIrisDataVisualizer;

public class BinaryIrisPredictionClient : MonoBehaviour
{
    private float[][] trainingData;
    private string[] trainingLabels;
    private float[][] testData;
    private string[] testLabels;
    private float accuracy;

    private IrisDataPoint[] irisDataPoints = new IrisDataPoint[] {
        new IrisDataPoint { sepal_length = 5.1f, sepal_width = 3.5f, petal_length = 1.4f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.9f, sepal_width = 3.0f, petal_length = 1.4f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.7f, sepal_width = 3.2f, petal_length = 1.3f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.6f, sepal_width = 3.1f, petal_length = 1.5f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.0f, sepal_width = 3.6f, petal_length = 1.4f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.4f, sepal_width = 3.9f, petal_length = 1.7f, petal_width = 0.4f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.6f, sepal_width = 3.4f, petal_length = 1.4f, petal_width = 0.3f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.0f, sepal_width = 3.4f, petal_length = 1.5f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.4f, sepal_width = 2.9f, petal_length = 1.4f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.9f, sepal_width = 3.1f, petal_length = 1.5f, petal_width = 0.1f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.4f, sepal_width = 3.7f, petal_length = 1.5f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.8f, sepal_width = 3.4f, petal_length = 1.6f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.8f, sepal_width = 3.0f, petal_length = 1.4f, petal_width = 0.1f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.3f, sepal_width = 3.0f, petal_length = 1.1f, petal_width = 0.1f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.8f, sepal_width = 4.0f, petal_length = 1.2f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.7f, sepal_width = 4.4f, petal_length = 1.5f, petal_width = 0.4f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.4f, sepal_width = 3.9f, petal_length = 1.3f, petal_width = 0.4f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.1f, sepal_width = 3.5f, petal_length = 1.4f, petal_width = 0.3f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.7f, sepal_width = 3.8f, petal_length = 1.7f, petal_width = 0.3f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.1f, sepal_width = 3.8f, petal_length = 1.5f, petal_width = 0.3f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.4f, sepal_width = 3.4f, petal_length = 1.7f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.1f, sepal_width = 3.7f, petal_length = 1.5f, petal_width = 0.4f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.6f, sepal_width = 3.6f, petal_length = 1.0f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.1f, sepal_width = 3.3f, petal_length = 1.7f, petal_width = 0.5f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.8f, sepal_width = 3.4f, petal_length = 1.9f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.0f, sepal_width = 3.0f, petal_length = 1.6f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.0f, sepal_width = 3.4f, petal_length = 1.6f, petal_width = 0.4f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.2f, sepal_width = 3.5f, petal_length = 1.5f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.2f, sepal_width = 3.4f, petal_length = 1.4f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.7f, sepal_width = 3.2f, petal_length = 1.6f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.8f, sepal_width = 3.1f, petal_length = 1.6f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.4f, sepal_width = 3.4f, petal_length = 1.5f, petal_width = 0.4f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.2f, sepal_width = 4.1f, petal_length = 1.5f, petal_width = 0.1f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.5f, sepal_width = 4.2f, petal_length = 1.4f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.9f, sepal_width = 3.1f, petal_length = 1.5f, petal_width = 0.1f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.0f, sepal_width = 3.2f, petal_length = 1.2f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.5f, sepal_width = 3.5f, petal_length = 1.3f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.9f, sepal_width = 3.1f, petal_length = 1.5f, petal_width = 0.1f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.4f, sepal_width = 3.0f, petal_length = 1.3f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.1f, sepal_width = 3.4f, petal_length = 1.5f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.0f, sepal_width = 3.5f, petal_length = 1.3f, petal_width = 0.3f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.5f, sepal_width = 2.3f, petal_length = 1.3f, petal_width = 0.3f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.4f, sepal_width = 3.2f, petal_length = 1.3f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.0f, sepal_width = 3.5f, petal_length = 1.6f, petal_width = 0.6f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.1f, sepal_width = 3.8f, petal_length = 1.9f, petal_width = 0.4f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.8f, sepal_width = 3.0f, petal_length = 1.4f, petal_width = 0.3f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.1f, sepal_width = 3.8f, petal_length = 1.6f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 4.6f, sepal_width = 3.2f, petal_length = 1.4f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.3f, sepal_width = 3.7f, petal_length = 1.5f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 5.0f, sepal_width = 3.3f, petal_length = 1.4f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new IrisDataPoint { sepal_length = 7.0f, sepal_width = 3.2f, petal_length = 4.7f, petal_width = 1.4f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.4f, sepal_width = 3.2f, petal_length = 4.5f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.9f, sepal_width = 3.1f, petal_length = 4.9f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.5f, sepal_width = 2.3f, petal_length = 4.0f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.5f, sepal_width = 2.8f, petal_length = 4.6f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.7f, sepal_width = 2.8f, petal_length = 4.5f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.3f, sepal_width = 3.3f, petal_length = 4.7f, petal_width = 1.6f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 4.9f, sepal_width = 2.4f, petal_length = 3.3f, petal_width = 1.0f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.6f, sepal_width = 2.9f, petal_length = 4.6f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.2f, sepal_width = 2.7f, petal_length = 3.9f, petal_width = 1.4f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.0f, sepal_width = 2.0f, petal_length = 3.5f, petal_width = 1.0f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.9f, sepal_width = 3.0f, petal_length = 4.2f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.0f, sepal_width = 2.2f, petal_length = 4.0f, petal_width = 1.0f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.1f, sepal_width = 2.9f, petal_length = 4.7f, petal_width = 1.4f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.6f, sepal_width = 2.9f, petal_length = 3.6f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.7f, sepal_width = 3.1f, petal_length = 4.4f, petal_width = 1.4f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.6f, sepal_width = 3.0f, petal_length = 4.5f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.8f, sepal_width = 2.7f, petal_length = 4.1f, petal_width = 1.0f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.2f, sepal_width = 2.2f, petal_length = 4.5f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.6f, sepal_width = 2.5f, petal_length = 3.9f, petal_width = 1.1f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.9f, sepal_width = 3.2f, petal_length = 4.8f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.1f, sepal_width = 2.8f, petal_length = 4.0f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.3f, sepal_width = 2.5f, petal_length = 4.9f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.1f, sepal_width = 2.8f, petal_length = 4.7f, petal_width = 1.2f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.4f, sepal_width = 2.9f, petal_length = 4.3f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.6f, sepal_width = 3.0f, petal_length = 4.4f, petal_width = 1.4f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.8f, sepal_width = 2.8f, petal_length = 4.8f, petal_width = 1.4f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.7f, sepal_width = 3.0f, petal_length = 5.0f, petal_width = 1.7f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.0f, sepal_width = 2.9f, petal_length = 4.5f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.7f, sepal_width = 2.6f, petal_length = 3.5f, petal_width = 1.0f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.5f, sepal_width = 2.4f, petal_length = 3.8f, petal_width = 1.1f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.5f, sepal_width = 2.4f, petal_length = 3.7f, petal_width = 1.0f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.8f, sepal_width = 2.7f, petal_length = 3.9f, petal_width = 1.2f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.0f, sepal_width = 2.7f, petal_length = 5.1f, petal_width = 1.6f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.4f, sepal_width = 3.0f, petal_length = 4.5f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.0f, sepal_width = 3.4f, petal_length = 4.5f, petal_width = 1.6f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.7f, sepal_width = 3.1f, petal_length = 4.7f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.3f, sepal_width = 2.3f, petal_length = 4.4f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.6f, sepal_width = 3.0f, petal_length = 4.1f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.5f, sepal_width = 2.5f, petal_length = 4.0f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.5f, sepal_width = 2.6f, petal_length = 4.4f, petal_width = 1.2f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.1f, sepal_width = 3.0f, petal_length = 4.6f, petal_width = 1.4f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.8f, sepal_width = 2.6f, petal_length = 4.0f, petal_width = 1.2f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.0f, sepal_width = 2.3f, petal_length = 3.3f, petal_width = 1.0f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.6f, sepal_width = 2.7f, petal_length = 4.2f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.7f, sepal_width = 3.0f, petal_length = 4.2f, petal_width = 1.2f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.7f, sepal_width = 2.9f, petal_length = 4.2f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.2f, sepal_width = 2.9f, petal_length = 4.3f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.1f, sepal_width = 2.5f, petal_length = 3.0f, petal_width = 1.1f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.7f, sepal_width = 2.8f, petal_length = 4.1f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.3f, sepal_width = 3.3f, petal_length = 6.0f, petal_width = 2.5f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.8f, sepal_width = 2.7f, petal_length = 5.1f, petal_width = 1.9f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 7.1f, sepal_width = 3.0f, petal_length = 5.9f, petal_width = 2.1f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.3f, sepal_width = 2.9f, petal_length = 5.6f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.5f, sepal_width = 3.0f, petal_length = 5.8f, petal_width = 2.2f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 7.6f, sepal_width = 3.0f, petal_length = 6.6f, petal_width = 2.1f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 4.9f, sepal_width = 2.5f, petal_length = 4.5f, petal_width = 1.7f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 7.3f, sepal_width = 2.9f, petal_length = 6.3f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.7f, sepal_width = 2.5f, petal_length = 5.8f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 7.2f, sepal_width = 3.6f, petal_length = 6.1f, petal_width = 2.5f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.5f, sepal_width = 3.2f, petal_length = 5.1f, petal_width = 2.0f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.4f, sepal_width = 2.7f, petal_length = 5.3f, petal_width = 1.9f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.8f, sepal_width = 3.0f, petal_length = 5.5f, petal_width = 2.1f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.7f, sepal_width = 2.5f, petal_length = 5.0f, petal_width = 2.0f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.8f, sepal_width = 2.8f, petal_length = 5.1f, petal_width = 2.4f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.4f, sepal_width = 3.2f, petal_length = 5.3f, petal_width = 2.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.5f, sepal_width = 3.0f, petal_length = 5.5f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 7.7f, sepal_width = 3.8f, petal_length = 6.7f, petal_width = 2.2f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 7.7f, sepal_width = 2.6f, petal_length = 6.9f, petal_width = 2.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.0f, sepal_width = 2.2f, petal_length = 5.0f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.9f, sepal_width = 3.2f, petal_length = 5.7f, petal_width = 2.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.6f, sepal_width = 2.8f, petal_length = 4.9f, petal_width = 2.0f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 7.7f, sepal_width = 2.8f, petal_length = 6.7f, petal_width = 2.0f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.3f, sepal_width = 2.7f, petal_length = 4.9f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.7f, sepal_width = 3.3f, petal_length = 5.7f, petal_width = 2.1f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 7.2f, sepal_width = 3.2f, petal_length = 6.0f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.2f, sepal_width = 2.8f, petal_length = 4.8f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.1f, sepal_width = 3.0f, petal_length = 4.9f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.4f, sepal_width = 2.8f, petal_length = 5.6f, petal_width = 2.1f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 7.2f, sepal_width = 3.0f, petal_length = 5.8f, petal_width = 1.6f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 7.4f, sepal_width = 2.8f, petal_length = 6.1f, petal_width = 1.9f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 7.9f, sepal_width = 3.8f, petal_length = 6.4f, petal_width = 2.0f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.4f, sepal_width = 2.8f, petal_length = 5.6f, petal_width = 2.2f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.3f, sepal_width = 2.8f, petal_length = 5.1f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.1f, sepal_width = 2.6f, petal_length = 5.6f, petal_width = 1.4f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 7.7f, sepal_width = 3.0f, petal_length = 6.1f, petal_width = 2.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.3f, sepal_width = 3.4f, petal_length = 5.6f, petal_width = 2.4f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.4f, sepal_width = 3.1f, petal_length = 5.5f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.0f, sepal_width = 3.0f, petal_length = 4.8f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.9f, sepal_width = 3.1f, petal_length = 5.4f, petal_width = 2.1f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.7f, sepal_width = 3.1f, petal_length = 5.6f, petal_width = 2.4f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.9f, sepal_width = 3.1f, petal_length = 5.1f, petal_width = 2.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.8f, sepal_width = 2.7f, petal_length = 5.1f, petal_width = 1.9f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.8f, sepal_width = 3.2f, petal_length = 5.9f, petal_width = 2.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.7f, sepal_width = 3.3f, petal_length = 5.7f, petal_width = 2.5f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.7f, sepal_width = 3.0f, petal_length = 5.2f, petal_width = 2.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.3f, sepal_width = 2.5f, petal_length = 5.0f, petal_width = 1.9f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.5f, sepal_width = 3.0f, petal_length = 5.2f, petal_width = 2.0f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 6.2f, sepal_width = 3.4f, petal_length = 5.4f, petal_width = 2.3f, prediction = "Iris-nonsetosa" },
        new IrisDataPoint { sepal_length = 5.9f, sepal_width = 3.0f, petal_length = 5.1f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
    };

    private NeuralNetwork net;

    private void Start()
    {
        InitializeData();

        // define neural network (same structure as in python: 2 layers)
        net = new NeuralNetwork(inputSize: 4, hiddenSize: 5, outputSize: 2);

        // train neural network
        TrainNetwork();
    }

    private void InitializeData()
    {
        // convert IrisDataPoint array into training data and labels
        List<float[]> dataArrayList = new List<float[]>();
        List<string> labelList = new List<string>();

        foreach (IrisDataPoint point in irisDataPoints)
        {
            dataArrayList.Add(new float[] { point.sepal_length, point.sepal_width, point.petal_length, point.petal_width });
            labelList.Add(point.prediction);
        }

        trainingData = dataArrayList.ToArray();
        trainingLabels = labelList.ToArray();

        // shuffle data and labels
        System.Random rand = new System.Random();
        var shuffledData = dataArrayList.Zip(labelList, (data, label) => new { data, label })
                                        .OrderBy(x => rand.Next())
                                        .ToArray();

        // split into training and testing sets (80% training, 20% testing)
        int splitIndex = (int)(0.8 * shuffledData.Length);

        trainingData = shuffledData.Take(splitIndex).Select(x => x.data).ToArray();
        trainingLabels = shuffledData.Take(splitIndex).Select(x => x.label).ToArray();
        testData = shuffledData.Skip(splitIndex).Select(x => x.data).ToArray();
        testLabels = shuffledData.Skip(splitIndex).Select(x => x.label).ToArray();

    }

    // train the network using a simple gradient descent
    private void TrainNetwork()
    {
        float learningRate = 0.01f;
        int epochs = 20;

        // binary encoding for labels ("Iris-setosa" -> 0, "Iris-nonsetosa" -> 1)
        int[] trainLabels = new int[trainingLabels.Length];
        for (int i = 0; i < trainingLabels.Length; i++)
        {
            trainLabels[i] = trainingLabels[i] == "Iris-setosa" ? 0 : 1;
        }

        for (int epoch = 0; epoch < epochs; epoch++)
        {
            float epochLoss = 0f;
            for (int i = 0; i < trainingData.Length; i++)
            {
                float[] input = trainingData[i];
                int label = trainLabels[i];

                // forward pass
                float[] output = net.Forward(input);

                // calculate loss
                float loss = CrossEntropyLoss(output, label);
                epochLoss += loss;

                // backpropagate and update weights
                net.Backpropagate(input, label, learningRate);
            }

            Debug.Log($"Epoch {epoch + 1}/{epochs}, Loss: {epochLoss / trainingData.Length}");
        }

        CalculateAccuracy();
    }

    private void CalculateAccuracy()
    {
        int correctPredictions = 0;

        // iterate over the entire dataset and predict labels
        for (int i = 0; i < testData.Length; i++)
        {
            float[] input = testData[i];
            int actualLabel = testLabels[i] == "Iris-setosa" ? 0 : 1;

            // forward pass (prediction)
            float[] output = net.Forward(input);
            int predictedClass = output[0] > output[1] ? 0 : 1;

            // check if the prediction is correct
            if (predictedClass == actualLabel)
            {
                correctPredictions++;
            }
        }

        // calculate accuracy as a percentage
        accuracy = (float)correctPredictions / testData.Length * 100f;

        // display the final accuracy
        Debug.Log($"Final Accuracy: {accuracy}%");
    }
    public float GetAccuracy()
    {
        return accuracy;
    }

    public void Predict(float[] input, Action<string> onOutputReceived, Action<Exception> fallback)
    {
        try
        {
            float[] output = net.Forward(input);
            int predictedClass = output[0] > output[1] ? 0 : 1; // choose class with highest value
            string prediction = predictedClass == 0 ? "Iris-setosa" : "Iris-nonsetosa";
            onOutputReceived(prediction);
        }
        catch (Exception ex)
        {
            fallback(ex);
        }
    }

    private float CrossEntropyLoss(float[] output, int target)
    {
        float epsilon = 1e-9f;
        float targetValue = target == 0 ? output[0] : output[1];
        return -Mathf.Log(targetValue + epsilon);
    }

}
public class NeuralNetwork
{
    private float[] hiddenWeights;
    private float[] outputWeights;
    private float biasHidden, biasOutput;
    private int inputSize;
    private int hiddenSize;
    private int outputSize;

    public NeuralNetwork(int inputSize, int hiddenSize, int outputSize)
    {
        this.inputSize = inputSize;
        this.hiddenSize = hiddenSize;
        this.outputSize = outputSize;

        // initialize random weights for hidden layer
        hiddenWeights = new float[inputSize * hiddenSize];
        for (int i = 0; i < hiddenWeights.Length; i++)
        {
            hiddenWeights[i] = UnityEngine.Random.Range(-1f, 1f);
        }

        // initialize random weights for output layer
        outputWeights = new float[hiddenSize * outputSize];
        for (int i = 0; i < outputWeights.Length; i++)
        {
            outputWeights[i] = UnityEngine.Random.Range(-1f, 1f);
        }

        biasHidden = UnityEngine.Random.Range(-1f, 1f);
        biasOutput = UnityEngine.Random.Range(-1f, 1f);
    }

    // forward pass: Input -> Hidden Layer -> Output Layer
    public float[] Forward(float[] input)
    {
        // hidden layer computation
        float[] hiddenOutput = new float[hiddenSize];
        for (int i = 0; i < hiddenSize; i++)
        {
            hiddenOutput[i] = biasHidden;
            for (int j = 0; j < inputSize; j++)
            {
                hiddenOutput[i] += input[j] * hiddenWeights[j * hiddenSize + i];
            }
            hiddenOutput[i] = ReLU(hiddenOutput[i]); // ReLU activation function
        }

        // output layer computation
        float[] finalOutput = new float[outputSize]; // binary output (2 classes)
        for (int i = 0; i < outputSize; i++)
        {
            finalOutput[i] = biasOutput;
            for (int j = 0; j < hiddenSize; j++)
            {
                finalOutput[i] += hiddenOutput[j] * outputWeights[j * outputSize + i];
            }
        }

        // apply softmax for output layer
        return Softmax(finalOutput);
    }

    // backpropagation for weight updates
    public void Backpropagate(float[] input, int target, float learningRate)
    {
        // forward pass to get the outputs
        float[] hiddenOutput = new float[hiddenSize];
        for (int i = 0; i < hiddenSize; i++)
        {
            hiddenOutput[i] = biasHidden;
            for (int j = 0; j < inputSize; j++)
            {
                hiddenOutput[i] += input[j] * hiddenWeights[j * hiddenSize + i];
            }
            hiddenOutput[i] = ReLU(hiddenOutput[i]); // ReLU activation function
        }

        // get the output from the forward pass
        float[] finalOutput = new float[outputSize];
        for (int i = 0; i < outputSize; i++)
        {
            finalOutput[i] = biasOutput;
            for (int j = 0; j < hiddenSize; j++)
            {
                finalOutput[i] += hiddenOutput[j] * outputWeights[j * outputSize + i];
            }
        }
        finalOutput = Softmax(finalOutput);

        // compute the output layer error (Cross entropy derivative)
        float[] outputError = new float[outputSize];
        for (int i = 0; i < outputSize; i++)
        {
            outputError[i] = finalOutput[i] - (i == target ? 1 : 0);
        }

        // backpropagate to the hidden layer
        float[] hiddenError = new float[hiddenSize];
        for (int i = 0; i < hiddenSize; i++)
        {
            hiddenError[i] = 0;
            for (int j = 0; j < outputSize; j++)
            {
                hiddenError[i] += outputError[j] * outputWeights[i * outputSize + j];
            }
            hiddenError[i] *= ReLUDerivative(hiddenOutput[i]);
        }

        // update output layer weights
        for (int i = 0; i < hiddenSize; i++)
        {
            for (int j = 0; j < outputSize; j++)
            {
                outputWeights[i * outputSize + j] -= learningRate * outputError[j] * hiddenOutput[i];
            }
        }

        // update hidden layer weights
        for (int i = 0; i < inputSize; i++)
        {
            for (int j = 0; j < hiddenSize; j++)
            {
                hiddenWeights[i * hiddenSize + j] -= learningRate * hiddenError[j] * input[i];
            }
        }

        // update biases
        for (int i = 0; i < outputSize; i++)
        {
            biasOutput -= learningRate * outputError[i];
        }
        for (int i = 0; i < hiddenSize; i++)
        {
            biasHidden -= learningRate * hiddenError[i];
        }
    }

    // ReLU activation function
    private float ReLU(float x)
    {
        return Mathf.Max(0, x);
    }

    // derivative of ReLU
    private float ReLUDerivative(float x)
    {
        return x > 0 ? 1f : 0f;
    }

    // softmax activation function for the output layer
    private float[] Softmax(float[] z)
    {
        float sumExp = Mathf.Exp(z[0]) + Mathf.Exp(z[1]);
        return new float[] { Mathf.Exp(z[0]) / sumExp, Mathf.Exp(z[1]) / sumExp };
    }
}

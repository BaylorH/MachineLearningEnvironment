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
using System.Linq;
using UnityEngine;
using static KNNIrisDataVisualizer;

public class BinaryIrisPredictionClient : MonoBehaviour
{
    private float[][] trainingData;
    private string[] trainingLabels;
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

    private void Start()
    {
        InitializeData();
    }

    private void InitializeData()
    {
        List<float[]> dataArrayList = new List<float[]>();
        List<string> labelList = new List<string>();

        foreach (KNNIrisDataVisualizer.IrisDataPoint point in irisDataPoints)
        {
            dataArrayList.Add(new float[] { point.sepal_length, point.sepal_width, point.petal_length, point.petal_width });
            labelList.Add(point.prediction);
        }

        trainingData = dataArrayList.ToArray();
        trainingLabels = labelList.ToArray();
    }

    public void Predict(float[] input, Action<string> onOutputReceived, Action<Exception> fallback)
    {
        try
        {
            string prediction = BinaryPredict(input);
            onOutputReceived(prediction);
        }
        catch (Exception ex)
        {
            fallback(ex);
        }
    }

    private string BinaryPredict(float[] input)
    {
        // Simple logistic regression prediction logic
        // This is a very basic implementation for demonstration purposes
        // Normally, you would train your model on the data

        float threshold = 0.5f; // Threshold for binary classification
        float linearCombination = LogisticFunction(input); // Calculate the linear combination for the logistic function

        // Use a simple threshold to determine class
        return linearCombination >= threshold ? "Iris-nonsetosa" : "Iris-setosa";
    }

    private float LogisticFunction(float[] input)
    {
        // Example weights for the logistic regression model (these would normally be learned through training)
        float[] weights = { 0.3f, 0.5f, -0.2f, 0.1f };
        float bias = -1.0f; // Example bias term

        // Calculate the linear combination of inputs and weights
        float z = bias;
        for (int i = 0; i < input.Length; i++)
        {
            z += weights[i] * input[i];
        }

        // Apply the sigmoid function
        return Sigmoid(z);
    }

    private float Sigmoid(float z)
    {
        // Sigmoid function
        return 1 / (1 + Mathf.Exp(-z));
    }
}
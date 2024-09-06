using TMPro;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Linq;

// Script to manipulate the training data
public class DataVisualizer : MonoBehaviour {
    public GameObject dataPointPrefab;
    public Material SetosaMaterial;
    public Material NonSetosaMaterial;

    // input fields
    public TMP_InputField petalLengthInput;
    public TMP_InputField petalWidthInput;
    public TMP_InputField sepalLengthInput;
    public TMP_InputField sepalWidthInput;

    public PredictionClient client;

    public TextMeshProUGUI predictionText;
    public TextMeshProUGUI ErrorText;
    private string prediction;

    [System.Serializable]
    public class DataPoint {
        public float sepal_length;
        public float sepal_width;
        public float petal_length;
        public float petal_width;
        public string prediction;
    }

    // Array of DataPoint objects
    private DataPoint[] dataPoints = new DataPoint[] {
        new DataPoint { sepal_length = 5.1f, sepal_width = 3.5f, petal_length = 1.4f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.9f, sepal_width = 3.0f, petal_length = 1.4f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.7f, sepal_width = 3.2f, petal_length = 1.3f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.6f, sepal_width = 3.1f, petal_length = 1.5f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.0f, sepal_width = 3.6f, petal_length = 1.4f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.4f, sepal_width = 3.9f, petal_length = 1.7f, petal_width = 0.4f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.6f, sepal_width = 3.4f, petal_length = 1.4f, petal_width = 0.3f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.0f, sepal_width = 3.4f, petal_length = 1.5f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.4f, sepal_width = 2.9f, petal_length = 1.4f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.9f, sepal_width = 3.1f, petal_length = 1.5f, petal_width = 0.1f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.4f, sepal_width = 3.7f, petal_length = 1.5f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.8f, sepal_width = 3.4f, petal_length = 1.6f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.8f, sepal_width = 3.0f, petal_length = 1.4f, petal_width = 0.1f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.3f, sepal_width = 3.0f, petal_length = 1.1f, petal_width = 0.1f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.8f, sepal_width = 4.0f, petal_length = 1.2f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.7f, sepal_width = 4.4f, petal_length = 1.5f, petal_width = 0.4f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.4f, sepal_width = 3.9f, petal_length = 1.3f, petal_width = 0.4f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.1f, sepal_width = 3.5f, petal_length = 1.4f, petal_width = 0.3f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.7f, sepal_width = 3.8f, petal_length = 1.7f, petal_width = 0.3f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.1f, sepal_width = 3.8f, petal_length = 1.5f, petal_width = 0.3f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.4f, sepal_width = 3.4f, petal_length = 1.7f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.1f, sepal_width = 3.7f, petal_length = 1.5f, petal_width = 0.4f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.6f, sepal_width = 3.6f, petal_length = 1.0f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.1f, sepal_width = 3.3f, petal_length = 1.7f, petal_width = 0.5f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.8f, sepal_width = 3.4f, petal_length = 1.9f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.0f, sepal_width = 3.0f, petal_length = 1.6f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.0f, sepal_width = 3.4f, petal_length = 1.6f, petal_width = 0.4f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.2f, sepal_width = 3.5f, petal_length = 1.5f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.2f, sepal_width = 3.4f, petal_length = 1.4f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.7f, sepal_width = 3.2f, petal_length = 1.6f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.8f, sepal_width = 3.1f, petal_length = 1.6f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.4f, sepal_width = 3.4f, petal_length = 1.5f, petal_width = 0.4f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.2f, sepal_width = 4.1f, petal_length = 1.5f, petal_width = 0.1f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.5f, sepal_width = 4.2f, petal_length = 1.4f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.9f, sepal_width = 3.1f, petal_length = 1.5f, petal_width = 0.1f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.0f, sepal_width = 3.2f, petal_length = 1.2f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.5f, sepal_width = 3.5f, petal_length = 1.3f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.9f, sepal_width = 3.1f, petal_length = 1.5f, petal_width = 0.1f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.4f, sepal_width = 3.0f, petal_length = 1.3f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.1f, sepal_width = 3.4f, petal_length = 1.5f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.0f, sepal_width = 3.5f, petal_length = 1.3f, petal_width = 0.3f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.5f, sepal_width = 2.3f, petal_length = 1.3f, petal_width = 0.3f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.4f, sepal_width = 3.2f, petal_length = 1.3f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.0f, sepal_width = 3.5f, petal_length = 1.6f, petal_width = 0.6f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.1f, sepal_width = 3.8f, petal_length = 1.9f, petal_width = 0.4f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.8f, sepal_width = 3.0f, petal_length = 1.4f, petal_width = 0.3f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.1f, sepal_width = 3.8f, petal_length = 1.6f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 4.6f, sepal_width = 3.2f, petal_length = 1.4f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.3f, sepal_width = 3.7f, petal_length = 1.5f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 5.0f, sepal_width = 3.3f, petal_length = 1.4f, petal_width = 0.2f, prediction = "Iris-setosa" },
        new DataPoint { sepal_length = 7.0f, sepal_width = 3.2f, petal_length = 4.7f, petal_width = 1.4f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.4f, sepal_width = 3.2f, petal_length = 4.5f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.9f, sepal_width = 3.1f, petal_length = 4.9f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.5f, sepal_width = 2.3f, petal_length = 4.0f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.5f, sepal_width = 2.8f, petal_length = 4.6f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.7f, sepal_width = 2.8f, petal_length = 4.5f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.3f, sepal_width = 3.3f, petal_length = 4.7f, petal_width = 1.6f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 4.9f, sepal_width = 2.4f, petal_length = 3.3f, petal_width = 1.0f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.6f, sepal_width = 2.9f, petal_length = 4.6f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.2f, sepal_width = 2.7f, petal_length = 3.9f, petal_width = 1.4f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.0f, sepal_width = 2.0f, petal_length = 3.5f, petal_width = 1.0f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.9f, sepal_width = 3.0f, petal_length = 4.2f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.0f, sepal_width = 2.2f, petal_length = 4.0f, petal_width = 1.0f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.1f, sepal_width = 2.9f, petal_length = 4.7f, petal_width = 1.4f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.6f, sepal_width = 2.9f, petal_length = 3.6f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.7f, sepal_width = 3.1f, petal_length = 4.4f, petal_width = 1.4f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.6f, sepal_width = 3.0f, petal_length = 4.5f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.8f, sepal_width = 2.7f, petal_length = 4.1f, petal_width = 1.0f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.2f, sepal_width = 2.2f, petal_length = 4.5f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.6f, sepal_width = 2.5f, petal_length = 3.9f, petal_width = 1.1f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.9f, sepal_width = 3.2f, petal_length = 4.8f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.1f, sepal_width = 2.8f, petal_length = 4.0f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.3f, sepal_width = 2.5f, petal_length = 4.9f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.1f, sepal_width = 2.8f, petal_length = 4.7f, petal_width = 1.2f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.4f, sepal_width = 2.9f, petal_length = 4.3f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.6f, sepal_width = 3.0f, petal_length = 4.4f, petal_width = 1.4f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.8f, sepal_width = 2.8f, petal_length = 4.8f, petal_width = 1.4f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.7f, sepal_width = 3.0f, petal_length = 5.0f, petal_width = 1.7f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.0f, sepal_width = 2.9f, petal_length = 4.5f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.7f, sepal_width = 2.6f, petal_length = 3.5f, petal_width = 1.0f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.5f, sepal_width = 2.4f, petal_length = 3.8f, petal_width = 1.1f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.5f, sepal_width = 2.4f, petal_length = 3.7f, petal_width = 1.0f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.8f, sepal_width = 2.7f, petal_length = 3.9f, petal_width = 1.2f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.0f, sepal_width = 2.7f, petal_length = 5.1f, petal_width = 1.6f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.4f, sepal_width = 3.0f, petal_length = 4.5f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.0f, sepal_width = 3.4f, petal_length = 4.5f, petal_width = 1.6f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.7f, sepal_width = 3.1f, petal_length = 4.7f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.3f, sepal_width = 2.3f, petal_length = 4.4f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.6f, sepal_width = 3.0f, petal_length = 4.1f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.5f, sepal_width = 2.5f, petal_length = 4.0f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.5f, sepal_width = 2.6f, petal_length = 4.4f, petal_width = 1.2f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.1f, sepal_width = 3.0f, petal_length = 4.6f, petal_width = 1.4f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.8f, sepal_width = 2.6f, petal_length = 4.0f, petal_width = 1.2f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.0f, sepal_width = 2.3f, petal_length = 3.3f, petal_width = 1.0f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.6f, sepal_width = 2.7f, petal_length = 4.2f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.7f, sepal_width = 3.0f, petal_length = 4.2f, petal_width = 1.2f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.7f, sepal_width = 2.9f, petal_length = 4.2f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.2f, sepal_width = 2.9f, petal_length = 4.3f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.1f, sepal_width = 2.5f, petal_length = 3.0f, petal_width = 1.1f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.7f, sepal_width = 2.8f, petal_length = 4.1f, petal_width = 1.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.3f, sepal_width = 3.3f, petal_length = 6.0f, petal_width = 2.5f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.8f, sepal_width = 2.7f, petal_length = 5.1f, petal_width = 1.9f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 7.1f, sepal_width = 3.0f, petal_length = 5.9f, petal_width = 2.1f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.3f, sepal_width = 2.9f, petal_length = 5.6f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.5f, sepal_width = 3.0f, petal_length = 5.8f, petal_width = 2.2f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 7.6f, sepal_width = 3.0f, petal_length = 6.6f, petal_width = 2.1f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 4.9f, sepal_width = 2.5f, petal_length = 4.5f, petal_width = 1.7f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 7.3f, sepal_width = 2.9f, petal_length = 6.3f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.7f, sepal_width = 2.5f, petal_length = 5.8f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 7.2f, sepal_width = 3.6f, petal_length = 6.1f, petal_width = 2.5f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.5f, sepal_width = 3.2f, petal_length = 5.1f, petal_width = 2.0f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.4f, sepal_width = 2.7f, petal_length = 5.3f, petal_width = 1.9f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.8f, sepal_width = 3.0f, petal_length = 5.5f, petal_width = 2.1f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.7f, sepal_width = 2.5f, petal_length = 5.0f, petal_width = 2.0f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.8f, sepal_width = 2.8f, petal_length = 5.1f, petal_width = 2.4f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.4f, sepal_width = 3.2f, petal_length = 5.3f, petal_width = 2.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.5f, sepal_width = 3.0f, petal_length = 5.5f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 7.7f, sepal_width = 3.8f, petal_length = 6.7f, petal_width = 2.2f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 7.7f, sepal_width = 2.6f, petal_length = 6.9f, petal_width = 2.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.0f, sepal_width = 2.2f, petal_length = 5.0f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.9f, sepal_width = 3.2f, petal_length = 5.7f, petal_width = 2.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.6f, sepal_width = 2.8f, petal_length = 4.9f, petal_width = 2.0f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 7.7f, sepal_width = 2.8f, petal_length = 6.7f, petal_width = 2.0f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.3f, sepal_width = 2.7f, petal_length = 4.9f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.7f, sepal_width = 3.3f, petal_length = 5.7f, petal_width = 2.1f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 7.2f, sepal_width = 3.2f, petal_length = 6.0f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.2f, sepal_width = 2.8f, petal_length = 4.8f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.1f, sepal_width = 3.0f, petal_length = 4.9f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.4f, sepal_width = 2.8f, petal_length = 5.6f, petal_width = 2.1f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 7.2f, sepal_width = 3.0f, petal_length = 5.8f, petal_width = 1.6f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 7.4f, sepal_width = 2.8f, petal_length = 6.1f, petal_width = 1.9f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 7.9f, sepal_width = 3.8f, petal_length = 6.4f, petal_width = 2.0f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.4f, sepal_width = 2.8f, petal_length = 5.6f, petal_width = 2.2f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.3f, sepal_width = 2.8f, petal_length = 5.1f, petal_width = 1.5f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.1f, sepal_width = 2.6f, petal_length = 5.6f, petal_width = 1.4f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 7.7f, sepal_width = 3.0f, petal_length = 6.1f, petal_width = 2.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.3f, sepal_width = 3.4f, petal_length = 5.6f, petal_width = 2.4f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.4f, sepal_width = 3.1f, petal_length = 5.5f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.0f, sepal_width = 3.0f, petal_length = 4.8f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.9f, sepal_width = 3.1f, petal_length = 5.4f, petal_width = 2.1f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.7f, sepal_width = 3.1f, petal_length = 5.6f, petal_width = 2.4f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.9f, sepal_width = 3.1f, petal_length = 5.1f, petal_width = 2.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.8f, sepal_width = 2.7f, petal_length = 5.1f, petal_width = 1.9f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.8f, sepal_width = 3.2f, petal_length = 5.9f, petal_width = 2.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.7f, sepal_width = 3.3f, petal_length = 5.7f, petal_width = 2.5f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.7f, sepal_width = 3.0f, petal_length = 5.2f, petal_width = 2.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.3f, sepal_width = 2.5f, petal_length = 5.0f, petal_width = 1.9f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.5f, sepal_width = 3.0f, petal_length = 5.2f, petal_width = 2.0f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 6.2f, sepal_width = 3.4f, petal_length = 5.4f, petal_width = 2.3f, prediction = "Iris-nonsetosa" },
        new DataPoint { sepal_length = 5.9f, sepal_width = 3.0f, petal_length = 5.1f, petal_width = 1.8f, prediction = "Iris-nonsetosa" },
    };

    // Start is called before the first frame update
    void Start() {
        FindInputFields();

        // Get min points
        float xMin = dataPoints.Min(p => p.sepal_length);
        float yMin = dataPoints.Min(p => p.sepal_width);
        float zMin = dataPoints.Min( p => p.petal_length);

        // For every data point in the set plot it
        foreach (DataPoint point in dataPoints) {
            // Adjust positions based on provided offsets and invert Z-coordinate
            float scaler = 20.0f;
            Vector3 position = new Vector3(
                point.sepal_length*(scaler) + 720 - xMin*scaler, // X-coordinate adjustment
                point.sepal_width*(scaler) + 547 - yMin*scaler,  // Y-coordinate adjustment
                -point.petal_length*(scaler) + 56 + zMin*scaler // Z-coordinate adjustment and inversion
            );

            // Adjust rotation to point up
            Quaternion rotation = Quaternion.Euler(-90, 0, 0);
            // Instantiate a sphere at the adjusted position and rotation
            GameObject sphere = Instantiate(dataPointPrefab, position, rotation);

            // Determine scale based on petal_width, with a chosen multiplier for visualization purposes
            float baseScale = 20.0f; // Minimum scale to ensure visibility
            float scaleMultiplier = 10f;
            float scale = baseScale + (point.petal_width * scaleMultiplier);
            sphere.transform.localScale = new Vector3(scale, scale, scale);
 
            // Assign material based on the prediction
            Material chosenMaterial = point.prediction == "Iris-setosa" ? SetosaMaterial : NonSetosaMaterial;
            sphere.GetComponent<Renderer>().material = chosenMaterial;

            Debug.Log($"Data point instantiated at: {position} with scale: {scale}");
        }
    }

    private void FindInputFields()
    {
        petalLengthInput = GameObject.Find("PetalLength").GetComponent<TMP_InputField>();
        petalWidthInput = GameObject.Find("PetalWidth").GetComponent<TMP_InputField>();
        sepalLengthInput = GameObject.Find("SepalLength").GetComponent<TMP_InputField>();
        sepalWidthInput = GameObject.Find("SepalWidth").GetComponent<TMP_InputField>();
        //dataPointPrefab = GameObject.Find("DataPoint");
    }
    public void AddData()
    {
        // Check if any input field is empty
        if (string.IsNullOrWhiteSpace(sepalLengthInput.text) ||
                string.IsNullOrWhiteSpace(sepalWidthInput.text) ||
                string.IsNullOrWhiteSpace(petalLengthInput.text) ||
                string.IsNullOrWhiteSpace(petalWidthInput.text)) {
            Debug.LogError("All input fields must be filled.");
            //predictionText.text = "Error: All fields must be filled.";
            ErrorText.text = "Error: All fields must be filled.";
            return; // Exit the method early if any field is empty
        }

        float[] inputs = new float[4];

        try {
            inputs[0] = float.Parse(sepalLengthInput.text);
            inputs[1] = float.Parse(sepalWidthInput.text);
            inputs[2] = float.Parse(petalLengthInput.text);
            inputs[3] = float.Parse(petalWidthInput.text);
        }
        catch (FormatException) {
            Debug.LogError("Input is not in a correct numeric format.");
            //predictionText.text = "Error: Please enter valid numbers.";
            ErrorText.text = "Error: Please enter valid numbers.";
            return; // Exit the method if parsing fails
        }

        // Get min points
        float xMin = dataPoints.Min(p => p.sepal_length);
        float yMin = dataPoints.Min(p => p.sepal_width);
        float zMin = dataPoints.Min(p => p.petal_length);

        Predict(inputs);
        System.Threading.Thread.Sleep(500);
        Debug.Log(prediction);
        predictionText.text = prediction;
        ErrorText.text = "";

        float scaler = 20.0f;
        Vector3 finalPosition = new Vector3(
            inputs[0] * scaler + 720 - xMin * scaler, // X-coordinate adjustment
            inputs[1] * scaler + 547 - yMin * scaler, // Y-coordinate adjustment
            -inputs[2] * scaler + 56 + zMin * scaler // Z-coordinate adjustment and inversion
        );
        Vector3 position = new Vector3(829, 648, -54);
        // Adjust rotation to point up
        Quaternion rotation = Quaternion.Euler(-90, 0, 0);
        GameObject newDataPoint = Instantiate(dataPointPrefab, position, rotation);
        float baseScale = 20f; // Minimum scale to ensure visibility
        float scaleMultiplier = 10.0f; // Adjust this multiplier based on your preference
        float scale = baseScale + (inputs[3] * scaleMultiplier);
        newDataPoint.transform.localScale = new Vector3(scale, scale, scale);
        Material chosenMaterial = prediction == "Prediction: Iris-setosa" ? SetosaMaterial : NonSetosaMaterial;
        Debug.Log(chosenMaterial);
        newDataPoint.GetComponent<Renderer>().material = chosenMaterial;
        MoveTowards mT = newDataPoint.GetComponent<MoveTowards>();
        mT.moveTowards(finalPosition);

        // Reset user input fields
        petalLengthInput.text = "";
        petalWidthInput.text = "";
        sepalLengthInput.text = "";
        sepalWidthInput.text = "";
    }

    private void Predict(float[] input)
    {
        client.Predict(input, output =>
        {
            prediction = "Prediction: " + output;
        }, error =>
        {
            // TODO: 
        });
    }

    //display the user input?
    public void displayInput() {
        string petalLength = petalLengthInput.text;
        string petalWidth = petalWidthInput.text;
        string sepalLength = sepalLengthInput.text;
        string sepalWidth = sepalWidthInput.text;

        string displayString = $"Petal Length: {petalLength}\n" +
                               $"Petal Width: {petalWidth}\n" +
                               $"Sepal Length: {sepalLength}\n" +
                               $"Sepal Width: {sepalWidth}";
        
        Debug.Log(displayString);
    }
}

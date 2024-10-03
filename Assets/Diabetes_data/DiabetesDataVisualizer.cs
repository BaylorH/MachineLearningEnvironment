using TMPro;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Linq;

public class DiabetesDataVisualizer : MonoBehaviour
{
    public GameObject dataPointPrefab;

    public KNNDiabetesPredictionClient client;

    public TextMeshProUGUI predictionText;
    public TextMeshProUGUI ErrorText;
    private string prediction;

    [System.Serializable]
    public class DiabetesDataPoint
    {
        public float glucose;
        public float blood_pressure;
        public float bmi;
        public float age;
        public string prediction;
    }

    private DiabetesDataPoint[] dataPoints = new DiabetesDataPoint[] {
        // put in diabetes data
    };

    void Start()
    {
        // copy from iris
    }

    void Update()
    {
        // copy from iris
    }
}

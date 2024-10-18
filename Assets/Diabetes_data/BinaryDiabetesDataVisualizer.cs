using TMPro;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Linq;

public class BinaryDiabetesDataVisualizer : MonoBehaviour
{
    public GameObject dataPointPrefab;
    public Material DiabeticMaterial;
    public Material NonDiabeticMaterial;

    public ObjectPooler objectPooler;

    // input fields
    public TMP_InputField glucoseInput;
    public TMP_InputField bloodPressureInput;
    public TMP_InputField bmiInput;
    public TMP_InputField ageInput;

    private TMP_InputField[] inputFields;

    public BinaryDiabetesPredictionClient client;

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
        new DiabetesDataPoint { glucose = 148.0f, blood_pressure = 72.0f, bmi = 33.6f, age = 50.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 85.0f, blood_pressure = 66.0f, bmi = 26.6f, age = 31.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 183.0f, blood_pressure = 64.0f, bmi = 23.3f, age = 32.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 89.0f, blood_pressure = 66.0f, bmi = 28.1f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 137.0f, blood_pressure = 40.0f, bmi = 43.1f, age = 33.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 116.0f, blood_pressure = 74.0f, bmi = 25.6f, age = 30.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 78.0f, blood_pressure = 50.0f, bmi = 31.0f, age = 26.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 115.0f, blood_pressure = 0.0f, bmi = 35.3f, age = 29.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 197.0f, blood_pressure = 70.0f, bmi = 30.5f, age = 53.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 125.0f, blood_pressure = 96.0f, bmi = 0.0f, age = 54.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 110.0f, blood_pressure = 92.0f, bmi = 37.6f, age = 30.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 168.0f, blood_pressure = 74.0f, bmi = 38.0f, age = 34.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 139.0f, blood_pressure = 80.0f, bmi = 27.1f, age = 57.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 189.0f, blood_pressure = 60.0f, bmi = 30.1f, age = 59.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 166.0f, blood_pressure = 72.0f, bmi = 25.8f, age = 51.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 100.0f, blood_pressure = 0.0f, bmi = 30.0f, age = 32.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 118.0f, blood_pressure = 84.0f, bmi = 45.8f, age = 31.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 107.0f, blood_pressure = 74.0f, bmi = 29.6f, age = 31.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 103.0f, blood_pressure = 30.0f, bmi = 43.3f, age = 33.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 115.0f, blood_pressure = 70.0f, bmi = 34.6f, age = 32.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 126.0f, blood_pressure = 88.0f, bmi = 39.3f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 99.0f, blood_pressure = 84.0f, bmi = 35.4f, age = 50.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 196.0f, blood_pressure = 90.0f, bmi = 39.8f, age = 41.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 119.0f, blood_pressure = 80.0f, bmi = 29.0f, age = 29.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 143.0f, blood_pressure = 94.0f, bmi = 36.6f, age = 51.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 125.0f, blood_pressure = 70.0f, bmi = 31.1f, age = 41.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 147.0f, blood_pressure = 76.0f, bmi = 39.4f, age = 43.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 97.0f, blood_pressure = 66.0f, bmi = 23.2f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 145.0f, blood_pressure = 82.0f, bmi = 22.2f, age = 57.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 117.0f, blood_pressure = 92.0f, bmi = 34.1f, age = 38.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 109.0f, blood_pressure = 75.0f, bmi = 36.0f, age = 60.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 158.0f, blood_pressure = 76.0f, bmi = 31.6f, age = 28.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 88.0f, blood_pressure = 58.0f, bmi = 24.8f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 92.0f, blood_pressure = 92.0f, bmi = 19.9f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 122.0f, blood_pressure = 78.0f, bmi = 27.6f, age = 45.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 103.0f, blood_pressure = 60.0f, bmi = 24.0f, age = 33.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 138.0f, blood_pressure = 76.0f, bmi = 33.2f, age = 35.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 102.0f, blood_pressure = 76.0f, bmi = 32.9f, age = 46.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 90.0f, blood_pressure = 68.0f, bmi = 38.2f, age = 27.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 111.0f, blood_pressure = 72.0f, bmi = 37.1f, age = 56.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 180.0f, blood_pressure = 64.0f, bmi = 34.0f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 133.0f, blood_pressure = 84.0f, bmi = 40.2f, age = 37.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 106.0f, blood_pressure = 92.0f, bmi = 22.7f, age = 48.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 171.0f, blood_pressure = 110.0f, bmi = 45.4f, age = 54.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 159.0f, blood_pressure = 64.0f, bmi = 27.4f, age = 40.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 180.0f, blood_pressure = 66.0f, bmi = 42.0f, age = 25.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 146.0f, blood_pressure = 56.0f, bmi = 29.7f, age = 29.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 71.0f, blood_pressure = 70.0f, bmi = 28.0f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 103.0f, blood_pressure = 66.0f, bmi = 39.1f, age = 31.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 105.0f, blood_pressure = 0.0f, bmi = 0.0f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 103.0f, blood_pressure = 80.0f, bmi = 19.4f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 101.0f, blood_pressure = 50.0f, bmi = 24.2f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 88.0f, blood_pressure = 66.0f, bmi = 24.4f, age = 30.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 176.0f, blood_pressure = 90.0f, bmi = 33.7f, age = 58.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 150.0f, blood_pressure = 66.0f, bmi = 34.7f, age = 42.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 73.0f, blood_pressure = 50.0f, bmi = 23.0f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 187.0f, blood_pressure = 68.0f, bmi = 37.7f, age = 41.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 100.0f, blood_pressure = 88.0f, bmi = 46.8f, age = 31.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 146.0f, blood_pressure = 82.0f, bmi = 40.5f, age = 44.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 105.0f, blood_pressure = 64.0f, bmi = 41.5f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 84.0f, blood_pressure = 0.0f, bmi = 0.0f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 133.0f, blood_pressure = 72.0f, bmi = 32.9f, age = 39.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 44.0f, blood_pressure = 62.0f, bmi = 25.0f, age = 36.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 141.0f, blood_pressure = 58.0f, bmi = 25.4f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 114.0f, blood_pressure = 66.0f, bmi = 32.8f, age = 42.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 99.0f, blood_pressure = 74.0f, bmi = 29.0f, age = 32.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 109.0f, blood_pressure = 88.0f, bmi = 32.5f, age = 38.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 109.0f, blood_pressure = 92.0f, bmi = 42.7f, age = 54.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 95.0f, blood_pressure = 66.0f, bmi = 19.6f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 146.0f, blood_pressure = 85.0f, bmi = 28.9f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 100.0f, blood_pressure = 66.0f, bmi = 32.9f, age = 28.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 139.0f, blood_pressure = 64.0f, bmi = 28.6f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 126.0f, blood_pressure = 90.0f, bmi = 43.4f, age = 42.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 129.0f, blood_pressure = 86.0f, bmi = 35.1f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 79.0f, blood_pressure = 75.0f, bmi = 32.0f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 0.0f, blood_pressure = 48.0f, bmi = 24.7f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 62.0f, blood_pressure = 78.0f, bmi = 32.6f, age = 41.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 95.0f, blood_pressure = 72.0f, bmi = 37.7f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 131.0f, blood_pressure = 0.0f, bmi = 43.2f, age = 26.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 112.0f, blood_pressure = 66.0f, bmi = 25.0f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 113.0f, blood_pressure = 44.0f, bmi = 22.4f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 74.0f, blood_pressure = 0.0f, bmi = 0.0f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 83.0f, blood_pressure = 78.0f, bmi = 29.3f, age = 36.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 101.0f, blood_pressure = 65.0f, bmi = 24.6f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 137.0f, blood_pressure = 108.0f, bmi = 48.8f, age = 37.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 110.0f, blood_pressure = 74.0f, bmi = 32.4f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 106.0f, blood_pressure = 72.0f, bmi = 36.6f, age = 45.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 100.0f, blood_pressure = 68.0f, bmi = 38.5f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 136.0f, blood_pressure = 70.0f, bmi = 37.1f, age = 43.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 107.0f, blood_pressure = 68.0f, bmi = 26.5f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 80.0f, blood_pressure = 55.0f, bmi = 19.1f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 123.0f, blood_pressure = 80.0f, bmi = 32.0f, age = 34.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 81.0f, blood_pressure = 78.0f, bmi = 46.7f, age = 42.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 134.0f, blood_pressure = 72.0f, bmi = 23.8f, age = 60.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 142.0f, blood_pressure = 82.0f, bmi = 24.7f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 144.0f, blood_pressure = 72.0f, bmi = 33.9f, age = 40.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 92.0f, blood_pressure = 62.0f, bmi = 31.6f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 71.0f, blood_pressure = 48.0f, bmi = 20.4f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 93.0f, blood_pressure = 50.0f, bmi = 28.7f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 122.0f, blood_pressure = 90.0f, bmi = 49.7f, age = 31.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 163.0f, blood_pressure = 72.0f, bmi = 39.0f, age = 33.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 151.0f, blood_pressure = 60.0f, bmi = 26.1f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 125.0f, blood_pressure = 96.0f, bmi = 22.5f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 81.0f, blood_pressure = 72.0f, bmi = 26.6f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 85.0f, blood_pressure = 65.0f, bmi = 39.6f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 126.0f, blood_pressure = 56.0f, bmi = 28.7f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 96.0f, blood_pressure = 122.0f, bmi = 22.4f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 144.0f, blood_pressure = 58.0f, bmi = 29.5f, age = 37.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 83.0f, blood_pressure = 58.0f, bmi = 34.3f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 95.0f, blood_pressure = 85.0f, bmi = 37.4f, age = 24.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 171.0f, blood_pressure = 72.0f, bmi = 33.3f, age = 24.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 155.0f, blood_pressure = 62.0f, bmi = 34.0f, age = 46.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 89.0f, blood_pressure = 76.0f, bmi = 31.2f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 76.0f, blood_pressure = 62.0f, bmi = 34.0f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 160.0f, blood_pressure = 54.0f, bmi = 30.5f, age = 39.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 146.0f, blood_pressure = 92.0f, bmi = 31.2f, age = 61.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 124.0f, blood_pressure = 74.0f, bmi = 34.0f, age = 38.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 78.0f, blood_pressure = 48.0f, bmi = 33.7f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 97.0f, blood_pressure = 60.0f, bmi = 28.2f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 99.0f, blood_pressure = 76.0f, bmi = 23.2f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 162.0f, blood_pressure = 76.0f, bmi = 53.2f, age = 25.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 111.0f, blood_pressure = 64.0f, bmi = 34.2f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 107.0f, blood_pressure = 74.0f, bmi = 33.6f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 132.0f, blood_pressure = 80.0f, bmi = 26.8f, age = 69.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 113.0f, blood_pressure = 76.0f, bmi = 33.3f, age = 23.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 88.0f, blood_pressure = 30.0f, bmi = 55.0f, age = 26.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 120.0f, blood_pressure = 70.0f, bmi = 42.9f, age = 30.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 118.0f, blood_pressure = 58.0f, bmi = 33.3f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 117.0f, blood_pressure = 88.0f, bmi = 34.5f, age = 40.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 105.0f, blood_pressure = 84.0f, bmi = 27.9f, age = 62.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 173.0f, blood_pressure = 70.0f, bmi = 29.7f, age = 33.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 122.0f, blood_pressure = 56.0f, bmi = 33.3f, age = 33.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 170.0f, blood_pressure = 64.0f, bmi = 34.5f, age = 30.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 84.0f, blood_pressure = 74.0f, bmi = 38.3f, age = 39.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 96.0f, blood_pressure = 68.0f, bmi = 21.1f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 125.0f, blood_pressure = 60.0f, bmi = 33.8f, age = 31.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 100.0f, blood_pressure = 70.0f, bmi = 30.8f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 93.0f, blood_pressure = 60.0f, bmi = 28.7f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 129.0f, blood_pressure = 80.0f, bmi = 31.2f, age = 29.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 105.0f, blood_pressure = 72.0f, bmi = 36.9f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 128.0f, blood_pressure = 78.0f, bmi = 21.1f, age = 55.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 106.0f, blood_pressure = 82.0f, bmi = 39.5f, age = 38.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 108.0f, blood_pressure = 52.0f, bmi = 32.5f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 108.0f, blood_pressure = 66.0f, bmi = 32.4f, age = 42.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 154.0f, blood_pressure = 62.0f, bmi = 32.8f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 102.0f, blood_pressure = 75.0f, bmi = 0.0f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 57.0f, blood_pressure = 80.0f, bmi = 32.8f, age = 41.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 106.0f, blood_pressure = 64.0f, bmi = 30.5f, age = 34.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 147.0f, blood_pressure = 78.0f, bmi = 33.7f, age = 65.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 90.0f, blood_pressure = 70.0f, bmi = 27.3f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 136.0f, blood_pressure = 74.0f, bmi = 37.4f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 114.0f, blood_pressure = 65.0f, bmi = 21.9f, age = 37.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 156.0f, blood_pressure = 86.0f, bmi = 34.3f, age = 42.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 153.0f, blood_pressure = 82.0f, bmi = 40.6f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 188.0f, blood_pressure = 78.0f, bmi = 47.9f, age = 43.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 152.0f, blood_pressure = 88.0f, bmi = 50.0f, age = 36.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 99.0f, blood_pressure = 52.0f, bmi = 24.6f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 109.0f, blood_pressure = 56.0f, bmi = 25.2f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 88.0f, blood_pressure = 74.0f, bmi = 29.0f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 163.0f, blood_pressure = 72.0f, bmi = 40.9f, age = 47.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 151.0f, blood_pressure = 90.0f, bmi = 29.7f, age = 36.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 102.0f, blood_pressure = 74.0f, bmi = 37.2f, age = 45.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 114.0f, blood_pressure = 80.0f, bmi = 44.2f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 100.0f, blood_pressure = 64.0f, bmi = 29.7f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 131.0f, blood_pressure = 88.0f, bmi = 31.6f, age = 32.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 104.0f, blood_pressure = 74.0f, bmi = 29.9f, age = 41.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 148.0f, blood_pressure = 66.0f, bmi = 32.5f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 120.0f, blood_pressure = 68.0f, bmi = 29.6f, age = 34.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 110.0f, blood_pressure = 66.0f, bmi = 31.9f, age = 29.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 111.0f, blood_pressure = 90.0f, bmi = 28.4f, age = 29.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 102.0f, blood_pressure = 82.0f, bmi = 30.8f, age = 36.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 134.0f, blood_pressure = 70.0f, bmi = 35.4f, age = 29.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 87.0f, blood_pressure = 0.0f, bmi = 28.9f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 79.0f, blood_pressure = 60.0f, bmi = 43.5f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 75.0f, blood_pressure = 64.0f, bmi = 29.7f, age = 33.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 179.0f, blood_pressure = 72.0f, bmi = 32.7f, age = 36.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 85.0f, blood_pressure = 78.0f, bmi = 31.2f, age = 42.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 129.0f, blood_pressure = 110.0f, bmi = 67.1f, age = 26.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 143.0f, blood_pressure = 78.0f, bmi = 45.0f, age = 47.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 130.0f, blood_pressure = 82.0f, bmi = 39.1f, age = 37.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 87.0f, blood_pressure = 80.0f, bmi = 23.2f, age = 32.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 119.0f, blood_pressure = 64.0f, bmi = 34.9f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 0.0f, blood_pressure = 74.0f, bmi = 27.7f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 73.0f, blood_pressure = 60.0f, bmi = 26.8f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 141.0f, blood_pressure = 74.0f, bmi = 27.6f, age = 40.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 194.0f, blood_pressure = 68.0f, bmi = 35.9f, age = 41.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 181.0f, blood_pressure = 68.0f, bmi = 30.1f, age = 60.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 128.0f, blood_pressure = 98.0f, bmi = 32.0f, age = 33.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 109.0f, blood_pressure = 76.0f, bmi = 27.9f, age = 31.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 139.0f, blood_pressure = 80.0f, bmi = 31.6f, age = 25.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 111.0f, blood_pressure = 62.0f, bmi = 22.6f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 123.0f, blood_pressure = 70.0f, bmi = 33.1f, age = 40.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 159.0f, blood_pressure = 66.0f, bmi = 30.4f, age = 36.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 135.0f, blood_pressure = 0.0f, bmi = 52.3f, age = 40.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 85.0f, blood_pressure = 55.0f, bmi = 24.4f, age = 42.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 158.0f, blood_pressure = 84.0f, bmi = 39.4f, age = 29.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 105.0f, blood_pressure = 58.0f, bmi = 24.3f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 107.0f, blood_pressure = 62.0f, bmi = 22.9f, age = 23.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 109.0f, blood_pressure = 64.0f, bmi = 34.8f, age = 26.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 148.0f, blood_pressure = 60.0f, bmi = 30.9f, age = 29.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 113.0f, blood_pressure = 80.0f, bmi = 31.0f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 138.0f, blood_pressure = 82.0f, bmi = 40.1f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 108.0f, blood_pressure = 68.0f, bmi = 27.3f, age = 32.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 99.0f, blood_pressure = 70.0f, bmi = 20.4f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 103.0f, blood_pressure = 72.0f, bmi = 37.7f, age = 55.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 111.0f, blood_pressure = 72.0f, bmi = 23.9f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 196.0f, blood_pressure = 76.0f, bmi = 37.5f, age = 57.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 162.0f, blood_pressure = 104.0f, bmi = 37.7f, age = 52.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 96.0f, blood_pressure = 64.0f, bmi = 33.2f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 184.0f, blood_pressure = 84.0f, bmi = 35.5f, age = 41.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 81.0f, blood_pressure = 60.0f, bmi = 27.7f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 147.0f, blood_pressure = 85.0f, bmi = 42.8f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 179.0f, blood_pressure = 95.0f, bmi = 34.2f, age = 60.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 140.0f, blood_pressure = 65.0f, bmi = 42.6f, age = 24.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 112.0f, blood_pressure = 82.0f, bmi = 34.2f, age = 36.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 151.0f, blood_pressure = 70.0f, bmi = 41.8f, age = 38.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 109.0f, blood_pressure = 62.0f, bmi = 35.8f, age = 25.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 125.0f, blood_pressure = 68.0f, bmi = 30.0f, age = 32.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 85.0f, blood_pressure = 74.0f, bmi = 29.0f, age = 32.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 112.0f, blood_pressure = 66.0f, bmi = 37.8f, age = 41.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 177.0f, blood_pressure = 60.0f, bmi = 34.6f, age = 21.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 158.0f, blood_pressure = 90.0f, bmi = 31.6f, age = 66.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 119.0f, blood_pressure = 0.0f, bmi = 25.2f, age = 37.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 142.0f, blood_pressure = 60.0f, bmi = 28.8f, age = 61.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 100.0f, blood_pressure = 66.0f, bmi = 23.6f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 87.0f, blood_pressure = 78.0f, bmi = 34.6f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 101.0f, blood_pressure = 76.0f, bmi = 35.7f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 162.0f, blood_pressure = 52.0f, bmi = 37.2f, age = 24.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 197.0f, blood_pressure = 70.0f, bmi = 36.7f, age = 31.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 117.0f, blood_pressure = 80.0f, bmi = 45.2f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 142.0f, blood_pressure = 86.0f, bmi = 44.0f, age = 22.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 134.0f, blood_pressure = 80.0f, bmi = 46.2f, age = 46.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 79.0f, blood_pressure = 80.0f, bmi = 25.4f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 122.0f, blood_pressure = 68.0f, bmi = 35.0f, age = 29.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 74.0f, blood_pressure = 68.0f, bmi = 29.7f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 171.0f, blood_pressure = 72.0f, bmi = 43.6f, age = 26.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 181.0f, blood_pressure = 84.0f, bmi = 35.9f, age = 51.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 179.0f, blood_pressure = 90.0f, bmi = 44.1f, age = 23.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 164.0f, blood_pressure = 84.0f, bmi = 30.8f, age = 32.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 104.0f, blood_pressure = 76.0f, bmi = 18.4f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 91.0f, blood_pressure = 64.0f, bmi = 29.2f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 91.0f, blood_pressure = 70.0f, bmi = 33.1f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 139.0f, blood_pressure = 54.0f, bmi = 25.6f, age = 22.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 119.0f, blood_pressure = 50.0f, bmi = 27.1f, age = 33.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 146.0f, blood_pressure = 76.0f, bmi = 38.2f, age = 29.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 184.0f, blood_pressure = 85.0f, bmi = 30.0f, age = 49.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 122.0f, blood_pressure = 68.0f, bmi = 31.2f, age = 41.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 165.0f, blood_pressure = 90.0f, bmi = 52.3f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 124.0f, blood_pressure = 70.0f, bmi = 35.4f, age = 34.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 111.0f, blood_pressure = 86.0f, bmi = 30.1f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 106.0f, blood_pressure = 52.0f, bmi = 31.2f, age = 42.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 129.0f, blood_pressure = 84.0f, bmi = 28.0f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 90.0f, blood_pressure = 80.0f, bmi = 24.4f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 86.0f, blood_pressure = 68.0f, bmi = 35.8f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 92.0f, blood_pressure = 62.0f, bmi = 27.6f, age = 44.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 113.0f, blood_pressure = 64.0f, bmi = 33.6f, age = 21.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 111.0f, blood_pressure = 56.0f, bmi = 30.1f, age = 30.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 114.0f, blood_pressure = 68.0f, bmi = 28.7f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 193.0f, blood_pressure = 50.0f, bmi = 25.9f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 155.0f, blood_pressure = 76.0f, bmi = 33.3f, age = 51.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 191.0f, blood_pressure = 68.0f, bmi = 30.9f, age = 34.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 141.0f, blood_pressure = 0.0f, bmi = 30.0f, age = 27.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 95.0f, blood_pressure = 70.0f, bmi = 32.1f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 142.0f, blood_pressure = 80.0f, bmi = 32.4f, age = 63.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 123.0f, blood_pressure = 62.0f, bmi = 32.0f, age = 35.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 96.0f, blood_pressure = 74.0f, bmi = 33.6f, age = 43.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 138.0f, blood_pressure = 0.0f, bmi = 36.3f, age = 25.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 128.0f, blood_pressure = 64.0f, bmi = 40.0f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 102.0f, blood_pressure = 52.0f, bmi = 25.1f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 146.0f, blood_pressure = 0.0f, bmi = 27.5f, age = 28.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 101.0f, blood_pressure = 86.0f, bmi = 45.6f, age = 38.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 108.0f, blood_pressure = 62.0f, bmi = 25.2f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 122.0f, blood_pressure = 78.0f, bmi = 23.0f, age = 40.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 71.0f, blood_pressure = 78.0f, bmi = 33.2f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 106.0f, blood_pressure = 70.0f, bmi = 34.2f, age = 52.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 100.0f, blood_pressure = 70.0f, bmi = 40.5f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 106.0f, blood_pressure = 60.0f, bmi = 26.5f, age = 29.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 104.0f, blood_pressure = 64.0f, bmi = 27.8f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 114.0f, blood_pressure = 74.0f, bmi = 24.9f, age = 57.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 108.0f, blood_pressure = 62.0f, bmi = 25.3f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 146.0f, blood_pressure = 70.0f, bmi = 37.9f, age = 28.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 129.0f, blood_pressure = 76.0f, bmi = 35.9f, age = 39.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 133.0f, blood_pressure = 88.0f, bmi = 32.4f, age = 37.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 161.0f, blood_pressure = 86.0f, bmi = 30.4f, age = 47.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 108.0f, blood_pressure = 80.0f, bmi = 27.0f, age = 52.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 136.0f, blood_pressure = 74.0f, bmi = 26.0f, age = 51.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 155.0f, blood_pressure = 84.0f, bmi = 38.7f, age = 34.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 119.0f, blood_pressure = 86.0f, bmi = 45.6f, age = 29.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 96.0f, blood_pressure = 56.0f, bmi = 20.8f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 108.0f, blood_pressure = 72.0f, bmi = 36.1f, age = 33.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 78.0f, blood_pressure = 88.0f, bmi = 36.9f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 107.0f, blood_pressure = 62.0f, bmi = 36.6f, age = 25.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 128.0f, blood_pressure = 78.0f, bmi = 43.3f, age = 31.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 128.0f, blood_pressure = 48.0f, bmi = 40.5f, age = 24.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 161.0f, blood_pressure = 50.0f, bmi = 21.9f, age = 65.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 151.0f, blood_pressure = 62.0f, bmi = 35.5f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 146.0f, blood_pressure = 70.0f, bmi = 28.0f, age = 29.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 126.0f, blood_pressure = 84.0f, bmi = 30.7f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 100.0f, blood_pressure = 78.0f, bmi = 36.6f, age = 46.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 112.0f, blood_pressure = 72.0f, bmi = 23.6f, age = 58.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 167.0f, blood_pressure = 0.0f, bmi = 32.3f, age = 30.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 144.0f, blood_pressure = 58.0f, bmi = 31.6f, age = 25.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 77.0f, blood_pressure = 82.0f, bmi = 35.8f, age = 35.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 115.0f, blood_pressure = 98.0f, bmi = 52.9f, age = 28.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 150.0f, blood_pressure = 76.0f, bmi = 21.0f, age = 37.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 120.0f, blood_pressure = 76.0f, bmi = 39.7f, age = 29.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 161.0f, blood_pressure = 68.0f, bmi = 25.5f, age = 47.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 137.0f, blood_pressure = 68.0f, bmi = 24.8f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 128.0f, blood_pressure = 68.0f, bmi = 30.5f, age = 25.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 124.0f, blood_pressure = 68.0f, bmi = 32.9f, age = 30.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 80.0f, blood_pressure = 66.0f, bmi = 26.2f, age = 41.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 106.0f, blood_pressure = 70.0f, bmi = 39.4f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 155.0f, blood_pressure = 74.0f, bmi = 26.6f, age = 27.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 113.0f, blood_pressure = 50.0f, bmi = 29.5f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 109.0f, blood_pressure = 80.0f, bmi = 35.9f, age = 43.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 112.0f, blood_pressure = 68.0f, bmi = 34.1f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 99.0f, blood_pressure = 80.0f, bmi = 19.3f, age = 30.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 182.0f, blood_pressure = 74.0f, bmi = 30.5f, age = 29.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 115.0f, blood_pressure = 66.0f, bmi = 38.1f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 194.0f, blood_pressure = 78.0f, bmi = 23.5f, age = 59.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 129.0f, blood_pressure = 60.0f, bmi = 27.5f, age = 31.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 112.0f, blood_pressure = 74.0f, bmi = 31.6f, age = 25.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 124.0f, blood_pressure = 70.0f, bmi = 27.4f, age = 36.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 152.0f, blood_pressure = 90.0f, bmi = 26.8f, age = 43.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 112.0f, blood_pressure = 75.0f, bmi = 35.7f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 157.0f, blood_pressure = 72.0f, bmi = 25.6f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 122.0f, blood_pressure = 64.0f, bmi = 35.1f, age = 30.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 179.0f, blood_pressure = 70.0f, bmi = 35.1f, age = 37.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 102.0f, blood_pressure = 86.0f, bmi = 45.5f, age = 23.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 105.0f, blood_pressure = 70.0f, bmi = 30.8f, age = 37.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 118.0f, blood_pressure = 72.0f, bmi = 23.1f, age = 46.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 87.0f, blood_pressure = 58.0f, bmi = 32.7f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 180.0f, blood_pressure = 0.0f, bmi = 43.3f, age = 41.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 106.0f, blood_pressure = 80.0f, bmi = 23.6f, age = 44.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 95.0f, blood_pressure = 60.0f, bmi = 23.9f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 165.0f, blood_pressure = 76.0f, bmi = 47.9f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 117.0f, blood_pressure = 0.0f, bmi = 33.8f, age = 44.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 115.0f, blood_pressure = 76.0f, bmi = 31.2f, age = 44.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 152.0f, blood_pressure = 78.0f, bmi = 34.2f, age = 33.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 178.0f, blood_pressure = 84.0f, bmi = 39.9f, age = 41.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 130.0f, blood_pressure = 70.0f, bmi = 25.9f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 95.0f, blood_pressure = 74.0f, bmi = 25.9f, age = 36.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 0.0f, blood_pressure = 68.0f, bmi = 32.0f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 122.0f, blood_pressure = 86.0f, bmi = 34.7f, age = 33.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 95.0f, blood_pressure = 72.0f, bmi = 36.8f, age = 57.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 126.0f, blood_pressure = 88.0f, bmi = 38.5f, age = 49.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 139.0f, blood_pressure = 46.0f, bmi = 28.7f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 116.0f, blood_pressure = 0.0f, bmi = 23.5f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 99.0f, blood_pressure = 62.0f, bmi = 21.8f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 0.0f, blood_pressure = 80.0f, bmi = 41.0f, age = 37.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 92.0f, blood_pressure = 80.0f, bmi = 42.2f, age = 29.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 137.0f, blood_pressure = 84.0f, bmi = 31.2f, age = 30.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 61.0f, blood_pressure = 82.0f, bmi = 34.4f, age = 46.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 90.0f, blood_pressure = 62.0f, bmi = 27.2f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 90.0f, blood_pressure = 78.0f, bmi = 42.7f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 165.0f, blood_pressure = 88.0f, bmi = 30.4f, age = 49.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 125.0f, blood_pressure = 50.0f, bmi = 33.3f, age = 28.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 129.0f, blood_pressure = 0.0f, bmi = 39.9f, age = 44.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 88.0f, blood_pressure = 74.0f, bmi = 35.3f, age = 48.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 196.0f, blood_pressure = 76.0f, bmi = 36.5f, age = 29.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 189.0f, blood_pressure = 64.0f, bmi = 31.2f, age = 29.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 158.0f, blood_pressure = 70.0f, bmi = 29.8f, age = 63.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 103.0f, blood_pressure = 108.0f, bmi = 39.2f, age = 65.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 146.0f, blood_pressure = 78.0f, bmi = 38.5f, age = 67.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 147.0f, blood_pressure = 74.0f, bmi = 34.9f, age = 30.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 99.0f, blood_pressure = 54.0f, bmi = 34.0f, age = 30.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 124.0f, blood_pressure = 72.0f, bmi = 27.6f, age = 29.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 101.0f, blood_pressure = 64.0f, bmi = 21.0f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 81.0f, blood_pressure = 86.0f, bmi = 27.5f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 133.0f, blood_pressure = 102.0f, bmi = 32.8f, age = 45.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 173.0f, blood_pressure = 82.0f, bmi = 38.4f, age = 25.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 118.0f, blood_pressure = 64.0f, bmi = 0.0f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 84.0f, blood_pressure = 64.0f, bmi = 35.8f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 105.0f, blood_pressure = 58.0f, bmi = 34.9f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 122.0f, blood_pressure = 52.0f, bmi = 36.2f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 140.0f, blood_pressure = 82.0f, bmi = 39.2f, age = 58.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 98.0f, blood_pressure = 82.0f, bmi = 25.2f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 87.0f, blood_pressure = 60.0f, bmi = 37.2f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 156.0f, blood_pressure = 75.0f, bmi = 48.3f, age = 32.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 93.0f, blood_pressure = 100.0f, bmi = 43.4f, age = 35.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 107.0f, blood_pressure = 72.0f, bmi = 30.8f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 105.0f, blood_pressure = 68.0f, bmi = 20.0f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 109.0f, blood_pressure = 60.0f, bmi = 25.4f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 90.0f, blood_pressure = 62.0f, bmi = 25.1f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 125.0f, blood_pressure = 70.0f, bmi = 24.3f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 119.0f, blood_pressure = 54.0f, bmi = 22.3f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 116.0f, blood_pressure = 74.0f, bmi = 32.3f, age = 35.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 105.0f, blood_pressure = 100.0f, bmi = 43.3f, age = 45.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 144.0f, blood_pressure = 82.0f, bmi = 32.0f, age = 58.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 100.0f, blood_pressure = 68.0f, bmi = 31.6f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 100.0f, blood_pressure = 66.0f, bmi = 32.0f, age = 42.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 166.0f, blood_pressure = 76.0f, bmi = 45.7f, age = 27.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 131.0f, blood_pressure = 64.0f, bmi = 23.7f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 116.0f, blood_pressure = 72.0f, bmi = 22.1f, age = 37.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 158.0f, blood_pressure = 78.0f, bmi = 32.9f, age = 31.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 127.0f, blood_pressure = 58.0f, bmi = 27.7f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 96.0f, blood_pressure = 56.0f, bmi = 24.7f, age = 39.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 131.0f, blood_pressure = 66.0f, bmi = 34.3f, age = 22.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 82.0f, blood_pressure = 70.0f, bmi = 21.1f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 193.0f, blood_pressure = 70.0f, bmi = 34.9f, age = 25.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 95.0f, blood_pressure = 64.0f, bmi = 32.0f, age = 31.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 137.0f, blood_pressure = 61.0f, bmi = 24.2f, age = 55.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 136.0f, blood_pressure = 84.0f, bmi = 35.0f, age = 35.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 72.0f, blood_pressure = 78.0f, bmi = 31.6f, age = 38.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 168.0f, blood_pressure = 64.0f, bmi = 32.9f, age = 41.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 123.0f, blood_pressure = 48.0f, bmi = 42.1f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 115.0f, blood_pressure = 72.0f, bmi = 28.9f, age = 46.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 101.0f, blood_pressure = 62.0f, bmi = 21.9f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 197.0f, blood_pressure = 74.0f, bmi = 25.9f, age = 39.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 172.0f, blood_pressure = 68.0f, bmi = 42.4f, age = 28.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 102.0f, blood_pressure = 90.0f, bmi = 35.7f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 112.0f, blood_pressure = 72.0f, bmi = 34.4f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 143.0f, blood_pressure = 84.0f, bmi = 42.4f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 143.0f, blood_pressure = 74.0f, bmi = 26.2f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 138.0f, blood_pressure = 60.0f, bmi = 34.6f, age = 21.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 173.0f, blood_pressure = 84.0f, bmi = 35.7f, age = 22.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 97.0f, blood_pressure = 68.0f, bmi = 27.2f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 144.0f, blood_pressure = 82.0f, bmi = 38.5f, age = 37.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 83.0f, blood_pressure = 68.0f, bmi = 18.2f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 129.0f, blood_pressure = 64.0f, bmi = 26.4f, age = 28.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 119.0f, blood_pressure = 88.0f, bmi = 45.3f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 94.0f, blood_pressure = 68.0f, bmi = 26.0f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 102.0f, blood_pressure = 64.0f, bmi = 40.6f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 115.0f, blood_pressure = 64.0f, bmi = 30.8f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 151.0f, blood_pressure = 78.0f, bmi = 42.9f, age = 36.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 184.0f, blood_pressure = 78.0f, bmi = 37.0f, age = 31.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 94.0f, blood_pressure = 0.0f, bmi = 0.0f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 181.0f, blood_pressure = 64.0f, bmi = 34.1f, age = 38.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 135.0f, blood_pressure = 94.0f, bmi = 40.6f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 95.0f, blood_pressure = 82.0f, bmi = 35.0f, age = 43.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 99.0f, blood_pressure = 0.0f, bmi = 22.2f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 89.0f, blood_pressure = 74.0f, bmi = 30.4f, age = 38.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 80.0f, blood_pressure = 74.0f, bmi = 30.0f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 139.0f, blood_pressure = 75.0f, bmi = 25.6f, age = 29.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 90.0f, blood_pressure = 68.0f, bmi = 24.5f, age = 36.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 141.0f, blood_pressure = 0.0f, bmi = 42.4f, age = 29.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 140.0f, blood_pressure = 85.0f, bmi = 37.4f, age = 41.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 147.0f, blood_pressure = 75.0f, bmi = 29.9f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 97.0f, blood_pressure = 70.0f, bmi = 18.2f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 107.0f, blood_pressure = 88.0f, bmi = 36.8f, age = 31.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 189.0f, blood_pressure = 104.0f, bmi = 34.3f, age = 41.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 83.0f, blood_pressure = 66.0f, bmi = 32.2f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 117.0f, blood_pressure = 64.0f, bmi = 33.2f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 108.0f, blood_pressure = 70.0f, bmi = 30.5f, age = 33.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 117.0f, blood_pressure = 62.0f, bmi = 29.7f, age = 30.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 180.0f, blood_pressure = 78.0f, bmi = 59.4f, age = 25.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 100.0f, blood_pressure = 72.0f, bmi = 25.3f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 95.0f, blood_pressure = 80.0f, bmi = 36.5f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 104.0f, blood_pressure = 64.0f, bmi = 33.6f, age = 22.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 120.0f, blood_pressure = 74.0f, bmi = 30.5f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 82.0f, blood_pressure = 64.0f, bmi = 21.2f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 134.0f, blood_pressure = 70.0f, bmi = 28.9f, age = 23.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 91.0f, blood_pressure = 68.0f, bmi = 39.9f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 119.0f, blood_pressure = 0.0f, bmi = 19.6f, age = 72.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 100.0f, blood_pressure = 54.0f, bmi = 37.8f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 175.0f, blood_pressure = 62.0f, bmi = 33.6f, age = 38.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 135.0f, blood_pressure = 54.0f, bmi = 26.7f, age = 62.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 86.0f, blood_pressure = 68.0f, bmi = 30.2f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 148.0f, blood_pressure = 84.0f, bmi = 37.6f, age = 51.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 134.0f, blood_pressure = 74.0f, bmi = 25.9f, age = 81.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 120.0f, blood_pressure = 72.0f, bmi = 20.8f, age = 48.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 71.0f, blood_pressure = 62.0f, bmi = 21.8f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 74.0f, blood_pressure = 70.0f, bmi = 35.3f, age = 39.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 88.0f, blood_pressure = 78.0f, bmi = 27.6f, age = 37.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 115.0f, blood_pressure = 98.0f, bmi = 24.0f, age = 34.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 124.0f, blood_pressure = 56.0f, bmi = 21.8f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 74.0f, blood_pressure = 52.0f, bmi = 27.8f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 97.0f, blood_pressure = 64.0f, bmi = 36.8f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 120.0f, blood_pressure = 0.0f, bmi = 30.0f, age = 38.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 154.0f, blood_pressure = 78.0f, bmi = 46.1f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 144.0f, blood_pressure = 82.0f, bmi = 41.3f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 137.0f, blood_pressure = 70.0f, bmi = 33.2f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 119.0f, blood_pressure = 66.0f, bmi = 38.8f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 136.0f, blood_pressure = 90.0f, bmi = 29.9f, age = 50.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 114.0f, blood_pressure = 64.0f, bmi = 28.9f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 137.0f, blood_pressure = 84.0f, bmi = 27.3f, age = 59.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 105.0f, blood_pressure = 80.0f, bmi = 33.7f, age = 29.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 114.0f, blood_pressure = 76.0f, bmi = 23.8f, age = 31.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 126.0f, blood_pressure = 74.0f, bmi = 25.9f, age = 39.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 132.0f, blood_pressure = 86.0f, bmi = 28.0f, age = 63.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 158.0f, blood_pressure = 70.0f, bmi = 35.5f, age = 35.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 123.0f, blood_pressure = 88.0f, bmi = 35.2f, age = 29.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 85.0f, blood_pressure = 58.0f, bmi = 27.8f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 84.0f, blood_pressure = 82.0f, bmi = 38.2f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 145.0f, blood_pressure = 0.0f, bmi = 44.2f, age = 31.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 135.0f, blood_pressure = 68.0f, bmi = 42.3f, age = 24.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 139.0f, blood_pressure = 62.0f, bmi = 40.7f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 173.0f, blood_pressure = 78.0f, bmi = 46.5f, age = 58.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 99.0f, blood_pressure = 72.0f, bmi = 25.6f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 194.0f, blood_pressure = 80.0f, bmi = 26.1f, age = 67.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 83.0f, blood_pressure = 65.0f, bmi = 36.8f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 89.0f, blood_pressure = 90.0f, bmi = 33.5f, age = 42.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 99.0f, blood_pressure = 68.0f, bmi = 32.8f, age = 33.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 125.0f, blood_pressure = 70.0f, bmi = 28.9f, age = 45.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 80.0f, blood_pressure = 0.0f, bmi = 0.0f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 166.0f, blood_pressure = 74.0f, bmi = 26.6f, age = 66.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 110.0f, blood_pressure = 68.0f, bmi = 26.0f, age = 30.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 81.0f, blood_pressure = 72.0f, bmi = 30.1f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 195.0f, blood_pressure = 70.0f, bmi = 25.1f, age = 55.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 154.0f, blood_pressure = 74.0f, bmi = 29.3f, age = 39.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 117.0f, blood_pressure = 90.0f, bmi = 25.2f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 84.0f, blood_pressure = 72.0f, bmi = 37.2f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 0.0f, blood_pressure = 68.0f, bmi = 39.0f, age = 41.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 94.0f, blood_pressure = 64.0f, bmi = 33.3f, age = 41.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 96.0f, blood_pressure = 78.0f, bmi = 37.3f, age = 40.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 75.0f, blood_pressure = 82.0f, bmi = 33.3f, age = 38.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 180.0f, blood_pressure = 90.0f, bmi = 36.5f, age = 35.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 130.0f, blood_pressure = 60.0f, bmi = 28.6f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 84.0f, blood_pressure = 50.0f, bmi = 30.4f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 120.0f, blood_pressure = 78.0f, bmi = 25.0f, age = 64.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 84.0f, blood_pressure = 72.0f, bmi = 29.7f, age = 46.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 139.0f, blood_pressure = 62.0f, bmi = 22.1f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 91.0f, blood_pressure = 68.0f, bmi = 24.2f, age = 58.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 91.0f, blood_pressure = 62.0f, bmi = 27.3f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 99.0f, blood_pressure = 54.0f, bmi = 25.6f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 163.0f, blood_pressure = 70.0f, bmi = 31.6f, age = 28.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 145.0f, blood_pressure = 88.0f, bmi = 30.3f, age = 53.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 125.0f, blood_pressure = 86.0f, bmi = 37.6f, age = 51.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 76.0f, blood_pressure = 60.0f, bmi = 32.8f, age = 41.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 129.0f, blood_pressure = 90.0f, bmi = 19.6f, age = 60.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 68.0f, blood_pressure = 70.0f, bmi = 25.0f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 124.0f, blood_pressure = 80.0f, bmi = 33.2f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 114.0f, blood_pressure = 0.0f, bmi = 0.0f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 130.0f, blood_pressure = 70.0f, bmi = 34.2f, age = 45.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 125.0f, blood_pressure = 58.0f, bmi = 31.6f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 87.0f, blood_pressure = 60.0f, bmi = 21.8f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 97.0f, blood_pressure = 64.0f, bmi = 18.2f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 116.0f, blood_pressure = 74.0f, bmi = 26.3f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 117.0f, blood_pressure = 66.0f, bmi = 30.8f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 111.0f, blood_pressure = 65.0f, bmi = 24.6f, age = 31.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 122.0f, blood_pressure = 60.0f, bmi = 29.8f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 107.0f, blood_pressure = 76.0f, bmi = 45.3f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 86.0f, blood_pressure = 66.0f, bmi = 41.3f, age = 29.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 91.0f, blood_pressure = 0.0f, bmi = 29.8f, age = 31.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 77.0f, blood_pressure = 56.0f, bmi = 33.3f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 132.0f, blood_pressure = 0.0f, bmi = 32.9f, age = 23.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 105.0f, blood_pressure = 90.0f, bmi = 29.6f, age = 46.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 57.0f, blood_pressure = 60.0f, bmi = 21.7f, age = 67.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 127.0f, blood_pressure = 80.0f, bmi = 36.3f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 129.0f, blood_pressure = 92.0f, bmi = 36.4f, age = 32.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 100.0f, blood_pressure = 74.0f, bmi = 39.4f, age = 43.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 128.0f, blood_pressure = 72.0f, bmi = 32.4f, age = 27.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 90.0f, blood_pressure = 85.0f, bmi = 34.9f, age = 56.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 84.0f, blood_pressure = 90.0f, bmi = 39.5f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 88.0f, blood_pressure = 78.0f, bmi = 32.0f, age = 29.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 186.0f, blood_pressure = 90.0f, bmi = 34.5f, age = 37.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 187.0f, blood_pressure = 76.0f, bmi = 43.6f, age = 53.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 131.0f, blood_pressure = 68.0f, bmi = 33.1f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 164.0f, blood_pressure = 82.0f, bmi = 32.8f, age = 50.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 189.0f, blood_pressure = 110.0f, bmi = 28.5f, age = 37.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 116.0f, blood_pressure = 70.0f, bmi = 27.4f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 84.0f, blood_pressure = 68.0f, bmi = 31.9f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 114.0f, blood_pressure = 88.0f, bmi = 27.8f, age = 66.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 88.0f, blood_pressure = 62.0f, bmi = 29.9f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 84.0f, blood_pressure = 64.0f, bmi = 36.9f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 124.0f, blood_pressure = 70.0f, bmi = 25.5f, age = 37.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 97.0f, blood_pressure = 70.0f, bmi = 38.1f, age = 30.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 110.0f, blood_pressure = 76.0f, bmi = 27.8f, age = 58.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 103.0f, blood_pressure = 68.0f, bmi = 46.2f, age = 42.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 85.0f, blood_pressure = 74.0f, bmi = 30.1f, age = 35.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 125.0f, blood_pressure = 76.0f, bmi = 33.8f, age = 54.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 198.0f, blood_pressure = 66.0f, bmi = 41.3f, age = 28.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 87.0f, blood_pressure = 68.0f, bmi = 37.6f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 99.0f, blood_pressure = 60.0f, bmi = 26.9f, age = 32.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 91.0f, blood_pressure = 80.0f, bmi = 32.4f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 95.0f, blood_pressure = 54.0f, bmi = 26.1f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 99.0f, blood_pressure = 72.0f, bmi = 38.6f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 92.0f, blood_pressure = 62.0f, bmi = 32.0f, age = 46.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 154.0f, blood_pressure = 72.0f, bmi = 31.3f, age = 37.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 121.0f, blood_pressure = 66.0f, bmi = 34.3f, age = 33.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 78.0f, blood_pressure = 70.0f, bmi = 32.5f, age = 39.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 130.0f, blood_pressure = 96.0f, bmi = 22.6f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 111.0f, blood_pressure = 58.0f, bmi = 29.5f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 98.0f, blood_pressure = 60.0f, bmi = 34.7f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 143.0f, blood_pressure = 86.0f, bmi = 30.1f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 119.0f, blood_pressure = 44.0f, bmi = 35.5f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 108.0f, blood_pressure = 44.0f, bmi = 24.0f, age = 35.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 118.0f, blood_pressure = 80.0f, bmi = 42.9f, age = 21.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 133.0f, blood_pressure = 68.0f, bmi = 27.0f, age = 36.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 197.0f, blood_pressure = 70.0f, bmi = 34.7f, age = 62.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 151.0f, blood_pressure = 90.0f, bmi = 42.1f, age = 21.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 109.0f, blood_pressure = 60.0f, bmi = 25.0f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 121.0f, blood_pressure = 78.0f, bmi = 26.5f, age = 62.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 100.0f, blood_pressure = 76.0f, bmi = 38.7f, age = 42.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 124.0f, blood_pressure = 76.0f, bmi = 28.7f, age = 52.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 93.0f, blood_pressure = 56.0f, bmi = 22.5f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 143.0f, blood_pressure = 66.0f, bmi = 34.9f, age = 41.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 103.0f, blood_pressure = 66.0f, bmi = 24.3f, age = 29.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 176.0f, blood_pressure = 86.0f, bmi = 33.3f, age = 52.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 73.0f, blood_pressure = 0.0f, bmi = 21.1f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 111.0f, blood_pressure = 84.0f, bmi = 46.8f, age = 45.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 112.0f, blood_pressure = 78.0f, bmi = 39.4f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 132.0f, blood_pressure = 80.0f, bmi = 34.4f, age = 44.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 82.0f, blood_pressure = 52.0f, bmi = 28.5f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 123.0f, blood_pressure = 72.0f, bmi = 33.6f, age = 34.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 188.0f, blood_pressure = 82.0f, bmi = 32.0f, age = 22.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 67.0f, blood_pressure = 76.0f, bmi = 45.3f, age = 46.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 89.0f, blood_pressure = 24.0f, bmi = 27.8f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 173.0f, blood_pressure = 74.0f, bmi = 36.8f, age = 38.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 109.0f, blood_pressure = 38.0f, bmi = 23.1f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 108.0f, blood_pressure = 88.0f, bmi = 27.1f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 96.0f, blood_pressure = 0.0f, bmi = 23.7f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 124.0f, blood_pressure = 74.0f, bmi = 27.8f, age = 30.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 150.0f, blood_pressure = 78.0f, bmi = 35.2f, age = 54.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 183.0f, blood_pressure = 0.0f, bmi = 28.4f, age = 36.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 124.0f, blood_pressure = 60.0f, bmi = 35.8f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 181.0f, blood_pressure = 78.0f, bmi = 40.0f, age = 22.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 92.0f, blood_pressure = 62.0f, bmi = 19.5f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 152.0f, blood_pressure = 82.0f, bmi = 41.5f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 111.0f, blood_pressure = 62.0f, bmi = 24.0f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 106.0f, blood_pressure = 54.0f, bmi = 30.9f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 174.0f, blood_pressure = 58.0f, bmi = 32.9f, age = 36.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 168.0f, blood_pressure = 88.0f, bmi = 38.2f, age = 40.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 105.0f, blood_pressure = 80.0f, bmi = 32.5f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 138.0f, blood_pressure = 74.0f, bmi = 36.1f, age = 50.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 106.0f, blood_pressure = 72.0f, bmi = 25.8f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 117.0f, blood_pressure = 96.0f, bmi = 28.7f, age = 30.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 68.0f, blood_pressure = 62.0f, bmi = 20.1f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 112.0f, blood_pressure = 82.0f, bmi = 28.2f, age = 50.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 119.0f, blood_pressure = 0.0f, bmi = 32.4f, age = 24.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 112.0f, blood_pressure = 86.0f, bmi = 38.4f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 92.0f, blood_pressure = 76.0f, bmi = 24.2f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 183.0f, blood_pressure = 94.0f, bmi = 40.8f, age = 45.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 94.0f, blood_pressure = 70.0f, bmi = 43.5f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 108.0f, blood_pressure = 64.0f, bmi = 30.8f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 90.0f, blood_pressure = 88.0f, bmi = 37.7f, age = 29.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 125.0f, blood_pressure = 68.0f, bmi = 24.7f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 132.0f, blood_pressure = 78.0f, bmi = 32.4f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 128.0f, blood_pressure = 80.0f, bmi = 34.6f, age = 45.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 94.0f, blood_pressure = 65.0f, bmi = 24.7f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 114.0f, blood_pressure = 64.0f, bmi = 27.4f, age = 34.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 102.0f, blood_pressure = 78.0f, bmi = 34.5f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 111.0f, blood_pressure = 60.0f, bmi = 26.2f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 128.0f, blood_pressure = 82.0f, bmi = 27.5f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 92.0f, blood_pressure = 62.0f, bmi = 25.9f, age = 31.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 104.0f, blood_pressure = 72.0f, bmi = 31.2f, age = 38.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 104.0f, blood_pressure = 74.0f, bmi = 28.8f, age = 48.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 94.0f, blood_pressure = 76.0f, bmi = 31.6f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 97.0f, blood_pressure = 76.0f, bmi = 40.9f, age = 32.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 100.0f, blood_pressure = 74.0f, bmi = 19.5f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 102.0f, blood_pressure = 86.0f, bmi = 29.3f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 128.0f, blood_pressure = 70.0f, bmi = 34.3f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 147.0f, blood_pressure = 80.0f, bmi = 29.5f, age = 50.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 90.0f, blood_pressure = 0.0f, bmi = 28.0f, age = 31.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 103.0f, blood_pressure = 72.0f, bmi = 27.6f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 157.0f, blood_pressure = 74.0f, bmi = 39.4f, age = 30.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 167.0f, blood_pressure = 74.0f, bmi = 23.4f, age = 33.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 179.0f, blood_pressure = 50.0f, bmi = 37.8f, age = 22.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 136.0f, blood_pressure = 84.0f, bmi = 28.3f, age = 42.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 107.0f, blood_pressure = 60.0f, bmi = 26.4f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 91.0f, blood_pressure = 54.0f, bmi = 25.2f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 117.0f, blood_pressure = 60.0f, bmi = 33.8f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 123.0f, blood_pressure = 74.0f, bmi = 34.1f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 120.0f, blood_pressure = 54.0f, bmi = 26.8f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 106.0f, blood_pressure = 70.0f, bmi = 34.2f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 155.0f, blood_pressure = 52.0f, bmi = 38.7f, age = 25.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 101.0f, blood_pressure = 58.0f, bmi = 21.8f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 120.0f, blood_pressure = 80.0f, bmi = 38.9f, age = 41.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 127.0f, blood_pressure = 106.0f, bmi = 39.0f, age = 51.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 80.0f, blood_pressure = 82.0f, bmi = 34.2f, age = 27.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 162.0f, blood_pressure = 84.0f, bmi = 27.7f, age = 54.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 199.0f, blood_pressure = 76.0f, bmi = 42.9f, age = 22.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 167.0f, blood_pressure = 106.0f, bmi = 37.6f, age = 43.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 145.0f, blood_pressure = 80.0f, bmi = 37.9f, age = 40.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 115.0f, blood_pressure = 60.0f, bmi = 33.7f, age = 40.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 112.0f, blood_pressure = 80.0f, bmi = 34.8f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 145.0f, blood_pressure = 82.0f, bmi = 32.5f, age = 70.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 111.0f, blood_pressure = 70.0f, bmi = 27.5f, age = 40.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 98.0f, blood_pressure = 58.0f, bmi = 34.0f, age = 43.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 154.0f, blood_pressure = 78.0f, bmi = 30.9f, age = 45.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 165.0f, blood_pressure = 68.0f, bmi = 33.6f, age = 49.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 99.0f, blood_pressure = 58.0f, bmi = 25.4f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 68.0f, blood_pressure = 106.0f, bmi = 35.5f, age = 47.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 123.0f, blood_pressure = 100.0f, bmi = 57.3f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 91.0f, blood_pressure = 82.0f, bmi = 35.6f, age = 68.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 195.0f, blood_pressure = 70.0f, bmi = 30.9f, age = 31.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 156.0f, blood_pressure = 86.0f, bmi = 24.8f, age = 53.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 93.0f, blood_pressure = 60.0f, bmi = 35.3f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 121.0f, blood_pressure = 52.0f, bmi = 36.0f, age = 25.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 101.0f, blood_pressure = 58.0f, bmi = 24.2f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 56.0f, blood_pressure = 56.0f, bmi = 24.2f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 162.0f, blood_pressure = 76.0f, bmi = 49.6f, age = 26.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 95.0f, blood_pressure = 64.0f, bmi = 44.6f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 125.0f, blood_pressure = 80.0f, bmi = 32.3f, age = 27.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 136.0f, blood_pressure = 82.0f, bmi = 0.0f, age = 69.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 129.0f, blood_pressure = 74.0f, bmi = 33.2f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 130.0f, blood_pressure = 64.0f, bmi = 23.1f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 107.0f, blood_pressure = 50.0f, bmi = 28.3f, age = 29.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 140.0f, blood_pressure = 74.0f, bmi = 24.1f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 144.0f, blood_pressure = 82.0f, bmi = 46.1f, age = 46.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 107.0f, blood_pressure = 80.0f, bmi = 24.6f, age = 34.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 158.0f, blood_pressure = 114.0f, bmi = 42.3f, age = 44.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 121.0f, blood_pressure = 70.0f, bmi = 39.1f, age = 23.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 129.0f, blood_pressure = 68.0f, bmi = 38.5f, age = 43.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 90.0f, blood_pressure = 60.0f, bmi = 23.5f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 142.0f, blood_pressure = 90.0f, bmi = 30.4f, age = 43.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 169.0f, blood_pressure = 74.0f, bmi = 29.9f, age = 31.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 99.0f, blood_pressure = 0.0f, bmi = 25.0f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 127.0f, blood_pressure = 88.0f, bmi = 34.5f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 118.0f, blood_pressure = 70.0f, bmi = 44.5f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 122.0f, blood_pressure = 76.0f, bmi = 35.9f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 125.0f, blood_pressure = 78.0f, bmi = 27.6f, age = 49.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 168.0f, blood_pressure = 88.0f, bmi = 35.0f, age = 52.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 129.0f, blood_pressure = 0.0f, bmi = 38.5f, age = 41.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 110.0f, blood_pressure = 76.0f, bmi = 28.4f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 80.0f, blood_pressure = 80.0f, bmi = 39.8f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 115.0f, blood_pressure = 0.0f, bmi = 0.0f, age = 30.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 127.0f, blood_pressure = 46.0f, bmi = 34.4f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 164.0f, blood_pressure = 78.0f, bmi = 32.8f, age = 45.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 93.0f, blood_pressure = 64.0f, bmi = 38.0f, age = 23.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 158.0f, blood_pressure = 64.0f, bmi = 31.2f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 126.0f, blood_pressure = 78.0f, bmi = 29.6f, age = 40.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 129.0f, blood_pressure = 62.0f, bmi = 41.2f, age = 38.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 134.0f, blood_pressure = 58.0f, bmi = 26.4f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 102.0f, blood_pressure = 74.0f, bmi = 29.5f, age = 32.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 187.0f, blood_pressure = 50.0f, bmi = 33.9f, age = 34.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 173.0f, blood_pressure = 78.0f, bmi = 33.8f, age = 31.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 94.0f, blood_pressure = 72.0f, bmi = 23.1f, age = 56.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 108.0f, blood_pressure = 60.0f, bmi = 35.5f, age = 24.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 97.0f, blood_pressure = 76.0f, bmi = 35.6f, age = 52.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 83.0f, blood_pressure = 86.0f, bmi = 29.3f, age = 34.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 114.0f, blood_pressure = 66.0f, bmi = 38.1f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 149.0f, blood_pressure = 68.0f, bmi = 29.3f, age = 42.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 117.0f, blood_pressure = 86.0f, bmi = 39.1f, age = 42.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 111.0f, blood_pressure = 94.0f, bmi = 32.8f, age = 45.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 112.0f, blood_pressure = 78.0f, bmi = 39.4f, age = 38.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 116.0f, blood_pressure = 78.0f, bmi = 36.1f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 141.0f, blood_pressure = 84.0f, bmi = 32.4f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 175.0f, blood_pressure = 88.0f, bmi = 22.9f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 92.0f, blood_pressure = 52.0f, bmi = 30.1f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 130.0f, blood_pressure = 78.0f, bmi = 28.4f, age = 34.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 120.0f, blood_pressure = 86.0f, bmi = 28.4f, age = 22.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 174.0f, blood_pressure = 88.0f, bmi = 44.5f, age = 24.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 106.0f, blood_pressure = 56.0f, bmi = 29.0f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 105.0f, blood_pressure = 75.0f, bmi = 23.3f, age = 53.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 95.0f, blood_pressure = 60.0f, bmi = 35.4f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 126.0f, blood_pressure = 86.0f, bmi = 27.4f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 65.0f, blood_pressure = 72.0f, bmi = 32.0f, age = 42.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 99.0f, blood_pressure = 60.0f, bmi = 36.6f, age = 21.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 102.0f, blood_pressure = 74.0f, bmi = 39.5f, age = 42.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 120.0f, blood_pressure = 80.0f, bmi = 42.3f, age = 48.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 102.0f, blood_pressure = 44.0f, bmi = 30.8f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 109.0f, blood_pressure = 58.0f, bmi = 28.5f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 140.0f, blood_pressure = 94.0f, bmi = 32.7f, age = 45.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 153.0f, blood_pressure = 88.0f, bmi = 40.6f, age = 39.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 100.0f, blood_pressure = 84.0f, bmi = 30.0f, age = 46.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 147.0f, blood_pressure = 94.0f, bmi = 49.3f, age = 27.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 81.0f, blood_pressure = 74.0f, bmi = 46.3f, age = 32.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 187.0f, blood_pressure = 70.0f, bmi = 36.4f, age = 36.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 162.0f, blood_pressure = 62.0f, bmi = 24.3f, age = 50.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 136.0f, blood_pressure = 70.0f, bmi = 31.2f, age = 22.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 121.0f, blood_pressure = 78.0f, bmi = 39.0f, age = 28.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 108.0f, blood_pressure = 62.0f, bmi = 26.0f, age = 25.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 181.0f, blood_pressure = 88.0f, bmi = 43.3f, age = 26.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 154.0f, blood_pressure = 78.0f, bmi = 32.4f, age = 45.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 128.0f, blood_pressure = 88.0f, bmi = 36.5f, age = 37.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 137.0f, blood_pressure = 90.0f, bmi = 32.0f, age = 39.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 123.0f, blood_pressure = 72.0f, bmi = 36.3f, age = 52.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 106.0f, blood_pressure = 76.0f, bmi = 37.5f, age = 26.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 190.0f, blood_pressure = 92.0f, bmi = 35.5f, age = 66.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 88.0f, blood_pressure = 58.0f, bmi = 28.4f, age = 22.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 170.0f, blood_pressure = 74.0f, bmi = 44.0f, age = 43.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 89.0f, blood_pressure = 62.0f, bmi = 22.5f, age = 33.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 101.0f, blood_pressure = 76.0f, bmi = 32.9f, age = 63.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 122.0f, blood_pressure = 70.0f, bmi = 36.8f, age = 27.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 121.0f, blood_pressure = 72.0f, bmi = 26.2f, age = 30.0f, prediction = "Not diabetic" },
        new DiabetesDataPoint { glucose = 126.0f, blood_pressure = 60.0f, bmi = 30.1f, age = 47.0f, prediction = "Diabetic" },
        new DiabetesDataPoint { glucose = 93.0f, blood_pressure = 70.0f, bmi = 30.4f, age = 23.0f, prediction = "Not diabetic" },


    };


    void Start()
    {
        FindInputFields();

        // Ensure the objectPooler is assigned
        if (objectPooler == null)
        {
            objectPooler = FindObjectOfType<ObjectPooler>();
            if (objectPooler == null)
            {
                Debug.LogError("ObjectPooler is not assigned and couldn't be found in the scene.");
                return;
            }
        }

        // Get min points
        float xMin = dataPoints.Min(p => p.glucose);
        float yMin = dataPoints.Min(p => p.blood_pressure);
        float zMin = dataPoints.Min(p => p.bmi);

        // For every data point in the set, get a pooled object and position it
        foreach (DiabetesDataPoint point in dataPoints)
        {
            GameObject pooledObject = objectPooler.GetPooledObject();

            if (pooledObject != null)
            {
                // Set the object active
                pooledObject.SetActive(true);

                // Adjust positions based on provided offsets and invert Z-coordinate
                float scaler = 0.5f;
                float glucoseScaler = 0.5f;
                float bloodPressureScaler = 0.5f;
                float bmiScaler = 1.7f;

                Vector3 position = new Vector3(
                    point.glucose * (glucoseScaler) + 720+37 - xMin * scaler, // X-coordinate adjustment
                    point.blood_pressure * (bloodPressureScaler) + 547-15 - yMin * scaler,  // Y-coordinate adjustment
                    -point.bmi * (bmiScaler) + 56 + zMin * scaler // Z-coordinate adjustment and inversion
                );

                // Adjust rotation to point up
                Quaternion rotation = Quaternion.Euler(-90, 0, 0);

                // Set the pooled object's position and rotation
                pooledObject.transform.position = position;
                pooledObject.transform.rotation = rotation;

                // Determine scale based on age, with a chosen multiplier for visualization purposes
                float baseScale = 1f; // Minimum scale to ensure visibility
                float scaleMultiplier = .1f;
                float scale = baseScale + (point.age * scaleMultiplier);
                pooledObject.transform.localScale = new Vector3(scale, scale, scale);

                // Assign material based on the prediction
                Material chosenMaterial = point.prediction == "Diabetic" ? DiabeticMaterial : NonDiabeticMaterial;
                pooledObject.GetComponent<Renderer>().material = chosenMaterial;

                Debug.Log($"Data point pooled at: {position} with scale: {scale}");
            }
            else
            {
                Debug.LogWarning("No available pooled objects.");
            }
        }
    }

    private void FindInputFields()
    {
        glucoseInput = GameObject.Find("Glucose").GetComponent<TMP_InputField>();
        bloodPressureInput = GameObject.Find("BloodPressure").GetComponent<TMP_InputField>();
        bmiInput = GameObject.Find("BMI").GetComponent<TMP_InputField>();
        ageInput = GameObject.Find("Age").GetComponent<TMP_InputField>();

        inputFields = new TMP_InputField[] { bmiInput, ageInput, glucoseInput, bloodPressureInput };
    }

    void Update()
    {
        // Detect Tab key press
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MoveFocusToNextInputField();
        }
    }

    void MoveFocusToNextInputField()
    {
        // Get the currently focused input field
        TMP_InputField currentField = GetCurrentlyFocusedField();

        if (currentField == null)
        {
            return;
        }

        // Find the index of currently focused input field
        int currentFieldIndex = System.Array.IndexOf(inputFields, currentField);

        // Move to the next input field
        currentFieldIndex++;
        if (currentFieldIndex >= inputFields.Length)
        {
            currentFieldIndex = 0;
        }

        // Set the selected input field to be the next one
        inputFields[currentFieldIndex].Select();
    }

    TMP_InputField GetCurrentlyFocusedField()
    {
        foreach (TMP_InputField field in inputFields)
        {
            if (field.isFocused)
            {
                return field;
            }
        }
        return null;
    }

    public void AddData()
    {
        // Check if any input field is empty
        if (string.IsNullOrWhiteSpace(glucoseInput.text) ||
                string.IsNullOrWhiteSpace(bloodPressureInput.text) ||
                string.IsNullOrWhiteSpace(bmiInput.text) ||
                string.IsNullOrWhiteSpace(ageInput.text))
        {
            // Debug.LogError("All input fields must be filled.");
            ErrorText.text = "Error: All fields must be filled.";
            return; // Exit the method early if any field is empty
        }

        float[] inputs = new float[4];

        try
        {
            inputs[0] = float.Parse(glucoseInput.text);
            inputs[1] = float.Parse(bloodPressureInput.text);
            inputs[2] = float.Parse(bmiInput.text);
            inputs[3] = float.Parse(ageInput.text);
        }
        catch (FormatException)
        {
            Debug.LogError("Input is not in a correct numeric format.");
            //predictionText.text = "Error: Please enter valid numbers.";
            ErrorText.text = "Error: Please enter valid numbers.";
            return; // Exit the method if parsing fails
        }

        // Get min points
        float xMin = dataPoints.Min(p => p.glucose);
        float yMin = dataPoints.Min(p => p.blood_pressure);
        float zMin = dataPoints.Min(p => p.bmi);

        string predictedLabel = Predict(inputs);
        System.Threading.Thread.Sleep(500);
        Debug.Log(prediction);
        Debug.Log("predicted label:" + predictedLabel);
        predictionText.text = prediction;
        ErrorText.text = "";

        float scaler = 0.5f;
        float glucoseScaler = 0.5f;
        float bloodPressureScaler = 0.5f;
        float bmiScaler = 1.7f;
        Vector3 finalPosition = new Vector3(
            inputs[0] * glucoseScaler + 720+37 - xMin * scaler, // X-coordinate adjustment
            inputs[1] * bloodPressureScaler + 547-15 - yMin * scaler, // Y-coordinate adjustment
            -inputs[2] * bmiScaler + 56 + zMin * scaler // Z-coordinate adjustment and inversion
        );
        Vector3 position = new Vector3(829, 648, -54);
        // Adjust rotation to point up
        Quaternion rotation = Quaternion.Euler(-90, 0, 0);
        GameObject newDataPoint = Instantiate(dataPointPrefab, position, rotation);
        float baseScale = 1f; // Minimum scale to ensure visibility
        float scaleMultiplier = .1f;
        float scale = baseScale + (inputs[3] * scaleMultiplier);
        newDataPoint.transform.localScale = new Vector3(scale, scale, scale);
        Material chosenMaterial = prediction == "Prediction: Diabetic" ? DiabeticMaterial : NonDiabeticMaterial;
        Debug.Log(chosenMaterial);
        newDataPoint.GetComponent<Renderer>().material = chosenMaterial;
        MoveTowards mT = newDataPoint.GetComponent<MoveTowards>();
        mT.moveTowards(finalPosition);

        // Reset user input fields
        glucoseInput.text = "";
        bloodPressureInput.text = "";
        bmiInput.text = "";
        ageInput.text = "";
        ageInput.text = "";
    }

    private string Predict(float[] input)
    {
        client.Predict(input, output =>
        {
            prediction = "Prediction: " + output;
        }, error =>
        {
        });
        return prediction;
    }
}

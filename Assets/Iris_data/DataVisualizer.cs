using UnityEngine;

public class DataVisualizer : MonoBehaviour {
    public GameObject dataPointPrefab;
    public Material SetosaMaterial;
    public Material NonSetosaMaterial;
    // Define your DataPoint class
    [System.Serializable]
    public class DataPoint {
        public float sepal_length;
        public float sepal_width;
        public float petal_length;
        public float petal_width;
        public string prediction;
    }

    // Embed your data directly as an array of DataPoint objects
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
    foreach (DataPoint point in dataPoints) {
        // Adjust positions based on provided offsets and invert Z-coordinate
        Vector3 position = new Vector3(
            point.sepal_length*(10) + 720, // X-coordinate adjustment
            point.sepal_width*(10) + 547,  // Y-coordinate adjustment
            -point.petal_length*(10) + 56  // Z-coordinate adjustment and inversion
        );

        // Instantiate a sphere at the adjusted position
        GameObject sphere = Instantiate(dataPointPrefab, position, Quaternion.identity);

        // Determine scale based on petal_width, with a chosen multiplier for visualization purposes
        float baseScale = 0.1f; // Minimum scale to ensure visibility
        float scaleMultiplier = 5.0f; // Adjust this multiplier based on your preference
        float scale = baseScale + (point.petal_width * scaleMultiplier);
        sphere.transform.localScale = new Vector3(scale, scale, scale);

        // Assign material based on the prediction
        Material chosenMaterial = point.prediction == "Iris-setosa" ? SetosaMaterial : NonSetosaMaterial;
        sphere.GetComponent<Renderer>().material = chosenMaterial;

        Debug.Log($"Data point instantiated at: {position} with scale: {scale}");
    }
}

}

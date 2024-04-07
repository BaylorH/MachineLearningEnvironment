using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlaceDataScript : MonoBehaviour
{
    public TMP_InputField petalLengthInput;
    public TMP_InputField petalWidthInput;
    public TMP_InputField sepalLengthInput;
    public TMP_InputField sepalWidthInput;
    public GameObject dataPointPrefab;

    public Client client;
    private string prediction;

    void Start()
    {
        FindInputFields();
    }

    private void FindInputFields()
    {
        petalLengthInput = GameObject.Find("PetalLength").GetComponent<TMP_InputField>();
        petalWidthInput = GameObject.Find("PetalWidth").GetComponent<TMP_InputField>();
        sepalLengthInput = GameObject.Find("SepalLength").GetComponent<TMP_InputField>();
        sepalWidthInput = GameObject.Find("SepalWidth").GetComponent<TMP_InputField>();
        dataPointPrefab = GameObject.Find("DataPoint");
    }

    public void AddData()
    {
        float[] inputs = new float[4];
        inputs[0] = float.Parse(petalLengthInput.text);
        inputs[1] = float.Parse(petalWidthInput.text);
        inputs[2] = float.Parse(sepalLengthInput.text);
        inputs[3] = float.Parse(sepalWidthInput.text);

        Predict(inputs);

        Vector3 position = new Vector3((float)inputs[0]*10 + 720, (float)inputs[1]*10 + 547, -(float)inputs[2]*10 + 56);
        GameObject newDataPoint = Instantiate(dataPointPrefab, position, Quaternion.identity);
        float baseScale = 0.1f; // Minimum scale to ensure visibility
        float scaleMultiplier = 5.0f; // Adjust this multiplier based on your preference
        float scale = baseScale + ((float)inputs[3] * scaleMultiplier);
        newDataPoint.transform.localScale = new Vector3(scale, scale, scale);

        
    }

    private void Predict(float[] inputs)
    {
        client.Predict(inputs, output =>
        {
            
        }, error =>
        {

        });
    }
}
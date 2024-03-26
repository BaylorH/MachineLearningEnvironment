using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceDataScript : MonoBehaviour
{
    public GameObject[] uiInputFields;

    void Start()
    {
        for (int i = 0; i < uiInputFields.Length; i++)
        {
            uiInputFields[i].SetActive(true);
        }
    }

    public void AddData()
    {
        double[] inputs = new double[4];
        for (int i = 0; i < 4; i++)
        {
            InputField inputField = uiInputFields[i].GetComponent<InputField>();
            inputs[i] = double.Parse(inputField.text);
           
        }
        GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newCube.transform.position = new Vector3((float)inputs[0], (float)inputs[1], (float)inputs[2]);
    }
}

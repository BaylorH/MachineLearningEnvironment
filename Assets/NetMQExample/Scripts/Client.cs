using UnityEngine;
using System;

public class Client : MonoBehaviour
{
    private Predictor predictionRequester;

    private void Start()
    {
        predictionRequester = new Predictor();
        predictionRequester.Start();
    }

    public void Predict(float[] inputs, Action<float[]> onOutputReceived, Action<Exception> fallback)
    {
        predictionRequester.SetOnArrayReceivedListener(onOutputReceived, fallback);
        predictionRequester.SendInput(inputs);
    }

    private void OnDestroy()
    {
        predictionRequester.Stop();
    }
}
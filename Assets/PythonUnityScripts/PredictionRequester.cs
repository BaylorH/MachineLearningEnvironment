﻿using System;
using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

public class PredictionRequester : RunAbleThread {
    private RequestSocket client;

    private Action<string> onOutputReceived;
    private Action<Exception> onFail;
    
    protected override void Run() {
        ForceDotNet.Force(); // this line is needed to prevent unity freeze after one use, not sure why yet
        using (RequestSocket client = new RequestSocket()) {
            this.client = client;
            client.Connect("tcp://localhost:5555");

            while (Running) {
                string output = null;
                bool gotMessage = false;
                while (Running) {
                    try {
                        gotMessage = client.TryReceiveFrameString(out output); // this returns true if it's successful
                        if (gotMessage) break;
                    }
                    catch (Exception e)
                    {
                    }
                }

                if (gotMessage) {
                    onOutputReceived?.Invoke(output);
                }
            }
        }

        NetMQConfig.Cleanup(); // this line is needed to prevent unity freeze after one use
    }

    public void SendInput(float[] input)
    {
        try
        {
            var byteArray = new byte[input.Length * 4];
            Buffer.BlockCopy(input, 0, byteArray, 0, byteArray.Length);
            client.SendFrame(byteArray);
        }
        catch (Exception e)
        {
            onFail(e);
        }
    }

    public void SetOnTextReceivedListener(Action<string> onOutputReceived, Action<Exception> fallback)
    {
        this.onOutputReceived = onOutputReceived;
        onFail = fallback;
    }
}
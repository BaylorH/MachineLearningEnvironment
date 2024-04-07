using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;
using System;

/// <summary>
///     Example of requester who only sends Hello. Very nice guy.
///     You can copy this class and modify Run() to suits your needs.
///     To use this class, you just instantiate, call Start() when you want to start and Stop() when you want to stop.
/// </summary>
public class Predictor : RunAbleThread
{
    private RequestSocket client;
    
    private Action<float[]> onOutputReceived;
    private Action<Exception> onFail;

    /// <summary>
    ///     Request Hello message to server and receive message back. Do it 10 times.
    ///     Stop requesting when Running=false.
    /// </summary>
    protected override void Run()
    {
        ForceDotNet.Force(); // this line is needed to prevent unity freeze after one use, not sure why yet
        using (RequestSocket client = new RequestSocket())
        {
            client.Connect("tcp://localhost:5555");
            while (Running)
            {
                byte[] outputBytes = new byte[0];
                bool gotMessage = false;
                while (Running)
                {
                    gotMessage = client.TryReceiveFrameBytes(out outputBytes); // this returns true if it's successful
                    if (gotMessage) break;
                }

                if (gotMessage)
                {
                    var output = new float[outputBytes.Length / 4];
                    Buffer.BlockCopy(outputBytes, 0, output, 0, outputBytes.Length);
                    onOutputReceived?.Invoke(output);
                }
                
            }

        }

        NetMQConfig.Cleanup(); // this line is needed to prevent unity freeze after one use, not sure why yet
    }


    public void SendInput(float[] inputs)
    {
        try
        {
            var byteArray = new byte[inputs.Length * 4];
            Buffer.BlockCopy(inputs, 0, byteArray, 0, byteArray.Length);
            client.SendFrame(byteArray);
        }
        catch (Exception e)
        {
            onFail(e);
        }
    }
    
    
    public void SetOnArrayReceivedListener(Action<float[]> onArrayReceivedListener, Action<Exception> fallback)
    {
        onFail = fallback;
    }


}
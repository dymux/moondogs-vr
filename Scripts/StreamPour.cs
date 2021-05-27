using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamPour : PourDetector
{
    public GameObject streamPrefab = null;

    private Stream currentStream = null;

    override public void StartPour()
    {
        //print("Start Pour");
        audio.Play();
        currentStream = CreateStream();
        currentStream.Begin();
    }

    override public void EndPour()
    {
        //print("End Pour");
        audio.Stop();
        if (currentStream != null)
        {
            currentStream.End();
            currentStream = null;
        }

    }

    private Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, pourOrigin.position, Quaternion.identity, transform);
        return streamObject.GetComponent<Stream>();
    }
}

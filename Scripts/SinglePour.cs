using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePour : PourDetector
{
    public GameObject pourObject;

    private bool pourReady = true;

    public override void StartPour()
    {
		if (pourReady)
		{
            pourReady = false;
            print("Start Pour");
            Instantiate(pourObject, pourOrigin.position, pourOrigin.rotation);
            audio.Play();
        }
        
    }

    public override void EndPour()
    {
        pourReady = true;

    }
}

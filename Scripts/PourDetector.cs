using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PourDetector : MonoBehaviour
{
    public float pourThreshold = 90f;
    public Transform pourOrigin = null;

    private bool isPouring = false;
    new public AudioSource audio;
    new public Rigidbody rigidbody;

    // Start is called before the first frame update
    public void Start()
    {
        audio = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void Update()
    {
        bool pourCheck =( Vector3.Angle(transform.up,Vector3.up) > pourThreshold);
        if(isPouring != pourCheck)
		{
            isPouring = pourCheck;

			if (isPouring)
			{
                print(Vector3.Angle(transform.up, Vector3.up));
                StartPour();
			}
			else
			{
                print(Vector3.Angle(transform.up, Vector3.up));
                EndPour();
			}
		}
    }

    public abstract void StartPour();

    public abstract void EndPour();

}

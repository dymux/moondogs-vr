using HurricaneVR.Framework.ControllerInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhistleZone : MonoBehaviour
{
    public GameObject Player = null;
    private HVRPlayerInputs Inputs;
    public DogController dc = null;
    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("player");
        Inputs = Player.GetComponent<HVRPlayerInputs>();
        audio = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Hand"))
		{
            if (Inputs.IsRightTriggerHoldActive || Inputs.IsLeftTriggerHoldActive)
            {
                if (!audio.isPlaying)
				{
                    audio.Play();
                    dc.ApproachPlayer();
                }

			}
        }
		
	}
}

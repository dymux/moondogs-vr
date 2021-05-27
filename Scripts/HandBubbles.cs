using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBubbles : MonoBehaviour
{
    public HVRSocket[] sockets;
    public HVRController RightController => HVRInputManager.Instance.RightController;
    public HVRController LeftController => HVRInputManager.Instance.LeftController;

    // Start is called before the first frame update
    void Start()
    {
        sockets = GetComponentsInChildren<HVRSocket>();
        Hide();
    }

    void Update()
	{
		if (RightController.PrimaryButtonState.JustActivated || RightController.PrimaryButtonState.Active)
		{
            Debug.Log("Show");
            Show();
		}else if(RightController.PrimaryButtonState.JustDeactivated)
		{
            Debug.Log("Hide");
            Hide();
		}
	}

    public void Show()
    {

        //gameObject.SetActive(true);
        foreach (HVRSocket socket in sockets)
        {
            socket.CanRemoveGrabbable = true;
            //socket.gameObject.SetActive(true);
            socket.GetComponentsInChildren<MeshRenderer>()[0].enabled = true;
            socket.GetComponentsInChildren<MeshRenderer>()[1].enabled = true;
        }
    }
    public void Hide()
    {

        foreach (HVRSocket socket in sockets)
        {
            socket.CanRemoveGrabbable = false;
            //socket.gameObject.SetActive(false);
            socket.GetComponentsInChildren<MeshRenderer>()[0].enabled = false;
            socket.GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
        }
        //gameObject.SetActive(false);

    }
}

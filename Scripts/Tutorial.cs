using HurricaneVR.Framework;
using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Shared;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public bool isEnabled = true;
    public GameObject Player = null;
    public GameObject LeftControllerModel = null;
    public GameObject RightControllerModel = null;
    public TextMeshProUGUI LeftHandText = null;
    public TextMeshProUGUI RightHandText = null;

    public HVRController RightController => HVRInputManager.Instance.RightController;
    public HVRController LeftController => HVRInputManager.Instance.LeftController;
    private Blink[] LeftInputIndicators;
    private Blink[] RightInputIndicators;
    private HVRPlayerInputs Inputs;
    private BoxCollider collider;


    const int A_BUTTON = 0;
    const int B_BUTTON = 1;
    const int X_BUTTON = 0;
    const int Y_BUTTON = 1;
    const int STICK_UP_BUTTON = 5;
    const int STICK_DOWN_BUTTON = 4;
    const int STICK_LEFT_BUTTON = 3;
    const int STICK_RIGHT_BUTTON = 2;
    const int GRIP_BUTTON = 7;
    const int TRIGGER_BUTTON = 6;




    public enum TutorialName {
        NEWGAME,
        BATH
    }

    public TutorialName tutorialType = TutorialName.NEWGAME;
    // Start is called before the first frame update
    void Start()
    {
            Player = GameObject.FindGameObjectWithTag("player");
            Inputs = Player.GetComponent<HVRPlayerInputs>();


            LeftControllerModel = GameObject.FindGameObjectWithTag("LeftTutController");
            RightControllerModel = GameObject.FindGameObjectWithTag("RightTutController");
            LeftHandText = GameObject.FindGameObjectWithTag("LeftTutText").GetComponent<TextMeshProUGUI>();
            RightHandText = GameObject.FindGameObjectWithTag("RightTutText").GetComponent<TextMeshProUGUI>();
            LeftInputIndicators = LeftControllerModel.GetComponentsInChildren<Blink>();
            RightInputIndicators = RightControllerModel.GetComponentsInChildren<Blink>();
            collider = GetComponent<BoxCollider>();

        StartTutorial();
    }

    // Update is called once per frame
    void Update()
    {
    }

	/*private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("player"))
		{
            // Turn off the collider to stop multiple tutorials
            collider.enabled = false;

            isEnabled = true;

            if (tutorialType == TutorialName.NEWGAME)
            {
                StartCoroutine(NewGame());
            }
        }
	}

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            isEnabled = false;

        }
    }*/

	public void Enable()
	{
        isEnabled = true;

        // Show the controller guides
        LeftControllerModel.SetActive(true);
        RightControllerModel.SetActive(true);
        DisableAllIndicators();
    }
    public void Disable()
	{
        isEnabled = false;

        // Hide the controller guides
        LeftControllerModel.SetActive(false);
        RightControllerModel.SetActive(false);
        DisableAllIndicators();


        StopAllCoroutines();
    }

    public void DisableAllIndicators()
	{
        foreach (Blink indicator in LeftInputIndicators)
        {
            indicator.isActive = false;
        }
        foreach (Blink indicator in RightInputIndicators)
        {
            indicator.isActive = false;
        }
    }
    public void Toggle()
	{
        isEnabled = !isEnabled;
	}

    public void StartTutorial()
    {
        if (isEnabled)
		{
            StopAllCoroutines();
            Enable();
            switch (tutorialType)
            {
                case TutorialName.NEWGAME:
                    StartCoroutine(NewGame());
                    break;
                case TutorialName.BATH:
                    StartCoroutine(NewGame());
                    break;
                default:
                    StartCoroutine(NewGame());
                    break;
            }
		}
		else
		{
            StopAllCoroutines();
            Disable();
        }
        

        
    }
	public IEnumerator NewGame()
	{
		while (true)
		{

            // === TURN ===
            // Show the thumbstick arrows on the right hand
            RightHandText.SetText("Rotate: Use the right thubstick to rotate");
            RightInputIndicators[STICK_LEFT_BUTTON].isActive = true;
            RightInputIndicators[STICK_RIGHT_BUTTON].isActive = true;

            // Wait for the player to rotate
            while (Mathf.Abs(Inputs.TurnAxis.x) < 0.8f)
            {
                yield return new WaitForSeconds(0.1f);
            }

            // Turn off the arrows and turn the text green
            RightInputIndicators[STICK_LEFT_BUTTON].isActive = false;
            RightInputIndicators[STICK_RIGHT_BUTTON].isActive = false;
            RightHandText.color = Color.green;
            yield return new WaitForSeconds(1.0f);
            RightHandText.SetText("");
            RightHandText.color = Color.white;

            // === TELEPORT MOVEMENT ===
            // Show the arrow on the right hand
            RightHandText.SetText("Teleport: Hold the thumbstick down and release to teleport");
            RightInputIndicators[STICK_DOWN_BUTTON].isActive = true;

            // Wait for the player to teleport
            while (Inputs.TurnAxis.y > -0.8f)
            {
                yield return new WaitForSeconds(0.1f);
            }
            while(Inputs.TurnAxis.y < -0.8f)
			{
                yield return new WaitForSeconds(0.1f);
            }

            // Turn off the arrows and turn the text green
            RightInputIndicators[STICK_DOWN_BUTTON].isActive = false;
            RightHandText.color = Color.green;
            yield return new WaitForSeconds(1.0f);
            RightHandText.SetText("");
            RightHandText.color = Color.white;

            // === CONTINUOUS MOVEMENT ===
            // Show the thumbstick arrows on left hand
            LeftHandText.text = "Movement: Use the left thubstick to move";
            LeftInputIndicators[STICK_UP_BUTTON].isActive = true;
            LeftInputIndicators[STICK_DOWN_BUTTON].isActive = true;
            LeftInputIndicators[STICK_LEFT_BUTTON].isActive = true;
            LeftInputIndicators[STICK_RIGHT_BUTTON].isActive = true;

            // Wait for the player to move
            while (Mathf.Abs(Inputs.MovementAxis.x) < 0.8f)
			{
                yield return new WaitForSeconds(0.1f);
            }

            // Turn off the arrows and turn the text green
            LeftInputIndicators[STICK_UP_BUTTON].isActive = false;
            LeftInputIndicators[STICK_DOWN_BUTTON].isActive = false;
            LeftInputIndicators[STICK_LEFT_BUTTON].isActive = false;
            LeftInputIndicators[STICK_RIGHT_BUTTON].isActive = false;
            LeftHandText.color = Color.green;
            yield return new WaitForSeconds(1.0f);
            LeftHandText.SetText("");
            LeftHandText.color = Color.white;


            // GRAB
            // Show the grip on the both hands
            RightHandText.SetText("Grab: Use the grip button to grab objects");
            LeftHandText.SetText("Grab: Use the grip button to grab objects");
            RightInputIndicators[GRIP_BUTTON].isActive = true;
            LeftInputIndicators[GRIP_BUTTON].isActive = true;

            // Wait for the player to hold an object
            while (!Inputs.IsLeftHoldActive && !Inputs.IsRightHoldActive)
            {
                yield return new WaitForSeconds(0.1f);
            }

            // Turn off the arrows and turn the text green
            RightInputIndicators[GRIP_BUTTON].isActive = false;
            LeftInputIndicators[GRIP_BUTTON].isActive = false;
            RightHandText.color = Color.green;
            LeftHandText.color = Color.green;
            yield return new WaitForSeconds(1.0f);
            LeftHandText.SetText("");
            RightHandText.SetText("");
            RightHandText.color = Color.white;
            LeftHandText.color = Color.white;

            // === FORCE HIGHLIGHT ===
            LeftHandText.SetText("Force grab: Point your hand towards an object until you feel a vibration...");
            RightHandText.SetText("Force grab: ...then hold down the grip button to highlight the object");
            RightInputIndicators[GRIP_BUTTON].isActive = true;
            LeftInputIndicators[GRIP_BUTTON].isActive = true;

            // Wait for the player to highlight an object
            while (!Inputs.IsLeftForceGrabActivated && !Inputs.IsRightForceGrabActivated)
            {
                yield return null;
            }

            // === FORCE GRAB ===
            LeftHandText.SetText("Force grab: Flick the highlighted object towards you");
            RightHandText.SetText("Force grab: Flick the highlighted object towards you");

            // Wait for the player to catch an object
            while (Inputs.IsLeftGrabActivated || Inputs.IsRightGrabActivated)
            {
                yield return null;
            }
            // Wait for the player to hold an object
            while (!Inputs.IsLeftGrabActivated && !Inputs.IsRightGrabActivated)
            {
                yield return null;
            }

            // Turn off the arrows and turn the text green
            RightInputIndicators[GRIP_BUTTON].isActive = false;
            LeftInputIndicators[GRIP_BUTTON].isActive = false;
            RightHandText.color = Color.green;
            LeftHandText.color = Color.green;
            yield return new WaitForSeconds(1.0f);
            LeftHandText.SetText("");
            RightHandText.SetText("");
            RightHandText.color = Color.white;
            LeftHandText.color = Color.white;

            // === WHISTLE ===  
            // Show the Trigger on both hands
            LeftHandText.SetText("Whistle: Put your hand near your face and press the trigger to call your dog over");
            RightHandText.SetText("Whistle: Put your hand near your face and press the trigger to call your dog over");

            RightInputIndicators[TRIGGER_BUTTON].isActive = true;
            LeftInputIndicators[TRIGGER_BUTTON].isActive = true;

            // Wait for the player to hold an object
            while (!Inputs.IsLeftTriggerHoldActive && !Inputs.IsRightTriggerHoldActive)
            {
                yield return null;
            }

            // Turn off the triggers and turn the text green
            RightInputIndicators[TRIGGER_BUTTON].isActive = false;
            LeftInputIndicators[TRIGGER_BUTTON].isActive = false;
            RightHandText.color = Color.green;
            LeftHandText.color = Color.green;
            yield return new WaitForSeconds(1.0f);
            LeftHandText.SetText("");
            RightHandText.SetText("");
            RightHandText.color = Color.white;
            LeftHandText.color = Color.white;

            // === CROUCH ===
            // Show the B button on the right hand
            RightHandText.SetText("Crouch: Press the B button to crouch");
            RightInputIndicators[B_BUTTON].isActive = true;

            // Wait for the player to crouch
            while (!Inputs.IsCrouchActivated)
            {
                yield return null;
            }

            RightInputIndicators[B_BUTTON].isActive = false;
            RightHandText.color = Color.green;
            yield return new WaitForSeconds(1.0f);
            RightHandText.SetText("");
            RightHandText.color = Color.white;

            // === GET UP ===
            // Show the B button on the right hand
            RightHandText.SetText("Crouch: Press the B button to get back up");
            RightInputIndicators[B_BUTTON].isActive = true;

            // Wait for the player to crouch
            while (!Inputs.IsCrouchActivated)
            {
                yield return null;
            }

            RightInputIndicators[B_BUTTON].isActive = false;
            RightHandText.color = Color.green;
            yield return new WaitForSeconds(1.0f);
            RightHandText.SetText("");
            RightHandText.color = Color.white;


            // === SHOW TRICK BUBBLES ===
            RightHandText.SetText("Tricks: Press the A button to show your dogs available tricks");
            RightInputIndicators[A_BUTTON].isActive = true;
			while (!RightController.PrimaryButtonState.Active)
			{
                yield return null;
			}

            RightInputIndicators[A_BUTTON].isActive = false;
            RightHandText.color = Color.green;
            yield return new WaitForSeconds(1.0f);
            RightHandText.SetText("");
            RightHandText.color = Color.white;

            // === GRAB TRICK BUBBLE ===
            RightHandText.SetText("Tricks: Throw a trick at your dog");
            RightInputIndicators[GRIP_BUTTON].isActive = true;

            // Wait for the player to hold an object
            while (!Inputs.IsLeftHoldActive && !Inputs.IsRightHoldActive)
            {
                yield return new WaitForSeconds(0.1f);
            }

            // Turn off the arrows and turn the text green
            RightInputIndicators[GRIP_BUTTON].isActive = false;
            LeftInputIndicators[GRIP_BUTTON].isActive = false;
            RightHandText.color = Color.green;
            LeftHandText.color = Color.green;
            yield return new WaitForSeconds(1.0f);
            LeftHandText.SetText("");
            RightHandText.SetText("");
            RightHandText.color = Color.white;
            LeftHandText.color = Color.white;

            // Exit the tutorial
            collider.enabled = false; // set to true if the tutorial should repeat

            isEnabled = false;

            Disable();
            yield break;
        }
        

	}
}

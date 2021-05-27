using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Bags;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core.Utils;
using HurricaneVR.Samples;
using RealisticEyeMovements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DogController : MonoBehaviour
{
	State currentState;
	public bool busy; // Controlled by states. If the DogController is busy state changes will be blocked.
	
	public string currentAnimationState = ""; // The animation the dog is currently playing

	// Stats for the dog - Shown to the player
	//	  Permanent
	public int bondLevel = 0; // 0 = Nervous - 5 Best Friend

	//	  Decrease over time
	public int hungerLevel = 0; // 0 = Hungry - 10 = Full
	public int thirstLevel = 0; // 0 = Thirsty - 10 = Quenched
	public int boredomLevel = 0; // 0 = Bored - 5 = Happy

	
	public Transform spineBase = null; // Spine for the bubbles to attach to so that they can move with the dog
	
	public GameObject player;
	public HVRPlayerInputs playerInputs; 
	public PlayerVars playerVars;
	public GameObject playerLeftHand = null; // Used to vibrate the controller
	public GameObject playerRightHand = null; // Used to vibrate the controller

	public float timer = 0f; // Timer used by some states

	public GameObject heldItem;

	// Components
	public Animator animator;
	public NavMeshAgent agent;
	public LookTargetController lookTargetController;
	public EyeAndHeadAnimator eyeAndHeadAnimator;
	public AudioSource walkOnWoodSound;
	public Text text = null;
	public GameObject sparkleParticle = null;
	public GameObject bubblePrefab = null;
	public Material eyeballMaterial = null;

	// Audio Clips
	public AudioClip DogSingleBark = null;
	public AudioClip DogShortGrowl = null;
	public AudioClip DogPanting = null;
	public AudioClip DogDrinkWater = null;
	public AudioClip DogCrunchFood = null;

	void Start()
	{
		// Cache components
		player = GameObject.FindGameObjectWithTag("player");
		playerInputs = player.GetComponent<HVRPlayerInputs>();
		playerVars = player.GetComponent<PlayerVars>();
		animator = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
		lookTargetController = GetComponent<LookTargetController>();
		eyeAndHeadAnimator = GetComponent<EyeAndHeadAnimator>();
		walkOnWoodSound = GetComponent<AudioSource>();

		// The starting state
		ChangeState(new IdleState(this));
	}

	void Update()
	{
		if(currentState == null)
		{
			ChangeState(new IdleState(this));
		}

		// Run the timer
		if(timer <= 0.0f)
		{
			timer = 0.0f;
		}
		else{
			timer -= Time.deltaTime;
		}
		
	}

	// Updates the current state if it is not busy
	// Returns true if state has been changed
	public bool ChangeState(State newState)
	{
		if (!busy)
		{
			if (currentState != null)
			{
				currentState.Exit();
				StopAllCoroutines();
			}

			currentState = newState;
			SetText(currentState.name);

			currentState.Enter();
			StartCoroutine(currentState.Execute()); // Start the Execute routine on the current state
			return true;
		}
		return false;
	}

	// Updates the animation state, but will not update if the newState is the same as the old
	public void ChangeAnimationState(string newState)
	{
		if (currentAnimationState == newState) return;

		animator.CrossFade(newState, 0.5f);
		currentAnimationState = newState;
	}

	// Updates the animation state reguardless of the states
	public void ForceChangeAnimationState(string newState)
	{
		if (currentAnimationState == newState)
		{
			animator.CrossFade(newState, 0.5f, - 1, 0f);
		}
		animator.CrossFade(newState, 0.5f);
		currentAnimationState = newState;
	}

	// Starts the timer
	public void SetTimer(float seconds)
	{
		timer = seconds;
	}

	// Set the currently held item
	public void SetHeldItem(GameObject item)
	{
		heldItem = item;
	}

	public GameObject GetHeldItem()
	{
		return heldItem;
	}

	public bool HasTimerEnded()
	{
		return timer <= 0;
	}

	// Changes the text shown above the dog in debug mode
	public void SetText(string content)
	{
		text.text = content;
	}

	void OnAnimatorMove()
	{
		// Update position to agent position
		transform.position = agent.nextPosition;
	}

	void OnCollisionEnter(Collision other)
	{
		if(currentState != null)
		{
			// Pass the collision to the current state
			currentState.OnCollisionEnter(other);
		}
		
	}

	void OnCollisionStay(Collision other)
	{
		if (currentState != null)
		{
			// Pass the collision to the current state
			currentState.OnCollisionStay(other);
		}
			
	}

	void OnCollisionExit(Collision other)
	{
		if (currentState != null)
		{
			// Pass the collision to the current state
			currentState.OnCollisionExit(other);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (currentState != null)
		{
			currentState.OnTriggerEnter(other);
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (currentState != null)
		{
			currentState.OnTriggerStay(other);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (currentState != null)
		{
			currentState.OnTriggerExit(other);
		}
	}

	// Changes the eye color of the dog
	public void ChangeEyeColor(Color color)
	{
		eyeballMaterial.SetColor("_IrisTexColor", color);
		eyeballMaterial.SetColor("_IrisColor", color);
	}


	// State methods
	// Called by states or objects in the scene to change the dogs state
	public void Follow()
	{
		ChangeState(new FollowState(this));
	}

	public void Idle()
	{
		ChangeState(new IdleState(this));
	}

	public void Wander()
	{
		ChangeState(new WanderState(this));
	}

	public void WanderNearPlayer()
	{
		ChangeState(new WanderNearPlayerState(this));
	}

	public void ApproachPlayer()
	{
		ChangeState(new ApproachPlayerState(this));
	}

	public void Move(GameObject target)
	{
		ChangeState(new MoveState(this,player));
	}

	public void EatFood(GameObject target)
	{
		ChangeState(new EatFoodState(this,target));
	}

	public void DrinkWater(GameObject target)
	{
		ChangeState(new DrinkWaterState(this, target));
	}

	public void RunAway()
	{
		ChangeState(new RunAwayState(this,player));
	}

	public void Bath()
	{
		ChangeState(new BathState(this, player));
	}

	public void Want(GameObject target)
	{
		ChangeState(new WantState(this, target));
	}

	public void Fetch(GameObject target)
	{
		ChangeState(new FetchState(this, target));
	}

	public void Trick(TrickBubble.Tricks trick)
	{
		ChangeState(new TrickState(this, trick));
	}
}
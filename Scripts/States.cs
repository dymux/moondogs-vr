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
public interface IState
{
	public void Enter();
	public IEnumerator Execute();
	public void Exit();

	public void OnCollisionEnter(Collision other);

	public void OnCollisionStay(Collision other);
	public void OnCollisionExit(Collision other);

	public void OnTriggerEnter(Collider other);

	public void OnTriggerStay(Collider other);
	public void OnTriggerExit(Collider other);
}

public class State : IState
{
	public DogController owner;
	public string name;

	public State(DogController owner) { this.owner = owner; }

	public virtual void Enter()
	{
		name = this.GetType().Name;
		Debug.Log("entering " + name);
		owner.SetText(name);
	}

	public virtual IEnumerator Execute()
	{
		while (true)
		{
			Debug.Log("updating " + name);
			yield break;
		}
	}

	public virtual void Exit()
	{
		Debug.Log("exiting " + name);
	}

	public virtual void OnCollisionEnter(Collision other)
	{
		//Debug.Log("Collision Enter");
	}

	public virtual void OnCollisionStay(Collision other)
	{
		//Debug.Log("Collision Stay");
	}

	public virtual void OnCollisionExit(Collision other)
	{
		//Debug.Log("Collision Exit");
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		//Debug.Log("Trigger Enter");
	}
	public virtual void OnTriggerStay(Collider other)
	{
		//Debug.Log("Trigger Stay");
	}
	public virtual void OnTriggerExit(Collider other)
	{
		//Debug.Log("Trigger Exit");
	}
}

public class IdleState : State
{
	bool handContact = false; // If the players hands are touching the dog
	public int contactPoints = 0;
	public int contactPointMax = 150; // Increase this to increase the amount of petting required for the sparkle to trigger
	public float sightDistance = 5f;  // If the player is farther than this the dog starts to wander near the player
	public bool eyesClosed = false;
	public IdleState(DogController dc) : base(dc) { }
	public override IEnumerator Execute()
	{

		while (true)
		{
			if (eyesClosed)
			{
				if (owner.HasTimerEnded())
				{
					OpenEyes();
				}
			}
			// Idle until the player has moved out of sight
			if (IsPlayerSeen())
			{

				yield return IdleForSeconds(0.5f);
			}
			else
			{
				owner.WanderNearPlayer();
				yield break;
			}

		}

	}

	// Makes the dog Idle for a set time
	public IEnumerator IdleForSeconds(float duration)
	{
		// 
		owner.agent.SetDestination(owner.transform.position);
		owner.agent.speed = 0f;
		owner.ChangeAnimationState("Idle");
		yield return new WaitForSeconds(duration);
	}

	public override void OnCollisionEnter(Collision other)
	{
		base.OnCollisionEnter(other);

		// Check for a collision with
		// ...The players hands
		if (other.gameObject.CompareTag("Hand"))
		{
			handContact = true;
			contactPoints++;
			other.gameObject.GetComponent<HVRHandGrabber>().Controller.Vibrate(0.5f, 0.1f);
			CloseEyes();
			owner.SetTimer(0.3f);
		}
		// ...Soap
		else if (other.gameObject.CompareTag("Soap"))
		{
			owner.Bath();
		}
		// ...Trick Bubble
		else if (other.gameObject.CompareTag("TrickBubble"))
		{
			owner.Trick(other.gameObject.GetComponent<TrickBubble>().trick);
		}
	}

	public override void OnCollisionStay(Collision other)
	{
		base.OnCollisionStay(other);

		if (other.gameObject.CompareTag("Hand"))
		{
			handContact = true;
			Debug.Log(contactPoints);
			if (contactPoints >= contactPointMax)
			{
				contactPoints = 0;
				BePet(other);
			}
			CloseEyes();
			owner.SetTimer(0.3f);
		}
	}

	public override void OnCollisionExit(Collision other)
	{
		base.OnCollisionExit(other);
		//Check for a collision with the players hands
		if (other.gameObject.CompareTag("Hand"))
		{
			handContact = false;
			owner.SetTimer(0.3f);

		}
	}

	public override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		Debug.Log("Trigger");
		//Check for a collision with the playertrigger zone
		if (other.gameObject.CompareTag("player"))
		{
			NoticePlayer();
		}
	}
	/*public void AttachAccessory(Collision collision)
	{
		// Get the first point of contact
		ContactPoint contact = collision.contacts[0];

		// Attach the accessory to the dog
		//collision.gameObject.transform.parent = owner.transform;

		// creates joint
		FixedJoint joint = owner.gameObject.AddComponent<FixedJoint>();
		// sets joint position to point of contact
		joint.anchor = contact.point;
		// conects the joint to the other object
		joint.connectedBody = contact.otherCollider.transform.GetComponentInParent<Rigidbody>();
		// Stops objects from continuing to collide and creating more joints
		joint.enableCollision = false;


	}*/

	public float DistanceToObject(GameObject obj)
	{

		float dist = Vector3.Distance(owner.transform.position, obj.transform.position);
		//Debug.Log(dist);
		return dist;
	}

	public bool IsPlayerSeen()
	{
		if (DistanceToObject(owner.player) < sightDistance)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public void BePet(Collision other)
	{

		if (handContact)
		{
			// Pant
			SFXPlayer.Instance.PlaySFX(owner.DogPanting, owner.transform.position);

			// Spawn  a Sparkle
			Debug.Log("Spawn Sparkle");
			owner.sparkleParticle.transform.position = other.transform.position;
			owner.sparkleParticle.GetComponent<ParticleSystem>().Play();
			owner.sparkleParticle.GetComponent<AudioSource>().Play();
		}
	}

	public void NoticePlayer()
	{
		Debug.Log("Noticed Player");

		// check if the player is holding an object
		if (owner.playerVars.IsHoldingToy())
		{
			owner.Fetch(owner.playerVars.GetLastHeldObject());
		}

		//owner.playerLeftGrabber.enabled = false;
		/*if (!owner.playerLeftGrabber.GrabbedTarget)
		{
			Debug.Log("Youre holding something");
		}*/
	}
	public void OpenEyes()
	{
		owner.animator.SetFloat("eyeCloseLevel", 0.0f, 0.3f, Time.deltaTime);
		owner.eyeAndHeadAnimator.eyelidsWeight = 1f;
		eyesClosed = false;
	}

	public void CloseEyes()
	{
		eyesClosed = true;
		owner.eyeAndHeadAnimator.eyelidsWeight = 0f;
		owner.animator.SetFloat("eyeCloseLevel", 1.0f, 0.3f, Time.deltaTime);
	}
}
public class MoveState : IdleState
{
	public GameObject target;

	protected bool continuousMovement;
	protected float waitTime;
	protected float walkThreshold;
	protected float runThreshold;
	protected float walkSpeed;
	protected float runSpeed;
	protected float damp;

	public MoveState(DogController dc, GameObject targetObject) : base(dc)
	{
		target = targetObject;
		continuousMovement = false;
		waitTime = 0.0f;
		walkThreshold = 1f;
		runThreshold = 3f;
		walkSpeed = 1f;
		runSpeed = 2f;
		damp = 0.01f;
	}

	public MoveState(DogController dc, GameObject targetObject, bool continuousMovement, float waitTime, float currentSpeed, float walkThreshold, float runThreshold, float walkSpeed, float runSpeed, float damp) : base(dc)
	{
		target = targetObject;
		this.continuousMovement = continuousMovement;
		this.waitTime = waitTime;
		this.walkThreshold = walkThreshold;
		this.runThreshold = runThreshold;
		this.walkSpeed = walkSpeed;
		this.runSpeed = runSpeed;
		this.damp = damp;
	}

	public override void Enter()
	{
		base.Enter();
		SetTarget(target);
	}

	public override IEnumerator Execute()
	{
		//Debug.Log("updating move state");
		while (true)
		{
			yield return MoveAutoSpeedToTarget();
		}
	}

	public void SetWalkThreshold(float n)
	{
		walkThreshold = n;
	}
	public void SetTarget(GameObject obj)
	{
		//Debug.Log(obj.name);
		target = obj;
	}
	private float DistanceToTarget()
	{
		return DistanceToObject(target);
	}
	public bool IsAtTarget()
	{
		float dist = DistanceToObject(target);
		return dist < walkThreshold;
	}
	public IEnumerator ApproachPlayer()
	{
		GameObject prevTarget = target;
		float prevWalk = walkThreshold;
		walkThreshold = 1.0f;
		while (true)
		{
			SetTarget(owner.player);
			owner.lookTargetController.LookAtPlayer(3f);
			yield return MoveAutoSpeedToTarget();
			if (!continuousMovement)
			{
				yield return IdleForSeconds(waitTime);
			}
			SetWalkThreshold(prevWalk);
			SetTarget(prevTarget);
			yield break;
		}
	}
	public IEnumerator PickUpTarget()
	{
		while (true)
		{
			float prevWalk = walkThreshold;
			// Pick up the item
			walkThreshold = 0.5f;
			owner.agent.speed = 0f;
			owner.lookTargetController.LookAtPoiDirectly(target.transform.position);
			owner.ChangeAnimationState("Arm_Dog|Eat_1");
			yield return new WaitForSeconds(1f);

			// After picked up
			owner.ChangeAnimationState("Idle");
			owner.lookTargetController.LookAroundIdly();
			owner.busy = false;
			walkThreshold = prevWalk;
			yield break;
		}
	}

	public IEnumerator DropHeldTarget()
	{
		while (true)
		{
			float prevWalk = walkThreshold;
			// Pick up the item
			walkThreshold = 0.5f;
			owner.agent.speed = 0f;
			owner.lookTargetController.LookAtPoiDirectly(target.transform.position);
			owner.ChangeAnimationState("Arm_Dog|Eat_1");
			yield return new WaitForSeconds(1f);

			// After picked up
			owner.ChangeAnimationState("Idle");
			owner.lookTargetController.LookAroundIdly();
			owner.busy = false;
			walkThreshold = prevWalk;
			yield break;
		}
	}
	public void DampSpeed(float desiredSpeed)
	{
		if (owner.agent.speed > desiredSpeed)
		{
			owner.agent.speed -= damp;

		}
		else if (owner.agent.speed < desiredSpeed)
		{
			owner.agent.speed += damp;
		}

		owner.ChangeAnimationState("Moving");
		owner.animator.SetFloat("speed", owner.agent.speed);
	}

	public IEnumerator MoveAutoSpeedToTarget()
	{

		while (true)
		{
			// Transition to fetch state if the player is holding a toy close
			/*if (owner.playerVars.IsHoldingToy() && IsPlayerSeen())
			{
				owner.Fetch(owner.playerVars.GetLastHeldObject());
			}*/

			// Start moving towards the target
			owner.agent.SetDestination(target.transform.position);

			// At target
			if (IsAtTarget())
			{
				// Stand still and look at the target
				owner.agent.speed = 0;
				//owner.lookTargetController.LookAtPoiDirectly(target.transform.position,2f);
				yield break;
			}
			// Close to target
			else if (DistanceToTarget() >= walkThreshold && DistanceToTarget() < runThreshold)
			{
				// Change speed to a walking pace
				DampSpeed(walkSpeed);
			}
			// Far from target
			else if (DistanceToTarget() >= runThreshold)
			{
				// Change speed to a running pace
				DampSpeed(runSpeed);
			}
			else
			{
				Debug.Log("ERROR: MoveAutoSpeedToTarget");
			}
			yield return null;
		}

	}
}

public class WanderState : MoveState
{
	public float wanderSwitchProbability = 0.2f;
	public GameObject[] waypoints;

	private bool waypointForward = true;
	private int currentWaypointIndex;

	public WanderState(DogController dc) : base(dc, null)
	{

	}

	public WanderState(DogController dc, GameObject target) : base(dc, target) { }

	public override void Enter()
	{
		base.Enter();
		waypoints = GameObject.FindGameObjectsWithTag("waypoint");
		if (waypoints != null && waypoints.Length >= 2 && owner.agent.enabled)
		{
			currentWaypointIndex = 0;
			SetTarget(waypoints[currentWaypointIndex].gameObject);
		}
		else
		{
			Debug.Log("Insufficient waypoints for wander state.");
		}
		waitTime = 1.25f;
	}

	public override IEnumerator Execute()
	{
		while (true)
		{
			// Move to the next waypoint
			owner.ChangeAnimationState("Moving");
			owner.walkOnWoodSound.Play();
			ChangeWaypoint();
			yield return MoveAutoSpeedToTarget();

			if (!continuousMovement)
			{
				// Wait
				owner.walkOnWoodSound.Pause();
				yield return IdleForSeconds(waitTime);
			}
		}
	}

	public virtual void ChangeWaypoint()
	{
		if (UnityEngine.Random.Range(0f, 1f) <= wanderSwitchProbability)
		{
			waypointForward = !waypointForward;
		}

		if (waypointForward)
		{
			currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
		}
		else
		{
			if (--currentWaypointIndex < 0)
			{
				currentWaypointIndex = waypoints.Length - 1;
			}
		}
		SetTarget(waypoints[currentWaypointIndex].gameObject);
	}
}

public class WanderNearPlayerState : WanderState
{
	public int waypointsAmount = 3;
	public WanderNearPlayerState(DogController dc) : base(dc, null)
	{

	}
	public WanderNearPlayerState(DogController dc, int amountOfWaypoints) : base(dc, null)
	{
		waypointsAmount = amountOfWaypoints;
	}
	public WanderNearPlayerState(DogController dc, GameObject target) : base(dc, target) { }

	public override void ChangeWaypoint()
	{
		float withinDistance = 3f;
		GameObject[] closeWaypoints = new GameObject[waypointsAmount];
		int index = 0;
		foreach (GameObject obj in waypoints)
		{
			float distance = Vector3.Distance(obj.transform.position, owner.player.transform.position);
			if (distance < withinDistance)
			{
				closeWaypoints[index] = obj;
				index++;
			}
			if (index == waypointsAmount)
			{
				int randomIndex = Random.Range(0, waypointsAmount);
				Debug.Log("Set Target: " + closeWaypoints[randomIndex].name);
				SetTarget(closeWaypoints[randomIndex]);
				return;
			}
		}

	}
}
public class RunAwayState : WanderState
{
	public RunAwayState(DogController dc, GameObject target) : base(dc, target) { }

	public override void Enter()
	{
		base.Enter();
		runThreshold = 1.0f;
		walkThreshold = 0.5f;

	}
	public override void ChangeWaypoint()
	{
		GameObject furthestObject = FindFurthestObjectFromPlayer(waypoints);

		SetTarget(furthestObject);
	}
	public override IEnumerator Execute()
	{
		while (true)
		{
			yield return RunAwayFromPlayer();
		}
	}
	public GameObject FindFurthestObjectFromPlayer(GameObject[] objects)
	{
		float maxDistance = 0;
		GameObject furthestObject = null;

		foreach (GameObject obj in objects)
		{
			float distance = Vector3.Distance(obj.transform.position, owner.player.transform.position);
			if (distance > maxDistance)
			{
				maxDistance = distance;
				furthestObject = obj;
			}
		}
		Debug.Log(furthestObject.name + " is the furthest from the player");
		return furthestObject;
	}

	public IEnumerator RunAwayFromPlayer()
	{
		while (true)
		{
			//owner.anim.SetBool("isWalking", true);
			owner.ChangeAnimationState("Moving");
			owner.walkOnWoodSound.Play();
			ChangeWaypoint();
			yield return MoveAutoSpeedToTarget();

			while (IsAtTarget())
			{
				owner.walkOnWoodSound.Pause();
				yield return IdleForSeconds(0.1f);
				ChangeWaypoint();
			}
		}
	}
}

public class BathState : RunAwayState
{
	public List<GameObject> bubbles = new List<GameObject>();
	public int maxBubbles = 100;
	public int soapContactPoints = 0;
	public int soapCreateParticleThreshold = 10; // How much soap contact is required to spawn foam
	private int bubbleIndex = 0;
	public BathState(DogController dc, GameObject target) : base(dc, target) { }

	public override IEnumerator Execute()
	{
		yield return null;
	}

	public override void OnCollisionEnter(Collision other)
	{
		base.OnCollisionEnter(other);
		//Check for a collision with soap
		if (other.gameObject.CompareTag("Soap"))
		{
			//owner.playerLeftHand.GetComponentInParent<HVRHandGrabber>().Controller.Vibrate(0.5f, 0.1f);
			owner.playerRightHand.GetComponentInParent<HVRHandGrabber>().Controller.Vibrate(0.5f, 0.1f);
			//CloseEyes();
		}
	}
	public override void OnCollisionStay(Collision other)
	{
		base.OnCollisionStay(other);
		// Colliding with soap adds to the point counter, after a certain amount of points create bubbles and reset counter
		if (other.gameObject.CompareTag("Soap"))
		{
			//owner.playerLeftHand.GetComponentInParent<HVRHandGrabber>().Controller.Vibrate(0.5f, 0.1f);
			owner.playerRightHand.GetComponentInParent<HVRHandGrabber>().Controller.Vibrate(0.5f, 0.1f);
			soapContactPoints++;
			Debug.Log(soapContactPoints);
			if (soapContactPoints >= soapCreateParticleThreshold)
			{
				soapContactPoints = 0;
				CreateBubble(other);
			}
			//CloseEyes();
		}
	}

	public override void OnCollisionExit(Collision other)
	{
		base.OnCollisionExit(other);
		if (other.gameObject.CompareTag("Soap"))
		{
			//OpenEyes();
		}
	}

	void CreateBubble(Collision collision)
	{
		// Get the first point of contact
		ContactPoint contact = collision.contacts[0];
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 pos = contact.point;

		if (bubbles.Count < maxBubbles)
		{
			// Create and attach a new bubble
			GameObject clone = UnityEngine.Object.Instantiate(owner.bubblePrefab, pos, rot);
			clone.transform.parent = owner.spineBase;


			// Make the bubble a random size
			float scale = Random.Range(0.025f, 0.1f);
			clone.transform.localScale = new Vector3(scale, scale, scale);

			bubbles.Add(clone);

		}
		else
		{
			// If max amount of bubbles already exist
			// reattach one of the old bubbles instead
			bubbles[bubbleIndex].SetActive(true);
			bubbles[bubbleIndex].transform.position = pos;
			bubbles[bubbleIndex].transform.rotation = rot;
			bubbles[bubbleIndex].transform.parent = owner.spineBase;
			float scale = Random.Range(0.025f, 0.1f);
			bubbles[bubbleIndex].transform.localScale = new Vector3(scale, scale, scale);

			if (bubbleIndex >= maxBubbles - 1)
			{
				bubbleIndex = 0;
			}
			else
			{
				bubbleIndex++;
			}
		}
	}
}

public class EatFoodState : MoveState
{
	public EatFoodState(DogController dc, GameObject target) : base(dc, target) { }

	public override void Enter()
	{
		base.Enter();
		walkThreshold = 0.25f;
	}

	public override IEnumerator Execute()
	{
		while (true)
		{
			yield return EatTarget();
			yield break;
		}

	}

	public IEnumerator EatTarget()
	{
		Rigidbody rb = target.GetComponent<Rigidbody>();
		while (true)
		{
			owner.busy = true;

			// Align to food
			yield return MoveAutoSpeedToTarget();
			owner.agent.speed = 0f;
			owner.lookTargetController.LookAtPoiDirectly(target.transform.position);
			//owner.anim.SetBool("isWalking", false);
			rb.constraints = RigidbodyConstraints.FreezeAll;

			// Eat
			owner.ChangeAnimationState("Arm_Dog|Eat_1");
			SFXPlayer.Instance.PlaySFX(owner.DogCrunchFood, owner.transform.position);
			//owner.anim.SetBool("isEating", true);
			yield return new WaitForSeconds(2f);
			// If just ate an eyecandy 
			if (target.CompareTag("EyeCandy"))
			{
				owner.ChangeEyeColor(target.GetComponent<EyeCandy>().color);
			}
			GameObject.Destroy(target);



			// Stop Eating
			//owner.anim.SetBool("isEating", false);
			owner.ChangeAnimationState("Idle");
			owner.lookTargetController.LookAroundIdly();
			owner.busy = false;
			owner.Idle();
			yield break;
		}
	}

}

public class DrinkWaterState : MoveState
{
	public DrinkWaterState(DogController dc, GameObject target) : base(dc, target) { }

	public override void Enter()
	{
		base.Enter();
		walkThreshold = 0.5f;

	}

	public override IEnumerator Execute()
	{
		Debug.Log("updating drink water state");

		while (true)
		{

			owner.busy = true;

			// Align to water
			yield return MoveAutoSpeedToTarget();
			owner.agent.speed = 0f;
			owner.lookTargetController.LookAtPoiDirectly(target.transform.position);
			//owner.anim.SetBool("isWalking", false);

			// Drink
			WaterBowl wb = target.GetComponent<WaterBowl>();
			//owner.anim.SetBool("isDrinking", true);
			owner.ChangeAnimationState("Arm_Dog|Drink");
			SFXPlayer.Instance.PlaySFX(owner.DogDrinkWater, owner.transform.position);
			while (wb.GetLevel() > 0)
			{
				wb.Empty(1f);
				yield return null;
			}
			yield return new WaitForSeconds(2f);


			// Stop Drinking
			//owner.anim.SetBool("isDrinking", false);
			owner.ChangeAnimationState("Idle");
			owner.lookTargetController.LookAroundIdly();
			owner.busy = false;
			owner.Idle();
			yield break;
		}
	}
}

public class TrickState : WantState
{
	public TrickBubble.Tricks currentTrick;
	public TrickState(DogController dc) : base(dc, null)
	{
	}

	public TrickState(DogController dc, TrickBubble.Tricks trick) : base(dc, null)
	{
		currentTrick = trick;
	}


	public override IEnumerator Execute()
	{
		while (true)
		{
			yield return PreformTrick();
			owner.Idle();
		}
	}

	public IEnumerator PreformTrick()
	{
		while (true)
		{
			if (currentTrick == TrickBubble.Tricks.NONE)
			{
				yield break;
			}
			else if (currentTrick == TrickBubble.Tricks.SIT)
			{
				yield return Sit();
				currentTrick = TrickBubble.Tricks.NONE;
			}
			else if (currentTrick == TrickBubble.Tricks.LIE)
			{
				yield return Lie();
				currentTrick = TrickBubble.Tricks.NONE;
			}
			else if (currentTrick == TrickBubble.Tricks.SPEAK)
			{
				yield return Speak();
				currentTrick = TrickBubble.Tricks.NONE;
			}
			else if (currentTrick == TrickBubble.Tricks.PLAYDEAD)
			{
				yield return PlayDead();
				currentTrick = TrickBubble.Tricks.NONE;
			}
			else if (currentTrick == TrickBubble.Tricks.JUMP)
			{
				yield return Jump();
				currentTrick = TrickBubble.Tricks.NONE;
			}
			else
			{
				Debug.Log("Trick type does not exist");
				currentTrick = TrickBubble.Tricks.NONE;
			}
			yield break;
		}
	}
	public override void OnCollisionEnter(Collision other)
	{
		base.OnCollisionEnter(other);
		//Check for a collision with trick bubble
		if (other.gameObject.CompareTag("TrickBubble"))
		{
			// Set the current trick
			TrickBubble tb = other.gameObject.GetComponent<TrickBubble>();
			currentTrick = tb.trick;
			Debug.Log(currentTrick);

			// Destroy the trick bubble
			GameObject.Destroy(other.gameObject);

			// Vibrate the controllers
			owner.playerLeftHand.GetComponentInParent<HVRHandGrabber>().Controller.Vibrate(0.1f, 0.1f);
			owner.playerRightHand.GetComponentInParent<HVRHandGrabber>().Controller.Vibrate(0.1f, 0.1f);
		}
	}

	public float GetAnimationLength(string animationName)
	{
		// Get the length of the animation
		AnimationClip[] clips = owner.animator.runtimeAnimatorController.animationClips;
		foreach (AnimationClip clip in clips)
		{
			if (clip.name == animationName)
			{
				return clip.length;
			}
		}
		Debug.Log("The clip " + animationName + " does not exist.");
		return 0f;
	}

	public IEnumerator Sit()
	{
		while (true)
		{
			owner.busy = true;
			string animationName = "Arm_Dog|Sitting";
			owner.ForceChangeAnimationState(animationName);
			yield return new WaitForSeconds(GetAnimationLength(animationName));
			owner.busy = false;
			yield break;
		}

	}
	public IEnumerator Lie()
	{
		while (true)
		{
			owner.busy = true;
			string animationName = "Arm_Dog|Lie";
			owner.ForceChangeAnimationState(animationName);
			yield return new WaitForSeconds(GetAnimationLength(animationName));
			owner.busy = false;
			yield break;
		}
	}

	public IEnumerator Speak()
	{
		while (true)
		{
			owner.busy = true;
			string animationName = "Arm_Dog|Agression";
			SFXPlayer.Instance.PlaySFX(owner.DogSingleBark, owner.transform.position);
			owner.ForceChangeAnimationState(animationName);
			yield return new WaitForSeconds(GetAnimationLength(animationName));
			owner.busy = false;
			yield break;
		}
	}

	public IEnumerator PlayDead()
	{
		while (true)
		{
			owner.busy = true;
			string animationName = "Arm_Dog|Death_2";
			owner.ForceChangeAnimationState(animationName);
			yield return new WaitForSeconds(GetAnimationLength(animationName));
			owner.busy = false;
			yield break;
		}

	}

	public IEnumerator Jump()
	{
		while (true)
		{
			owner.busy = true;
			string animationName = "Arm_Dog|Jump";
			owner.ForceChangeAnimationState(animationName);
			yield return new WaitForSeconds(GetAnimationLength(animationName));
			owner.busy = false;
			yield break;
		}
	}

}
public class FollowState : MoveState
{
	public FollowState(DogController dc) : base(dc, null) { target = owner.player; }

	public FollowState(DogController dc, GameObject target) : base(dc, target) { }

	public FollowState(DogController dc, GameObject targetObject, bool continuousMovement, float waitTime, float currentSpeed, float walkThreshold, float runThreshold, float walkSpeed, float runSpeed, float damp) : base(dc, targetObject, continuousMovement, waitTime, currentSpeed, walkThreshold, runThreshold, walkSpeed, runSpeed, damp)
	{

	}

	public override void Enter()
	{
		base.Enter();
	}

	public override IEnumerator Execute()
	{
		Debug.Log("updating following state");
		while (true)
		{
			yield return MoveAutoSpeedToTarget();
			if (!continuousMovement)
			{
				yield return IdleForSeconds(waitTime);
			}

		}
	}
}
public class ApproachPlayerState : FollowState
{
	public ApproachPlayerState(DogController dc) : base(dc, null) { }

	public ApproachPlayerState(DogController dc, GameObject target) : base(dc, target) { }

	public override IEnumerator Execute()
	{
		while (true)
		{
			yield return ApproachPlayer();
			owner.Idle();
		}
	}
}

public class WantState : FollowState
{
	public WantState(DogController dc, GameObject target) : base(dc, target) { }

	public override void Enter()
	{
		base.Enter();
		walkThreshold = 0.5f;
	}
	public override IEnumerator Execute()
	{
		while (true)
		{
			yield return WantTarget();
		}
	}

	public IEnumerator WantTarget()
	{
		while (true)
		{
			// The dog is holding the object
			if (target.GetComponent<HVRGrabbable>().IsSocketed)
			{
				yield break;
			}
			// Player is holding the wanted item
			else if (target.GetComponent<HVRGrabbable>().IsBeingHeld)
			{
				walkThreshold = 1f;
				yield return ApproachPlayer();

			}
			// The target is not being held and is not near the dog
			else if (!IsAtTarget() && !target.GetComponent<HVRGrabbable>().IsBeingHeld)
			{
				walkThreshold = 0.5f;
				yield return MoveAutoSpeedToTarget();
			}
			// The target is not being held and is near the dog
			else if (IsAtTarget() && !target.GetComponent<HVRGrabbable>().IsBeingHeld)
			{
				yield return PickUpTarget();
			}
			else
			{
				// Align to target
				walkThreshold = 0.5f;
				yield return MoveAutoSpeedToTarget();
			}
		}
	}
}

public class FetchState : WantState
{

	public FetchState(DogController dc, GameObject target) : base(dc, target) { }

	public override IEnumerator Execute()
	{
		while (true)
		{
			yield return WantTarget();
			yield return ApproachPlayer();
			// While the dog is holding the object
			owner.SetTimer(7f);
			while (target.GetComponent<HVRGrabbable>().IsSocketed)
			{
				// Player doesn't grab target from dog for awhile
				if (owner.HasTimerEnded())
				{
					Debug.Log("TIMER ENDED");
					owner.WanderNearPlayer();
				}

				//Debug.Log(owner.timer);
				if (IsPlayerSeen())
				{
					owner.lookTargetController.LookAtPlayer();
					yield return null;
				}
				else if (!IsPlayerSeen())
				{
					owner.lookTargetController.LookAroundIdly();
					yield return null;
				}
			}
		}
	}
}
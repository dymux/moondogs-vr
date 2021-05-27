using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
	public DogController dogController = null;
	private ParticleSystem popEffect;
	float timer;
	// Start is called before the first frame update
	void Start()
    {
		popEffect = GetComponentInChildren<ParticleSystem>();
		timer = Random.Range(0.0f, 2.0f);

	}

	// Update is called once per frame
	void Update()
	{
		
		
	}

	private void OnTriggerStay(Collider other)
	{
		// If the bubble comes into contact with water
		if (gameObject.activeInHierarchy && other.gameObject.CompareTag("Water"))
		{
			// If the timer is running play the effect or else disactivate the bubble
			if (timer > 0.0f)
			{
				if (popEffect.isStopped)
				{
					popEffect.Play();
				}
				
				timer -= Time.deltaTime;
			}
			if (timer <= 0.0f)
			{
				gameObject.SetActive(false);
			}

		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Water"))
		{
			popEffect.Stop();
		}
	}
}

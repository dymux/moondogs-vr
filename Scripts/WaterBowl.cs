using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBowl : MonoBehaviour
{
    public float level = 0.0f;
    private float maxFill = 1.0f;
    public float dropsToFill = 100f;
    private Animator anim;
    private AudioSource audio;
    private BoxCollider trigger;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        trigger = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("level", level);
        if(level > 0.8)
		{
            trigger.enabled = true;
		}
		else
		{
            trigger.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        audio.Play();
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.CompareTag("Dog") && !other.gameObject.GetComponentInParent<DogController>().busy)
        {
            other.gameObject.GetComponentInParent<DogController>().DrinkWater(gameObject);
        }
    }

	public float GetLevel()
	{
        return level;
	}

    public void SetLevel(float level)
	{
        this.level = level;
        anim.SetFloat("level", level);
    }
    public void Fill(float drops)
	{
        if(level + (drops / dropsToFill) < maxFill)
		{
            level += (drops / dropsToFill);
		}
		else
		{
            level = maxFill;
		}
        anim.SetFloat("level", level);
    }

    public void Empty()
    {
        level = 0;
        anim.SetFloat("level", level);
    }

    public void Empty(float drops)
    {
        if (level - (drops / dropsToFill) >= 0)
        {
            level -= (drops / dropsToFill);
        }
        else
        {
            level = 0;
        }
        anim.SetFloat("level", level);
    }
}

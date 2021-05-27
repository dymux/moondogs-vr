using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSwitchDisplay : MonoBehaviour
{
	public enum  TutorialDisplayColor { 
    Red,
    Green
    }

    public TutorialDisplayColor startColor;
    private TutorialDisplayColor currentColor;
    public Tutorial tutorial = null;
    private Text text;

    public void Start()
    {
        Debug.Log("TutSwtichDis Start");
        text = GetComponent<Text>();
        currentColor = startColor;
        if(startColor == TutorialDisplayColor.Red)
		{
            text.color = Color.red;
        }
        else
        {
            text.color = Color.green;
        }
    }

    public void SwapColor()
	{
        Debug.Log("Swapping color");
        if(currentColor == TutorialDisplayColor.Red)
		{
            currentColor = TutorialDisplayColor.Green;
            text.color = Color.green;
            Debug.Log("Green");
        }
		else
		{
            currentColor = TutorialDisplayColor.Red;
            text.color = Color.red;
            Debug.Log("Red");
        }
	}
}
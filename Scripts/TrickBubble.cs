using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickBubble : MonoBehaviour
{
    public enum Tricks
	{
        NONE,
        SIT,
        LIE,
        SPEAK,
        PLAYDEAD,
        JUMP
	}

    public Tricks trick;
}

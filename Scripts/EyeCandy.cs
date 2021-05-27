using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCandy : MonoBehaviour
{
    public Material material;
    public Color color = Color.red;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        material.color = color;
    }

    public void SetColor(Color color_in)
	{
        color = color_in;
        material.color = color;
    }
}

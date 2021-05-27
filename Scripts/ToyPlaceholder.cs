using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToyPlaceholder : MonoBehaviour
{
    public Dropdown shapeDrop = null;
    public Dropdown colorDrop = null;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    public GameObject[] toys;
    public Material[] colors;


    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter.mesh = toys[0].GetComponent<MeshFilter>().sharedMesh;
        meshRenderer.material = colors[0];
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 25* Time.deltaTime, 25* Time.deltaTime);
    }

    public void UpdatePlaceholder()
	{
        meshFilter.mesh = toys[shapeDrop.value].GetComponent<MeshFilter>().sharedMesh;
        meshRenderer.material = colors[colorDrop.value];
    }
}

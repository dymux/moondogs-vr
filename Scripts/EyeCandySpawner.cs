using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCandySpawner : MonoBehaviour
{
    public GameObject spawnObject;
    public FlexibleColorPicker fcp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SpawnSingleCandy()
    {

        GameObject clone = Instantiate(spawnObject, transform.position, transform.rotation);
        clone.GetComponent<Rigidbody>().velocity = Vector3.zero;
        clone.GetComponent<EyeCandy>().SetColor(fcp.color);
    }

}

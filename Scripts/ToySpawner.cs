using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToySpawner : MonoBehaviour
{
    public GameObject ToyPlaceholder = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnToy()
    {

        GameObject clone = Instantiate(ToyPlaceholder, transform.position, transform.rotation);
        clone.GetComponent<ToyPlaceholder>().enabled = false;
        clone.GetComponent<Rigidbody>().useGravity = true;


    }
}

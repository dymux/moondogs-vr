using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnObject;
    public bool spawnContinuous = false;
    public bool activated;
    public float delay;
    public int maxObjects = 10;
    public List<GameObject> objects;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        objects = new List<GameObject>();
        if(spawnContinuous)
		{
            StartCoroutine(SpawnContinuous());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SpawnSingle()
    {

            if (objects.Count < maxObjects)
            {
                GameObject clone = Instantiate(spawnObject, transform.position, transform.rotation);
                objects.Add(clone);

            }
            else
            {
                objects[index].transform.position = transform.position;
                objects[index].transform.rotation = transform.rotation;
                objects[index].GetComponent<Rigidbody>().velocity = Vector3.zero;

                if (index >= maxObjects - 1)
                {
                    index = 0; 
                }
                else
                {
                    index++;
                }
            }
    }
    private IEnumerator SpawnContinuous()
    {
		while (true)
		{
            if (objects.Count < maxObjects)
            {
                GameObject clone = Instantiate(spawnObject, transform.position, transform.rotation);
                objects.Add(clone);
                
            }
            else
            {
                objects[index].transform.position = transform.position;
                objects[index].transform.rotation = transform.rotation;
                objects[index].GetComponent<Rigidbody>().velocity = Vector3.zero;

                if (index >= maxObjects - 1)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
            }

            yield return new WaitForSeconds(delay);
        }

    }

}

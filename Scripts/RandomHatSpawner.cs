using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHatSpawner : MonoBehaviour
{
    public GameObject[] hatPrefabs = null;
    public GameObject hatSphere = null;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnSingleHat()
    {
        StartCoroutine("SpinHatSphere");
        int randomIndex = Random.Range(0, hatPrefabs.Length);
        GameObject clone = Instantiate(hatPrefabs[randomIndex], transform.position, transform.rotation);
        StopCoroutine("SpinHatSphere");
    }
    
    public IEnumerator SpinHatSphere()
	{
		while (true)
		{
            hatSphere.transform.Rotate(Vector3.up * Time.deltaTime);
            yield return null;
		}
	}
}

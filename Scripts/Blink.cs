using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public bool isActive = false;
    protected MeshRenderer rend;
    public float blinkDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        StartCoroutine(BlinkMaterial());
    }

    protected IEnumerator BlinkMaterial()
    {
        while (true)
        {
            if (isActive)
            {
                //Debug.Log("Blink Off");
                rend.enabled = false;
                yield return new WaitForSeconds(blinkDuration);

                //Debug.Log("Blink On");
                rend.enabled = true;
                yield return new WaitForSeconds(blinkDuration);
            }
            else
            {
                rend.enabled = false;
                yield return new WaitForSeconds(blinkDuration);
            }

        }
    }
}

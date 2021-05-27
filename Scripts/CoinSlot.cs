using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinSlot : MonoBehaviour
{
    public GameObject blockedButton = null;
    public GameObject coinBlocker = null;
    protected TextMeshPro tm;
    // Start is called before the first frame update
    void Start()
    {
        tm = blockedButton.GetComponentInChildren<TextMeshPro>();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Coin"))
		{
            // Block the coin slot with a cube
            coinBlocker.SetActive(true);

            // Unlock the button
            tm.text = "PUSH";
            tm.fontSize = 0.3f;
            blockedButton.GetComponent<Rigidbody>().isKinematic = false;

            // Destroy the coin
            GameObject.Destroy(other.gameObject);
		}
	}

	public void Reset()
	{
        // Unblock the coin slot
        coinBlocker.SetActive(false);
        
        // Lock the button
        tm.text = "PLEASE INSERT COIN";
        tm.fontSize = 0.1562f;
        blockedButton.GetComponent<Rigidbody>().isKinematic = true;

        
    }
}

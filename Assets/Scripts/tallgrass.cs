using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tallgrass : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (GetComponent<SpriteRenderer>().enabled == true)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}

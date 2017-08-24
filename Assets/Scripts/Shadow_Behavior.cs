using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow_Behavior : MonoBehaviour {

    public GameObject parent;
    public Sprite shadowLarge;
    public Sprite shadowSmall;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (parent.GetComponent<Animator>().GetBool("Jumping"))
        {
            GetComponent<SpriteRenderer>().sprite = shadowSmall;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = shadowLarge;
        }
	}
}

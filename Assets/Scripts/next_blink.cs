using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class next_blink : MonoBehaviour {

    private Text text;
    private float delay;
    private string nextText;

	// Use this for initialization
	void Awake () {
        text = GetComponent<Text>();
        delay = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.realtimeSinceStartup > delay + .5f)
        {
            text.text = nextText;
            if (nextText == "¦")
                nextText = " ";
            else
                nextText = "¦";
            delay = Time.realtimeSinceStartup;
        }
        
    }
}

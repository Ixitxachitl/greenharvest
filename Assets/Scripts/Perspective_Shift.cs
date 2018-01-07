using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[ExecuteInEditMode]
public class Perspective_Shift : MonoBehaviour {

    //private Transform player;
    public Transform body;

	// Use this for initialization
	void Awake () {
        //player = Player_Controller.player_controller.GetComponent<Transform>();
        if (body == null)
        {
            body = this.transform;
        }
        GetComponent<SpriteRenderer>().sortingOrder = 10000 - Mathf.RoundToInt(body.position.y * 100);
    }
	
	// Update is called once per frame
	void Update () {
        //if ((int)Player_Controller.player_controller.transform.position.y != (int)transform.position.y)
            GetComponent<SpriteRenderer>().sortingOrder = 10000 - Mathf.RoundToInt(body.position.y*100);
        //else
        //    GetComponent<SpriteRenderer>().sortingOrder = 1000 - (int)Mathf.Round(transform.position.y-1);
    }
}

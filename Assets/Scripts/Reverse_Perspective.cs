using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reverse_Perspective : MonoBehaviour
{

    private Transform player;


    // Use this for initialization
    void Start()
    {
        player = Player_Controller.player_controller.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.y > GetComponentInParent<Transform>().position.y + 1)
            GetComponent<SpriteRenderer>().sortingOrder = 10000 - Mathf.RoundToInt((player.transform.position.y) * 100) - 100;
        else
            GetComponent<SpriteRenderer>().sortingOrder = 10000 - Mathf.RoundToInt((player.transform.position.y) * 100) + 100;
    }
}

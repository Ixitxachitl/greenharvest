using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore;

public class Perspective_tiles : MonoBehaviour
{

    //private Transform player;


    // Use this for initialization
    void Start()
    {
        //player = Player_Controller.player_controller.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (player.position.y > transform.position.y)
            GetComponent<CreativeSpore.SuperTilemapEditor.STETilemap>().OrderInLayer = 10000 - Mathf.RoundToInt(transform.position.y * 100);
        //else
           // GetComponent<CreativeSpore.SuperTilemapEditor.Tilemap>().OrderInLayer = 0;
    }
}

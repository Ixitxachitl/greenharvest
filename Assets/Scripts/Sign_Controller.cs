using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using CnControls;

public class Sign_Controller : MonoBehaviour
{
    [Multiline]
    public string signText;
    public LayerMask playerLayer;
    public bool signOpen;
    public bool delayed;
    private Text text;
    public GameObject canvas;

    private float delayTime;

    // Use this for initialization
    void Start()
    {
        signOpen = false;
        text = GetComponentInChildren<Text>(true);
        canvas.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup > delayTime + .5f && delayed == false)
        {
            delayed = true;
        }

        if (Physics2D.Raycast(transform.position, Vector2.down, 1, playerLayer) && CnInputManager.GetButtonDown("Jump") && !signOpen && Player_Controller.player_controller.lastMove.y == 1 && Camera_Controller.paused == false && Player_Controller.player_controller.inDialogue == false)
        {
            canvas.SetActive(true);
            Player_Controller.player_controller.disableControls = true;
            Player_Controller.player_controller.inDialogue = true;
            signOpen = true;
            delayed = false;
            delayTime = Time.realtimeSinceStartup;
        }
        if (CnInputManager.GetButtonDown("Jump") && signOpen == true && delayed)
        {
            signOpen = false;
            canvas.SetActive(false);
            Player_Controller.player_controller.disableControls = false;
            Player_Controller.player_controller.inDialogue = false;
        }

        if (text.text != signText)
        {
            text.text = signText;
        }
    }
}


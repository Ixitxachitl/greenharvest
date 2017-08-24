using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class NPC_Dialogue : MonoBehaviour {

    public RetroPrinterScript text;
    [Multiline]
    public string dialogueText;
    public LayerMask playerLayer;
    public bool dialogueOpen;
    public bool delayed;
    public GameObject canvas;

    private float delayTime;

    // Use this for initialization
    void Start()
    {
        dialogueOpen = false;
        canvas.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup > delayTime + .5f && delayed == false)
        {
            delayed = true;
        }

        if (Physics2D.Raycast(transform.position, Vector2.down, 1, playerLayer) && CrossPlatformInputManager.GetButtonDown("Jump") && !dialogueOpen && Player_Controller.player_controller.lastMove.y == 1)
        {
            canvas.SetActive(true);
            text.SetText(dialogueText);
            Debug.Log("set text: " + dialogueText);
            text.Run();
            Debug.Log("ran");
            Player_Controller.player_controller.disableControls = true;
            dialogueOpen = true;
            delayed = false;
            delayTime = Time.realtimeSinceStartup;
        }
        if (CrossPlatformInputManager.GetButtonDown("Jump") && dialogueOpen == true && delayed && !text.IsRunning)
        {
            dialogueOpen = false;
            text.GetComponent<Text>().text = "";
            text.Stop();
            canvas.SetActive(false);
            Player_Controller.player_controller.disableControls = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;



public class NPC_Dialogue : MonoBehaviour {
    [SerializeField]
    private RetroPrinterScript text;
    [Multiline]
    [TextArea(5, 3)]
    [SerializeField]
    private string[] dialogueText;
    [SerializeField]
    private LayerMask playerLayer;
    private bool dialogueOpen;
    private bool delayed;
    [SerializeField]
    private GameObject canvas;
    private int dialoguePage;

    private float delayTime;

    // Use this for initialization
    void Start()
    {
        dialogueOpen = false;
        canvas.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        dialoguePage = 0;
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
            text.SetText(dialogueText[dialoguePage]);
            Debug.Log("set text: " + dialogueText[dialoguePage]);
            text.Run();
            Debug.Log("ran");
            Player_Controller.player_controller.disableControls = true;
            dialogueOpen = true;
            delayed = false;
            delayTime = Time.realtimeSinceStartup;
        }
        if (CrossPlatformInputManager.GetButtonDown("Jump") && dialogueOpen == true && delayed && !text.IsRunning)
        {
            if (dialoguePage < dialogueText.Length - 1)
            {
                dialoguePage++;
                text.SetText(dialogueText[dialoguePage]);
                Debug.Log("set text: " + dialogueText[dialoguePage]);
                text.Run();
                delayed = false;
                delayTime = Time.realtimeSinceStartup;
            }
            else
            {
                dialoguePage = 0;
                dialogueOpen = false;
                text.GetComponent<Text>().text = "";
                text.Stop();
                canvas.SetActive(false);
                Player_Controller.player_controller.disableControls = false;
            }
        }
    }
}

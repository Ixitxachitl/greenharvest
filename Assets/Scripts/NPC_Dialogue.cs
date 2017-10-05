using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CnControls;



public class NPC_Dialogue : MonoBehaviour {
    [SerializeField]
    private RetroPrinterScript text;
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
    [SerializeField]
    private GameObject next;

    private float delayTime;
    private float idleTime;
    private Animator avatarState;
    private bool cooldown;

    private bool wasClosed;

    // Use this for initialization
    void Start()
    {
        dialogueOpen = false;
        wasClosed = false;
        canvas.GetComponentInChildren<Canvas>(true).worldCamera = Camera.main;
        avatarState = GetComponentInChildren<Canvas>(true).GetComponentInChildren<Animator>(true);
        dialoguePage = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (wasClosed == true)
        {
            Player_Controller.player_controller.disableControls = false;
            Player_Controller.player_controller.inDialogue = false;
            Debug.Log("poke");
            wasClosed = false;
        }
        if (Time.realtimeSinceStartup > delayTime + .5f && delayed == false)
        {
            delayed = true;
        }
        if (Time.realtimeSinceStartup > idleTime + 1f && cooldown == true && avatarState.isActiveAndEnabled)
        {
            int triggerProb = Random.Range(1, 101);
            if (triggerProb <= 10)
            {
                avatarState.SetTrigger("Blink");
            }
            else if (triggerProb > 10 && triggerProb <= 20)
            {
                avatarState.SetTrigger("Wiggle");
            }
            else if (triggerProb > 20 && triggerProb <= 25)
            {
                avatarState.SetTrigger("Tongue");
            }

            cooldown = false;
        }
        if (cooldown == false && !text.IsRunning && canvas.activeSelf){
            cooldown = true;
            idleTime = Time.realtimeSinceStartup;
            avatarState.SetBool("Talking", false);
            next.SetActive(true);
        }

        if (Physics2D.CircleCast(transform.position, 1, Vector2.zero, 10, playerLayer) && CnInputManager.GetButtonDown("Jump") && !dialogueOpen && Camera_Controller.paused == false && Player_Controller.player_controller.inDialogue == false)
        {
            canvas.SetActive(true);
            text.SetText(dialogueText[dialoguePage]);
            Debug.Log("set text: " + dialogueText[dialoguePage]);
            text.Run();
            Debug.Log("ran");
            Player_Controller.player_controller.disableControls = true;
            Player_Controller.player_controller.inDialogue = true;
            dialogueOpen = true;
            delayed = false;
            delayTime = Time.realtimeSinceStartup;
            avatarState.SetBool("Talking", true);
            next.SetActive(false);
    
        }
        if (CnInputManager.GetButtonDown("Jump") && dialogueOpen == true && delayed && !text.IsRunning)
        {
            if (dialoguePage < dialogueText.Length - 1)
            {
                dialoguePage++;
                text.SetText(dialogueText[dialoguePage]);
                Debug.Log("set text: " + dialogueText[dialoguePage]);
                text.Run();
                delayed = false;
                delayTime = Time.realtimeSinceStartup;
                avatarState.SetBool("Talking", true);
                next.SetActive(false);
            }
            else
            {
                dialoguePage = 0;
                dialogueOpen = false;
                text.GetComponent<Text>().text = "";
                text.Stop();
                avatarState.SetBool("Talking", false);
                canvas.SetActive(false);
                next.SetActive(false);
                wasClosed = true;
            }
        }
    }
}

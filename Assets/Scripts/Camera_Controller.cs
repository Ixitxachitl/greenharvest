using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CreativeSpore.SuperTilemapEditor;
using UnityEngine.SceneManagement;
using CnControls;

public class Camera_Controller : MonoBehaviour
{

    public static Camera_Controller camera_controller;

    private Transform player;

    private float targetPositionX;
    private float targetPositionY;
    private Vector2 targetPosition;
    public float speed;
    public int levelSizeX;
    public int levelSizeY;
    public float xoffset;
    public float yoffset;
    private float realTimeSinceLastFrame;
    private float realTimeAtLastFrame;

    private STETilemap levelsize;
    private string lastScene;

    public Image fadeImage;
    public bool isInTransition;
    private float transition;
    private bool isShowing;
    private float duration;

    public float xBorder;
    public float yBorder;

    [SerializeField]
    private GameObject mobileControls;
    [SerializeField]
    private bool enableMobile;
    [SerializeField]
    private GameObject eventSystem;

    public static bool paused;
    public Text pauseText;

    private bool exitSelected;
    public Text _Exit;
    public Text _Continue;
    public Text _ExitShadow;
    public Text _ContinueShadow;
    private float pauseDelta;


    public void Fade(bool showing, float duration)
    {
        isShowing = showing;
        isInTransition = true;
        this.duration = duration;
        transition = (isShowing) ? 0 : 1;
    }

    private void Awake()
    {
        if (eventSystem.activeSelf)
        {
            eventSystem.SetActive(false);
        }
        if (camera_controller == null)
        {
            eventSystem.SetActive(true);
            camera_controller = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    public void OnSceneChange(Scene scene1, Scene scene2)
    {
        //Fade(false, 1f);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fadeImage.color = Color.black;
    }

    public void OnSceneUnloaded(Scene scene1)
    {
        Fade(false, 1f);
        player = Player_Controller.player_controller.transform;
        levelSizeX = 0;
        levelSizeY = 0;
        STETilemap[] objs = FindObjectsOfType(typeof(STETilemap)) as STETilemap[];
        foreach (STETilemap map in objs)
        {
            if (map.MaxGridX > levelSizeX)
            {
                levelSizeX = map.MaxGridX + 1;
            }
            if (map.MaxGridY > levelSizeY)
            {
                levelSizeY = map.MaxGridY + 1;
            }
        }
        if (levelSizeX <= (2* xBorder))
        {
            transform.position = new Vector2((levelSizeX / 2), transform.position.y);
        }
        else if (player.transform.position.x < xBorder)
        {
            transform.position = new Vector2(xBorder, transform.position.y);
        }
        else if (player.transform.position.x > levelSizeX - xBorder + 1)
        {
            transform.position = new Vector2(levelSizeX - xBorder, transform.position.y);
        }
        if (levelSizeY <= (2* yBorder))
        {
            transform.position = new Vector2(transform.position.x, (levelSizeY / 2) + 1);
        }
        else if (player.transform.position.y < yBorder)
        {
            transform.position = new Vector2(transform.position.x, yBorder + 1);
        }
        else if (player.transform.position.y > levelSizeY - yBorder)
        {
            transform.position = new Vector2(transform.position.x, (levelSizeY - yBorder));
        }
    }

    // Use this for initialization
    void Start()
    {
        realTimeSinceLastFrame = 0;
        realTimeAtLastFrame = Time.realtimeSinceStartup;
        OnSceneUnloaded(SceneManager.GetActiveScene());

        paused = false;

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            mobileControls.SetActive(true);
            enableMobile = true;
        }
        else
        {
            mobileControls.SetActive(false);
            enableMobile = false;
        }
        exitSelected = false;
    }

    private void Update()
    {
        if (enableMobile == true && !mobileControls.activeSelf)
        {
            mobileControls.SetActive(true);
        } else if (enableMobile == false && mobileControls.activeSelf)
        {
            mobileControls.SetActive(false);
        }

        if (Screen.height != 1920)
            Screen.SetResolution(1920, 1080, true);
        
        if ((Input.GetButtonDown("Pause") || Input.GetKeyDown(KeyCode.Pause)) && Player_Controller.player_controller.disableControls == false && paused == false)
        {
            Time.timeScale = 0;
            Player_Controller.player_controller.disableControls = true;
            Player_Controller.player_controller.inDialogue = true;
            pauseText.enabled = true;
            gameObject.GetComponentInChildren<GrayscaleFilter>().enabled = true;
            paused = !paused;

            _Exit.enabled = true;
            _ExitShadow.enabled = true;
            _Continue.enabled = true;
            _ContinueShadow.enabled = true;
            exitSelected = false;
            pauseDelta = 0;
        }
        else if ((CnInputManager.GetButtonDown("Pause") || Input.GetKeyDown(KeyCode.Pause) || ((CnInputManager.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Return)) && exitSelected == false)) && paused == true)
        {
            Time.timeScale = 1;
            Player_Controller.player_controller.disableControls = false;
            Player_Controller.player_controller.inDialogue = false;
            pauseText.enabled = false;
            gameObject.GetComponentInChildren<GrayscaleFilter>().enabled = false;
            paused = !paused;

            _Exit.enabled = false;
            _ExitShadow.enabled = false;
            _Exit.color = new Color(1, 0, 0, 1);
            _Continue.enabled = false;
            _ContinueShadow.enabled = false;
            _Continue.color = new Color(0, 1, 0, 1);
            exitSelected = false;
        }



        //Select Continue or Exit
        if (paused)
        {
            pauseDelta += .1f * realTimeSinceLastFrame * 10;
            if (CnInputManager.GetAxisRaw("Horizontal") > 0 && exitSelected == false)
            {
                exitSelected = true;
                _Continue.color = new Color(0, 1, 0, 1);
                pauseDelta = 0;
            }
            else if (CnInputManager.GetAxisRaw("Horizontal") < 0 && exitSelected == true)
            {
                exitSelected = false;
                _Exit.color = new Color(1, 0, 0, 1);
                pauseDelta = 0;
            }
            //If Exit is selected and the player presses jump or enter quits
            if (exitSelected==true && (CnInputManager.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Return)))
            {
                Application.Quit();
            }
            if (exitSelected == false)
            {
                _Continue.color = new Color(0, 1, 0, 1 - Mathf.PingPong(pauseDelta, .5f));
            }
            else if (exitSelected == true)
            {
                _Exit.color = new Color(1, 0, 0, 1 - Mathf.PingPong(pauseDelta, .5f));
            }
        }


        if (levelSizeX <= 2*xBorder)
        {
            targetPositionX = (levelSizeX / 2);
        }
        else if (player.transform.position.x >= xBorder && player.transform.position.x <= levelSizeX - xBorder)
        {
            targetPositionX = player.transform.position.x + xoffset;
        }
        else if (player.transform.position.x < xBorder)
        {
            targetPositionX = xBorder;
        }
        else if (player.transform.position.x > levelSizeX - xBorder)
        {
            targetPositionX = levelSizeX - xBorder;
        }
        if (levelSizeY <= 2*yBorder)
        {
            targetPositionY = (levelSizeY / 2) + 1;
        }
        else if (player.transform.position.y >= yBorder && player.transform.position.y <= levelSizeY - yBorder)
        {
            targetPositionY = player.transform.position.y + yoffset;
        }
        else if (player.transform.position.y < yBorder)
        {
            targetPositionY = yBorder;
        }
        else if (player.transform.position.y > levelSizeY - yBorder)
        {
            targetPositionY = levelSizeY - yBorder;
        }

        realTimeSinceLastFrame = Time.realtimeSinceStartup - realTimeAtLastFrame;

        targetPosition = new Vector2(targetPositionX, targetPositionY);
        transform.position = Vector2.Lerp(transform.position, targetPosition, speed * realTimeSinceLastFrame);

        realTimeAtLastFrame = Time.realtimeSinceStartup;

        if (!isInTransition) return;

        transition += (isShowing) ? realTimeSinceLastFrame * (1 / duration) : -realTimeSinceLastFrame * (1 / duration);
        fadeImage.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, transition);

        if (transition > 1 || transition < 0) isInTransition = false;
    }
}

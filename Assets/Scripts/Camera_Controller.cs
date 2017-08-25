using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CreativeSpore;
using UnityEngine.SceneManagement;

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

    private CreativeSpore.SuperTilemapEditor.Tilemap levelsize;
    private string lastScene;

    public Image fadeImage;
    private bool isInTransition;
    private float transition;
    private bool isShowing;
    private float duration;

    public float xBorder;
    public float yBorder;

    public void Fade(bool showing, float duration)
    {
        isShowing = showing;
        isInTransition = true;
        this.duration = duration;
        transition = (isShowing) ? 0 : 1;
    }

    private void Awake()
    {
        if (camera_controller == null)
        {
            camera_controller = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fadeImage.color = Color.black;
    }

    public void OnSceneUnloaded(Scene scene1)
    {
        Fade(false, 1f);

        levelSizeX = 0;
        levelSizeY = 0;
        CreativeSpore.SuperTilemapEditor.Tilemap[] objs = FindObjectsOfType(typeof(CreativeSpore.SuperTilemapEditor.Tilemap)) as CreativeSpore.SuperTilemapEditor.Tilemap[];
        foreach (CreativeSpore.SuperTilemapEditor.Tilemap map in objs)
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
        player = Player_Controller.player_controller.transform;
        OnSceneUnloaded(SceneManager.GetActiveScene());
    }

    // Update is called once per frame
    void LateUpdate()
    {

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

        transition += (isShowing) ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
        fadeImage.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, transition);

        if (transition > 1 || transition < 0) isInTransition = false;
    }
}

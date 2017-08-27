using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class On_Level_Load : MonoBehaviour {

    private float targetLocationX;
    private float targetLocationY;

    private Vector2 targetLocation;
    private float coolDown;
    private bool transitioning;

    void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChange;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        transitioning = false;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }


    private void OnSceneLoaded (Scene scene, LoadSceneMode mode)
    {

    }
    private void OnSceneUnloaded (Scene scene)
    {
        if (Player_Controller.player_controller.targetLoadX != 0 && Player_Controller.player_controller.targetLoadY != 0)
        {
            targetLocationX = Player_Controller.player_controller.targetLoadX;
            targetLocationY = Player_Controller.player_controller.targetLoadY;
            targetLocation = new Vector2(targetLocationX, targetLocationY);
            Player_Controller.player_controller.transform.position = targetLocation;
            Camera_Controller.camera_controller.transform.position = Player_Controller.player_controller.transform.position;
            Player_Controller.player_controller.disableControls = true;
            Debug.Log("Controls Disabled");
            coolDown = Time.realtimeSinceStartup;
            Debug.Log("cooling down");
            transitioning = true;
        }
    }
    private void OnSceneChange(Scene scene1, Scene scene2)
    {

    }
    private void Update()
    {
        if (Player_Controller.player_controller.disableControls && Time.realtimeSinceStartup > coolDown + .2f && transitioning == true)
        {
            Debug.Log("cooled down");
            Player_Controller.player_controller.disableControls = false;
            Debug.Log("Controls Enabled");
            transitioning = false;
        }
    }
}

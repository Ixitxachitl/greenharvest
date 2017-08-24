using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class On_Level_Load : MonoBehaviour {

    private float targetLocationX;
    private float targetLocationY;

    private Vector2 targetLocation;

    void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChange;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
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

    }
    private void OnSceneChange(Scene scene1, Scene scene2)
    {
        if (Player_Controller.player_controller.targetLoadX != 0 && Player_Controller.player_controller.targetLoadY != 0)
        {
            targetLocationX = Player_Controller.player_controller.targetLoadX;
            targetLocationY = Player_Controller.player_controller.targetLoadY;
            targetLocation = new Vector2(targetLocationX, targetLocationY);
            Player_Controller.player_controller.transform.position = targetLocation;
            Camera_Controller.camera_controller.transform.position = Player_Controller.player_controller.transform.position;
        }

    }
}

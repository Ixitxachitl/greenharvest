using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class door_control : MonoBehaviour {
    
    public string level;
    public float targetLocationX;
    public float targetLocationY;
    AsyncOperation asyncOperation;

    //private Vector2 targetLocation;

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //targetLocation = new Vector2(targetLocationX, targetLocationY);
        //Player_Controller.player_controller.transform.position = targetLocation;
        if (collision.gameObject.name == "Player Location")
        {
            Player_Controller.player_controller.targetLoadX = targetLocationX;
            Player_Controller.player_controller.targetLoadY = targetLocationY;
            StartLoading();
        }
        
    }
    public void StartLoading()
    {
        StartCoroutine("load");
    }

    IEnumerator load()
    {
        Debug.LogWarning("ASYNC LOAD STARTED - " +
           "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
        Player_Controller.player_controller.disableControls = true;
        Camera_Controller.camera_controller.Fade(true, 1f);
        yield return new WaitWhile(() => Camera_Controller.camera_controller.isInTransition);
        string currentScene = SceneManager.GetActiveScene().name;
        asyncOperation = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = true;
        yield return asyncOperation;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(level));
        SceneManager.UnloadSceneAsync(currentScene);
        Camera_Controller.camera_controller.Fade(false, 1f);
        yield return new WaitWhile(() => Camera_Controller.camera_controller.isInTransition);
        Player_Controller.player_controller.disableControls = false;
    }

}

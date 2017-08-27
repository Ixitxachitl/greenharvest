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
        string currentScene = SceneManager.GetActiveScene().name;
        asyncOperation = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
        Player_Controller.player_controller.disableControls = true;
        asyncOperation.allowSceneActivation = true;
        yield return asyncOperation;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(level));
        SceneManager.UnloadSceneAsync(currentScene);
        Player_Controller.player_controller.disableControls = false;
    }

}

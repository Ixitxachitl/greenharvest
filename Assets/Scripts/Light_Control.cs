using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Light_Control : MonoBehaviour {
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
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
    }
        private void OnSceneUnloaded(Scene scene)
    {
        GetComponent<Light>().enabled = true;
    }
    // Use this for initialization
    void Awake () {
        GetComponent<Light>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

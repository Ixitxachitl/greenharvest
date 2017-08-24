using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class GUItext2point : MonoBehaviour
{
    void Awake()
    {
        gameObject.GetComponent<Text>().font.material.mainTexture.filterMode = FilterMode.Point;
    }
}
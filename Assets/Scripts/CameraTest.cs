using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour {

    public float orthographicSize = 16.875f;
    public float aspect = 16/9;
    void Start()
    {
        Camera.main.projectionMatrix = Matrix4x4.Ortho(
                -orthographicSize * aspect, orthographicSize * aspect,
                -orthographicSize, orthographicSize,
                Camera.main.nearClipPlane, Camera.main.farClipPlane);
    }
}

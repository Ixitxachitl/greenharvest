using UnityEngine;
using System;

public class Debug_CameraMovement : MonoBehaviour
{
    float speed = .03f;

    void Update()
    {
        float xAxisValue = Input.GetAxisRaw("Horizontal") * speed;
        float yAxisValue = Input.GetAxisRaw("Vertical") * speed;

        if (!Input.anyKey)
        {
            if (transform.position.x > Math.Round(transform.position.x) + .01f)
            {
                transform.Translate(Vector3.left * Time.deltaTime);
            }
            else if (transform.position.x < Math.Round(transform.position.x) - .01f)
            {
                transform.Translate(Vector3.right * Time.deltaTime);
            }
            else if (transform.position.y > Math.Round(transform.position.y) + .01f)
            {
                transform.Translate(Vector3.down * Time.deltaTime);
            }
            else if (transform.position.y < Math.Round(transform.position.y) - .01f)
            {
                transform.Translate(Vector3.up * Time.deltaTime);
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x + xAxisValue, transform.position.y + yAxisValue, transform.position.z);
        }
        
    }
}
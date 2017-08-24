using UnityEngine;
using System;

public class SnapToPixelGrid : MonoBehaviour
{
    [SerializeField]
    private float pixelsPerUnit = 16f;

    private Transform parent;

    private void Start()
    {
        parent = transform.parent;
        Vector2 newLocalPosition = Vector2.zero;

        newLocalPosition.x = (Mathf.RoundToInt(parent.position.x * pixelsPerUnit) / pixelsPerUnit) - parent.position.x;
        newLocalPosition.y = (Mathf.RoundToInt(parent.position.y * pixelsPerUnit) / pixelsPerUnit) - parent.position.y;
        transform.localPosition = newLocalPosition;
    }

    /// <summary>
    /// Snap the object to the pixel grid determined by the given pixelsPerUnit.
    /// Using the parent's world position, this moves to the nearest pixel grid location by 
    /// offseting this GameObject by the difference between the parent position and pixel grid.
    /// </summary>
    private void LateUpdate()
    {
        Vector2 newLocalPosition = Vector2.zero;

        newLocalPosition.x = (Mathf.RoundToInt(parent.position.x * pixelsPerUnit) / pixelsPerUnit) - parent.position.x;
        newLocalPosition.y = (Mathf.RoundToInt(parent.position.y * pixelsPerUnit) / pixelsPerUnit) - parent.position.y;

        transform.localPosition = newLocalPosition;
    }
}
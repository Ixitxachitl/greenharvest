using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTriangle : MonoBehaviour
{
    [SerializeField]
    private Texture2D SpriteTexture;
    [SerializeField]
    private float PixelsPerUnit;

    // Use this for initialization
    void Start()
    {
        DrawTriange2D(new Vector2(0, 0), new Vector2(-1, 1), new Vector2(1, 1), Color.red, SpriteTexture, PixelsPerUnit);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void DrawTriange2D(Vector2 vertexA, Vector2 vertexB, Vector2 vertexC, Color color, Texture2D tex, float ppu)
    {
        GameObject triangle = new GameObject();
        SpriteRenderer sr = triangle.AddComponent<SpriteRenderer>();
        sr.color = color;
        Vector2[] vertices = new Vector2[3] { vertexA, vertexB, vertexC };
        Sprite s = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0, 0), ppu);
    }
}
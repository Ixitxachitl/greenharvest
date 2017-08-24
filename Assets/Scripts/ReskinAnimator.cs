using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReskinAnimator : MonoBehaviour
{
    [SerializeField]
    private string spriteSheetName; //this will be the name of your spritesheet, no file extension

    void LateUpdate()
    {

        foreach (var renderer in GetComponents<SpriteRenderer>())
        {
            string spriteName = renderer.sprite.name; //finds the name of the sprite to be rendered
            var subSprites = Resources.LoadAll<Sprite>(spriteSheetName); //loads all the sprites in your new sprite sheet
            foreach (var sprite in subSprites)
            {

                if (sprite.name == spriteName) //if the sprite has the same name as one you're trying to replace than replace it
                {
                    renderer.sprite = sprite;
                }
            }
        }
    }
}

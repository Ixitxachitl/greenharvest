using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tallGrassControl : MonoBehaviour {

    public static bool position = false;
    [SerializeField]
    private List<Sprite> deepGrassSprite;
    private Transform player;

	// Use this for initialization
	void OnEnable () {
        player = Player_Controller.player_controller.GetComponent<Transform>();
        transform.position = new Vector2(player.position.x - .5f, player.position.y- .5f);
        if (position)
        {
            transform.position = new Vector2(transform.position.x + 1f, transform.position.y);
            GetComponent<SpriteRenderer>().sprite = deepGrassSprite[1];
        }
        else
            GetComponent<SpriteRenderer>().sprite = deepGrassSprite[0];
        position = !position;
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector2.Distance(transform.position, player.position) > 1.5f)
        {
            gameObject.SetActive(false);
        }
	}
    private void OnDisable()
    {
        CancelInvoke();
    }
}

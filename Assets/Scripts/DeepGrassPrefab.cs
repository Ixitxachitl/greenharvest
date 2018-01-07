using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepGrassPrefab : MonoBehaviour
{

    [SerializeField]
    private GameObject leftGrass;
    [SerializeField]
    private GameObject rightGrass;
    private bool grassShowing;

    // Use this for initialization
    void Start()
    {
        grassShowing = false;
        hideGrass();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y), new Vector2(1,1), 0, Vector2.zero, 10);
        if (hit)
            CancelInvoke();
        if (hit && !grassShowing)
        {
            var i = Random.Range(0, 3);
            if (i == 0)
                leftGrass.SetActive(true);
            else if (i == 1)
                rightGrass.SetActive(true);
            else if (i == 2)
            {
                leftGrass.SetActive(true);
                rightGrass.SetActive(true);
            }
            grassShowing = true;
        }
        else if (!hit && grassShowing)
        {
            Invoke("hideGrass", .2f);
        }
    }
    void hideGrass()
    {
        if (leftGrass.activeSelf == true)
            leftGrass.SetActive(false);
        if (rightGrass.activeSelf == true)
            rightGrass.SetActive(false);
        grassShowing = false;
    }
}

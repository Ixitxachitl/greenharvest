using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Movement : MonoBehaviour {

    private Animator anim;
    private Rigidbody2D rb2d;
    [SerializeField]
    private float movementSpeed = 1;
    [SerializeField]
    private float detectionRadius = 5;
    private GameObject player;
    [SerializeField]
    private LayerMask playerlayer;
    private bool moving;
    private bool stoppedMoving;
    private float countdownSit;
    private float countdownStop;
    private float sitDelay;
    public Vector2 lastMove;

    public bool MovementEnabled = false;

	// Use this for initialization
	void Start () {
        player = Player_Controller.player_controller.gameObject;
        anim = GetComponentInChildren<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        moving = false;
        sitDelay = 1;
        lastMove = Vector2.down;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > countdownSit + sitDelay)
            anim.SetBool("Sitting", true);
        else if (!moving)
            anim.SetBool("Sitting", false);

        if (Time.time > countdownStop + .2f && stoppedMoving == true)
        {
            moving = false;
            stoppedMoving = false;
        }

        if (moving)
        {
            Vector2 relitiveDistance = player.transform.position - transform.position;
            relitiveDistance =new Vector2(Mathf.Clamp(relitiveDistance.x,-1, 1), Mathf.Clamp(relitiveDistance.y,-1,  1));
            if (Mathf.Approximately(relitiveDistance.y, 1) && !Mathf.Approximately(Mathf.Abs(relitiveDistance.x), 1))
            {
                anim.SetFloat("MoveY", 1);
                anim.SetFloat("MoveX", 0);
                lastMove = new Vector2(0f, 1f);
            }
            else if (Mathf.Approximately(relitiveDistance.y, -1) && !Mathf.Approximately(Mathf.Abs(relitiveDistance.x), 1))
            {
                anim.SetFloat("MoveY", -1);
                anim.SetFloat("MoveX", 0);
                lastMove = new Vector2(0f, -1f);
            }
            else if (Mathf.Approximately(relitiveDistance.x, 1) && !Mathf.Approximately(Mathf.Abs(relitiveDistance.y), 1))
            {
                anim.SetFloat("MoveY", 0);
                anim.SetFloat("MoveX", 1);
                lastMove = new Vector2(1f, 0);
            }
            else if (Mathf.Approximately(relitiveDistance.x, -1) && !Mathf.Approximately(Mathf.Abs(relitiveDistance.y), 1))
            {
                anim.SetFloat("MoveY", 0);
                anim.SetFloat("MoveX", -1);
                lastMove = new Vector2(-1f, 0);
            }
            else if (Mathf.Approximately(relitiveDistance.x, 1) && Mathf.Approximately(relitiveDistance.y, 1))
            {
                anim.SetFloat("MoveY", 1);
                anim.SetFloat("MoveX", 1);
                lastMove = new Vector2(1f, 1f);
            }
            else if (Mathf.Approximately(relitiveDistance.x, 1) && Mathf.Approximately(relitiveDistance.y, -1))
            {
                anim.SetFloat("MoveY", -1);
                anim.SetFloat("MoveX", 1);
                lastMove = new Vector2(1f, -1f);
            }
            else if (Mathf.Approximately(relitiveDistance.x, -1) && Mathf.Approximately(relitiveDistance.y, 1))
            {
                anim.SetFloat("MoveY", 1);
                anim.SetFloat("MoveX", -1);
                lastMove = new Vector2(-1f, 1f);
            }
            else if (Mathf.Approximately(relitiveDistance.x, -1) && Mathf.Approximately(relitiveDistance.y, -1))
            {
                anim.SetFloat("MoveY", -1);
                anim.SetFloat("MoveX", -1);
                lastMove = new Vector2(-1f, -1f);
            }
        }

        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
        anim.SetBool("Moving", moving);

        if (moving == true)
        {
            countdownSit = Time.time;
        }

    }
    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= detectionRadius && Vector2.Distance(transform.position, player.transform.position) >= 4 && MovementEnabled)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.InverseTransformPoint(player.transform.position), Vector2.Distance(transform.position, player.transform.position) + .5f, ~(1 << 9 | 1 << 11 | 1 << 12));
            if ((hit && hit.transform.gameObject.layer == 10) || !hit)
            {
                moving = true;
                stoppedMoving = false;
                rb2d.AddForce(transform.InverseTransformPoint(player.transform.position).normalized * movementSpeed * Time.timeScale * 10 * rb2d.mass * (rb2d.drag/10));
                Debug.DrawRay(transform.position, transform.InverseTransformPoint(player.transform.position), Color.green);
            }
            else
            {
                if (stoppedMoving == false)
                {
                    stoppedMoving = true;
                    countdownStop = Time.time;
                }
                Debug.DrawRay(transform.position, transform.InverseTransformPoint(player.transform.position), Color.red);
            }
        }
        else if (stoppedMoving == false)
        {
            stoppedMoving = true;
            countdownStop = Time.time;
        }
    }
}

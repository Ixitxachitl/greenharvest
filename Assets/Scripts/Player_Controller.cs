using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;


public class Player_Controller : MonoBehaviour
{

    public static Player_Controller player_controller;

    public string lastLevel;
    public float targetLoadX = 0;
    public float targetLoadY = 0;

    public LayerMask nojumpLayer;
    public LayerMask interactable;
    public LayerMask tallGrass;
    private LineRenderer line;

    private float movementSpeed;
    public bool keyInput;
    private float walkSpeed = 6f;
    private float runSpeed = 8f;

    public Animator anim;
    private Rigidbody2D rb2d;
    private PolygonCollider2D PC;
    private bool moving;
    public Vector2 lastMove;
    private float countdownSit;
    private float sitDelay = 3f;

    private float countdownJump;
    private float jumpTime = .25f;
    public bool jumping;
    private bool running;
    public bool nojump;

    private Vector2 jumpTargetA;
    private Vector2 jumpTargetB;

    private Collider2D boxCol;

    //public GameObject squareBlock;

    private bool newline;
    private float linedelay;
    private float linetime;
    public bool disableControls;

    public int maxTallGrass = 5;
    public GameObject tallGrassObject;
    private List<GameObject> tallGrassObjects;
    private bool tallGrassEnter;
    private Vector3 tallGrassPosition;


    void DrawLine(Vector2 startingPoint, Vector2 endPoint, Color color, int delay)
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = true;
        line.sortingLayerName = "UI";
        line.sortingOrder = 2;
        line.positionCount = 2 ;
        line.SetPosition(0, startingPoint);
        line.SetPosition(1, endPoint);
        line.startWidth = .1f;
        line.endWidth = .1f;
        line.useWorldSpace = true;
        line.startColor = color;
        line.endColor = color;
        newline = true;
        linedelay = delay;
    }
    void HideLine()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = false;
    }
    private void Awake()
    {
        if (player_controller == null)
        {
            player_controller = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim.SetBool("Sitting", true);
        moving = false;
        running = false;
        anim.SetBool("Moving", false);
        anim.SetBool("Jumping", false);
        lastMove = new Vector2(0f, 0f);
        nojump = false;
        PC = GetComponentInChildren<PolygonCollider2D>();
        disableControls = false;
        tallGrassPosition = new Vector3();
        tallGrassObjects = new List<GameObject>();
        for (int i = 0; i < maxTallGrass; i++)
        {
            GameObject obj = (GameObject)Instantiate(tallGrassObject);
            DontDestroyOnLoad(obj);
            obj.SetActive(false);
            tallGrassObjects.Add(obj);
        }
    }

    void ActivateTallGrass()
    {
        for(int i = 0; i < tallGrassObjects.Count; i++)
        {
            if (!tallGrassObjects[i].activeInHierarchy)
            {
                tallGrassObjects[i].SetActive(true);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (disableControls && Time.timeScale !=0)
            Time.timeScale = 0;
        else if (Time.timeScale == 0 && !disableControls)
            Time.timeScale = 1;

        if (Time.time > countdownSit + sitDelay)
            anim.SetBool("Sitting", true);
        else if (!moving)
            anim.SetBool("Sitting", false);

        if (newline)
        {
            newline = false;
            linetime = Time.time;
        }
        //if (Time.time > linetime + linedelay)
        //{ 
        //    HideLine();
        //}
        RaycastHit2D onTallGrass = Physics2D.Raycast(transform.position, Vector2.zero,100,tallGrass);
        if (onTallGrass)
        {
            if (!tallGrassEnter)
            {
                tallGrassEnter = true;
                tallGrassPosition = transform.position;
                ActivateTallGrass();
            }
            if (Vector2.Distance(tallGrassPosition, transform.position) > .5f)
            {
                ActivateTallGrass();
                tallGrassPosition = transform.position;
            }
        }
        if (!onTallGrass && tallGrassEnter)
        {
            tallGrassEnter = false;
        }

        if (Physics2D.Raycast(transform.position,lastMove,4, nojumpLayer))
        {
            nojump = true;
            PC.enabled = true;
        }
        if (Physics2D.Raycast(transform.position, lastMove, 4, nojumpLayer))
        {
            nojump = true;
            PC.enabled = true;
        }
        else
        {
            nojump = false;
        }

        if (jumping == true && Time.time > countdownJump + jumpTime)
        {
            jumping = false;
            nojump = false;
            walkSpeed -= 4;
            runSpeed -= 4;
            PC.enabled = true;
        }

        if (CnInputManager.GetButtonDown("Fire3") && moving == true && movementSpeed == runSpeed && !disableControls)
        {
            runSpeed += 4;
            running = true;
        }
        else if (CnInputManager.GetButtonUp("Fire3") && running == true && !disableControls)
        {
            running = false;
            runSpeed -= 4;
        }
        if (running == false && CnInputManager.GetButton("Fire3") && movementSpeed == runSpeed && !disableControls)
        {
            runSpeed += 4;
            running = true;
        }

        if ((!Input.anyKey || (CnInputManager.GetAxisRaw("Horizontal") > -.2f && CnInputManager.GetAxisRaw("Horizontal") < .2f && CnInputManager.GetAxisRaw("Vertical") > -.2f && CnInputManager.GetAxisRaw("Vertical") < .2f)) && !disableControls)
        {
            moving = false;
            rb2d.velocity = Vector2.zero;
            anim.SetFloat("MoveX", 0);
            anim.SetFloat("MoveY", 0);
        }

        if (Input.GetButtonUp("Horizontal"))
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        if (Input.GetButtonUp("Vertical"))
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);

        if ((CnInputManager.GetAxis("Horizontal") > 0.7f || CnInputManager.GetAxis("Horizontal") < -0.7f || CnInputManager.GetAxis("Vertical") > 0.7f || CnInputManager.GetAxis("Vertical") < -0.7f) && !disableControls)
            movementSpeed = runSpeed;
        else
        {
            movementSpeed = walkSpeed;
            if (running == true)
            {
                running = false;
                runSpeed -= 4;
            }
        }


        if (CnInputManager.GetAxis("Horizontal") > 0f && CnInputManager.GetAxis("Vertical") < .2f && CnInputManager.GetAxis("Vertical") > -.2f && !disableControls)
        {
            rb2d.velocity = new Vector2(1 * movementSpeed, 0);
            anim.SetFloat("MoveX", 1);
            anim.SetFloat("MoveY", 0);
            moving = true;
            lastMove = new Vector2(1f, 0f);
        }
        if (CnInputManager.GetAxis("Horizontal") < 0f && CnInputManager.GetAxis("Vertical") < .2f && CnInputManager.GetAxis("Vertical") > -.2f && !disableControls)
        {
            rb2d.velocity = new Vector2(-1 * movementSpeed, 0);
            anim.SetFloat("MoveX", -1);
            anim.SetFloat("MoveY", 0);
            moving = true;
            lastMove = new Vector2(-1f, 0f);
        }
        if (CnInputManager.GetAxis("Vertical") > 0f && CnInputManager.GetAxis("Horizontal") < .2f && CnInputManager.GetAxis("Horizontal") > -.2f && !disableControls)
        {
            rb2d.velocity = new Vector2(0, 1 * movementSpeed);
            anim.SetFloat("MoveY", 1);
            anim.SetFloat("MoveX", 0);
            moving = true;
            lastMove = new Vector2(0f, 1f);
        }
        if (CnInputManager.GetAxis("Vertical") < 0f && CnInputManager.GetAxis("Horizontal") < .2f && CnInputManager.GetAxis("Horizontal") > -.2f && !disableControls)
        {
            rb2d.velocity = new Vector2(0, -1 * movementSpeed);
            anim.SetFloat("MoveY", -1);
            anim.SetFloat("MoveX", 0);
            moving = true;
            lastMove = new Vector2(0f, -1f);
        }
        if (CnInputManager.GetAxis("Horizontal") > .2f && CnInputManager.GetAxis("Vertical") > .2f && !disableControls)
        {
            rb2d.velocity = new Vector2(1 * movementSpeed, 1 * movementSpeed);
            anim.SetFloat("MoveX", 1);
            anim.SetFloat("MoveY", 1);
            moving = true;
            lastMove = new Vector2(1f, 1f);
        }
        if (CnInputManager.GetAxis("Horizontal") < -.2f && CnInputManager.GetAxis("Vertical") > .2f && !disableControls)
        {
            rb2d.velocity = new Vector2(-1 * movementSpeed, 1 * movementSpeed);
            anim.SetFloat("MoveX", -1);
            anim.SetFloat("MoveY", 1);
            moving = true;
            lastMove = new Vector2(-1f, 1f);
        }
        if (CnInputManager.GetAxis("Vertical") < -.2f && CnInputManager.GetAxis("Horizontal") < -.2f && !disableControls)
        {
            rb2d.velocity = new Vector2(-1 * movementSpeed, -1 * movementSpeed);
            anim.SetFloat("MoveY", -1);
            anim.SetFloat("MoveX", -1);
            moving = true;
            lastMove = new Vector2(-1f, -1f);
        }
        if (CnInputManager.GetAxis("Vertical") < -.2f && CnInputManager.GetAxis("Horizontal") > .2f && !disableControls)
        {
            rb2d.velocity = new Vector2(1 * movementSpeed, -1 * movementSpeed);
            anim.SetFloat("MoveY", -1);
            anim.SetFloat("MoveX", 1);
            moving = true;
            lastMove = new Vector2(1f, -1f);
        }

        anim.SetBool("Jumping", jumping);
        anim.SetBool("Moving", moving);
        anim.SetBool("Running", running);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);


        if (lastMove == new Vector2(0f, 1f))
        {
            jumpTargetA = new Vector2(transform.position.x - .5f, transform.position.y + movementSpeed / 3f + 1f);
            jumpTargetB = new Vector2(transform.position.x + .5f, transform.position.y + movementSpeed / 3f + 2f);
            //squareBlock.transform.position = new Vector2(transform.position.x, transform.position.y + movementSpeed / 3f + 1f);
        }
        if (lastMove == new Vector2(0f, -1f) || lastMove == new Vector2(0f, 0f))
        {
            jumpTargetA = new Vector2(transform.position.x - .5f, transform.position.y - movementSpeed / 3f + .05f);
            jumpTargetB = new Vector2(transform.position.x + .5f, transform.position.y - movementSpeed / 3f - 1f);
            //squareBlock.transform.position = new Vector2(transform.position.x, transform.position.y - movementSpeed / 3f);
        }
        if (lastMove == new Vector2(-1f, 0f))
        {
            jumpTargetA = new Vector2(transform.position.x - movementSpeed / 3f, transform.position.y - .5f);
            jumpTargetB = new Vector2(transform.position.x - movementSpeed / 3f, transform.position.y + .5f);
            //squareBlock.transform.position = new Vector2(transform.position.x - movementSpeed / 3 - .5f, transform.position.y + .5f);
        }
        if (lastMove == new Vector2(1f, 0f))
        {
            jumpTargetA = new Vector2(transform.position.x + movementSpeed / 3f, transform.position.y - .5f);
            jumpTargetB = new Vector2(transform.position.x + movementSpeed / 3f, transform.position.y + .5f);
            //squareBlock.transform.position = new Vector2(transform.position.x + movementSpeed / 3f + .5f, transform.position.y + .5f);
        }
        if (lastMove == new Vector2(1f, 1f))
        {
            jumpTargetA = new Vector2(transform.position.x + movementSpeed / 3f - .5f, transform.position.y + movementSpeed / 3f);
            jumpTargetB = new Vector2(transform.position.x + movementSpeed / 3f + .5f, transform.position.y + movementSpeed / 3f + 1f);
            //squareBlock.transform.position = new Vector2(transform.position.x + movementSpeed / 3f, transform.position.y + movementSpeed / 3f + .5f);
        }
        if (lastMove == new Vector2(-1f, 1f))
        {
            jumpTargetA = new Vector2(transform.position.x - movementSpeed / 3f + .5f, transform.position.y + movementSpeed / 3f);
            jumpTargetB = new Vector2(transform.position.x - movementSpeed / 3f - .5f, transform.position.y + movementSpeed / 3f + 1f);
            //squareBlock.transform.position = new Vector2(transform.position.x - movementSpeed / 3f, transform.position.y + movementSpeed / 3f + .5f);
        }
        if (lastMove == new Vector2(-1f, -1f))
        {
            jumpTargetA = new Vector2(transform.position.x - movementSpeed / 3f + .5f, transform.position.y - movementSpeed / 3f + 1f);
            jumpTargetB = new Vector2(transform.position.x - movementSpeed / 3f - .5f, transform.position.y - movementSpeed / 3f);
            //squareBlock.transform.position = new Vector2(transform.position.x - movementSpeed / 3f, transform.position.y - movementSpeed / 3f + .5f);
        }
        if (lastMove == new Vector2(1f, -1f))
        {
            jumpTargetA = new Vector2(transform.position.x + movementSpeed / 3f - .5f, transform.position.y - movementSpeed / 3f + 1f);
            jumpTargetB = new Vector2(transform.position.x + movementSpeed / 3f + .5f, transform.position.y - movementSpeed / 3f);
            //squareBlock.transform.position = new Vector2(transform.position.x + movementSpeed / 3f, transform.position.y - movementSpeed / 3f + .5f);
        }
        if (moving == true)
        {
            countdownSit = Time.time;
            //squareBlock.GetComponent<SpriteRenderer>().enabled = true;
        }
        //else
            //squareBlock.GetComponent<SpriteRenderer>().enabled = false;

        boxCol = Physics2D.OverlapArea(jumpTargetA, jumpTargetB);
        if (boxCol != null)
        {
            PC.enabled = true;
        }


        if (CnInputManager.GetButtonDown("Jump") && jumping == false && !disableControls && !Physics2D.Raycast(transform.position, lastMove, 2, interactable))
        {
            jumping = true;
            countdownJump = Time.time;
            countdownSit = Time.time;
            walkSpeed += 4;
            runSpeed += 4;

            //DrawLine(new Vector2(transform.position.x, transform.position.y + 1), new Vector2(transform.position.x, transform.position.y + 1)
            //    + new Vector2 (lastMove.x * 4, lastMove.y * 4), Color.red, 1);

            if (boxCol == null && nojump == false)
            {
                PC.enabled = false;
            }
        }
    }
}
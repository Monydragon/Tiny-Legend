using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class NpcController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Vector2 minRangeWalk;
    public Vector2 maxRangeWalk;
    public float timeToWaitAfterMove;
    public Flowchart npcFlowchart;
    public BlockReference startBlock;

    private Vector2 startLocation;
    private Vector2 randomPos;
    private Animator anim;
    private bool randomGen;
    private float counter;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        startLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!randomGen)
        {
            randomPos = new Vector2(Random.Range(minRangeWalk.x, maxRangeWalk.x), Random.Range(minRangeWalk.y, maxRangeWalk.y));
            randomGen = true;
        }
        var movement = Vector2.MoveTowards(transform.position, randomPos, moveSpeed * Time.deltaTime);
        var posX = randomPos.x - transform.position.x;
        var posY = randomPos.y - transform.position.y;
        anim.SetFloat("MoveX", posX);
        anim.SetFloat("MoveY", posY);
        if (Vector2.Distance(transform.position, randomPos) > 0.01f)
        {
            anim.SetBool("isMoving", true);
            var lastMove = movement;
            anim.SetFloat("LastMoveX", posX);
            anim.SetFloat("LastMoveY", posY);
            transform.position = movement;
        }
        else if (Vector2.Distance(transform.position, randomPos) < 0.01f && counter <= 0)
        {
            anim.SetBool("isMoving", false);
            Debug.Log("Reached Destination");
            randomGen = false;
            counter = timeToWaitAfterMove;
        }
        else
        {
            anim.SetBool("isMoving", false);
            randomGen = false;
        }

        counter -= Time.deltaTime;
        
    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision != null && collision.gameObject.tag == "Player")
        {
            npcFlowchart.ExecuteBlock(startBlock.block);
        }
    }
}

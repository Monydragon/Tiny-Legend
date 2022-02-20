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
    public InventoryObject inventory;

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
        counter -= Time.deltaTime;
        if (!randomGen && counter <= 0)
        {
            randomPos = new Vector2(Random.Range(minRangeWalk.x, maxRangeWalk.x), Random.Range(minRangeWalk.y, maxRangeWalk.y));
            randomGen = true;
        }
        else if (randomGen)
        {
            var movement = Vector2.MoveTowards(transform.position, randomPos, moveSpeed * Time.deltaTime);
            var posX = randomPos.x - transform.position.x;
            var posY = randomPos.y - transform.position.y;
            anim.SetFloat("MoveX", posX);
            anim.SetFloat("MoveY", posY);
            if (Vector2.Distance(transform.position, randomPos) > 0.1f)
            {
                anim.SetBool("isMoving", true);
                var lastMove = movement;
                anim.SetFloat("LastMoveX", posX);
                anim.SetFloat("LastMoveY", posY);
                transform.position = movement;
            }
            else if (Vector2.Distance(transform.position, randomPos) < 0.1f)
            {
                anim.SetBool("isMoving", false);
                randomGen = false;
                counter = timeToWaitAfterMove;
            }
        }
        
    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision != null && collision.gameObject.tag == "Player")
        {
            npcFlowchart.ExecuteBlock(startBlock.block);
        }
    }
}

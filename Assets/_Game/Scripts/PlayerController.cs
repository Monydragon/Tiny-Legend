using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health = 100;
    public float speed;
    public float attackSpeed = 0.5f;
    public int damage;
    private Vector2 movement;
    private Vector2 lastMove;
    [SerializeField]
    private Transform playerWeaponCol;
    private Collider2D playerWeaponTriggerCol;
    [SerializeField]
    private Animator playerAnim;
    [SerializeField]
    private Animator playerAttackAnim;
    private Coroutine attackRef;

    private void Awake()
    {

        if(playerWeaponCol != null)
        {
            playerWeaponTriggerCol = playerWeaponCol.GetComponent<Collider2D>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        playerAnim.SetFloat("MoveX", movement.x);
        playerAnim.SetFloat("MoveY", movement.y);
        if (movement != Vector2.zero)
        {
            playerAnim.SetBool("isMoving", true);
            lastMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            playerAnim.SetFloat("LastMoveX", lastMove.x);
            playerAnim.SetFloat("LastMoveY", lastMove.y);
            
            if(movement.x > 0 && movement.y < movement.x)
            {
                playerWeaponCol.localPosition = new Vector3(1, 0);
            }
            else if (movement.x < 0 && movement.y > movement.x)
            {
                playerWeaponCol.localPosition = new Vector3(-1, 0);
            }
            else if(movement.y > 0 && movement.x < movement.y)
            {
                playerWeaponCol.localPosition = new Vector3(0, 1);
            }
            else
            {
                playerWeaponCol.localPosition = new Vector3(0, -1);
            }
        }
        else
        {
            playerAnim.SetBool("isMoving", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (attackRef == null)
            {
                attackRef = StartCoroutine(StartAttack());
            }
        }

        transform.position += (Vector3)movement * Time.deltaTime * speed;
    }

    public IEnumerator StartAttack()
    {
        playerAttackAnim.SetBool("isAttacking", true);
        var hits = Physics2D.BoxCastAll(playerWeaponTriggerCol.bounds.min, playerWeaponTriggerCol.bounds.size, 0, Vector2.zero);
        for (int i = 0; i < hits.Length; i++)
        {
            if(hits[i].collider.gameObject.tag == "Enemy")
            {
                var enemy = hits[i].collider.gameObject.GetComponent<EnemyController>();
                enemy.health -= damage;
                if(enemy.health <= 0)
                {
                    Destroy(enemy.gameObject);
                }
            }
        }

        yield return new WaitForSeconds(attackSpeed);
        playerAttackAnim.SetBool("isAttacking", false);
        attackRef = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health = 10;
    public float speed;
    public float attackSpeed = 0.5f;
    public int damage;
    public InventoryObject inventory;

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
    private void OnEnable()
    {
        EventManager.onActorDamaged += EventManager_onActorDamaged;
        EventManager.onItemUse += EventManager_onItemUse;
    }


    private void EventManager_onItemUse(ItemObject _item, GameObject _obj)
    {
        if(_obj == this.gameObject)
        {
            Debug.Log($"Player Used Item: {_item.name}");
            inventory.RemoveItem(_item);
        }
    }

    private void EventManager_onActorDamaged(GameObject _obj, int _dmg)
    {
        if (_obj == this.gameObject)
        {
            health -= _dmg;
            if (health <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        EventManager.onActorDamaged -= EventManager_onActorDamaged;
        EventManager.onItemUse -= EventManager_onItemUse;
    }

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

        transform.position += (Vector3)movement.normalized * Time.deltaTime * speed;

        if (Input.GetKeyDown(KeyCode.L))
        {
            inventory.Load();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            inventory.Save();
        }
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
                EventManager.ActorDamaged(enemy.gameObject, damage);
            }
        }

        yield return new WaitForSeconds(attackSpeed);
        playerAttackAnim.SetBool("isAttacking", false);
        attackRef = null;
    }

    private void OnApplicationQuit()
    {
        inventory.container.Clear();
    }
}

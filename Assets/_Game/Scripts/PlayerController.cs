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
        EventManager.onItemPickup += EventManager_onItemPickup;
        EventManager.onItemUse += EventManager_onItemUse;
        EventManager.onItemRemove += EventManager_onItemRemove;
    }

    private void EventManager_onItemRemove(InventoryObject _inventory, ItemObject _item, int _amount)
    {
        var foundItem = _inventory.container.Find(x=> x.item == _item);
        if (foundItem != null)
        {
            if(foundItem.amount > 1)
            {
                foundItem.amount -= _amount;
            }
            else
            {
                _inventory.RemoveItem(_item);
            }
        }
    }

    private void EventManager_onItemUse(ItemObject _item, GameObject _obj)
    {
        if(_obj == this.gameObject)
        {
            Debug.Log($"Player Used Item: {_item.name}");
            EventManager.ItemRemove(inventory,_item, 1);
        }
    }

    private void EventManager_onItemPickup(InventoryObject _inventory, ItemObject _item, int _amount)
    {
        _inventory.AddItem(_item,_amount);
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
        EventManager.onItemPickup -= EventManager_onItemPickup;
        EventManager.onItemUse -= EventManager_onItemUse;
        EventManager.onItemRemove -= EventManager_onItemRemove;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 50;
    public float attackSpeed = 1f;
    public int damage = 10;
    public float followSpeed;
    public float followDistance = 2f;
    private Coroutine attackRef;
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, playerTransform.position) > followDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, followSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && attackRef == null)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            attackRef = StartCoroutine(StartDamage(player, damage));
        }
    }

    private IEnumerator StartDamage(PlayerController player, int damage)
    {
        yield return new WaitForSeconds(attackSpeed);
        player.health -= damage;
        if(player.health <= 0)
        {
            player.gameObject.SetActive(false);
        }
        attackRef = null;
    }
    private void OnDestroy()
    {
        Debug.Log($"{name} Defeated");
    }
}

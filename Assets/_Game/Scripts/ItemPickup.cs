using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemObject item;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            player.inventory.AddItem(item, amount);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiController : MonoBehaviour
{
    public Image[] heartContainers;
    public Sprite fullHeartContainer;
    public Sprite emptyHeartContainer;
    public GameObject inventoryContainer;
    public int maxInventorySize = 8;
    public GameObject itemSlotPrefab;
    public List<GameObject> inventoryItems;

    private void OnEnable()
    {
        EventManager.onActorDamaged += EventManager_onActorDamaged;
        EventManager.onItemPickup += EventManager_onItemPickup;
        EventManager.onItemRemove += EventManager_onItemRemove;
    }

    private void EventManager_onItemRemove(InventoryObject _inventory, ItemObject _item, int _amount)
    {
        var slot = inventoryItems.Find(x=> x.name == _item.name);
        var slotAmount = int.Parse(slot.transform.GetChild(2).GetComponent<TMP_Text>().text);
        if(slotAmount > 1)
        {
            slot.transform.GetChild(2).GetComponent<TMP_Text>().text = (slotAmount - _amount).ToString();
        }
        else
        {
            inventoryItems.Remove(slot);
            Destroy(slot);
        }
    }

    private void EventManager_onItemPickup(InventoryObject _inventory, ItemObject _item, int _amount)
    {
        if(inventoryItems.Count <= maxInventorySize)
        {
            var itemFound = _inventory.container.Find(x => x.item == _item);

            if(itemFound != null && inventoryItems.Exists(x=> x.name == _item.name))
            {
                var slot = inventoryItems.Find(x=> x.name == _item.name);
                slot.transform.GetChild(2).GetComponent<TMP_Text>().text = itemFound.amount.ToString();
            }
            else
            {
                var itemSlot = Instantiate(itemSlotPrefab, inventoryContainer.transform);
                itemSlot.name = _item.name;
                itemSlot.GetComponent<Image>().sprite = _item.icon;
                itemSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = _item.name;
                itemSlot.transform.GetChild(1).GetComponent<TMP_Text>().text = _item.description;
                itemSlot.transform.GetChild(2).GetComponent<TMP_Text>().text = _amount.ToString();
                itemSlot.GetComponent<ItemSlotUse>().item = _item;
                inventoryItems.Add(itemSlot);
            }

        }



    }

    private void EventManager_onActorDamaged(GameObject _obj, int _dmg)
    {
        if(_obj.tag == "Player")
        {
            var player = _obj.GetComponent<PlayerController>();
            for (int i = 0; i < heartContainers.Length; i++)
            {
                if (player.health - _dmg > i)
                {
                    heartContainers[i].sprite = fullHeartContainer;
                }
                else
                {
                    heartContainers[i].sprite = emptyHeartContainer;

                }
            }
        }
    }

    private void OnDisable()
    {
        EventManager.onActorDamaged -= EventManager_onActorDamaged;
        EventManager.onItemPickup -= EventManager_onItemPickup;
        EventManager.onItemRemove -= EventManager_onItemRemove;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}

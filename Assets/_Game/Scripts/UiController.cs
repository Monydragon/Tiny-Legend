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
        EventManager.onInventoryChanged += UpdateInventoryUI;
    }

    public void UpdateInventoryUI(InventoryObject _inventory)
    {
        inventoryItems.Clear();
        foreach (Transform child in inventoryContainer.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < _inventory.container.Count; i++)
        {
            var _item = _inventory.container[i];

            var itemSlot = Instantiate(itemSlotPrefab, inventoryContainer.transform);
            itemSlot.name = _item.item.name;
            itemSlot.GetComponent<Image>().sprite = _item.item.icon;
            itemSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = _item.item.name;
            itemSlot.transform.GetChild(1).GetComponent<TMP_Text>().text = _item.item.description;
            itemSlot.transform.GetChild(2).GetComponent<TMP_Text>().text = _item.amount.ToString();
            itemSlot.GetComponent<ItemSlotUse>().item = _item.item;
            inventoryItems.Add(itemSlot);

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
        EventManager.onInventoryChanged -= UpdateInventoryUI;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "DLS/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> container = new List<InventorySlot>();

    public void AddItem(ItemObject _item, int _amount)
    {
        var foundItem = container.Find(x=> x.item == _item);
        if (foundItem != null)
        {
            foundItem.amount += _amount;
        }
        else
        {
            container.Add(new InventorySlot(_item, _amount));
        }
    }

    public void RemoveItem(ItemObject _item)
    {
        var foundItem = container.Find(x => x.item == _item);
        if(foundItem != null)
        {
            if(foundItem.amount > 1)
            {
                foundItem.amount--;
            }
            else
            {
                container.Remove(foundItem);
            }
        }
    }

    public ItemObject GetItem(string _name)
    {
        return container.Find(x=> x.item.name == _name).item;
    }

    public InventorySlot GetItemSlot(string _name)
    {
        return container.Find(x => x.item.name == _name);
    }

    public bool ItemExists(string _name)
    {
        return container.Exists(x=> x.item.name == _name);
    }

}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;

    public InventorySlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
}

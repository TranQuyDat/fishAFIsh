using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour 
{
    public InventoryCell[] inventoryCells;
    public GameObject itemPrefap;
    public int MaxCountInSlot = 4;
    public Text TextItemselected;
    private InventoryCell useitemslot=null;
    public bool AddItem(Items item)
    {

        for (int i = 0; i < inventoryCells.Length; i++)
        {
            InventoryCell slot = inventoryCells[i];
            InventoryItems itemInSlot = slot.GetComponentInChildren<InventoryItems>();
            if (itemInSlot != null && 
                itemInSlot.item == item &&
                itemInSlot.Count < MaxCountInSlot &&
                itemInSlot.item.stackable == true)
            {
                itemInSlot.Count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0;i<inventoryCells.Length; i++)
        {
            InventoryCell slot = inventoryCells[i];
            InventoryItems itemInSlot = slot.GetComponentInChildren<InventoryItems>();
            if (itemInSlot == null) 
            {
                SpawnItem(item, slot);
                return true;
            }
        }
        return false;
    }
    public void UseItem(InventoryCell slot)
    {
        useitemslot = slot.GetComponent<InventoryCell>();
        InventoryItems item = useitemslot.GetComponentInChildren<InventoryItems>();
        if(item != null)
        {
            TextItemselected.text = item.name;
            return;
        }
        TextItemselected.text = "Null";
        useitemslot = null;

    }
    public void SpawnItem(Items item, InventoryCell slot)
    {
        
        GameObject newItem = Instantiate(itemPrefap, slot.transform);
        InventoryItems inventoryItem = newItem.GetComponent<InventoryItems>();
        inventoryItem.IntialiseItem(item);
    }
}
 
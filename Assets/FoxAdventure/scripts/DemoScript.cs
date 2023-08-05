using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Items[] items;
    public void pickUpitem(int id)
    {
        bool result = inventoryManager.AddItem(items[id]);
        if (result)
        {
            Debug.Log("item added");
        }
        else
            Debug.Log("inventory full");
    }   
}

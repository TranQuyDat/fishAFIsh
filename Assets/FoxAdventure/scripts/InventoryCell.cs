using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InventoryCell : MonoBehaviour, IDropHandler
{
    public InventoryCell inventoryCell;
    GameObject inventoryManagerOJ;
    InventoryManager inventoryManager;
    private void Awake()
    {
        inventoryManagerOJ = GameObject.Find("InventoryManager");
        inventoryManager = inventoryManagerOJ.GetComponent<InventoryManager>();
    }
    public void btnClickCell( )
    {
        
        inventoryManager.UseItem(inventoryCell);
    }
    public void OnDrop(PointerEventData eventData)
    {
        
        if (transform.childCount == 0)
        {
            InventoryItems inventoryItem = eventData.pointerDrag.GetComponent<InventoryItems>();
            if(inventoryItem != null) 
            { 
                inventoryItem.parentAfterDrag = transform;  
            }
        }
    }

}

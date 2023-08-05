using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryItems : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    [Header("UI")]
    public Image image;
    public Text CountText;

    [HideInInspector] public int Count = 1;
    [HideInInspector] public Items item;
    [HideInInspector] public Transform parentAfterDrag;

    GameObject inventoryManagerOJ;
    InventoryManager inventoryManager;
    private void Awake()
    {
        inventoryManagerOJ = GameObject.Find("InventoryManager");
        inventoryManager = inventoryManagerOJ.GetComponent<InventoryManager>();
    }
    public void IntialiseItem(Items newItem)
    {
        
        item = newItem;
        image.sprite = newItem.Image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        CountText.text = Count.ToString();
        bool CountActive = Count > 1;
        CountText.gameObject.SetActive(CountActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryItems dragging = eventData.pointerDrag.GetComponent<InventoryItems>();
        inventoryManager.TextItemselected.text = dragging.name;
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }
}
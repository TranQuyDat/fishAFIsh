using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName ="Scriptable object/Item")]
public class Items : ScriptableObject
{
    [Header("Only game play")]
    public TileBase tile;
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);
    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite Image;
    public enum ItemType
    {
        food,
        tool
    }
    public enum ActionType
    {
        dig,
        eat
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DataFish", menuName = "Data/DataFish")]
public class DataFish : ScriptableObject
{
    public string name;
    public EnemyType type;
    public float speed;
    public int lv;
    public Sprite sprite;
    public RuntimeAnimatorController ani;

}

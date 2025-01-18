using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DataFish", menuName = "Data/DataFish")]
public class DataFish : ScriptableObject
{
    public string name;
    public EnemyType type;
    public Vector2 scale;
    public float speed;
    public Sprite sprite;
    public RuntimeAnimatorController ani;
    public float expReward;

}

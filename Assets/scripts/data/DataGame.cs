using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DataGame", menuName = "Data/DataGame")]
public class DataGame : ScriptableObject
{
    public MapData[] allMap;
    public float musicBGVolume;
    public float soundFXVolume;
}
[Serializable]
public class MapData
{
    public SceneType sceneType;
    public Sprite spriteFish;
}

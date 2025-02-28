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

    public void save()
    {
        PlayerPrefs.SetFloat(KeySave.bgm.ToString(), musicBGVolume);

        PlayerPrefs.SetFloat(KeySave.sfx.ToString(), soundFXVolume);
    }
    public void load()
    {
        soundFXVolume = PlayerPrefs.GetFloat(KeySave.sfx.ToString());
        musicBGVolume = PlayerPrefs.GetFloat(KeySave.bgm.ToString());
    }
}
[Serializable]
public class MapData
{
    public SceneType sceneType;
    public Sprite spriteFish;
}

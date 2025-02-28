using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType { swim, flee, chase, eat , jump }
public enum EnemyType
{
    whale, whalekiller, nemo, anglefish, shark, blueFinTuna,redTilaPia,tetra,
}
public enum EnemyState
{
    NormalEnemy, DangerousEnemy,
}
public enum PanelType { pause , play , win , lose}
public enum LevelType
{
    child = 0 , young = 1 , old = 2,
}
public enum SceneType {map1,map2,map3}
public enum ButtonTyle { none = 0, resume = 1 , nextMap = 2 , pause =3
        , restart = 4 , quit = 5 , selectMap = 6 , openSelectMap = 7 , arowLeft = 8 , arowRight = 9 ,  }

public enum KeySave
{
    bgm,sfx
}
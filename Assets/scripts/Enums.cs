using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType { swim, flee, chase, eat , jump }
public enum EnemyType
{
    whale, whalekiller, nemo, anglefish, shark,
}
public enum PanelTyle { pause , play , win , lose}
public enum LevelType
{
    child = 1 , young = 2 , old = 3,
}

public enum ButtonTyle { none = 0, resume = 1 , nextMap = 2 , pause =3
        , restart = 4 , quit = 5 , }
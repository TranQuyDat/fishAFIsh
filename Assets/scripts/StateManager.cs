using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public enum StateTyle
{
    idle , swim , eat , jump , 
}
public class StateManager : MonoBehaviour
{
    public static StateTyle stateTyle;
    public StateTyle displayState;
    public static bool isLoop;

    static IState curState;
    public float speed;

    // Update is called once per frame
    void Update()
    {

        if (curState == null  ) return;
        displayState = stateTyle;
        curState.Execute();

    }
    
    public static void changeState(IState newState , bool isloop = true)
    {
        if (curState != null) curState.Exit();
        curState = newState;
        curState.Enter();
        isLoop = isloop;
    }



    public void stopState()
    {
        curState.Exit();
        curState = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StateTyle
{
    idle , swim , eat
}
public class StateManager : MonoBehaviour
{
    public float speed;
    IState curState;
    public static StateTyle stateTyle;
    bool isChangeState = false;

    // Update is called once per frame
    void Update()
    {

        if (curState == null || isChangeState == false) return;
        isChangeState = false;
       
        curState.Execute();

    }

    public void changeState(IState newState)
    {
        if (curState != null) curState.Exit();
        curState = newState;
        curState.Enter();
        isChangeState = true;
    }

    public void RepeatState()
    {
        curState.Execute();
    }
    public void stopState()
    {
        curState.Exit();
        curState = null;
    }
}

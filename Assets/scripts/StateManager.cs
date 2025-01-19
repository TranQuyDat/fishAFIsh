using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class StateManager
{
    private IState curState;
    public StateManager( )
    {

    }
    public void excute()
    { 
        if (curState != null)
            curState.Execute();
    }
    public  void changeState(IState newState )
    {
        if ( curState != null) curState.Exit();
        curState = newState;
        curState.Enter();
    }



    public void stopState()
    {
        curState.Exit();
        curState = null;
    }
}

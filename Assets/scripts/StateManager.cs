using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StateManager : MonoBehaviour
{
    static IState actionPlayerState;
    static IState uiState;

    // Update is called once per frame
    void Update()
    {

        if (actionPlayerState != null)
            actionPlayerState.Execute();
        if(uiState !=null)
            uiState.Execute();
    }
    
    public static void changeState(IState newState)
    {
        if (actionPlayerState != null) actionPlayerState.Exit();
        actionPlayerState = newState;
        actionPlayerState.Enter();
    }



    public void stopState()
    {
        actionPlayerState.Exit();
        actionPlayerState = null;
    }
}

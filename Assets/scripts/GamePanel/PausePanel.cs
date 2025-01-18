using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : IState
{
    public void Enter()
    {
        GameManager.instance.uiGame.panelTyle = PanelTyle.pause;
        GameManager.instance.uiGame.ui_PausePanel.SetActive(true);
    }

    public void Execute()
    {
        //dk chuyen sang play
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
        }
    }

    public void Exit()
    {
        GameManager.instance.uiGame.ui_PausePanel.SetActive(false);
    }
}

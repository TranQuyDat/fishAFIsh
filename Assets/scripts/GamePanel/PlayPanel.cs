using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPanel : IState
{
    public void Enter()
    {
        GameManager.instance.uiGame.panelTyle = PanelTyle.play;
        GameManager.instance.uiGame.ui_PlayPanel.SetActive(true);
    }

    public void Execute()
    {
        //dk chuyen sang pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
        //dk chuyen sang win
        if (GameManager.instance.statGame.isWin)
        {

        }
        //dk chuyen sang lose
        if (GameManager.instance.statGame.isLose)
        {

        }
    }

    public void Exit()
    {

        GameManager.instance.uiGame.ui_PlayPanel.SetActive(false);
    }
}

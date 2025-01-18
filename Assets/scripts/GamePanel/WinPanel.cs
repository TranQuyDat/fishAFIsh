using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : IState
{
    public void Enter()
    {
        GameManager.instance.uiGame.panelTyle = PanelTyle.win;
        GameManager.instance.uiGame.ui_WinPanel.SetActive(true);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        GameManager.instance.uiGame.ui_WinPanel.SetActive(false);
    }
}

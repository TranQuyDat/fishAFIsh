using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePanel : IState
{
    public void Enter()
    {
        GameManager.instance.uiGame.panelTyle = PanelTyle.lose;
        GameManager.instance.uiGame.ui_LosePanel.SetActive(true);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        GameManager.instance.uiGame.ui_LosePanel.SetActive(false);
    }
}

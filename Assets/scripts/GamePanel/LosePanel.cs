using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePanel : IState
{
    public LosePanel() { }
    public void Enter()
    {
        GameManager.instance.uiGame.panelTyle = PanelTyle.lose;
        GameManager.instance.uiGame.ui_LosePanel.SetActive(true);
    }

    public void Execute()
    {
        //dk restart game
        if(GameManager.instance.getBtnClked() == ButtonTyle.restart)
        {
            Debug.Log("restart game");
        }
        //dk quit to menu
        if (GameManager.instance.getBtnClked() == ButtonTyle.quit)
        {
            GameManager.instance.onClick(0);
            Debug.Log("quit to menu");
            // change sence
        }
    }

    public void Exit()
    {
        GameManager.instance.uiGame.ui_LosePanel.SetActive(false);
    }
}

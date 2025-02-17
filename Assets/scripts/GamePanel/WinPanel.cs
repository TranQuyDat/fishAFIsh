using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : IState
{
    public WinPanel() { }
    public void Enter()
    {
        GameManager.instance.uiGame.panelTyle = PanelTyle.win;
        GameManager.instance.uiGame.ui_WinPanel.SetActive(true);
    }

    public void Execute()
    {
        //dk next map
        if(GameManager.instance.getBtnClked() == ButtonTyle.nextMap)
        {
            GameManager.instance.onClick(0);
            Debug.Log("next map");
            //change scene
        }
        //dk restart game
        if (GameManager.instance.getBtnClked() == ButtonTyle.restart)
        {
            GameManager.instance.onClick(0);
            GameManager.instance.btn_Restart();
        }
        //dk quit to menu
        if (GameManager.instance.getBtnClked() == ButtonTyle.quit)
        {
            GameManager.instance.onClick(0);
            GameManager.instance.btn_Quit();
        }
    }

    public void Exit()
    {
        GameManager.instance.uiGame.ui_WinPanel.SetActive(false);
    }
}

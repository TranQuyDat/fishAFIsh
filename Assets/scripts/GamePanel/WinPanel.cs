using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : IState
{
    public Uigame uiGame;
    public WinPanel() 
    {
        this.uiGame = GameManager.instance.uiGame;
    }
    public void Enter()
    {
        GameManager.instance.uiGame.panelType = PanelType.win;
        GameManager.instance.uiGame.ui_WinPanel.SetActive(true);
    }

    public void Execute()
    {
        //dk next map
        if(GameManager.instance.getBtnClked() == ButtonTyle.nextMap)
        {
            GameManager.instance.onClick(0);
            GameManager.instance.changScene(GameManager.instance.setting.nextScene);
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

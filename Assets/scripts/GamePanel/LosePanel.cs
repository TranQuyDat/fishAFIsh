using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : IState
{
    public Uigame uiGame;
    public LosePanel() 
    {
        this.uiGame = GameManager.instance.uiGame; ;
    }
    public void Enter()
    {
        uiGame.panelType = PanelType.lose;
        uiGame.ui_LosePanel.SetActive(true);
    }

    public void Execute()
    {
        //dk restart game
        if(GameManager.instance.getBtnClked() == ButtonTyle.restart)
        {
            GameManager.instance.onClick(0);
            GameManager.instance.btn_Restart();
        }
        //dk quit to menu
        if (GameManager.instance.getBtnClked() == ButtonTyle.quit)
        {
            GameManager.instance.onClick(0);;
            GameManager.instance.btn_Quit();
        }
    }

    

    public void Exit()
    {
        GameManager.instance.uiGame.ui_LosePanel.SetActive(false);
    }
}

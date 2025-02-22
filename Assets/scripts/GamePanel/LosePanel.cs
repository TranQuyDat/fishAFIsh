using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : IState
{
    public Uigame uiGame;
    GameManager gameManager;
    public LosePanel() 
    {
        gameManager = GameManager.instance;
        this.uiGame = gameManager.uiGame; ;
    }
    public void Enter()
    {
        uiGame.panelType = PanelType.lose;
        uiGame.ui_LosePanel.SetActive(true);
    }

    public void Execute()
    {
        //dk restart game
        if(gameManager.getBtnClked() == ButtonTyle.restart)
        {
            gameManager.onClick(0);
            gameManager.btn_Restart();
        }
        //dk quit to menu
        if (gameManager.getBtnClked() == ButtonTyle.quit)
        {
            gameManager.onClick(0);;
            gameManager.btn_Quit();
        }
    }

    

    public void Exit()
    {
        gameManager.uiGame.ui_LosePanel.SetActive(false);
    }
}

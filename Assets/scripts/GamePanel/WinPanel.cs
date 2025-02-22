using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : IState
{
    public Uigame uiGame;
    GameManager gameManager;
    public WinPanel()
    {
        this.uiGame = GameManager.instance.uiGame;
    }
    public void Enter()
    {
        gameManager = GameManager.instance;
        gameManager.uiGame.panelType = PanelType.win;
        uiGame.ui_WinPanel.SetActive(true);
        setStar();
    }

    public void Execute()
    {
        //dk next map
        if (GameManager.instance.getBtnClked() == ButtonTyle.nextMap)
        {
            gameManager.onClick(0);
            int idScene = gameManager.setting.idScene;
            gameManager.changScene(gameManager.dataGame.allMap[idScene + 1].sceneType);
            //change scene
        }
        //dk restart game
        if (gameManager.getBtnClked() == ButtonTyle.restart)
        {
            gameManager.onClick(0);
            gameManager.btn_Restart();
        }
        //dk quit to menu
        if (gameManager.getBtnClked() == ButtonTyle.quit)
        {
            gameManager.onClick(0);
            gameManager.btn_Quit();
        }
    }

    public void setStar()
    {
        int countStar=0;
        if (gameManager.playerCtrl.score < 100) countStar = 1;
        else if (gameManager.playerCtrl.score >= 100) countStar = 2;
        else if (gameManager.playerCtrl.score >=150) countStar = 3;

        for(int i =0;i < gameManager.uiGame.stars.Length ;i++)
        {
            gameManager.uiGame.stars[i].sprite = (i+1<=countStar)
                ? gameManager.uiGame.starOn
                : gameManager.uiGame.starOff;
        }
    }

    public void Exit()
    {
        gameManager.uiGame.ui_WinPanel.SetActive(false);
    }
}

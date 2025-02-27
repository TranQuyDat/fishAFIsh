using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPanel : IState
{
    public Uigame uiGame;
    GameManager gameManager;
    float curscore = 0;
    public PlayPanel() 
    {
        gameManager = GameManager.instance;
        this.uiGame = gameManager.uiGame;
    }
    public void Enter()
    {
        uiGame.panelType = PanelType.play;
        uiGame.ui_PlayPanel.SetActive(true);
    }

    public void Execute()
    {
        updateScore();
        //dk chuyen sang pause
        if (Input.GetKeyDown(KeyCode.Escape) || gameManager.getBtnClked() == ButtonTyle.pause )
        {
            gameManager.stateManager.changeState(new PausePanel());
        }
        //dk chuyen sang win
        if (gameManager.statGame.isWin)
        {
            gameManager.stateManager.changeState(new WinPanel() );
        }
        //dk chuyen sang lose
        if (gameManager.statGame.isLose)
        {
            gameManager.stateManager.changeState(new LosePanel() );
        }
    }

    public void updateScore()
    {
        if (curscore == gameManager.playerCtrl.score) return;
        uiGame.txt_score.text = ""+ gameManager.playerCtrl.score;
        curscore = gameManager.playerCtrl.score;
    }

    public void Exit()
    {
        gameManager.onClick(0);
        uiGame.ui_PlayPanel.SetActive(false);
    }
}

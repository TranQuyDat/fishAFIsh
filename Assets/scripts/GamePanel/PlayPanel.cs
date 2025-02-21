using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPanel : IState
{
    public Uigame uiGame;
    public PlayPanel() 
    {
        this.uiGame = GameManager.instance.uiGame;
    }
    public void Enter()
    {
        uiGame.panelType = PanelType.play;
        uiGame.ui_PlayPanel.SetActive(true);
    }

    public void Execute()
    {
        //dk chuyen sang pause
        if (Input.GetKeyDown(KeyCode.Escape) || GameManager.instance.getBtnClked() == ButtonTyle.pause )
        {
            GameManager.instance.stateManager.changeState(new PausePanel());
        }
        //dk chuyen sang win
        if (GameManager.instance.statGame.isWin)
        {
            GameManager.instance.stateManager.changeState(new WinPanel() );
        }
        //dk chuyen sang lose
        if (GameManager.instance.statGame.isLose)
        {
            GameManager.instance.stateManager.changeState(new LosePanel() );
        }
    }

    public void Exit()
    {
        GameManager.instance.onClick(0);
        uiGame.ui_PlayPanel.SetActive(false);
    }
}

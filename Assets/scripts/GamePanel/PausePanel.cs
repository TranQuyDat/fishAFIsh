using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : IState
{
    public PausePanel() { }
    public void Enter()
    {
        GameManager.instance.uiGame.panelTyle = PanelTyle.pause;
        GameManager.instance.uiGame.ui_PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Execute()
    {
        //dk chuyen sang play
        if (Input.GetKeyDown(KeyCode.Escape) || GameManager.instance.getBtnClked() == ButtonTyle.resume)
        {
            GameManager.instance.stateManager.changeState(new PlayPanel());
        }

        // dk quit to menu
        if(GameManager.instance.getBtnClked() == ButtonTyle.quit)
        {
            GameManager.instance.onClick(0);
            Application.Quit();
            // change sence
        }
    }

    public void Exit()
    {
        GameManager.instance.onClick(0);
        GameManager.instance.uiGame.ui_PausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}

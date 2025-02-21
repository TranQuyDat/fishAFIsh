using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : IState
{
    public Uigame uiGame;
    Vector3 oldPosSelectMap;
    bool isOpentSelectMap = false;
    public PausePanel() 
    {
        this.uiGame = GameManager.instance.uiGame; ;
        this.oldPosSelectMap = uiGame.ui_selectMap.anchoredPosition;
    }
    public void Enter()
    {
        uiGame.panelType = PanelType.pause;
        uiGame.ui_PausePanel.SetActive(true);
        GameManager.instance.statGame.isStart = false;
        GameManager.instance.playerCtrl.rb.gravityScale = 0f;
        GameManager.instance.playerCtrl.rb.velocity = Vector2.zero;
    }

    public void Execute()
    {
        //btn openSelectMap
        if(GameManager.instance.getBtnClked() == ButtonTyle.openSelectMap)
        {
            Vector3 pos = new Vector3(oldPosSelectMap.x * -1, oldPosSelectMap.y,oldPosSelectMap.z);
            
            uiGame.ui_selectMap.anchoredPosition =
                Vector3.MoveTowards(uiGame.ui_selectMap.anchoredPosition, pos, 1000 * Time.deltaTime);

            if(uiGame.ui_selectMap.anchoredPosition.x == -oldPosSelectMap.x)
            {
                GameManager.instance.onClick(0);
                oldPosSelectMap.x = -oldPosSelectMap.x;

                isOpentSelectMap = (!isOpentSelectMap) 
                    ? true 
                    : false;

                uiGame.btn_OpentSelectMap.transform.localScale = (isOpentSelectMap)
                    ? new Vector3(1,1,1)
                    : new Vector3(-1, 1, 1);
                
            }
        }
        //btn select Map
        if (GameManager.instance.getBtnClked() == ButtonTyle.selectMap)
        {

        }
        //btn arowlefp
        if (GameManager.instance.getBtnClked() == ButtonTyle.arowLeft)
        {

        }
        //btn arowRight
        if (GameManager.instance.getBtnClked() == ButtonTyle.arowRight)
        {

        }
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
        }



    }

    public void Exit()
    {
        GameManager.instance.onClick(0);
        GameManager.instance.statGame.isStart = true;
        uiGame.ui_selectMap.anchoredPosition = new Vector2(Mathf.Abs(oldPosSelectMap.x)
            ,oldPosSelectMap.y);
        uiGame.btn_OpentSelectMap.transform.localScale = new Vector3(-1, 1, 1);
        uiGame.ui_PausePanel.SetActive(false);

        GameManager.instance.playerCtrl.rb.gravityScale = 1f;
    }
}

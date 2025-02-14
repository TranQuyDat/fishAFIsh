using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            GameManager.instance.onClick(0);
            GameManager.instance.StartCoroutine( restart());
        }
        //dk quit to menu
        if (GameManager.instance.getBtnClked() == ButtonTyle.quit)
        {
            GameManager.instance.onClick(0);
            Debug.Log("quit to menu");
            // change sence
        }
    }

    IEnumerator restart() 
    {
        // Unload assets không sử dụng từ scene cũ
        yield return Resources.UnloadUnusedAssets();

        yield return null;

        // restart
        string sceneName = SceneManager.GetActiveScene().name;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while ( asyncLoad.progress<=0.9f)
        {
            yield return null;
        }
    }

    public void Exit()
    {
        GameManager.instance.uiGame.ui_LosePanel.SetActive(false);
    }
}

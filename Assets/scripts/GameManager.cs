using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public struct SettingScene
{
    public float mapScale;
}
[Serializable]
public class Uigame
{
    public PanelTyle panelTyle;
    [Header("Play")]
    public GameObject ui_PlayPanel;
    public Slider slider_Evolution;
    public Image img_Avt;
    [Header("Pause")]
    public GameObject ui_PausePanel;
    public Slider slider_Music;
    public Slider slider_Sound;
    [Header("Lose")]
    public GameObject ui_LosePanel;
    [Header("Win")]
    public GameObject ui_WinPanel;
}
[Serializable]
public class StatGame
{
    public bool isWin;
    public bool isLose;
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public StateManager stateManager;
    public EnemyManager enemyManager;
    public SettingScene setting;
    public Uigame uiGame;
    public StatGame statGame;
    public ButtonTyle displayBtnClked = ButtonTyle.none;
    private ButtonTyle btnClicked = ButtonTyle.none;
    private void Awake()
    {
        if(instance == null) instance = this;
        if(instance !=null && instance != this)
        {
            Destroy(this);
        }
        Application.targetFrameRate = 60;

    }

    private void Start()
    {
        stateManager = new StateManager();
        stateManager.changeState(new PlayPanel());
    }
    private void Update()
    {
        stateManager.excute();
    }
    public void onClick(int id)
    {
        btnClicked = (ButtonTyle)id;
        displayBtnClked = btnClicked;
    }

    public ButtonTyle getBtnClked()
    {
        return btnClicked;
    }
    
    public void btn_Quit()
    {
        Application.Quit();
    }
    public void btn_Restart()
    {
        StartCoroutine(restart());
    }

    IEnumerator restart()
    {
        // Unload assets không sử dụng từ scene cũ
        yield return Resources.UnloadUnusedAssets();

        yield return null;

        // restart
        string sceneName = SceneManager.GetActiveScene().name;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (asyncLoad.progress <= 0.9f)
        {
            yield return null;
        }

    }
}

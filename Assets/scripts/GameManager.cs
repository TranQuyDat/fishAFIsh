using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class SettingScene
{
    public float mapScale = 1;
    public int idScene ;
}
[Serializable]
public class Uigame
{
    public PanelType panelType;
    [Header("Play")]
    public GameObject ui_PlayPanel;
    public Slider slider_Evolution;
    public Image img_Avt;
    public TextMeshProUGUI txt_score;
    [Header("Pause")]
    public GameObject ui_PausePanel;
    public Slider slider_Music;
    public Slider slider_Sound;
    public RectTransform ui_selectMap;
    public GameObject btn_OpentSelectMap;
    public TextMeshProUGUI txt_selectMap;
    public Image img_fishSelectMap;
    [Header("Lose")]
    public GameObject ui_LosePanel;
    [Header("Win")]
    public GameObject ui_WinPanel;
    public Image[] stars;
    public Sprite starOn;
    public Sprite starOff;
}
[Serializable]
public class StatGame
{
    public bool isStart;
    public bool isWin;
    public bool isLose;
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public StateManager stateManager;
    public EnemyManager enemyManager;
    public PlayerController playerCtrl;

    public SettingScene setting;
    public Uigame uiGame;
    public StatGame statGame;

    public DataGame dataGame;
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
        stateManager.changeState(new PausePanel());
        SoundManager.Instance.PlayBGM(BGMType.UpSurFaceWater);
    }
    private void Update()
    {
        stateManager.excute();
    }
    public void onClick(int id)
    {
        btnClicked = (ButtonTyle)id;
        displayBtnClked = btnClicked;
        if(id ==0) return;
        SoundManager.Instance.PlaySFX(SFXType.Click);
    }

    public ButtonTyle getBtnClked()
    {
        return btnClicked;
    }
    
    public void changScene( SceneType scene)
    {
        SceneManager.LoadScene(scene.ToString());
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

using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerStart : MonoBehaviour
{
    public UIPanel uiObj;
    public string GameSceneName = "GameScene";

    private GButton btnContinueGame;
    private GButton btnNewGame;
    private GButton btnExitGame;
    /// <summary>
    /// 是否存在存档
    /// </summary>
    private bool hasSave;

    // Start is called before the first frame update
    void Start()
    {
        GComponent senceRoot = uiObj.ui;
        btnContinueGame = senceRoot.GetChild("btn_continue_game").asButton;
        btnNewGame = senceRoot.GetChild("btn_new_game").asButton;
        btnExitGame = senceRoot.GetChild("btn_exit").asButton;

        hasSave = SaveManager.ExistSaveFile;

        //没有存档只能开新档
        if (!hasSave)
        {
            btnContinueGame.visible = false;
        }


        btnContinueGame.onClick.Add(ContinueGame);
        btnNewGame.onClick.Add(NewGame);
        btnExitGame.onClick.Add(ExitGame);
    }

    private void ContinueGame()
    {
        if (hasSave)
        {

            SceneManager.LoadScene(GameSceneName);

        }

    }
    private void NewGame()
    {
        if (hasSave)
            SaveManager.DelSaveFile();


        SceneManager.LoadScene(GameSceneName);

    }
    //退出
    private void ExitGame()
    {
        Application.Quit();
    }
}

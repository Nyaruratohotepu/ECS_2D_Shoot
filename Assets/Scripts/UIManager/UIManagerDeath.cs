using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerDeath : MonoBehaviour
{
    public UIPanel UI;
    public AudioSource Audio;
    public AudioClip BtnSound;
    private GButton BtnBackToGame;
    public string GameSceneName = "GameScene";
    // Start is called before the first frame update
    void Start()
    {
        BtnBackToGame = UI.ui.GetChild("btn_reset").asButton;
        BtnBackToGame.onClick.Add(ResetGame);
    }

    private void ResetGame()
    {
        Audio.PlayOneShot(BtnSound, 1f);
        //自动读取上个存档
        SceneManager.LoadScene(GameSceneName);
    }

}

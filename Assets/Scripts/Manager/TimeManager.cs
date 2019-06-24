using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIManager), typeof(UIManagerCity), typeof(ItemManager))]
public class TimeManager : MonoBehaviour
{
    [Tooltip("玩家出生点")]
    public static Vector3 PlayerInitPosition = new Vector3(-6, 19, 0);
    public GameObject Player;
    /// <summary>
    /// 游戏内一天对应的现实时长
    /// </summary>
    public static TimeSpan TimePerDay = new TimeSpan(0, 5, 0);
    public TimeSpan timeLeft;
    private UIManager uiGame;
    private UIManagerCity uiCity;
    private ItemManager itemManager;
    private PlayerPackage playerPackage;
    //生存日期
    public int dayCount;

    [Tooltip("显示UI的Stage Camera")]
    public GameUIChange uiChanger;
    private bool isTiming = false;

    private SaveManager save;
    public string StartSceneName = "StartScene";
    // Start is called before the first frame update
    void Start()
    {
        save = SaveManager.GetOrCreatInstance();


        //存档恢复
        uiGame = gameObject.GetComponent<UIManager>();
        uiCity = gameObject.GetComponent<UIManagerCity>();
        itemManager = GetComponent<ItemManager>();
        playerPackage = Player.GetComponent<PlayerPackage>();

        uiGame.pauseBtn.onClick.Add(PauseTime);
        uiGame.continueBtn.onClick.Add(StartTime);
        uiGame.exitBtn.onClick.Add(SaveExit);
        uiCity.StartGameBtn.onClick.Add(StartDay);



        save.InitFromSave(this);
        StartTime();
    }

    // Update is called once per frame
    void Update()
    {
        uiGame.DayTimeLeft = timeLeft;
        if (isTiming)
        {
            //此帧消耗多少毫秒
            int milliS = Mathf.RoundToInt(Time.deltaTime * 1000);
            timeLeft -= new TimeSpan(0, 0, 0, 0, milliS);
            if (timeLeft <= TimeSpan.Zero)
            {
                //回合结束
                EndDay();
            }
        }
    }

    /// <summary>
    /// 暂停计时
    /// </summary>
    private void PauseTime()
    {
        isTiming = false;
        Time.timeScale = 0;
    }

    /// <summary>
    /// 开始计时
    /// </summary>
    private void StartTime()
    {
        isTiming = true;
        Time.timeScale = 1;
        uiGame.SetDayCount(dayCount);
    }

    /// <summary>
    /// 计时器复位到最大
    /// </summary>
    private void ResetTime()
    {
        timeLeft = TimePerDay;
    }
    /// <summary>
    /// 一天结束
    /// </summary>
    private void EndDay()
    {
        uiChanger.ChangeTo(UISence.City);
        PauseTime();
        Player.transform.position = PlayerInitPosition;
        //通知itemManager本日结束
        itemManager.OnEnterHome();
    }
    /// <summary>
    /// 开始新一天
    /// </summary>
    private void StartDay()
    {
        ++dayCount;
        ResetTime();
        StartTime();
        //自动存档
        save.SaveInfo(this);
    }

    /// <summary>
    /// 存盘并退出
    /// </summary>
    public void SaveExit()
    {
        save.SaveInfo(itemManager, playerPackage, this);
        save.SaveToDisk();

        SceneManager.LoadScene(StartSceneName);

    }
}

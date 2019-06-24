using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System;

public class UIManager : MonoBehaviour
{
    public GameObject FairyGUIObj;
    public GameObject Player;
    public UIPanel UIObj;

    private PlayerHP playerHP;
    private PlayerPackage playerAmmo;
    private AimMouseMove playerGun;

    /// <summary>
    /// sence_game根组件
    /// </summary>
    public GComponent SenceGameRoot { get; private set; }
    /// <summary>
    /// 计时器
    /// </summary>
    public TimeSpan DayTimeLeft
    {
        get
        {
            string[] timeString = timeLeftTxt.text.Split(TIMESPLITER);
            return new TimeSpan(0, int.Parse(timeString[0]), int.Parse(timeString[1]));
        }
        set
        {
            timeLeftTxt.text = value.ToString(@"mm\:ss");
        }
    }
    private GProgressBar playerHPBar;
    private GTextField ammoBarTxt;
    private GImage ammoIcon;
    private GImage ammosIcon;
    private GTextField dayCounterTxt;
    private GTextField timeLeftTxt;


    /// <summary>
    /// 暂停按钮
    /// </summary>
    public GButton pauseBtn { get; private set; }
    public GButton continueBtn { get; private set; }
    /// <summary>
    /// 退出游戏按钮
    /// </summary>
    public GButton exitBtn { get; private set; }

    private GComponent menuPause;
    // Start is called before the first frame update
    void Start()
    {


        //初始化引用UI部件
        SenceGameRoot = UIObj.ui;

        playerHPBar = SenceGameRoot.GetChild("pb_hp_player").asProgress;

        GComponent comAmmo = SenceGameRoot.GetChild("com_ammo_count").asCom;
        ammoBarTxt = comAmmo.GetChild("txt_ammo_count").asTextField;
        ammoIcon = comAmmo.GetChild("icon_ammo").asImage;
        ammosIcon = comAmmo.GetChild("icon_ammo_s").asImage;

        dayCounterTxt = SenceGameRoot.GetChild("com_day_counter").asCom.GetChild("txt_day_count").asTextField;
        timeLeftTxt = SenceGameRoot.GetChild("com_timer").asCom.GetChild("txt_timer").asTextField;
        pauseBtn = SenceGameRoot.GetChild("btn_pause").asButton;

        //初始化引用玩家数据
        playerAmmo = Player.GetComponent<PlayerPackage>();
        playerHP = Player.GetComponent<PlayerHP>();
        playerGun = Player.GetComponent<AimMouseMove>();

        pauseBtn.onClick.Add(ShowPauseMenu);



        //初始化暂停菜单
        menuPause = SenceGameRoot.GetChild("com_notice_pause").asCom;
        continueBtn = menuPause.GetChild("btn_continue").asButton;
        continueBtn.onClick.Add(HidePauseMenu);
        exitBtn = menuPause.GetChild("btn_exit").asButton;

        HidePauseMenu();
    }

    void Update()
    {
        if (Input.GetButton("Pause"))
        {
            pauseBtn.onClick.Call();
        }
    }

    void LateUpdate()
    {
        UpdateAmmo();
        UpdatePlayerHP();
    }

    /// <summary>
    /// 刷新血条
    /// </summary>
    private void UpdatePlayerHP()
    {
        playerHPBar.max = playerHP.HPMax;
        playerHPBar.value = playerHP.HPLeft;
    }
    /// <summary>
    /// 刷新子弹条
    /// </summary>
    private void UpdateAmmo()
    {

        switch (playerGun.Weapon.BulletType)
        {
            case BulletType.AssaultRifleBullet:
                ammoIcon.visible = false;
                ammosIcon.visible = true;
                ammoBarTxt.text = playerAmmo.ARAmmoCount + "/" + PlayerPackage.ARAmmoCountMax;
                return;
            case BulletType.PistolBullet:
                ammoIcon.visible = true;
                ammosIcon.visible = false;
                ammoBarTxt.text = playerAmmo.PistolAmmoCount + "/" + PlayerPackage.PistolAmmoCountMax;
                return;
        }
    }
    private void ShowPauseMenu()
    {
        //显示暂停菜单
        menuPause.visible = true;
        menuPause.height = Screen.height;
    }
    private void HidePauseMenu()
    {
        //隐藏暂停菜单
        menuPause.visible = false;
        menuPause.height = 0;
    }

    public void SetDayCount(int day)
    {
        dayCounterTxt.text = day.ToString();
    }


    private const char TIMESPLITER = ':';
}

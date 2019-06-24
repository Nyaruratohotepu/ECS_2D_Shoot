using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UIManagerCity : MonoBehaviour
{
    public UIPanel uiObj;
    public GameUIChange uiChanger;
    [Tooltip("出击动画持续时间，单位秒")]
    public float ExitTime = 1.5f;
    private float exitTimePass = 0;
    [Tooltip("出击动画播放后，多少秒后出击，单位秒")]
    public float WaitTime = 0.5f;
    private float waitTimePass = 0;
    //建筑栏
    private GProgressBar canteenBar;
    private GProgressBar storeBar;
    private GProgressBar dormBar;
    private GProgressBar factoryBar;
    //建筑栏升级按钮
    public GButton btnCanteenUp { get; private set; }
    public GButton btnGasStoreUp { get; private set; }
    public GButton btnDormUp { get; private set; }
    public GButton btnFactoryUp { get; private set; }
    //总变化栏
    private GTextField foodAccountTxt;
    private GTextField gasAccountTxt;
    private GTextField populationAccountTxt;
    private GTextField productionAccountTxt;
    //具体变化栏
    private GTextField foodLogTxt;
    private GTextField gasLogTxt;
    private GTextField populationLogTxt;
    private GTextField productionLogTxt;
    //升级详情栏
    private GComponent upgradeTooltip;
    private GTextField txtBuildingName;
    private GTextField txtDelta;
    private GTextField txtGasCost;
    private GTextField txtProductionCost;
    public GButton btnUpgradeBuilding { get; private set; }
    //出击按钮
    public GButton StartGameBtn { get; private set; }


    /// <summary>
    /// 食物显示值
    /// </summary>
    public int Food
    {
        get
        {
            return (int)canteenBar.value;
        }
        set
        {
            canteenBar.value = value;
            foodAccountTxt.SetVar("current", value.ToString()).FlushVars();
        }
    }
    /// <summary>
    /// 食物上限显示值
    /// </summary>
    public int FoodMax
    {
        get
        {
            return (int)canteenBar.max;
        }
        set
        {
            canteenBar.max = value;
        }
    }

    /// <summary>
    /// 燃料显示值
    /// </summary>
    public int Gas
    {
        get
        {
            return (int)storeBar.value;
        }
        set
        {
            storeBar.value = value;
            gasAccountTxt.SetVar("current", value.ToString()).FlushVars();
        }
    }
    /// <summary>
    /// 燃料上限显示值
    /// </summary>
    public int GasMax
    {
        get
        {
            return (int)storeBar.max;
        }
        set
        {
            storeBar.max = value;
        }
    }

    /// <summary>
    /// 人口显示值
    /// </summary>
    public int Population
    {
        get
        {
            return (int)dormBar.value;
        }
        set
        {
            dormBar.value = value;
            populationAccountTxt.SetVar("current", value.ToString()).FlushVars();
        }
    }
    /// <summary>
    /// 人口上限显示值
    /// </summary>
    public int PopulationMax
    {
        get
        {
            return (int)dormBar.max;
        }
        set
        {
            dormBar.max = value;
        }
    }

    /// <summary>
    /// 产品显示值
    /// </summary>
    public int Production
    {
        get
        {
            return (int)factoryBar.value;
        }
        set
        {
            factoryBar.value = value;
            productionAccountTxt.SetVar("current", value.ToString()).FlushVars();
        }
    }
    /// <summary>
    /// 产品上限显示值
    /// </summary>
    public int ProductionMax
    {
        get
        {
            return (int)factoryBar.max;
        }
        set
        {
            factoryBar.max = value;
        }
    }



    /// <summary>
    /// 设定某项资源的变化详情
    /// </summary>
    /// <param name="itemType">资源类型</param>
    /// <param name="logs">变化条目</param>
    /// <param name="deltaTotal">总变化量</param>
    public void UpdateLog(ItemType itemType, List<ItemDeltaLog> logs, int deltaTotal)
    {
        GTextField accountTxt = null;
        GTextField logTxt = null;

        //定位修改控件
        switch (itemType)
        {
            case ItemType.food:
                accountTxt = foodAccountTxt;
                logTxt = foodLogTxt;
                break;
            case ItemType.gas:
                accountTxt = gasAccountTxt;
                logTxt = gasLogTxt;
                break;
            case ItemType.population:
                accountTxt = populationAccountTxt;
                logTxt = populationLogTxt;
                break;
            case ItemType.production:
                accountTxt = productionAccountTxt;
                logTxt = productionLogTxt;
                break;
        }

        if (accountTxt != null)
        {
            accountTxt.SetVar("delta", ColorValue(deltaTotal)).FlushVars();
        }


        if (logTxt != null)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ItemDeltaLog log in logs)
            {
                sb.AppendFormat("{0}  {1}\n", ColorValue(log.Delta), log.Reason);
            }
            logTxt.text = sb.ToString();
        }
    }

    //结算后的参数
    private int foodTarget;
    private int gasTarget;
    private int populationTarget;
    private int productionTarget;




    // Start is called before the first frame upgrade
    void Start()
    {
        //获取组件

        // 根UI组件
        GComponent homeRoot = uiObj.ui;
        //建筑栏
        canteenBar = homeRoot.GetChild("pb_canteen").asProgress;
        storeBar = homeRoot.GetChild("pb_store").asProgress;
        dormBar = homeRoot.GetChild("pb_dorm").asProgress;
        factoryBar = homeRoot.GetChild("pb_factory").asProgress;
        //建筑栏升级按钮
        btnCanteenUp = canteenBar.GetChild("btn_upgrade").asButton;
        btnGasStoreUp = storeBar.GetChild("btn_upgrade").asButton;
        btnDormUp = dormBar.GetChild("btn_upgrade").asButton;
        btnFactoryUp = factoryBar.GetChild("btn_upgrade").asButton;


        GComponent foodCom = homeRoot.GetChild("item_food").asCom;
        GComponent gasCom = homeRoot.GetChild("item_gas").asCom;
        GComponent populationCom = homeRoot.GetChild("item_population").asCom;
        GComponent productionCom = homeRoot.GetChild("item_production").asCom;

        //总变化栏
        foodAccountTxt = foodCom.GetChild("txt_account").asTextField;
        gasAccountTxt = gasCom.GetChild("txt_account").asTextField;
        populationAccountTxt = populationCom.GetChild("txt_account").asTextField;
        productionAccountTxt = productionCom.GetChild("txt_account").asTextField;
        //具体变化栏
        foodLogTxt = foodCom.GetChild("txt_reason").asTextField;
        gasLogTxt = gasCom.GetChild("txt_reason").asTextField;
        populationLogTxt = populationCom.GetChild("txt_reason").asTextField;
        productionLogTxt = productionCom.GetChild("txt_reason").asTextField;
        //升级详情栏
        upgradeTooltip = homeRoot.GetChild("upgrade_tooltip").asCom;
        txtBuildingName = upgradeTooltip.GetChild("txt_building").asTextField;
        txtDelta = upgradeTooltip.GetChild("txt_delta").asTextField;
        txtGasCost = upgradeTooltip.GetChild("txt_gas_cost").asTextField;
        txtProductionCost = upgradeTooltip.GetChild("txt_production_cost").asTextField;
        btnUpgradeBuilding = upgradeTooltip.GetChild("btn_upgrade").asButton;
        HideTooltip();
        //下一回合出击按钮
        StartGameBtn = homeRoot.GetChild("btn_fight").asButton;
    }

    /// <summary>
    /// 升级详情栏显示信息
    /// </summary>
    /// <param name="building">建筑</param>
    /// <param name="allowUpdate">允许升级吗</param>
    /// <param name="gasEnough">燃料足够吗</param>
    /// <param name="productionEnough">产品足够吗</param>
    public void ShowTooltip(Building building, bool allowUpdate, bool gasEnough, bool productionEnough)
    {
        txtBuildingName.text = building.Name;

        txtDelta.SetVar("lv", "lv" + building.CurrentLevel).FlushVars();
        txtDelta.SetVar("capacity_current", building.Capacity.ToString()).FlushVars();
        if (building.CurrentLevel < building.MaxLevel)
        {
            txtDelta.SetVar("lv_next", ColorString("lv" + building.CurrentLevel + 1, GREEN)).FlushVars();
            txtDelta.SetVar("capatity_next", building.CapacityNextLevel.ToString()).FlushVars();
            txtDelta.SetVar("capacity_delta", ColorValue(building.CapacityNextLevel - building.Capacity)).FlushVars();
        }
        else
        {
            txtDelta.SetVar("lv_next", "lv" + building.CurrentLevel).FlushVars();
            txtDelta.SetVar("capatity_next", building.Capacity.ToString()).FlushVars();
            txtDelta.SetVar("capacity_delta", ColorString("等级已最高", RED)).FlushVars();

        }

        string gasColor = gasEnough ? GREEN : RED;
        string productionColor = productionEnough ? GREEN : RED;
        txtGasCost.text = ColorString(building.UpgradeCostCurrent.GasCost.ToString(), gasColor);
        txtProductionCost.text = ColorString(building.UpgradeCostCurrent.ProductionCost.ToString(), productionColor);


        btnUpgradeBuilding.touchable = allowUpdate;
        btnUpgradeBuilding.grayed = !allowUpdate;

        upgradeTooltip.visible = true;

    }
    private void HideTooltip()
    {
        upgradeTooltip.visible = false;
    }


    /// <summary>
    /// 更新结算后数值，播放结算动画，结算后出击
    /// </summary>
    /// <param name="foodVal">新food值</param>
    /// <param name="gasVal">新gas值</param>
    /// <param name="populationVal">新population值</param>
    /// <param name="productionVal">新production值</param>
    public void Balance(int foodVal, int gasVal, int populationVal, int productionVal)
    {
        //禁用按钮，防止再次触摸
        StartGameBtn.touchable = false;
        StartGameBtn.grayed = true;
        foodTarget = foodVal;
        gasTarget = gasVal;
        populationTarget = populationVal;
        productionTarget = productionVal;
        //使用协程结算，结算后自动跳转到游戏
        StartCoroutine(BalanceAnim());
    }
    /// <summary>
    /// 播放结算动画，使UI的资源数值在动画时间内平滑过渡到结算后数值
    /// </summary>
    /// <returns></returns>
    private IEnumerator BalanceAnim()
    {
        int foodOld = Food;
        int gasOld = Gas;
        int populationOld = Population;
        int productionOld = Production;
        exitTimePass = 0;
        waitTimePass = 0;
        while (exitTimePass < ExitTime)
        {
            exitTimePass += Time.deltaTime;

            Food = Mathf.RoundToInt(Mathf.Lerp(foodOld, foodTarget, exitTimePass / ExitTime));
            Gas = Mathf.RoundToInt(Mathf.Lerp(gasOld, gasTarget, exitTimePass / ExitTime));
            Population = Mathf.RoundToInt(Mathf.Lerp(populationOld, populationTarget, exitTimePass / ExitTime));
            Production = Mathf.RoundToInt(Mathf.Lerp(productionOld, productionTarget, exitTimePass / ExitTime));
            yield return null;
        }
        //数值归正，展示
        Food = foodTarget;
        Gas = gasTarget;
        Population = populationTarget;
        Production = productionTarget;

        while (waitTimePass < WaitTime)
        {
            waitTimePass += Time.deltaTime;
            yield return null;
        }
        //恢复游戏
        uiChanger.ChangeTo(UISence.Game);

        StartGameBtn.touchable = true;
        StartGameBtn.grayed = false;


    }





    /// <summary>
    /// 用UBB语法包装数值，使正值为绿色，负值为红色，加上符号
    /// </summary>
    /// <param name="value">数值</param>
    /// <returns>UBB语法包装后的字符串</returns>
    private string ColorValue(int value)
    {
        string color = value < 0 ? RED : GREEN;
        char sign = value < 0 ? '-' : '+';
        return string.Format("[color={0}]{1}{2}[/color]", color, sign, value);
    }
    /// <summary>
    /// 用UBB语法包装字符串
    /// </summary>
    /// <param name="value">待包装字符串</param>
    /// <param name="color">颜色十六进制表示，例如：“#1CC747”</param>
    /// <returns></returns>
    private string ColorString(string value, string color)
    {
        return string.Format("[color={0}]{1}[/color]", color, value);
    }

    // 红色和绿色的常量
    private const string GREEN = "#1CC747";
    private const string RED = "#A30B0B";
}

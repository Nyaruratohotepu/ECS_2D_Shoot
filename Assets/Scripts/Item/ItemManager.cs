using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(UIManagerCity))]
public class ItemManager : MonoBehaviour
{
    public PlayerPackage player;

    [Tooltip("1 产品=？步枪子弹")]
    public int AmmoPerProduction = 100;
    [Tooltip("1 产品=？手枪子弹")]
    public int AmmoPistolPerProduction = 200;

    private UIManagerCity ui;
    private List<ItemDeltaLog> foodLogs;
    private List<ItemDeltaLog> gasLogs;
    private List<ItemDeltaLog> populationLogs;
    private List<ItemDeltaLog> productionLogs;
    private int foodDelta = 0;
    private int gasDelta = 0;
    private int populationDelta = 0;
    private int productionDelta = 0;

    public ItemAmount items;
    public Canteen canteen;
    public GasStore gasStore;
    public Dorm dorm;
    public Factory factory;
    public Building selectedBuilding;


    private SaveManager save;
    // Start is called before the first frame update
    void Start()
    {
        save = SaveManager.GetOrCreatInstance();

        //读取存档，生成建筑
        save.InitFromSave(this);
        //读取存档物资数量

        //UI
        foodLogs = new List<ItemDeltaLog>();
        gasLogs = new List<ItemDeltaLog>();
        populationLogs = new List<ItemDeltaLog>();
        productionLogs = new List<ItemDeltaLog>();

        ui = GetComponent<UIManagerCity>();

        //按钮事件绑定
        ui.btnCanteenUp.onClick.Add(OnUpgradeCanteenBtnClick);
        ui.btnDormUp.onClick.Add(OnUpgradeDormBtnClick);
        ui.btnFactoryUp.onClick.Add(OnUpgradeFactoryBtnClick);
        ui.btnGasStoreUp.onClick.Add(OnUpgradeGasStoreBtnClick);
        ui.btnUpgradeBuilding.onClick.Add(OnUpgradeBtnClick);

        ui.StartGameBtn.onClick.Add(OnExitHome);
    }

    /// <summary>
    /// 处理一天的物资收益
    /// </summary>
    public void OnEnterHome()
    {


        ui.GasMax = gasStore.Capacity;
        ui.FoodMax = canteen.Capacity;
        ui.PopulationMax = dorm.Capacity;
        ui.ProductionMax = factory.Capacity;


        LoadPackage();
        //计算分项变化
        LogItem();
        //总计
        AccountItem();
        ui.UpdateLog(ItemType.food, foodLogs, foodDelta);
        ui.UpdateLog(ItemType.gas, gasLogs, gasDelta);
        ui.UpdateLog(ItemType.population, populationLogs, populationDelta);
        ui.UpdateLog(ItemType.production, productionLogs, productionDelta);
        UpdateItems();
    }
    public void OnExitHome()
    {

        //清空日志
        foodLogs.Clear();
        gasLogs.Clear();
        populationLogs.Clear();
        productionLogs.Clear();



        //数值更新
        ExecuteItemDelta();
        //ui更新
        ui.Balance(items.FoodAmount, items.GasAmount, items.PopulationAmount, items.ProductionAmount);

        TryFullfillAmmo();
        //自动存档
        save.SaveInfo(this);
    }
    public void OnUpgradeCanteenBtnClick()
    {
        selectedBuilding = canteen;
        bool gasEnough;
        bool productionEnough;
        bool canUpgrade = CanUpgrade(selectedBuilding, out gasEnough, out productionEnough);

        ui.ShowTooltip(selectedBuilding, canUpgrade, gasEnough, productionEnough);
    }
    public void OnUpgradeGasStoreBtnClick()
    {
        selectedBuilding = gasStore;
        bool gasEnough;
        bool productionEnough;
        bool canUpgrade = CanUpgrade(selectedBuilding, out gasEnough, out productionEnough);

        ui.ShowTooltip(selectedBuilding, canUpgrade, gasEnough, productionEnough);
    }
    public void OnUpgradeDormBtnClick()
    {
        selectedBuilding = dorm;
        bool gasEnough;
        bool productionEnough;
        bool canUpgrade = CanUpgrade(selectedBuilding, out gasEnough, out productionEnough);

        ui.ShowTooltip(selectedBuilding, canUpgrade, gasEnough, productionEnough);
    }
    public void OnUpgradeFactoryBtnClick()
    {
        selectedBuilding = factory;
        bool gasEnough;
        bool productionEnough;
        bool canUpgrade = CanUpgrade(selectedBuilding, out gasEnough, out productionEnough);

        ui.ShowTooltip(selectedBuilding, canUpgrade, gasEnough, productionEnough);
    }
    public void OnUpgradeBtnClick()
    {
        UpgradeBuilding(selectedBuilding);
        //刷新上限
        switch (selectedBuilding.ItemTypeContain)
        {
            case ItemType.food:
                ui.FoodMax = canteen.Capacity;
                break;
            case ItemType.gas:
                ui.GasMax = gasLogs.Capacity;
                break;
            case ItemType.population:
                ui.PopulationMax = dorm.Capacity;
                break;
            case ItemType.production:
                ui.ProductionMax = factory.Capacity;
                break;
        }
        UpdateItems();

    }
    /// <summary>
    /// 刷新界面资源数值
    /// </summary>
    private void UpdateItems()
    {
        ui.Gas = items.GasAmount;
        ui.Food = items.FoodAmount;
        ui.Population = items.PopulationAmount;
        ui.Production = items.ProductionAmount;
    }



    /// <summary>
    /// 由各个Log计算总变化
    /// </summary>
    private void AccountItem()
    {
        foodDelta = 0;
        foreach (ItemDeltaLog log in foodLogs)
        {
            foodDelta += log.Delta;
        }

        gasDelta = 0;
        foreach (ItemDeltaLog log in gasLogs)
        {
            gasDelta += log.Delta;
        }

        populationDelta = 0;
        foreach (ItemDeltaLog log in populationLogs)
        {
            populationDelta += log.Delta;
        }

        productionDelta = 0;
        foreach (ItemDeltaLog log in productionLogs)
        {
            productionDelta += log.Delta;
        }
    }

    /// <summary>
    /// 执行数值变化
    /// </summary>
    private void ExecuteItemDelta()
    {
        items.FoodAmount += foodDelta;
        items.GasAmount += gasDelta;
        items.PopulationAmount += populationDelta;
        items.ProductionAmount += productionDelta;
    }


    private void LoadPackage()
    {
        //最多装货
        int foodLoadMax = canteen.Capacity - items.FoodAmount;
        int gasLoadMax = gasStore.Capacity - items.GasAmount;
        //交易数量
        int foodLoad = foodLoadMax > player.Food ? player.Food : foodLoadMax;
        int gasLoad = gasLoadMax > player.Gas ? player.Gas : gasLoadMax;
        //交易
        items.FoodAmount += foodLoad;
        items.GasAmount += gasLoad;
        player.Food -= foodLoad;
        player.Gas -= gasLoad;
    }
    private void TryFullfillAmmo()
    {
        int AmmoPistolNeed = PlayerPackage.PistolAmmoCountMax - player.PistolAmmoCount;
        int AmmoARNeed = PlayerPackage.ARAmmoCountMax - player.PistolAmmoCount;

        //先补满AR弹药
        int AmmoARAddMax = items.ProductionAmount / AmmoPerProduction;
        int AmmoARAdd = AmmoARNeed > AmmoARAddMax ? AmmoARAddMax : AmmoARNeed;

        player.ARAmmoCount += AmmoARAdd;
        items.ProductionAmount -= (AmmoARAdd * AmmoPerProduction);

        //再补满手枪子弹
        int AmmoPistolAddMax = items.ProductionAmount / AmmoPistolPerProduction;
        int AmmoPistolAdd = AmmoPistolNeed > AmmoPistolAddMax ? AmmoPistolAddMax : AmmoPistolNeed;

        player.PistolAmmoCount += AmmoPistolAdd;
        items.ProductionAmount -= (AmmoPistolAdd * AmmoPistolPerProduction);
    }

    /// <summary>
    /// 计算本回合，各项资源应该发生的数量变化
    /// </summary>
    private void LogItem()
    {
        //Population
        if (items.FoodAmount >= items.PopulationAmount)
        {
            //有余粮
            if (dorm.Capacity > items.PopulationAmount)
            {
                populationLogs.Add(new ItemDeltaLog(1, "自然增长"));
            }
            else
            {
                populationLogs.Add(new ItemDeltaLog(0, "床位不足"));
            }
        }
        else
        {
            //粮食不足
            populationLogs.Add(new ItemDeltaLog(items.FoodAmount - items.PopulationAmount, "食物不足"));
        }

        //Food
        foodLogs.Add(new ItemDeltaLog(-items.PopulationAmount, "粮食消耗"));

        //Gas
        //Production

        int productionSpace = factory.Capacity - items.ProductionAmount;
        if (productionSpace >= items.PopulationAmount)
        {
            //空间＞人员 
            if (items.PopulationAmount >= items.GasAmount)
            {
                //空间>人员>燃料  用完所有燃料  
                gasLogs.Add(new ItemDeltaLog(-items.GasAmount, "生产产品"));
                productionLogs.Add(new ItemDeltaLog(items.GasAmount, "生产（燃料短缺）"));
            }
            else
            {
                //空间>燃料>人员 燃料足够，按人头生产产品
                gasLogs.Add(new ItemDeltaLog(-items.PopulationAmount, "生产产品"));
                productionLogs.Add(new ItemDeltaLog(items.PopulationAmount, "生产产品"));
            }
        }
        else
        {
            //人员＞空间
            if (productionSpace >= items.GasAmount)
            {
                //人员＞空间＞燃料
                gasLogs.Add(new ItemDeltaLog(-items.GasAmount, "生产产品"));
                productionLogs.Add(new ItemDeltaLog(items.GasAmount, "生产（燃料短缺）"));
            }
            else
            {
                //人员＞燃料＞空间
                gasLogs.Add(new ItemDeltaLog(-productionSpace, "生产产品"));
                productionLogs.Add(new ItemDeltaLog(productionSpace, "生产（仓库爆满）"));
            }
        }

    }

    /// <summary>
    /// 某建筑是否能升级
    /// </summary>
    /// <param name="building">建筑</param>
    /// <returns>能否升级</returns>
    public bool CanUpgrade(Building building, out bool gasEnough, out bool productionEnough)
    {
        if (building == null)
        {
            gasEnough = false;
            productionEnough = false;
            return false;
        }


        gasEnough = items.GasAmount >= building.UpgradeCostCurrent.GasCost;
        productionEnough = items.ProductionAmount >= building.UpgradeCostCurrent.ProductionCost;
        bool isLvNotMax = building.CurrentLevel < building.MaxLevel;

        return gasEnough && productionEnough && isLvNotMax;
    }
    /// <summary>
    /// 升级建筑
    /// </summary>
    /// <param name="building">建筑</param>
    public void UpgradeBuilding(Building building)
    {
        bool gasEnough;
        bool productionEnough;
        if (!CanUpgrade(building, out gasEnough, out productionEnough)) return;
        items.GasAmount -= building.UpgradeCostCurrent.GasCost;
        items.ProductionAmount -= building.UpgradeCostCurrent.ProductionCost;
        building.Upgrade();

    }
    private Building ItemTypeToBuilding(ItemType type)
    {
        switch (type)
        {
            case ItemType.food:
                return canteen;
            case ItemType.gas:
                return gasStore;
            case ItemType.population:
                return dorm;
            case ItemType.production:
                return factory;
            default:
                return null;
        }
    }
}
//升级所需物资
public struct UpgradeCost
{
    public int GasCost;
    public int ProductionCost;
    public UpgradeCost(int gasCost, int productionCost)
    {
        GasCost = gasCost;
        ProductionCost = productionCost;
    }
}

[Serializable]
public struct ItemAmount
{
    /// <summary>
    /// 食物量
    /// </summary>
    public int FoodAmount;

    /// <summary>
    /// 燃料量
    /// </summary>
    public int GasAmount;

    /// <summary>
    /// 人口量
    /// </summary>
    public int PopulationAmount;

    /// <summary>
    /// 产品量
    /// </summary>
    public int ProductionAmount;
    public ItemAmount(int food, int gas, int population, int production)
    {
        FoodAmount = food;
        GasAmount = gas;
        PopulationAmount = population;
        ProductionAmount = production;
    }
}
public struct ItemDeltaLog
{
    public int Delta;
    public string Reason;
    public ItemDeltaLog(int delta, string reason)
    {
        Delta = delta;
        Reason = reason;
    }
}

public enum ItemType
{
    /// <summary>
    /// 食物，对应建筑食堂
    /// </summary>
    food,
    /// <summary>
    /// 燃料，对应建筑燃料库
    /// </summary>
    gas,
    /// <summary>
    /// 人口，对应建筑宿舍
    /// </summary>
    population,
    /// <summary>
    /// 产品，对应建筑工坊
    /// </summary>
    production
}



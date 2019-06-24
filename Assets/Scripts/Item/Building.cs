using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Building
{
    public string Name { get; private set; }
    public ItemType ItemTypeContain { get; private set; }
    /// <summary>
    /// 最高等级
    /// </summary>
    public int MaxLevel { get; private set; }
    /// <summary>
    /// 当前等级
    /// </summary>
    public int CurrentLevel { get; private set; }
    /// <summary>
    /// capacities[i]：第i级的容量
    /// </summary>
    private int[] capacities;
    /// <summary>
    /// upCosts[i]：第i级升级到第i+1级的消耗
    /// </summary>
    private UpgradeCost[] upCosts;
    /// <summary>
    /// 当前容量
    /// </summary>
    public int Capacity
    {
        get
        {
            return capacities[CurrentLevel];
        }
    }
    /// <summary>
    /// 下一级容量
    /// </summary>
    public int CapacityNextLevel
    {
        get
        {
            if (CurrentLevel < MaxLevel)
                return capacities[CurrentLevel + 1];
            else
                return capacities[MaxLevel];
        }
    }
    /// <summary>
    /// 当前升级到下一级所需材料
    /// </summary>
    public UpgradeCost UpgradeCostCurrent
    {
        get
        {
            return upCosts[CurrentLevel];
        }
    }
    /// <summary>
    /// 升级，但不会超过最大等级
    /// </summary>
    public void Upgrade()
    {
        if (CurrentLevel < MaxLevel)
            CurrentLevel++;
    }
    /// <summary>
    /// 设定某等级建筑容量和升级消耗资源数量
    /// </summary>
    /// <param name="level">等级</param>
    /// <param name="capacity">容量</param>
    /// <param name="upCost">升级消耗资源量</param>
    protected void SetLevel(int level, int capacity, UpgradeCost upCost)
    {
        if (level > MaxLevel) return;

        capacities[level] = capacity;
        upCosts[level] = upCost;
    }
    public Building(string name, ItemType itemType, int lvMax, int lvCurrent)
    {
        Name = name;
        ItemTypeContain = itemType;
        MaxLevel = lvMax;
        CurrentLevel = lvCurrent > lvMax ? lvMax : lvCurrent;
        capacities = new int[lvMax + 1];
        upCosts = new UpgradeCost[lvMax + 1];
    }
}
/// <summary>
/// 食堂
/// </summary>
public class Canteen : Building
{
    public Canteen(int lvCurrent) : base("食堂", ItemType.food, 6, lvCurrent)
    {
        SetLevel(1, 50, new UpgradeCost(30, 20));
        SetLevel(2, 100, new UpgradeCost(55, 65));
        SetLevel(3, 150, new UpgradeCost(75, 90));
        SetLevel(4, 250, new UpgradeCost(95, 120));
        SetLevel(5, 350, new UpgradeCost(115, 145));
        SetLevel(6, 550, new UpgradeCost(150, 170));
    }
}
/// <summary>
/// 燃料库
/// </summary>
public class GasStore : Building
{
    public GasStore(int lvCurrent) : base("燃料库", ItemType.gas, 6, lvCurrent)
    {
        SetLevel(1, 50, new UpgradeCost(20, 30));
        SetLevel(2, 100, new UpgradeCost(50, 70));
        SetLevel(3, 150, new UpgradeCost(70, 100));
        SetLevel(4, 300, new UpgradeCost(90, 130));
        SetLevel(5, 500, new UpgradeCost(110, 150));
        SetLevel(6, 700, new UpgradeCost(140, 180));
    }
}
/// <summary>
/// 宿舍
/// </summary>
public class Dorm : Building
{
    public Dorm(int lvCurrent) : base("宿舍", ItemType.population, 6, lvCurrent)
    {
        SetLevel(1, 25, new UpgradeCost(20, 20));
        SetLevel(2, 50, new UpgradeCost(50, 50));
        SetLevel(3, 75, new UpgradeCost(70, 70));
        SetLevel(4, 100, new UpgradeCost(90, 90));
        SetLevel(5, 125, new UpgradeCost(110, 110));
        SetLevel(6, 150, new UpgradeCost(140, 140));
    }
}
/// <summary>
/// 工厂
/// </summary>
public class Factory : Building
{
    public Factory(int lvCurrent) : base("工厂", ItemType.production, 6, lvCurrent)
    {
        SetLevel(1, 50, new UpgradeCost(20, 30));
        SetLevel(2, 100, new UpgradeCost(50, 70));
        SetLevel(3, 170, new UpgradeCost(70, 100));
        SetLevel(4, 250, new UpgradeCost(90, 130));
        SetLevel(5, 340, new UpgradeCost(110, 150));
        SetLevel(6, 500, new UpgradeCost(140, 180));
    }
}

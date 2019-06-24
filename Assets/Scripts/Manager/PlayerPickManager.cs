using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerPickManager
{
    private GunFactory gunFactory;
    private static PlayerPickManager instance;
    public static PlayerPickManager GetOrCreatInstance()
    {
        if (instance == null)
            instance = new PlayerPickManager();
        return instance;
    }

    private PlayerPickManager()
    {
        gunFactory = GunFactory.GetOrCreatInstance();
    }

    /// <summary>
    /// 治疗玩家
    /// </summary>
    /// <param name="maxHealHp">提供的治疗量</param>
    public void HealPlayer(int maxHealHp, PlayerHP player)
    {
        int hpNeed = player.HPMax - player.HPLeft;
        int addHp = maxHealHp > hpNeed ? hpNeed : maxHealHp;
        player.HPLeft += addHp;
    }
    /// <summary>
    /// 补充玩家子弹
    /// </summary>
    /// <param name="type">子弹类型</param>
    /// <param name="amount">子弹数量</param>
    public void AddAmmo(BulletType type, int amount, PlayerPackage player)
    {
        switch (type)
        {
            case BulletType.AssaultRifleBullet:
                int arSpace = PlayerPackage.ARAmmoCountMax - player.ARAmmoCount;
                int addARAmount = amount > arSpace ? arSpace : amount;
                player.ARAmmoCount += addARAmount;
                break;
            case BulletType.PistolBullet:
                int pistolSpace = PlayerPackage.PistolAmmoCountMax - player.PistolAmmoCount;
                int addPistolAmount = amount > pistolSpace ? pistolSpace : amount;
                player.PistolAmmoCount += addPistolAmount;
                break;
        }
    }
    /// <summary>
    /// 给玩家一把新枪
    /// </summary>
    /// <param name="gunType">枪类型</param>
    public void AddWeapon(GunEnum gunType, PlayerPackage player)
    {
        //防止重复添加多只枪
        foreach (Gun gun in player.Weapons)
        {
            if (gun.GunType == gunType) return;
        }
        player.Weapons.Add(gunFactory.CreatGun(gunType));
    }

    /// <summary>
    /// 向玩家背包加入某项物资，只能是food或gas
    /// </summary>
    /// <param name="item">物资</param>
    /// <param name="amount">数量</param>
    public void AddItem(ItemType item, int amount, PlayerPackage player)
    {
        switch (item)
        {
            case ItemType.food:
                int foodSpace = player.FoodMax - player.Food;
                int addFoodAmount = foodSpace > amount ? amount : foodSpace;
                player.Food += addFoodAmount;
                break;
            case ItemType.gas:
                int gasSpace = player.GasMax - player.Gas;
                int addGasAmount = gasSpace > amount ? amount : gasSpace;
                player.Gas += addGasAmount;
                break;
        }
    }
}


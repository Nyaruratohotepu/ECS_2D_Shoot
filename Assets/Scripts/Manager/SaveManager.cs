using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SaveManager
{
    /// <summary>
    /// 存档文件后缀名
    /// </summary>
    public static string SaveFileName { get; private set; } = "ECSGAME.ecsave";
    public static string SaveFilePath { get; private set; } = Application.persistentDataPath;
    public static string SaveFilePathFull
    {
        get
        {
            return SaveFilePath + '/' + SaveFileName;
        }
    }
    /// <summary>
    /// 是否存在存档文件
    /// </summary>
    public static bool ExistSaveFile
    {
        get
        {
            return File.Exists(SaveFilePathFull);
        }
    }
    /// <summary>
    /// 删除存档文件
    /// </summary>
    public static void DelSaveFile()
    {
        if (ExistSaveFile)
        {
            File.Delete(SaveFilePathFull);
        }
    }

    private static SaveManager instance;
    public static SaveManager GetOrCreatInstance()
    {
        if (instance == null)
        {
            instance = new SaveManager();
        }
        return instance;
    }
    private SaveManager()
    {
        gunFactory = GunFactory.GetOrCreatInstance();
        InitFromDisk();
    }
    private Save save;
    private GunFactory gunFactory;
    /// <summary>
    /// 保存基地的物资和建筑信息
    /// </summary>
    /// <param name="itemManager"></param>
    public void SaveInfo(ItemManager itemManager)
    {
        save.ItemsBase = itemManager.items;
        save.LevelCanteen = itemManager.canteen.CurrentLevel;
        save.LevelDorm = itemManager.dorm.CurrentLevel;
        save.LevelFactory = itemManager.dorm.CurrentLevel;
        save.LevelStore = itemManager.gasStore.CurrentLevel;
    }
    /// <summary>
    /// 从存档中恢复建筑和物资
    /// </summary>
    /// <param name="itemManager"></param>
    public void InitFromSave(ItemManager itemManager)
    {
        itemManager.items = save.ItemsBase;
        itemManager.canteen = new Canteen(save.LevelCanteen);
        itemManager.dorm = new Dorm(save.LevelDorm);
        itemManager.factory = new Factory(save.LevelFactory);
        itemManager.gasStore = new GasStore(save.LevelStore);
    }
    /// <summary>
    /// 存储子弹、背包物资、武器
    /// </summary>
    /// <param name="player"></param>
    public void SaveInfo(PlayerPackage player)
    {
        save.ItemsPackage.FoodAmount = player.Food;
        save.ItemsPackage.GasAmount = player.Gas;
        save.ItemsPackage.PopulationAmount = 0;
        save.ItemsPackage.ProductionAmount = 0;

        save.AmmoAR = player.ARAmmoCount;
        save.AmmoPistol = player.PistolAmmoCount;

        save.Guns.Clear();
        foreach (Gun gun in player.Weapons)
        {
            save.Guns.Add(gun.GunType);
        }
        save.PlayerPositionX = player.transform.position.x;
        save.PlayerPositionY = player.transform.position.y;
    }
    /// <summary>
    /// 从存档恢复子弹、背包物资、武器
    /// </summary>
    /// <param name="player"></param>
    public void InitFromSave(PlayerPackage player)
    {
        player.Food = save.ItemsPackage.FoodAmount;
        player.Gas = save.ItemsPackage.GasAmount;
        player.ARAmmoCount = save.AmmoAR;
        player.PistolAmmoCount = save.AmmoPistol;

        player.Weapons.Clear();
        foreach (GunEnum gunType in save.Guns)
        {
            player.Weapons.Add(gunFactory.CreatGun(gunType));
        }
        player.gameObject.transform.position = new Vector3(save.PlayerPositionX, save.PlayerPositionY, 0);
    }
    /// <summary>
    /// 存储时间信息
    /// </summary>
    /// <param name="time"></param>
    public void SaveInfo(TimeManager time)
    {
        save.TimeLeft = time.timeLeft;
        save.DayCount = time.dayCount;
    }
    /// <summary>
    /// 从存档恢复时间信息
    /// </summary>
    /// <param name="time"></param>
    public void InitFromSave(TimeManager time)
    {
        time.timeLeft = save.TimeLeft;
        time.dayCount = save.DayCount;
    }
    public void SaveToDisk()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(SaveFilePathFull);
        bf.Serialize(file, save);
        file.Close();
        MonoBehaviour.print("save to" + Application.persistentDataPath);
    }
    public void InitFromDisk()
    {

        if (ExistSaveFile)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(SaveFilePathFull, FileMode.Open);
            save = (Save)bf.Deserialize(file);
            file.Close();
            MonoBehaviour.print("load from" + Application.persistentDataPath);
        }
        else
        {
            save = new Save();
        }

    }
    /// <summary>
    /// 存储所有信息
    /// </summary>
    /// <param name="item"></param>
    /// <param name="player"></param>
    /// <param name="time"></param>
    public void SaveInfo(ItemManager item, PlayerPackage player, TimeManager time)
    {
        SaveInfo(item);
        SaveInfo(player);
        SaveInfo(time);
    }
}

[Serializable]
public class Save
{
    //基地相关
    public ItemAmount ItemsBase;

    public int LevelCanteen;
    public int LevelStore;
    public int LevelDorm;
    public int LevelFactory;

    //时间相关
    public TimeSpan TimeLeft;
    public int DayCount;

    //玩家相关
    public int AmmoPistol;
    public int AmmoAR;
    public ItemAmount ItemsPackage;
    public List<GunEnum> Guns;
    public float PlayerPositionX;
    public float PlayerPositionY;

    /// <summary>
    /// 新存档
    /// </summary>
    public Save()
    {
        ItemsBase = new ItemAmount(0, 0, 0, 0);
        LevelCanteen = LevelDorm = LevelFactory = LevelStore = 1;
        TimeLeft = TimeManager.TimePerDay;
        DayCount = 1;
        AmmoPistol = PlayerPackage.PistolAmmoCountMax;
        AmmoAR = PlayerPackage.ARAmmoCountMax;
        ItemsPackage = new ItemAmount(0, 0, 0, 0);
        Guns = new List<GunEnum>();
        Guns.Add(GunEnum.GunM4);
        PlayerPositionX = TimeManager.PlayerInitPosition.x;
        PlayerPositionY = TimeManager.PlayerInitPosition.y;
    }
}


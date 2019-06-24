using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AimMouseMove))]
public class PlayerPackage : MonoBehaviour
{
    //拾取到背包的物资
    public int Gas;
    public int Food;
    //背包物资上限
    public int GasMax { get; private set; }
    public int FoodMax { get; private set; }

    //两种弹药的最大和当前数量
    public int PistolAmmoCount { get; set; }
    public static readonly int PistolAmmoCountMax = 240;
    public int ARAmmoCount { get; set; }
    public static readonly int ARAmmoCountMax = 360;

    public float WeaponChangeSensitivity = 0.05f;
    public List<Gun> Weapons;
    private AimMouseMove player;
    private int weaponIndex;
    private SaveManager save;
    // Start is called before the first frame update
    void Start()
    {
        save = SaveManager.GetOrCreatInstance();
        //从存档恢复物资
        GasMax = 100;
        FoodMax = 150;

        //从存档恢复武器
        Weapons = new List<Gun>();
        save.InitFromSave(this);
        weaponIndex = 0;
        player = gameObject.GetComponent<AimMouseMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < -WeaponChangeSensitivity)
            ChangeWeapon(true);
        else if (Input.GetAxis("Mouse ScrollWheel") > WeaponChangeSensitivity)
            ChangeWeapon(false);

    }
    /// <summary>
    /// 切枪
    /// </summary>
    /// <param name="isNext">向后切吗</param>
    private void ChangeWeapon(bool isNext)
    {
        if (isNext)
            ++weaponIndex;
        else
        {
            weaponIndex += Weapons.Count;
            --weaponIndex;
        }
        weaponIndex %= Weapons.Count;
        player.Weapon = Weapons[weaponIndex];
    }

    //尝试消耗子弹
    public bool TryConsumeAmmo()
    {
        BulletType type = player.Weapon.BulletType;
        switch (type)
        {
            case BulletType.AssaultRifleBullet:
                if (ARAmmoCount > 0)
                {
                    --ARAmmoCount;
                    return true;
                }
                else return false;
            case BulletType.PistolBullet:
                if (PistolAmmoCount > 0)
                {
                    --PistolAmmoCount;
                    return true;
                }
                else return false;
        }
        return false;
    }

    /// <summary>
    /// 加满子弹
    /// </summary>
    public void FullfillAmmo()
    {
        ARAmmoCount = ARAmmoCountMax;
        PistolAmmoCount = PistolAmmoCountMax;
    }

}

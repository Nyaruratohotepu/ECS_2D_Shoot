using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class BulletInfo
{
    public static readonly string PrefabPath = @"Prefabs\Bullets\";
    /// <summary>
    /// 子弹名称
    /// </summary>
    public string BulletName { private set; get; }
    public string PrefabFullPath
    {
        get
        {
            return PrefabPath + BulletName;
        }
    }
    /// <summary>
    /// 伤害量
    /// </summary>
    public int Damage { private set; get; }
    /// <summary>
    /// 子弹攻击物体类型
    /// </summary>
    public BulletDamageType Side { private set; get; }
    /// <summary>
    /// 飞行速度
    /// </summary>
    public float Speed { private set; get; }

    public BulletInfo(string bulletName, int damage, BulletDamageType side, float speed)
    {
        BulletName = bulletName;
        Damage = damage;
        Side = side;
        Speed = speed;
    }

    public BulletComponent CreatBulletComponent(float lifeLeft)
    {
        BulletComponent bulletComponent = new BulletComponent();
        bulletComponent.Damage = Damage;
        bulletComponent.LifeLeft = lifeLeft;
        bulletComponent.Side = Side;
        return bulletComponent;
    }

    public VelocityComponent CreatVelocityComponent(Vector2 direction)
    {
        VelocityComponent velocityComponent = new VelocityComponent();
        velocityComponent.Speed = Speed;
        velocityComponent.Velocity = direction.normalized * Speed;
        return velocityComponent;
    }
}
public enum BulletType
{
    /// <summary>
    /// 手枪、冲锋枪使用
    /// </summary>
    PistolBullet,
    /// <summary>
    /// 突击步枪使用
    /// </summary>
    AssaultRifleBullet
}

/// <summary>
/// 子弹会伤害哪些物体
/// </summary>
public enum BulletDamageType
{
    AttackEnemy,
    AttackPlayer
}

public class MeleeBulletInfo : BulletInfo
{
    public MeleeBulletInfo(int damage) : base("0", damage, BulletDamageType.AttackPlayer, 0)
    {

    }
}

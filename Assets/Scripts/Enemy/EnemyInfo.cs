using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 存储初始化固定信息
/// </summary>
public class EnemyInfo
{
    //渲染相关
    public static readonly string PrefabPath = @"Prefabs\Enemy\";
    public string PrefabName { get; private set; }
    public string PrefabFullPath
    {
        get
        {
            return PrefabPath + PrefabName;
        }
    }
    public int HPMax { get; private set; }
    //攻击相关
    public int DamagePerHit { get; private set; }
    public float AttackCD { get; private set; }
    public float DamageRange { get; private set; }
    /// <summary>
    /// 子弹速度，近战设为0
    /// </summary>
    public float BulletSpeed { get; private set; }
    /// <summary>
    /// 子弹存在最长时间，近战攻击持续时间
    /// </summary>
    public float BulletLife { get; private set; }
    /// <summary>
    /// ECS架构不允许使用string
    /// </summary>
    public int BulletSpriteName { get; private set; }
    //移动相关
    public float Speed { get; private set; }
    /// <summary>
    /// 玩家距离小于此值，开始追逐玩家
    /// </summary>
    public float HuntDistance { get; private set; }
    /// <summary>
    /// 距离目标小于此值会开始攻击
    /// </summary>
    public float AttackDistance { get; private set; }
    /// <summary>
    /// 巡逻半径
    /// </summary>
    public float PatrolRadius { get; private set; }
    public float MoveDeviation { get; private set; }
    public float JumpForce { get; private set; }
    public float JumpDistance { get; private set; }
    public EnemyInfo(string spriteName, int hpMax, int damagePerHit, float attackCD, float damageRange, float bulletSpeed, float bulletLife, int bulletSpriteName, float speed, float huntDistance, float attackDistance, float patrolRadius, float moveDeviation, float jumpForce, float jumpDistance)
    {
        PrefabName = spriteName;
        HPMax = hpMax;
        DamagePerHit = damagePerHit;
        AttackCD = attackCD;
        DamageRange = damageRange;
        BulletSpeed = bulletSpeed;
        BulletLife = bulletLife;
        BulletSpriteName = bulletSpriteName;
        Speed = speed;
        HuntDistance = huntDistance;
        AttackDistance = attackDistance;
        PatrolRadius = patrolRadius;
        MoveDeviation = moveDeviation;
        JumpForce = jumpForce;
        JumpDistance = jumpDistance;
    }

    public EnemyAttackComponent CreatAttckComponent()
    {
        EnemyAttackComponent attackCom = new EnemyAttackComponent();
        attackCom.DamagePerHit = DamagePerHit;
        attackCom.DamageRange = DamageRange;
        attackCom.AttackCD = AttackCD;
        attackCom.AttackCDLeft = AttackCD;
        attackCom.BulletSpeed = BulletSpeed;
        attackCom.BulletLife = BulletLife;
        attackCom.BulletSpriteName = BulletSpriteName;
        attackCom.AttackDistance = AttackDistance;
        return attackCom;
    }
    public EnemyPatrolComponent CreatPatrolComponent(Vector2 guardPosition)
    {
        EnemyPatrolComponent patrolCom = new EnemyPatrolComponent();
        patrolCom.IsPatrolling = 1;
        patrolCom.HuntDistance = HuntDistance;
        patrolCom.GuardPosition = guardPosition;
        patrolCom.PatrolLeftBorder = new Vector2(guardPosition.x - PatrolRadius, guardPosition.y);
        patrolCom.PatrolRightBorder = new Vector2(guardPosition.x + PatrolRadius, guardPosition.y);
        patrolCom.MoveToRightBorder = 1;
        patrolCom.Deviation = MoveDeviation;
        patrolCom.JumpDistance = JumpDistance;
        patrolCom.JumpForce = JumpForce;
        return patrolCom;
    }
    public VelocityComponent CreatVelocityComponent()
    {
        VelocityComponent velocityCom = new VelocityComponent();
        velocityCom.Speed = this.Speed;
        velocityCom.Velocity = new Vector2(Speed, 0);
        return velocityCom;
    }
}
public class EnemyPrototype
{
    //哈奇，近程怪
    public static EnemyInfo Hatch = new EnemyInfo("Hatch", 100, 10, 0.5f, 0.5f, 0, 0.2f, 0, 0.6f, 5, 1, 2f, 0.2f, 250, 1f);
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

public struct EnemyAttackComponent : IComponentData
{
    /// <summary>
    /// 单次攻击伤害
    /// </summary>
    public int DamagePerHit;
    /// <summary>
    /// 攻击间隔
    /// </summary>
    public float AttackCD;
    public float AttackCDLeft;
    /// <summary>
    /// 近战攻击判定范围，或远程攻击子弹判定范围
    /// </summary>
    public float DamageRange;
    /// <summary>
    /// 子弹运动时间，近战攻击设定为0
    /// </summary>
    public float BulletSpeed;
    /// <summary>
    /// 超过这段时间后攻击体自动销毁
    /// </summary>
    public float BulletLife;
    /// <summary>
    /// 子弹贴图，近战可以不设定
    /// </summary>
    public int BulletSpriteName;
    /// <summary>
    /// 距离目标小于此值时开始攻击
    /// </summary>
    public float AttackDistance;
}


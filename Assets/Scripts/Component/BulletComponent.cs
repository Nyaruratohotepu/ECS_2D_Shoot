using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

public struct BulletComponent:IComponentData
{
    /// <summary>
    /// 伤害量
    /// </summary>
    public int Damage;
    /// <summary>
    /// 子弹攻击群体
    /// </summary>
    public BulletDamageType Side;
    /// <summary>
    /// 还能存在多少秒
    /// </summary>
    public float LifeLeft;
}




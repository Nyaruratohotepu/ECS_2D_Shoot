using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Gun
{
    public static readonly string SpritePath = @"GunSprite\";
    public GunEnum GunType { get; set; }
    public GunTriggerType TriggerType { get; set; }
    public GunAnimType AnimType { get; set; }
    public BulletType BulletType { get; set; }
    public string Name { get; set; }
    public float FireCD { get; set; }
    public BulletInfo BulletInfo { get; set; }
    public Gun(string name, GunTriggerType triggerType, GunAnimType animType, BulletType bulletType, float fireCD, GunEnum gunType)
    {
        Name = name;
        TriggerType = triggerType;
        AnimType = animType;
        BulletType = bulletType;
        FireCD = fireCD;
        GunType = gunType;
    }
}
public enum GunAnimType
{
    /// <summary>
    /// 单手动画
    /// </summary>
    OneHand,
    /// <summary>
    /// 双手动画
    /// </summary>
    TwoHand
}

/// <summary>
/// 快慢机，决定发射方式
/// </summary>
public enum GunTriggerType
{
    /// <summary>
    /// 半自动：按下发射一枚
    /// </summary>
    SemiAutomatic,
    /// <summary>
    /// 全自动：按下后持续发射
    /// </summary>
    Automatic,
    /// <summary>
    /// 蓄力：按下瞄准，松开发射
    /// </summary>
    Charge
}


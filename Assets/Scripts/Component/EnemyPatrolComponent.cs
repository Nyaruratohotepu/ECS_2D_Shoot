using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 拥有此组件才能按照预定轨迹巡逻
/// </summary>
public struct EnemyPatrolComponent : IComponentData
{
    /// <summary>
    /// 是否巡逻中
    /// </summary>
    public int IsPatrolling;
    /// <summary>
    /// 发现玩家的最大距离
    /// </summary>
    public float HuntDistance;

    /// <summary>
    /// 看守位置
    /// </summary>
    public Vector2 GuardPosition;
    /// <summary>
    /// 巡逻轨迹
    /// 首元素是开始巡逻的边界范围
    /// </summary>
    public Vector2 PatrolLeftBorder;
    public Vector2 PatrolRightBorder;
    public int MoveToRightBorder;
    /// <summary>
    /// 允许的误差
    /// 若当前坐标和目的坐标距离小于这个误差，视为到达了目的地
    /// </summary>
    public float Deviation;
    /// <summary>
    /// 跳跃力度
    /// </summary>
    public float JumpForce;
    /// <summary>
    /// 怪物和前方障碍距离小于此值，触发跳跃
    /// </summary>
    public float JumpDistance;

}


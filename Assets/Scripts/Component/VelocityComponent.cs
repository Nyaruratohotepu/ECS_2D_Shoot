using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
public struct VelocityComponent : IComponentData
{
    /// <summary>
    /// 移动速度
    /// </summary>
    public float Speed;

    /// <summary>
    /// 运动矢量
    /// </summary>
    public float2 Velocity;


}

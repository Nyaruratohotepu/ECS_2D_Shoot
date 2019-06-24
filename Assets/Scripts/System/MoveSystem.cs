using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class MoveSystem : ComponentSystem
{

    //纯ECS架构的移动用过滤器，符合此过滤条件的实体均会被处理
    public struct MoveFilter
    {
        public readonly int Length; //自动填充为数据个数
        public ComponentDataArray<VelocityComponent> velocities;
        public ComponentDataArray<Position> positions;
    }

    //混合模式的移动过滤器
    public struct MoveHybirdFilter
    {
        public readonly int Length;
        public ComponentDataArray<VelocityComponent> velocities;
        public ComponentArray<Transform> transforms;
    }

    [Inject] MoveHybirdFilter moveHybirdEntities;

    protected override void OnUpdate()
    {
        ComponentDataArray<VelocityComponent> Velocities = moveHybirdEntities.velocities;
        ComponentArray<Transform> Transforms = moveHybirdEntities.transforms;
        for (int i = 0; i < moveHybirdEntities.Length; ++i)
        {
            VelocityComponent velocity = Velocities[i];
            Transform transform = Transforms[i];
            //直接操作传统component
            Vector2 movement = velocity.Velocity * Time.deltaTime;
            transform.position += (Vector3)movement;


            Vector2 direction = new Vector2();
            direction.x = velocity.Velocity.x;
            direction.y = velocity.Velocity.y;
            transform.rotation *= Quaternion.FromToRotation(transform.right, direction);

        }
    }
}

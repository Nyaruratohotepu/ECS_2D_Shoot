using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class EnemyAISystem : ComponentSystem
{
    private float RayHeight = 0.3f;
    public struct HatchFilter
    {
        public readonly int Length;
        public ComponentDataArray<EnemyComponent> enemyIds;
        public ComponentDataArray<VelocityComponent> enemyVelocities;
        public ComponentDataArray<EnemyPatrolComponent> enemyPatrols;
        public ComponentDataArray<EnemyAttackComponent> enemyAttacks;
        public ComponentArray<Transform> enemyTransforms;
        public ComponentArray<Rigidbody2D> enemyRigidbody2Ds;

    }

    [Inject] HatchFilter enemies;

    public struct PlayerFilter
    {
        public ComponentDataArray<PlayerComponent> players;
        public ComponentArray<Transform> transforms;
    }
    [Inject] PlayerFilter players;

    protected override void OnUpdate()
    {
        //获取玩家位置

        Vector3 playerPosition = players.transforms[0].position;

        ComponentDataArray<EnemyComponent> Ids = enemies.enemyIds;
        ComponentDataArray<VelocityComponent> Velocities = enemies.enemyVelocities;
        ComponentDataArray<EnemyPatrolComponent> Patrols = enemies.enemyPatrols;
        ComponentDataArray<EnemyAttackComponent> Attacks = enemies.enemyAttacks;
        ComponentArray<Transform> Transforms = enemies.enemyTransforms;
        ComponentArray<Rigidbody2D> Rigidbody2Ds = enemies.enemyRigidbody2Ds;
        for (int i = 0; i < enemies.Length; ++i)
        {
            //读
            EnemyPatrolComponent newPatrol = Patrols[i];
            VelocityComponent newVelocity = Velocities[i];
            Vector2 enemyPositon = Transforms[i].position;
            EnemyAttackComponent attack = Attacks[i];
            //判断和玩家距离，确定是否继续巡逻
            newPatrol.IsPatrolling = (Vector2.Distance(Transforms[i].position, playerPosition) > Patrols[i].HuntDistance) ? 1 : 0;

            if (newPatrol.IsPatrolling == 1)
            {
                //巡逻中

                Vector2 enemyDestination = newPatrol.MoveToRightBorder == 1 ? newPatrol.PatrolRightBorder : newPatrol.PatrolLeftBorder;
                //到达后转向，修改速度
                if (Mathf.Abs(enemyPositon.x - enemyDestination.x) < newPatrol.Deviation)
                {
                    //下一个路径点
                    ++newPatrol.MoveToRightBorder;
                    newPatrol.MoveToRightBorder %= 2;
                    enemyDestination = newPatrol.MoveToRightBorder == 1 ? newPatrol.PatrolRightBorder : newPatrol.PatrolLeftBorder;

                }
                newVelocity.Velocity.x = (enemyDestination.x - enemyPositon.x) > 0 ? newVelocity.Speed : -newVelocity.Speed;
            }
            else
            {

                if (Vector2.Distance(playerPosition, enemyPositon) > attack.AttackDistance)
                    //追逐玩家中
                    newVelocity.Velocity.x = (playerPosition.x - enemyPositon.x) > 0 ? newVelocity.Speed : -newVelocity.Speed;
                else
                    Transforms[i].GetComponentInChildren<Animator>().SetTrigger("TriAttack");
            }



            //跳过障碍物
            if (Physics2D.Raycast(Transforms[i].position + new Vector3(0, RayHeight, 0), Transforms[i].right, newPatrol.JumpDistance, LayerMask.GetMask("Map")))
            {
                //在地上
                if (Physics2D.Raycast(Transforms[i].position, new Vector3(0, -1, 0), 0.1f, LayerMask.GetMask("Map")))
                    //施加力，跳一下
                    enemies.enemyRigidbody2Ds[i].AddForce(new Vector2(0, newPatrol.JumpForce));
            }

            Transforms[i].GetComponentInChildren<Animator>().SetBool("IsRunning", (newVelocity.Velocity.x != 0 || newVelocity.Velocity.y != 0));


            //更新组件数值
            Patrols[i] = newPatrol;
            Velocities[i] = newVelocity;
        }
    }

}

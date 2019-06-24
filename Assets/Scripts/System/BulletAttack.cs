using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;

public class BulletAttack : ComponentSystem
{
    AttackManager attackManager;
    private List<GameObject> toBeDel=new List<GameObject>();

    public struct BulletFilter
    {
        public readonly int Length;
        public ComponentDataArray<BulletComponent> bullets;
        public ComponentArray<CapsuleCollider2D> colliders;
    }
    [Inject] BulletFilter bullets;


    protected override void OnUpdate()
    {

        if (attackManager == null)
        {
            attackManager = GameObject.Find("Manager").GetComponent<AttackManager>();
        }
        for (int i = 0; i < bullets.Length; ++i)
        {
            BulletComponent bullet = bullets.bullets[i];
            CapsuleCollider2D collider = bullets.colliders[i];

            bullet.LifeLeft -= Time.deltaTime;

            switch (bullet.Side)
            {
                case BulletDamageType.AttackEnemy:
                    ContactFilter2D contact = new ContactFilter2D();
                    contact.SetLayerMask(LayerMask.GetMask("Enemy"));
                    Collider2D[] results = new Collider2D[1];
                    //击中敌人
                    if (collider.OverlapCollider(contact, results) > 0)
                    {
                        GameObject enemy = results[0].gameObject;
                        attackManager.AttackEnemy(enemy, bullet.Damage);


                        //伤害完成，子弹销毁
                        bullet.LifeLeft = 0;
                    }
                    break;
                case BulletDamageType.AttackPlayer:
                    if (collider.IsTouchingLayers(LayerMask.GetMask("Player")))
                    {
                        attackManager.AttackPlayer(bullet.Damage);
                        //伤害完成，子弹销毁
                        bullet.LifeLeft = 0;
                    }
                    break;
            }

            if (bullet.LifeLeft > 0)
            {
                bullets.bullets[i] = bullet;
            }
            else
            {
                //时间到
                toBeDel.Add(collider.gameObject);
                
                //MonoBehaviour.Destroy(collider.gameObject);
                //PostUpdateCommands.DestroyEntity(collider.gameObject.GetComponent<GameObjectEntity>().Entity);
            }

        }
        //必须延迟销毁，否则报错
        foreach(GameObject bulletObj in toBeDel)
        {
            MonoBehaviour.Destroy(bulletObj);
        }
        toBeDel.Clear();
    }
}


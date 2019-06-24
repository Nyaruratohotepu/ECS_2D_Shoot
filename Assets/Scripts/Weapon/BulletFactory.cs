using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;

public class BulletFactory
{
    private static BulletFactory instance;
    private EntityManager entityManager;
    public static BulletFactory GetOrCreatInstance()
    {
        if (instance == null)
            instance = new BulletFactory();
        return instance;
    }
    private BulletFactory()
    {
        entityManager = World.Active.GetOrCreateManager<EntityManager>();
    }
    public void CreatBullet(Vector2 position, Vector2 direction, float lifeTime, BulletInfo info)
    {

        GameObject bulletObject = MonoBehaviour.Instantiate<GameObject>(Resources.Load<GameObject>(info.PrefabFullPath), position, Quaternion.FromToRotation(Vector2.right, direction));
        GameObjectEntity bulletEntity = bulletObject.AddComponent<GameObjectEntity>();
        entityManager.AddComponentData(bulletEntity.Entity, info.CreatBulletComponent(lifeTime));
        entityManager.AddComponentData(bulletEntity.Entity, info.CreatVelocityComponent(direction));
    }
}


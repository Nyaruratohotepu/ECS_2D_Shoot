using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EnemyFactory
{
    private EnemyInfo HatchModel;
    private GameObject HatchPrefab;
    private EntityManager entityManager;

    public static EnemyFactory GetOrCreatInstance()
    {
        if (instance == null) instance = new EnemyFactory();
        return instance;
    }
    private EnemyFactory()
    {
        HatchModel = EnemyPrototype.Hatch;
        HatchPrefab = Resources.Load<GameObject>(HatchModel.PrefabFullPath);

        entityManager = World.Active.GetOrCreateManager<EntityManager>();
    }
    private static EnemyFactory instance;

    public void CreatHatchEntity(Vector2 position)
    {

        GameObject hatchObject = MonoBehaviour.Instantiate<GameObject>(HatchPrefab, position, Quaternion.identity);
        EnemyHP hp = hatchObject.AddComponent<EnemyHP>();
        hp.HPMax = hp.HPLeft = HatchModel.HPMax;
        hatchObject.GetComponentInChildren<MeleeEnemyAttack>().enemy = HatchModel;


        //为GameObject绑定上ECS的数据
        GameObjectEntity hatchEntity = hatchObject.AddComponent<GameObjectEntity>();
        //敌人标识
        entityManager.AddComponentData(hatchEntity.Entity, new EnemyComponent());
        //位移组件
        entityManager.AddComponentData(hatchEntity.Entity, HatchModel.CreatVelocityComponent());
        //攻击组件
        entityManager.AddComponentData(hatchEntity.Entity, HatchModel.CreatAttckComponent());
        //巡逻组件
        entityManager.AddComponentData(hatchEntity.Entity, HatchModel.CreatPatrolComponent(position));

    }
}


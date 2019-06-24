using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Rendering;

public class BootstrapManager : MonoBehaviour
{
    public GameObjectEntity Player;
    public GameObject EnemyCreatPosition;
    public List<Transform> EnemyCreatPositions;

    [Tooltip("哈奇 初始数量")]
    public int hatchCount;


    // Start is called before the first frame update
    void Start()
    {
        World.Active.GetOrCreateManager<EntityManager>().AddComponentData(Player.Entity, new PlayerComponent());


        EnemyCreatPosition.GetComponentsInChildren<Transform>(EnemyCreatPositions);
        for (int i = 0; i < hatchCount; i++)
        {
            Vector2 position = EnemyCreatPositions[UnityEngine.Random.Range(0, EnemyCreatPositions.Count)].position;
            EnemyFactory.GetOrCreatInstance().CreatHatchEntity(position);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

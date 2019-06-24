using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public AudioGame Audio;
    public GameObject Player;
    private EntityManager entityManager;
    private PlayerHP playerHP;
    // Start is called before the first frame update
    void Start()
    {
        EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();
        playerHP = Player.GetComponent<PlayerHP>();
        Audio = gameObject.GetComponent<AudioGame>();
    }

    /// <summary>
    /// 对目标造成伤害
    /// </summary>
    /// <param name="target">目标，须绑定有EnemyHP</param>
    /// <param name="damage">伤害量</param>
    public void AttackEnemy(GameObject target, int damage)
    {
        EnemyHP enemyHP = target.GetComponent<EnemyHP>();
        //扣血
        enemyHP.HPLeft -= damage;
    }
    /// <summary>
    /// 对角色造成伤害
    /// </summary>
    /// <param name="damage">伤害量</param>
    public void AttackPlayer(int damage)
    {
        Audio.PlayerSound(SoundEnum.PlayerHitted);
        playerHP.HPLeft -= damage;
    }
    /// <summary>
    /// 治疗主角
    /// </summary>
    /// <param name="heal">治疗量</param>
    public void HealPlayer(int heal)
    {
        playerHP.HPLeft += heal;
        if (playerHP.HPLeft > playerHP.HPMax)
            playerHP.HPLeft = playerHP.HPMax;
    }



}

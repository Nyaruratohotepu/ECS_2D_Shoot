using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAttack : MonoBehaviour
{
    public EnemyInfo enemy;
    public int damage;
    public AudioGame speaker;
    private MeleeBulletInfo attackInfo;
    // Start is called before the first frame update
    void Start()
    {
        attackInfo = new MeleeBulletInfo(enemy.DamagePerHit);
        speaker = GameObject.Find("Manager").GetComponent<AudioGame>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Attack()
    {
        speaker.PlayerSound(SoundEnum.MonsterAttack);
        BulletFactory.GetOrCreatInstance().CreatBullet(transform.position, Vector2.right, 0.1f, attackInfo);
    }


}

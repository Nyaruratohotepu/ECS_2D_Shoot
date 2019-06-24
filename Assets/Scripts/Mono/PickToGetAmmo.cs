using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player触碰后会加子弹
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class PickToGetAmmo : MonoBehaviour
{
    [Tooltip("子弹类型")]
    public BulletType AmmoType = BulletType.AssaultRifleBullet;
    [Tooltip("子弹数量")]
    public int AmmoCount = 60;
    private PlayerPickManager pickManager;

    // Start is called before the first frame update
    void Start()
    {
        pickManager = PlayerPickManager.GetOrCreatInstance();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerPackage player = collision.gameObject.GetComponent<PlayerPackage>();
            pickManager.AddAmmo(AmmoType, AmmoCount, player);
            Destroy(gameObject);
        }
    }
}

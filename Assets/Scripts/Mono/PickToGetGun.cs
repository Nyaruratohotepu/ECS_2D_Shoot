using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player触碰后会添加一把枪
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class PickToGetGun : MonoBehaviour
{
    [Tooltip("枪类型")]
    public GunEnum gun = GunEnum.GunAK47;
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
            pickManager.AddWeapon(gun, player);
            Destroy(gameObject);
        }
    }
}

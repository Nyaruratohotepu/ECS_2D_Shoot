using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player触碰后会治疗Player
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class PickToHeal : MonoBehaviour
{
    private PlayerPickManager pickManager;
    [Tooltip("治疗量")]
    public int HealValue = 20;
    // Start is called before the first frame update
    void Start()
    {
        pickManager = PlayerPickManager.GetOrCreatInstance();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerHP player = collision.gameObject.GetComponent<PlayerHP>();
            pickManager.HealPlayer(HealValue, player);
            Destroy(gameObject);
        }

    }

}

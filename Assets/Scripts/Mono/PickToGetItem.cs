using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player触碰后会添加物资
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class PickToGetItem : MonoBehaviour
{
    [Tooltip("物资类型")]
    public ItemType Item = ItemType.food;
    [Tooltip("物资数量")]
    public int ItemCount = 10;
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
            pickManager.AddItem(Item, ItemCount, player);
            Destroy(gameObject);
        }
    }
}

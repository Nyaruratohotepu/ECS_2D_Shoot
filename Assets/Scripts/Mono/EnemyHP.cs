using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int HPMax;
    public int HPLeft;
    private List<Transform> InitPositions;
    private KillToCreatItem item;
    // Start is called before the first frame update
    void Start()
    {
        InitPositions = GameObject.Find("Manager").GetComponent<BootstrapManager>().EnemyCreatPositions;
        item = gameObject.GetComponent<KillToCreatItem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HPLeft <= 0)
        {
            //播放死亡动画

            //爆装备
            if (item != null)
            {
                //爆出装备
                item.CreatObj();
                //选择下一个装备
                item.ChooseObj();
            }

            //瞬移
            Vector2 initPosition;
            Vector2 viewPosition;
            //防止刷新在屏幕内
            do
            {
                initPosition = InitPositions[Random.Range(0, InitPositions.Count)].position;
                //判断其是否在屏幕内侧
                viewPosition = Camera.main.WorldToViewportPoint(initPosition);
            } while (viewPosition.x > 0 && viewPosition.x < 1 && viewPosition.y > 0 && viewPosition.y < 1);
            transform.position = initPosition;

            //动画恢复正常
            //HP恢复
            HPLeft = HPMax;

        }
    }
}

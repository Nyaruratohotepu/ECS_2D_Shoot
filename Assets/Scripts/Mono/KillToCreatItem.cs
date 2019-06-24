using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// destory或主动调用可以爆出物体
/// </summary>
public class KillToCreatItem : MonoBehaviour
{
    //最多支持五个

    public GameObject[] pickObjs;

    [Tooltip("爆出物品离自身距离")]
    public Vector3 obj_position = new Vector3(0, 0.5f, 0);
    private int result = 0;
    private int objCount;


    private void OnDestroy()
    {

    }
    /// <summary>
    /// 选择死亡后爆出的物体
    /// </summary>
    public void ChooseObj()
    {
        do
        {
            result = Random.Range(1, objCount);
        } while (pickObjs[result] == null);

    }
    /// <summary>
    /// 爆出物体
    /// </summary>
    public void CreatObj()
    {
        GameObject obj = Instantiate<GameObject>(pickObjs[result]);
        obj.transform.position = gameObject.transform.position + obj_position;
    }

    // Start is called before the first frame update
    void Start()
    {
        objCount = pickObjs.Length;
        ChooseObj();
    }
}

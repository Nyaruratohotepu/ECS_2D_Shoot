using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    public int HPMax = 200;
    public int HPLeft;
    public string DeathSenceName = "DeathSence";
    // Start is called before the first frame update
    void Start()
    {
        HPLeft = HPMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (HPLeft <= 0)
        {
            //主角死亡
            SceneManager.LoadScene(DeathSenceName);
        }
    }
}

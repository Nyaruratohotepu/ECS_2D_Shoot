using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 用于使UI界面在sence_game和sence_city间切换
/// </summary>
[RequireComponent(typeof(StageCamera))]
public class GameUIChange : MonoBehaviour
{
    public Vector3 GameCameraPosition = new Vector3(8.888889f, -5, 0);
    public Vector3 CityCameraPosition = new Vector3(-8.888889f, -5, 0);
    private Transform cameraTransform;
    void Start()
    {
        cameraTransform = gameObject.GetComponent<Transform>();
    }
    /// <summary>
    /// 切换场景UI
    /// </summary>
    /// <param name="ui">切换为</param>
    public void ChangeTo(UISence ui)
    {
        switch (ui)
        {
            case UISence.City:
                cameraTransform.localPosition = CityCameraPosition;
                break;
            case UISence.Game:
                cameraTransform.localPosition = GameCameraPosition;
                break;

        }
    }
}

public enum UISence
{
    Game,
    City
}

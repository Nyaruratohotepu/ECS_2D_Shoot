using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathTool
{
    /// <summary>
    /// xy平面上，一条向量绕某轴旋转，使其旋转后恰好指向目标点坐标，返回需要的旋转
    /// </summary>
    /// <param name="pointBegin">原向量起点坐标</param>
    /// <param name="pointEnd">原向量终点坐标</param>
    /// <param name="pointTarget">目标点坐标</param>
    /// <param name="pointAxis">旋转轴在xy平面上坐标</param>
    /// <param name="maxShotAngle">最大射高，0-90，与x轴夹角</param>
    /// <returns>需要的旋转</returns>
    public static Quaternion RotateToAimAtXY(Vector2 pointBegin, Vector2 pointEnd, Vector2 pointTarget, Vector2 pointAxis, float maxShotAngle = 90)
    {


        Vector2 BE = pointEnd - pointBegin;

        Vector2 BENomalized = BE.normalized;
        Vector2 OB = pointBegin - pointAxis;
        Vector2 AB = (Vector2.Dot(BE, OB) * BENomalized) / BE.magnitude;
        Vector2 pointA = pointBegin - AB;
        Vector2 OA = OB - AB;
        float lOA = OA.magnitude;
        float lOP = (pointTarget - pointAxis).magnitude;
        float lAP = Mathf.Sqrt(lOP * lOP - lOA * lOA);
        Vector2 pointP = pointA + BENomalized * lAP;
        Vector2 OP = pointP - pointAxis;
        Vector2 OT = pointTarget - pointAxis;

        Quaternion QToTarget = Quaternion.FromToRotation(OP, OT);

        //尝试将枪管旋转到目标方向，若新枪管方向和y轴夹角过小，则为越界
        if (Mathf.Abs(Vector2.Angle(QToTarget * BE, Vector2.up)) < (90 - maxShotAngle))
        {
            //越界则旋转到极限值
            float maxY = Mathf.Tan(maxShotAngle * Mathf.Deg2Rad);
            Vector2 targetMax = BE.x > 0 ? new Vector2(1, maxY) : new Vector2(-1, maxY);
            return Quaternion.FromToRotation(BE, targetMax);
        }
        else return QToTarget;

    }

}

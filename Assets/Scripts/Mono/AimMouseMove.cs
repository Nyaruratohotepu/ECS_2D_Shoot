using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;

public class AimMouseMove : MonoBehaviour
{
    public Transform shoulder;
    [Tooltip("右手IK如果绕真shoulder转动会累计误差，需绕相对IK父物体固定的位置旋转")]
    public Transform shoulderFake;
    public Transform root;
    public Animator AnimatorPlayer;
    public Transform AimBeginOneHand;
    public Transform AimEndOneHand;
    public Transform AimBeginTwoHand;
    public Transform AimEndTwoHand;
    public Transform IKHandRight;
    public Transform IKHandLeft;
    public Transform HandRightOne;
    public Transform HandRightTwo;
    public Transform HandLeft;
    public CCDSolver2D IKHandRightSolverOneHand;
    public CCDSolver2D IKHandRightSolverTwoHand;
    public CCDSolver2D IKHandLeftSolver;
    public SpriteRenderer SpriteOneHandWeapon;
    public SpriteRenderer SpriteTwoHandWeapon;

    public Gun Weapon
    {
        set
        {
            weapon = value;
            switch (weapon.AnimType)
            {
                case (GunAnimType.OneHand):
                    aimBegin = AimBeginOneHand;
                    aimEnd = AimEndOneHand;
                    iKManager2D.solvers.Clear();
                    iKManager2D.solvers.Add(IKHandRightSolverOneHand);
                    SpriteOneHandWeapon.sprite = Resources.Load<Sprite>(Gun.SpritePath + weapon.Name);
                    SpriteOneHandWeapon.enabled = true;
                    SpriteTwoHandWeapon.enabled = false;
                    break;
                case (GunAnimType.TwoHand):
                    aimBegin = AimBeginTwoHand;
                    aimEnd = AimEndTwoHand;
                    iKManager2D.solvers.Clear();
                    iKManager2D.solvers.Add(IKHandRightSolverTwoHand);
                    iKManager2D.solvers.Add(IKHandLeftSolver);
                    SpriteTwoHandWeapon.sprite = Resources.Load<Sprite>(Gun.SpritePath + weapon.Name);
                    SpriteTwoHandWeapon.enabled = true;
                    SpriteOneHandWeapon.enabled = false;
                    break;
            }
        }
        get
        {
            return weapon;
        }
    }
    private Gun weapon;
    private IKManager2D iKManager2D;

    public float MinSpeedInput = 0.2f;

    [Tooltip("水平最大速度")]
    public float SpeedHorMax = 10;

    [Tooltip("弹跳力度")]
    public float JumpForce = 100;

    [Tooltip("最多几段跳")]
    public int JumpCount = 2;

    [Tooltip("最高射角，0-90度，x轴到最高射击线")]
    public float MaxShootAngle = 70;

    public Transform Muzzle
    {
        get
        {
            return aimEnd;
        }
    }
    private Rigidbody2D rigidbodyPlayer;
    private Transform aimBegin;
    private Transform aimEnd;
    private int jumpCountLeft;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyPlayer = gameObject.GetComponent<Rigidbody2D>();
        iKManager2D = gameObject.GetComponent<IKManager2D>();
        Weapon = new GunM4();
    }


    void Update()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Time.timeScale > 0)
        {
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //旋转瞄准部分
            //若瞄准后方，则转身
            Vector2 rootToMouse = mousePoint - root.position;
            Vector2 rootDirection = root.right;
            float angle = Vector2.Angle(rootToMouse, rootDirection);
            if (angle > 90)
                root.rotation *= Quaternion.AngleAxis(180, root.up);


            //计算出右臂需要旋转的角度
            Quaternion handRightQuate = MathTool.RotateToAimAtXY(aimBegin.position, aimEnd.position, mousePoint, shoulder.position, MaxShootAngle);
            //右臂旋转
            shoulder.rotation *= handRightQuate;
            //IK也绕着右臂旋转
            IKHandRight.RotateAround(shoulderFake.position, Vector3.forward, handRightQuate.normalized.eulerAngles.z);
            //动画控制部分
            float inputAxisHor = Input.GetAxis("Horizontal");
            if (Mathf.Abs(inputAxisHor) < MinSpeedInput)
            {
                //输入过小，视为静止
                AnimatorPlayer.SetBool("isRunning", false);
            }
            else
            {
                //移动
                AnimatorPlayer.SetBool("isRunning", true);
                //不乘后面一项会导致所有的左向运动均使用反向跑动动画，无论面朝何处
                AnimatorPlayer.SetFloat("speed", inputAxisHor * Mathf.Sign(root.right.x));
                transform.position += (inputAxisHor * SpeedHorMax * Vector3.right);
            }

            if (rigidbodyPlayer.velocity.y == 0)
                jumpCountLeft = JumpCount;
            if (jumpCountLeft > 0 && Input.GetKeyDown(KeyCode.Space))
            {
                --jumpCountLeft;
                rigidbodyPlayer.AddForce(Vector2.up * JumpForce);
            }
        }
    }
}

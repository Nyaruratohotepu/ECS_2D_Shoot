using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 控制开枪的行为
/// </summary>
[RequireComponent(typeof(AimMouseMove))]
public class PlayerFire : MonoBehaviour
{
    public float LineLength = 200f;
    [Tooltip("后坐力枪位移大小")]
    public float KickForce = 20f;
    public float KickResetSpeed = 10f;
    public Transform OneHandWeapon;
    public Transform TwoHandWeapon;
    public Transform OneHandWeaponOrigin;
    public Transform TwoHandWeaponOrigin;
    public AudioGame Audio;
    private AimMouseMove player;
    private PlayerPackage ammo;
    private BulletFactory bulletFactory;
    private Gun weapon;
    private float cdPass;




    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<AimMouseMove>();
        ammo = gameObject.GetComponent<PlayerPackage>();
        bulletFactory = BulletFactory.GetOrCreatInstance();

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;

        OneHandWeapon.position = Vector3.Lerp(OneHandWeapon.position, OneHandWeaponOrigin.position, Time.deltaTime * KickResetSpeed);
        TwoHandWeapon.position = Vector3.Lerp(TwoHandWeapon.position, TwoHandWeaponOrigin.position, Time.deltaTime * KickResetSpeed);
        weapon = player.Weapon;
        cdPass += Time.deltaTime;
        RaycastHit2D hitFire = Physics2D.Raycast(player.Muzzle.position, player.Muzzle.right, LineLength, LayerMask.GetMask("Enemy", "Map"));
        if (cdPass >= weapon.FireCD && hitFire)
        {
            Debug.DrawLine(player.Muzzle.position, hitFire.point, Color.red);
            switch (weapon.TriggerType)
            {
                case GunTriggerType.SemiAutomatic:
                    if (Input.GetMouseButtonDown(0) && !Stage.isTouchOnUI)
                    {
                        TryFire(hitFire.point);
                    }
                    break;
                case GunTriggerType.Automatic:
                    if (Input.GetMouseButton(0) && !Stage.isTouchOnUI)
                    {
                        TryFire(hitFire.point);
                    }
                    break;
            }
        }
    }

    private void TryFire(Vector2 targetPosition)
    {
        if (!ammo.TryConsumeAmmo())
        {
            //弹药不足

            //播放卡壳声
            Audio.PlayerSound(SoundEnum.NoAmmo);
            return;
        }
        //弹药充足

        Vector2 direction = targetPosition - (Vector2)player.Muzzle.position;
        BulletInfo bulletInfo = weapon.BulletInfo;
        float lifeTime = direction.magnitude / bulletInfo.Speed;
        bulletFactory.CreatBullet(player.Muzzle.position, direction, lifeTime, bulletInfo);
        Audio.PlayerSound(SoundEnum.Fire);
        cdPass = 0;

        //施加后座位移
        switch (weapon.AnimType)
        {
            case GunAnimType.OneHand:
                OneHandWeapon.position = OneHandWeaponOrigin.position - KickForce * OneHandWeaponOrigin.right;
                break;
            case GunAnimType.TwoHand:
                TwoHandWeapon.position = TwoHandWeaponOrigin.position - KickForce * TwoHandWeaponOrigin.right;
                break;
        }

    }

}

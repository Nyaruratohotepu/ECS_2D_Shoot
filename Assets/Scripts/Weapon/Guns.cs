using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//枪实例
public class GunM4 : Gun
{
    public GunM4() : base("m4", GunTriggerType.Automatic, GunAnimType.TwoHand, BulletType.AssaultRifleBullet, 0.2f, GunEnum.GunM4)
    {
        base.BulletInfo = new BulletInfo("1", 10, BulletDamageType.AttackEnemy, 30f);
    }
}
public class GunPistol : Gun
{
    public GunPistol() : base("pistol", GunTriggerType.SemiAutomatic, GunAnimType.OneHand, BulletType.PistolBullet, 0.1f, GunEnum.GunPistol)
    {
        base.BulletInfo = new BulletInfo("2", 20, BulletDamageType.AttackEnemy, 35f);
    }
}
public class GunAK47 : Gun
{
    public GunAK47() : base("ak47", GunTriggerType.Automatic, GunAnimType.TwoHand, BulletType.AssaultRifleBullet, 0.3f, GunEnum.GunAK47)
    {
        base.BulletInfo = new BulletInfo("1", 20, BulletDamageType.AttackEnemy, 30f);
    }
}
public class GunFN2000 : Gun
{
    public GunFN2000() : base("fn2000", GunTriggerType.Automatic, GunAnimType.TwoHand, BulletType.AssaultRifleBullet, 0.2f, GunEnum.GunFN2000)
    {
        base.BulletInfo = new BulletInfo("3", 15, BulletDamageType.AttackEnemy, 30f);
    }
}
public class GunFusee : Gun
{
    public GunFusee() : base("fusee", GunTriggerType.SemiAutomatic, GunAnimType.TwoHand, BulletType.AssaultRifleBullet, 0.5f, GunEnum.GunFusee)
    {
        base.BulletInfo = new BulletInfo("1", 30, BulletDamageType.AttackEnemy, 20f);
    }
}
public class GunMP5 : Gun
{
    public GunMP5() : base("mp5", GunTriggerType.Automatic, GunAnimType.TwoHand, BulletType.PistolBullet, 0.1f, GunEnum.GunMP5)
    {
        base.BulletInfo = new BulletInfo("2", 15, BulletDamageType.AttackEnemy, 30f);
    }
}
public class GunRevolver : Gun
{
    public GunRevolver() : base("revolver", GunTriggerType.SemiAutomatic, GunAnimType.OneHand, BulletType.PistolBullet, 0.6f, GunEnum.GunRevolver)
    {
        BulletInfo = new BulletInfo("3", 30, BulletDamageType.AttackEnemy, 30f);
    }
}

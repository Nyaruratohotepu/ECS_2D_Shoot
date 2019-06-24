using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GunFactory
{
    private static GunFactory instance;
    public static GunFactory GetOrCreatInstance()
    {
        if (instance == null) instance = new GunFactory();
        return instance;
    }

    public Gun CreatGun(GunEnum gun)
    {
        switch (gun)
        {
            case GunEnum.GunAK47:
                return new GunAK47();
            case GunEnum.GunFN2000:
                return new GunFN2000();
            case GunEnum.GunFusee:
                return new GunFusee();
            case GunEnum.GunM4:
                return new GunM4();
            case GunEnum.GunMP5:
                return new GunMP5();
            case GunEnum.GunPistol:
                return new GunPistol();
            case GunEnum.GunRevolver:
                return new GunRevolver();
            default: return null;
        }
    }






}


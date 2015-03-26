using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieShooter
{
    public class Handgun : Gun
    {
        public Handgun(Sprite newOwner)
        {
            this.owner = newOwner;
            this.GunName = "Handgun";

            this.bulletHealth = 100;
            this.bulletDamage = 30;
            this.bulletVelocity = 4;
            this.shootRadius = new float[] { 0f };
            this.Ammo = 21;
            this.Clip[0] = 7;
            this.shootSpeed[0] = 15;
            this.reloadSpeed[0] = 60;
            this.Clip[1] = this.Clip[0];
            this.shootSpeed[1] = this.shootSpeed[0];
            this.reloadSpeed[1] = this.reloadSpeed[0];
        }
    }

    public class Shotgun : Gun
    {
        public Shotgun(Sprite newOwner)
        {
            this.owner = newOwner;
            this.GunName = "Shotgun";

            this.bulletHealth = 75;
            this.bulletDamage = 30;
            this.bulletVelocity = 3;
            this.shootRadius = new float[] { -0.1f, 0f, 0.1f };
            this.Ammo = 18;
            this.Clip[0] = 6;
            this.shootSpeed[0] = 30;
            this.reloadSpeed[0] = 120;
            this.Clip[1] = this.Clip[0];
            this.shootSpeed[1] = this.shootSpeed[0];
            this.reloadSpeed[1] = this.reloadSpeed[0];
        }
    }

    public class Rifle : Gun
    {
        public Rifle(Sprite newOwner)
        {
            this.owner = newOwner;
            this.GunName = "Rifle";

            this.bulletHealth = 150;
            this.bulletDamage = 40;
            this.bulletVelocity = 5;
            this.shootRadius = new float[] { 0f };
            this.Ammo = 45;
            this.Clip[0] = 15;
            this.shootSpeed[0] = 15;
            this.reloadSpeed[0] = 120;
            this.Clip[1] = this.Clip[0];
            this.shootSpeed[1] = this.shootSpeed[0];
            this.reloadSpeed[1] = this.reloadSpeed[0];
        }
    }

    public class SubmachineGun : Gun
    {
        public SubmachineGun(Sprite newOwner)
        {
            this.owner = newOwner;
            this.GunName = "Submachine Gun";

            this.bulletHealth = 75;
            this.bulletDamage = 15;
            this.bulletVelocity = 4;
            this.shootRadius = new float[] { 0f };
            this.Ammo = 75;
            this.Clip[0] = 25;
            this.shootSpeed[0] = 5;
            this.reloadSpeed[0] = 90;
            this.Clip[1] = this.Clip[0];
            this.shootSpeed[1] = this.shootSpeed[0];
            this.reloadSpeed[1] = this.reloadSpeed[0];
        }
    }
}
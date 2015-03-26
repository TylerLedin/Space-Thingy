using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ZombieShooter
{
   public class Gun
    {
        public int bulletHealth = 100; // How much deterioration the bullet should withstand
        public int bulletDamage = 25; // Damage bullet causes
        public int bulletVelocity = 4; // Speed of bullet
        public float[] shootRadius = { 0f }; // Determines how many bullets are released per shot and at which angle they are released (in radians)
        public string GunName; // The name of gun (Handgun, Shotgun, etc.)
        public Sprite owner; // The sprite the gun is held by

        public int Ammo = 21; // How much ammo the gun has
        public int[] Clip = { 7, 7 }; // How many rounds in the clip (1st element is current, 2nd element is max)
        public int[] shootSpeed = { 15, 15 }; // How long between shots in miliseconds (1st element is current, 2nd element is max)
        public int[] reloadSpeed = { 60, 60 }; // How long reloading takes (1st element is current, 2nd element is max)

        public Gun() // Empty; Values are managed through subclasses
        {
        }

        public void UpdateTimers() // Updates the timers for shooting and reloading
        {
            if (shootSpeed[0] < shootSpeed[1])
                shootSpeed[0]++;
            if (reloadSpeed[0] < reloadSpeed[1])
                reloadSpeed[0]++;
        }

        public void Shoot()
        {
            if ((Clip[0] > 0) && (shootSpeed[0] == shootSpeed[1])
                && (reloadSpeed[0] == reloadSpeed[1]))
            { // Only shoots if not reloading, still has ammo, and shot is ready
                foreach (float spread in shootRadius)
                { // For each rotation designated release a bullet, setting the values and then adding it to the game's bullet list
                    Bullet newBullet = new Bullet();
                    newBullet.Health = bulletHealth;
                    newBullet.Damage = bulletDamage;
                    newBullet.velocity = bulletVelocity;
                    newBullet.Rotation = owner.Rotation + spread;
                    newBullet.Position = owner.Position;
                    Game1.Bullets.Add(newBullet);
                }
                Clip[0]--; // Decrements clip
                shootSpeed[0] = 0; // Activates countdown on shooting
            }
        }

        public void Reload()
        {
            if (Ammo != 0 && reloadSpeed[0] == reloadSpeed[1] && Clip[0] != Clip[1])
            { // Checks to see if reload is possible (timer not activated, clip not max, and still have ammo)
                if (Ammo < Clip[1])
                { // Not enough ammo, so add only what you can
                    Clip[0] = Ammo;
                    Ammo = 0;
                }
                else
                { // Add the normal amount, decrementing from ammo
                    Ammo -= Clip[1] - Clip[0];
                    Clip[0] = Clip[1];
                }
                reloadSpeed[0] = 0; // Activates countdown on reloading
            }
        }
    }
}

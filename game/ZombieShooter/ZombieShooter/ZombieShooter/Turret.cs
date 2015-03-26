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
   public class Turret : Unit
    {
        public Gun gun;
        public int[] Range = { 2, 240 }; // min/max distance for firing
        float targetRotation; // Allows the turret to turn
        public int health = 500;

        public Turret() // Initializes the turret with a rifle, the turret texture, sets the health and the rotation
        {
            this.Texture = Game1.turret;
            gun = new Rifle(this);
            this.Health = new int[] { 50, 50 };
            targetRotation = this.Rotation;
        }

        public override void Update()
        {
            gun.UpdateTimers(); // Updates gun timers

            if (gun.Clip[0] == 0)
            { // Reloads and resets ammo so turret isn't rendered useless
                gun.Reload();
                gun.Ammo = 100;
            }

            Zombie nearZombie = Game1.getNearestZombie(this.Position, Range); // Gets target
     
            if (nearZombie != null)
            {
                // Faces target
                Vector2 direction = new Vector2(this.Position.X, this.Position.Y)
                    - new Vector2(nearZombie.Position.X, nearZombie.Position.Y);
                direction.Normalize();
                targetRotation = (float)(Math.Atan2(direction.Y, direction.X) - Math.PI / 2);

                // When close enough to target rotation, shoot at target
                if (targetRotation >= this.Rotation - 0.02f && targetRotation <= this.Rotation + 0.02f)
                    gun.Shoot();
            }

            // Snap to target rotation
            if (targetRotation != this.Rotation)
                this.Rotation = MathHelper.Lerp(this.Rotation, targetRotation, 0.25f);

            // Remove from turret list when dead
            this.health--;
            if (this.health < 1 || this.IsDead)
                Game1.Turrets.Remove(this);
            
       
           
            

   


            // Updates unit
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch) // Draws unit
        {
            base.Draw(spriteBatch);
        }
    }
}

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
    public class Zombie : Unit
    {
        public List<Gun> guns = new List<Gun>(); // The guns held by the player
        public int selectedGun = 0; // Index of selected gun
        public int[] Range = { 2, 240 };
        public int damage;
        public Gun gun;

        public int[] attackSpeed = { 30, 30 }; // Timer for attack; in miliseconds
        public int Damage
        
        {
            get
            { // Only deals damage if attack is ready
                if (attackSpeed[0] == attackSpeed[1])
                {
                    attackSpeed[0] = 0;
                    return damage;
                }
                else
                    return 0;
            }
            set { damage = value; }
        }

        public Zombie() // Randomizes texture and sets damage and health
        {
            Random random = new Random();
            this.Texture = Game1.zombie[random.Next(0, Game1.zombie.Count())];

            damage = 5;
            Health[0] = 50;
        }

        public void attack(Unit unit) // Allows attacking of unit
        {
            unit.Hurt(Damage);
           
        }
        public Unit getTarget()
        {
            // Gets nearest player and turret; there is only one player currently, but this adds support for more players later
            Turret nearTurret = Game1.getNearestTurret(this.Position);
            Player nearPlayer = Game1.getNearestPlayer(this.Position);
            Unit targetUnit;
            // Assigns the target unit based on proximity (preference is given to the player)
            if (nearPlayer != null && nearTurret != null)
                targetUnit = (Vector2.Distance(nearPlayer.Position, this.Position)
                    > Vector2.Distance(nearTurret.Position, this.Position) * 3) ? (Unit)nearTurret : (Unit)nearPlayer;
            else
                targetUnit = (nearPlayer == null) ? (Unit)nearTurret : (Unit)nearPlayer;
            

            return targetUnit;
        }
        public override void Update()
        {
          
            // Gets target
            Unit targetUnit = getTarget();

            if (targetUnit != null)
            { // Moves to target and attacks once in range
                Vector2 direction = new Vector2(this.Position.X, this.Position.Y)
                    - new Vector2(targetUnit.Position.X, targetUnit.Position.Y);
                direction.Normalize();
                this.Position -= direction;
                this.Rotation = (float)(Math.Atan2(direction.Y, direction.X) - Math.PI / 2);

                if (Vector2.Distance(this.Position, targetUnit.Position) < 16)
                    attack(targetUnit);
                if (Vector2.Distance(this.Position, targetUnit.Position) > 16)
                    guns.Add(new Rifle(this));
                    guns[selectedGun].Shoot();
                    
                
                
            }

            // updates timer
            if (attackSpeed[0] < attackSpeed[1])
                attackSpeed[0]++;

            base.Update(); // Updates unit
        }
       
         

        public override void Draw(SpriteBatch spriteBatch) // Draws unit
        {
            base.Draw(spriteBatch);
        }
    }
}

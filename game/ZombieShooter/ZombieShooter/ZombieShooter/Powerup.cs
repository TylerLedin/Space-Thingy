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
    public class Powerup : Sprite
    { // Parent class, has no functionality by itself
        public Powerup()
        {
        }

        public virtual void Pickup(Player unit)
        {
            Game1.powerUps.Remove(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }

    public class AmmoCrate : Powerup
    {
        public AmmoCrate()
        {
            this.Texture = Game1.ammoBox; // Sets texture
        }

        public override void Pickup(Player unit) // Applies effect and then removes powerup from powerup list
        {
            unit.guns[unit.selectedGun].Ammo += unit.guns[unit.selectedGun].Clip[1] * 2;
            Game1.powerUps.Remove(this);
        }
    }

    public class MedKit : Powerup
    {
        public MedKit()
        {
            this.Texture = Game1.medKit; // Sets texture
        }

        public override void Pickup(Player unit) // Applies effect and then removes powerup from powerup list
        {
            if (!unit.AtFullHealth)
            {
                unit.Heal(5);
                Game1.powerUps.Remove(this);
            }
        }
    }
}
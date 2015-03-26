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
   public class Bullet : Sprite
   {
       int maxHealth;
       ParticleEngine engine;
        int health; // Manages the deterioration of the bullet
        int damage; // Damage the bullet will inflict
        public double velocity; // Speed of bullet
       public int Health // Property allowing access to health; when setting, automatically sets the health and maxHealth to the value
        {
            get { return health; }
            set
            {
                maxHealth = value;
                health = maxHealth;
            }
        }

        public int Damage // Access to damage; Makes the damage proportionate to deterioration
        {
            get
            {
                return (int)(damage * ((double)health / (double)maxHealth));
            }
            set { damage = value; }
        }

        public Bullet() // Initializes bullet with default texture
        {
            this.Texture = Game1.bullet;
            engine = new ParticleEngine(Game1.particles, this.Position);
        }
		
		public void Deteriorate(int amount)
        {
            health -= amount;
        }

        public void Update()
        {
			// Moves the bullet forward
            this.Position.X -= (float)((Math.Cos(this.Rotation + Math.PI / 2)) * velocity);
            this.Position.Y -= (float)((Math.Sin(this.Rotation + Math.PI / 2)) * velocity);
			
			// Updates collision values
            this.UpdateTransform();

			// Fades the bulllet, removing it once it is deteriorated
            health--;
            if (health <= 0)
                Game1.Bullets.Remove(this);
            engine.EmitterLocation = this.Position;
            engine.Update();
        }
      
        public override void Draw(SpriteBatch spriteBatch) // Draws bullet
        {
            base.Draw(spriteBatch);
            engine.Draw(spriteBatch);
        }
     }
    
   }

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
  public  class Unit : Sprite
    {
        public int[] Health = { 50, 50 };

        public List<Bullet> immuneBullets = new List<Bullet>(); // A list of all bullets the unit is immune to; Used so that bullets don't hit the same target more than once

        public bool IsDead
        {
            get { return Health[0] <= 0; }
        }

        public bool AtFullHealth
        {
            get { return Health[0] == Health[1]; }
        }

        public virtual void Hurt(Bullet bullet) // Method for receiving damage from bullet
        {
            if (!immuneBullets.Contains(bullet))
            { // Only does damage if not contained in the immune bullet list
                int damage = bullet.Damage;
                this.Hurt(damage); // Deals damage
                // Adds a new message in the direction of the bullet, then adds it to the message list
                Message newMess = new Message(damage.ToString(), this.Position, Game1.font, Color.Red);
                newMess.TimeSpan = 60;
                newMess.Rotation = bullet.Rotation;
                Game1.messages.Add(newMess);
                this.Position = new Vector2(this.Position.X + (float)Math.Cos(this.Rotation + Math.PI / 2) * 2,
                    this.Position.Y + (float)Math.Sin(this.Rotation + Math.PI / 2) * 2); // Displaces unit slightly

                immuneBullets.Add(bullet); // Adds bullet to immunebullet list
            }
        }

        public virtual void Hurt(int amount) // Deals damage in a way that health never drops below 0
        {
            if (amount > Health[0])
                Health[0] = 0;
            else
                Health[0] -= amount;
        }

        public virtual void Heal(int amount) // Heals the unit in a way that it never goes past the maximum
        {
            if (amount + Health[0] > Health[1])
                Health[0] = Health[1];
            else
                Health[0] += amount;
        }
        public Unit() // Empty; No entity will actually be brought in as a unit, but rather one of the inherited classes
        {
        }

        public void FacePosition(Vector2 position) // Faces the unit to the given position
        {
            Vector2 direction = new Vector2(this.Position.X, this.Position.Y) - position;
            direction.Normalize();
            this.Rotation = (float)(Math.Atan2(direction.Y, direction.X) - Math.PI / 2);
        }

        public virtual void Update() // Unit update method
        {
            base.UpdateTransform();
        }

        public override void Draw(SpriteBatch spriteBatch) // Unit draw method
        {
            base.Draw(spriteBatch);
        }
    }
}

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
    public class AoETurret : Unit
    {
        int radius = 128;

        public AoETurret()
        {
        }

        public bool isColliding(Vector2 position, float radius)
        {
            return CircleCollision(this.Position.X, this.Position.Y, this.radius, position.X, position.Y, radius);
        }

        public static bool CircleCollision(float x1, float y1, float radius1, float x2, float y2, float radius2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;
            float radii = radius1 + radius2;
            if ((dx * dx) + (dy * dy) < radii * radii)
                return true;
            return false;
        }

        public virtual void Effect(Unit unit)
        {
        }

        public Texture2D CreateCircle(int radius)
        {
            int outerRadius = radius * 2 + 2;
            Texture2D texture = new Texture2D(Game1.graphicsDevice, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = Color.White;
            }

            texture.SetData(data);
            return texture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CreateCircle(radius), this.Position - new Vector2(radius, radius), Color.White);
            base.Draw(spriteBatch);
        }
    }

    public class HealTurret : AoETurret
    {
        public HealTurret()
        {
            this.Texture = Game1.healTurret;
        }

        public override void Effect(Unit unit)
        {
            unit.Heal(100);
        }
    }
}
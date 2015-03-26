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
   public class Sprite
    {
        Texture2D texture; // This will hold the Sprites texture, and is what is actually drawn on screen

        public Vector2 Position; // This is the position the Sprite will be drawn by,
        public Vector2 Origin; // Offset by the origin so that the center of the texture is always directly at the position vector
        public float Rotation; // Stores the rotation of the Sprite
        public float Scale = 1.0f; // The size of the sprite (in terms of percent, 1.0f = 100% size; this means that 0.5f would be 50% size and 1.5f would be 150% size)
        private Color[] colorData; // Holds the color data of each pixel
        private Matrix transform; // Holds the transform value of the sprite

        public Matrix Transform // Allows access to transform
        {
            get { return transform; }
        }

        public Texture2D Texture // A property for accessing our texture; When setting the texture it automatically sets the origin to be at the center of the texture
        {
            get { return texture; }
            set
            { // In addition to setting the texture and origin, color data is now set
                texture = value;
                colorData = new Color[texture.Width * texture.Height];
                texture.GetData(colorData);
                Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            }
        }

        public Sprite(Texture2D texture, Vector2 position) // Accepts texture and position values to construct the sprite
        {
            this.Texture = texture;
            this.Position = position;
        }
        public bool IntersectPixels(Sprite b)
        { // Passes the sprite's values to the intersect method
            return IntersectPixels(transform, texture.Width, texture.Height, colorData,
                           b.transform, b.texture.Width, b.texture.Height, b.colorData);
        }

        public static bool IntersectPixels(
            Matrix transformA, int widthA, int heightA, Color[] dataA,
            Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Cycles through each color array, checking to see if both are not transparent at a given point (if not transparent, they collide)

            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            for (int yA = 0; yA < heightA; yA++)
            {
                Vector2 posInB = yPosInB;

                for (int xA = 0; xA < widthA; xA++)
                {
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            return true;
                        }
                    }
                    posInB += stepX;
                }
                yPosInB += stepY;
            }
            return false;
        }

        public Sprite() // An empty constructor that accepts no values; any sprite using this will require values to be added manually
        {
        }

        public void UpdateTransform() // Placeholder for now
        { // Sets the transform based on the position, origin, scale, and rotation
            transform = Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                        Matrix.CreateScale(Scale) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateTranslation(new Vector3(Position, 0.0f));
        }

        public virtual void Draw(SpriteBatch spriteBatch) // Accepts only a spritebatch object, redirecting the color white to our true draw method
        {
            this.Draw(spriteBatch, Color.White);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Color color) // Draws the Sprite
        {
            spriteBatch.Draw(Texture, Position, null, color, Rotation, Origin,
                Scale, SpriteEffects.None, 0.0f);
        }
    }
}

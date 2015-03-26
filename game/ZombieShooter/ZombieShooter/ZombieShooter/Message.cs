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
   public class Message
    {
        string text;
        Vector2 position;
        Color color;
        SpriteFont font;
        int[] timeSpan = new int[2]; // How long the message will be visible for; in miliseconds
        float rotation;

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public bool Disappear // Checks if the message is ready to disappear
        {
            get { return timeSpan[0] >= timeSpan[1]; }
        }

        public int TimeSpan
        {
            get { return timeSpan[1]; }
            set
            {
                // Resets the current time whenever the max time is set
                timeSpan[1] = value;
                timeSpan[0] = 0;
            }
        }

        public Message(string text, Vector2 position, SpriteFont font, Color color) // Passes the message, its position, the font used to draw it, and its color
        {
            this.text = text;
            this.position = position;
            this.color = color;
            this.font = font;
        }

        public void Update(GameTime time)
        {
            timeSpan[0]++; // Increment timer

            // Floats the message over time
            position.X -= time.ElapsedGameTime.Milliseconds * (float)Math.Cos(rotation + Math.PI / 2) * 0.1f;
            position.Y -= time.ElapsedGameTime.Milliseconds * (float)Math.Sin(rotation + Math.PI / 2) * 0.1f;

            if (Disappear) // When ready to disappear, remove from message list
                Game1.messages.Remove(this);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, position, color * ((float)timeSpan[0] / (float)timeSpan[1])); // Draw message at a color proportionate to time left
        }
    }
}

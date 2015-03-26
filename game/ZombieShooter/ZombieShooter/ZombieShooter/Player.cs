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
    public class Player : Unit
    {
        public int[] stamina = { 320, 320 }; // Holds the amount of stamina the player has, limiting sprint time (1st element is current, 2nd element is max)
        public List<Gun> guns = new List<Gun>(); // The guns held by the player
        public int selectedGun = 0; // Index of selected gun
		

        public Player() // Accesses soldier texture and sets position
        {
            this.Texture = Game1.soldier;
            this.Position = new Vector2(288, 288);
            // Adds each of the guns
            guns.Add(new Handgun(this));
            guns.Add(new Shotgun(this));
            guns.Add(new Rifle(this));
            guns.Add(new SubmachineGun(this));
        }

        public virtual void Update(KeyboardState curKeyboard, KeyboardState prevKeyboard,
            MouseState curMouse, MouseState prevMouse) // Updates the player with input states as it is the only controlled entity
        {
            HandleMovement(curKeyboard); // Moves the player
            base.FacePosition(new Vector2(curMouse.X, curMouse.Y)); // Faces the player at the mouse
            UpdateTimers(curKeyboard); // Updates timers (stamina, reload time, etc.)
            if ((curMouse.LeftButton == ButtonState.Pressed)) // If left-mouse button is pressed, shoot
                guns[selectedGun].Shoot();
            if (curKeyboard.IsKeyDown(Keys.R) && prevKeyboard.IsKeyUp(Keys.R)) // If R is pressed and released, reload
                guns[selectedGun].Reload();

            // Change which gun is selected while not going out of bounds
            if ((curKeyboard.IsKeyDown(Keys.Q) && prevKeyboard.IsKeyUp(Keys.Q)
                || curMouse.ScrollWheelValue > prevMouse.ScrollWheelValue) && selectedGun > 0)
                selectedGun--;
            if ((curKeyboard.IsKeyDown(Keys.E) && prevKeyboard.IsKeyUp(Keys.E)
                || curMouse.ScrollWheelValue < prevMouse.ScrollWheelValue) && selectedGun < guns.Count() - 1)
                selectedGun++;
            base.Update(); // Calls the inherited Unit's update
        }

        public void UpdateTimers(KeyboardState curKeyboard) // Updates stamina based on input state
        {
            if (stamina[0] < stamina[1] && !(curKeyboard.IsKeyDown(Keys.LeftShift) || curKeyboard.IsKeyDown(Keys.RightShift)))
                stamina[0]++;
            guns[selectedGun].UpdateTimers(); // Updates the timer on the selected gun
        }

        public void HandleMovement(KeyboardState curKeyboard)
        {
            int displacement = 2; // Sets the displacement the player normally moves at
            if ((curKeyboard.IsKeyDown(Keys.LeftShift) || curKeyboard.IsKeyDown(Keys.RightShift)) && stamina[0] != 0)
            { // Doubles the displacement if shift is pressed, as well as decrementing stamina
                displacement += 2;
                stamina[0]--;
            }

            // Moves the player based on direction
            if (curKeyboard.IsKeyDown(Keys.Up) || curKeyboard.IsKeyDown(Keys.W))
                this.Position.Y -= displacement;

            if (curKeyboard.IsKeyDown(Keys.Down) || curKeyboard.IsKeyDown(Keys.S))
                this.Position.Y += displacement;

            if (curKeyboard.IsKeyDown(Keys.Left) || curKeyboard.IsKeyDown(Keys.A))
                this.Position.X -= displacement;

            if (curKeyboard.IsKeyDown(Keys.Right) || curKeyboard.IsKeyDown(Keys.D))
                this.Position.X += displacement;

            // Ensures the player never goes off screen
            this.Position.X = MathHelper.Clamp(this.Position.X, 0, Game1.ViewPort.Width);
            this.Position.Y = MathHelper.Clamp(this.Position.Y, 0, Game1.ViewPort.Height);
        }

        public override void Draw(SpriteBatch spriteBatch) // Calls inherited Unit draw
        {
            base.Draw(spriteBatch);
            // Displays ammo, stamina, health amounts and bars to show percentage
            if (guns[selectedGun].reloadSpeed[0] != guns[selectedGun].reloadSpeed[1])
                spriteBatch.Draw(Game1.pixel, new Rectangle(0, 40, (int)(78 * guns[selectedGun].reloadSpeed[0] / guns[selectedGun].reloadSpeed[1]), 12), Color.Yellow);
            spriteBatch.Draw(Game1.pixel, new Rectangle(0, 4, (int)(78 * ((double)Health[0] / (double)Health[1])), 12), Color.Red);
            spriteBatch.Draw(Game1.pixel, new Rectangle(0, 4, 78, 12), Color.Red * 0.2f);
            spriteBatch.Draw(Game1.pixel, new Rectangle(0, 28, (int)(78 * ((double)guns[selectedGun].Clip[0] / (double)guns[selectedGun].Clip[1])), 12), Color.Yellow);
            spriteBatch.Draw(Game1.pixel, new Rectangle(0, 28, 78, 12), Color.Yellow * 0.2f);
            spriteBatch.Draw(Game1.pixel, new Rectangle(0, 16, (int)(78 * ((double)stamina[0] / (double)stamina[1])), 12), Color.LightBlue);
            spriteBatch.Draw(Game1.pixel, new Rectangle(0, 16, 78, 12), Color.LightBlue * 0.2f);
            spriteBatch.DrawString(Game1.font, "" + guns[selectedGun].Clip[0] + " / " + guns[selectedGun].Ammo, new Vector2(0, 24), Color.Black);
            spriteBatch.DrawString(Game1.font, "" + stamina[0] + " / " + stamina[1], new Vector2(0, 12), Color.Black);
            spriteBatch.DrawString(Game1.font, "" + Health[0] + " / " + Health[1], new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(Game1.font, "" + guns[selectedGun].GunName, new Vector2(80, 24), Color.Black);
        }
   
    }
}

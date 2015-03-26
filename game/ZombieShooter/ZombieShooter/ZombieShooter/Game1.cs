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

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
      
        Texture2D background;       
        public static Texture2D soldier; // Holds the player's texture
        public static Rectangle ViewPort; // The dimensions of the visible screen
        KeyboardState curKey, prevKey; // Input states for the keyboard
        MouseState curMouse, prevMouse; // Input states for the mouse
        public static Player player; // The Player
        public static Texture2D bullet; // Holds bullet texture
        public static List<Bullet> Bullets = new List<Bullet>(); // Holds every bullet on screen
        public static int salvage = 0; // Used to track score and soon to buy turrets
        double zombieSpawnChance = 0.01; // Chance to spawn a new zombie
        public static List<Texture2D> zombie = new List<Texture2D>(); // Contains the variety of zombie textures
        public static List<Zombie> Zombies = new List<Zombie>(); // Holds every zombie on screen
        public static Random randomNumber; // For randomization
        public static int score;
        public static SpriteFont font; // Holds font for displaying text
        public static Texture2D pixel; // A single pixel used for drawing rectangles
        public static List<Message> messages = new List<Message>(); // Holds messages
        public static Texture2D turret; // Texture of a turret
        public static List<Turret> Turrets = new List<Turret>(); // Holds every turret on screen
        public static int turretCost = 50; // Cost of placing a turret
		public static List<Powerup> powerUps = new List<Powerup>(); // Holds every powerup on screen
        double medKitDropChance = 0.001; // medkit and ammocrate drop chance
        double ammoDropChance = 0.001;
        public static Texture2D ammoBox; // Holds powerup textures
        public static Texture2D medKit;
        public static List<Texture2D> particles = new List<Texture2D>();
        public static List<AoETurret> AreaTurrets = new List<AoETurret>();
        public static Texture2D healTurret;
        public static GraphicsDevice graphicsDevice;       
        Texture2D t2dGameScreen;
    

        
	
		void spawnAmmo()
        { // Spawns ammo at random location on screen
            AmmoCrate newAmmo = new AmmoCrate();
            newAmmo.Position = new Vector2(randomNumber.Next(32, 618), randomNumber.Next(32, 618));
            powerUps.Add(newAmmo);
        }

        void spawnMedKits()
        { // Spawns medkit at random location on screen
            MedKit newMedKit = new MedKit();
            newMedKit.Position = new Vector2(randomNumber.Next(32, 618), randomNumber.Next(32, 618));
            powerUps.Add(newMedKit);
        }
       

        public static Player getNearestPlayer(Vector2 checkPosition) // Adds support for more players
        {
            return player;
        }

        public static Zombie getNearestZombie(Vector2 checkPosition, int[] range)
        {
            if (Zombies.Count() != 0)
            { // If there are zombies, sort zombies within range by distance to find closest zombie using linq
                var nearZombies = from Zombie zombie in Zombies
                                  where (Vector2.Distance(checkPosition, zombie.Position) > range[0]
                                  && (Vector2.Distance(checkPosition, zombie.Position) < range[1]))
                                  orderby (Vector2.Distance(checkPosition, zombie.Position))
                                  select zombie;

                if (nearZombies.Count() != 0)
                    return nearZombies.First();
            }
            return null;
        }
        public static Turret getNearestTurret(Vector2 checkPosition)
        {
            if (Turrets.Count() != 0)
            {
                Turret nearTurret = (Turret)(from Turret turret in Turrets
                                             orderby (Vector2.Distance(checkPosition, turret.Position))
                                             select turret).First();
                return nearTurret;
            }
            return null;
        }


        public bool IsEmpty(Vector2 spawnLocation) // Makes sure no turret is placed at the given location
        {
            foreach (Turret turret in Turrets)
                if (spawnLocation == turret.Position)
                    return false;
            foreach (AoETurret turret in AreaTurrets)
                if (spawnLocation == turret.Position)
                    return false;

            return true;
           
        }
		

        void checkSpawns()
        {
            if (randomNumber.NextDouble() < zombieSpawnChance) // If roll is within chance, spawn
                spawnZombies();
              // Checks powerup rolls
            if (randomNumber.NextDouble() < ammoDropChance)
                spawnAmmo();
            if (randomNumber.NextDouble() < medKitDropChance)
                spawnMedKits();
        }

        void spawnZombies() // Spawns zombie at random location, then adds to zombie list
        {
            Zombie newZombie = new Zombie();
            int positionType = randomNumber.Next(0, 4);
            if (positionType == 1)
                newZombie.Position = new Vector2(-32, randomNumber.Next(0, ViewPort.Height - 32));
            else if (positionType == 2)
                newZombie.Position = new Vector2(ViewPort.Width + 32, randomNumber.Next(0, ViewPort.Height - 32));
            else if (positionType == 3)
                newZombie.Position = new Vector2(randomNumber.Next(0, ViewPort.Width - 32), -32);
            else
                newZombie.Position = new Vector2(randomNumber.Next(0, ViewPort.Width - 32), ViewPort.Height + 32);
            Zombies.Add(newZombie);
        }

        void newGame() // Resets all game values
        {           
            player = new Player();            
            Zombies = new List<Zombie>();
            Bullets = new List<Bullet>();
            salvage = 0;
            zombieSpawnChance = 0.01;
            messages = new List<Message>(); // Resets message list
            Turrets = new List<Turret>(); // Resets the turret list
             powerUps = new List<Powerup>(); // Resets powerup list 
             AreaTurrets = new List<AoETurret>();
       }
 

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 640; // Sets window
            graphics.PreferredBackBufferWidth = 640; // dimensions
            graphics.IsFullScreen = false;            
            ViewPort = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            this.IsMouseVisible = true; // Makes the mouse visible, making navigation easier
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {           
            score = 0;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            t2dGameScreen = Content.Load<Texture2D>("GameOver");
          
          
            background = Content.Load<Texture2D>
                ("background");
            soldier = Content.Load<Texture2D>("soldier"); // Pulls the soldier.png texture from our content, assigning it to the Texture2D variable soldier

            player = new Player(); // Initializes the player
            bullet = Content.Load<Texture2D>("bullet"); // Pulls texture from content folder
            // Pulls zombie textures
            zombie.Add(Content.Load<Texture2D>(@"Zombies\zombie1"));
            zombie.Add(Content.Load<Texture2D>(@"Zombies\zombie2"));
            zombie.Add(Content.Load<Texture2D>(@"Zombies\zombie3"));
            zombie.Add(Content.Load<Texture2D>(@"Zombies\zombie4"));
            randomNumber = new Random();
            font = Content.Load<SpriteFont>("font"); // Pulls spritefont data into font variable
            pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color); // Initializes pixel as a single white pixel texture
            pixel.SetData(new[] { Color.White });
            turret = Content.Load<Texture2D>(@"Turrets\Turret"); // Pulls turret data from content
            ammoBox = Content.Load<Texture2D>(@"Power Ups\ammo-box"); // Pulls powerup texture from content
            medKit = Content.Load<Texture2D>(@"Power Ups\healthPack");
            particles.Add(Content.Load<Texture2D>("circle"));
            particles.Add(Content.Load<Texture2D>("diamond"));
            particles.Add(Content.Load<Texture2D>("star"));
            healTurret = Content.Load<Texture2D>(@"Turrets\HealingTurret");
            graphicsDevice = GraphicsDevice;


            
        }
    
        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            curKey = Keyboard.GetState(); // Gets the current status
            curMouse = Mouse.GetState(); // of the mouse and keyboard

            player.Update(curKey, prevKey, curMouse, prevMouse);
            for (int i = 0; i < Turrets.Count(); i++)
                Turrets[i].Update(); // Updates each turret
            if (curKey.IsKeyDown(Keys.Space) && prevKey.IsKeyUp(Keys.Space) && (salvage >= turretCost))
            {
                // If spawnLocation does not have a turret, place a turret there and subtract the turret cost from salvage
                Vector2 spawnLocation = new Vector2((int)(player.Position.X / 32) * 32 + 16, (int)(player.Position.Y / 32) * 32 + 16);

                if (IsEmpty(spawnLocation))
                {
                    Turret newTurret = new Turret();
                    newTurret.Position = spawnLocation;
                    Turrets.Add(newTurret);
                    salvage -= turretCost;
                }
            }

            prevKey = curKey; // Assigns the current values
            prevMouse = curMouse; // To the previous ones
            for (int i = 0; i < Bullets.Count(); i++)
                Bullets[i].Update(); // Updates each bullet on screen
            checkSpawns(); // Checks to see if new entity needs to spawn
            for (int i = 0; i < Zombies.Count(); i++)
            {
                Zombies[i].Update(); // Updates zombie

                for (int j = 0; j < Bullets.Count(); j++)
                {
                    if (Vector2.Distance(Bullets[j].Position, Zombies[i].Position) < 16)
                        if (Bullets[j].IntersectPixels(Zombies[i]))
                        {
                            Bullets[j].Deteriorate(5);
                            Zombies[i].Hurt(Bullets[j]);
                        }
                    if (Vector2.Distance(Bullets[j].Position, player.Position) < 16)
                        if (Bullets[j].IntersectPixels(player))
                        {
                            Bullets[j].Deteriorate(5);
                            player.Hurt(Bullets[j]);
                        }
                }
               
                

                if (Zombies[i].IsDead)
                { // If the zombie dies, increase salvage and spawn chance as well as remove the zombie
                    salvage += 5;
                    Zombies.RemoveAt(i);
                    score++;
                    zombieSpawnChance += 0.0005;
                }
                

            }

        
                
            for (int i = 0; i < messages.Count(); i++)
                messages[i].Update(gameTime); // Updates
for (int i = 0; i < powerUps.Count(); i++)
            { // Checks each powerup for player collision, in which case activate pickup
                powerUps[i].UpdateTransform();
                    if (Vector2.Distance(player.Position, powerUps[i].Position) < 32)
                        if (powerUps[i].IntersectPixels(player))
                            powerUps[i].Pickup(player);
            }
            for (int i = 0; i < AreaTurrets.Count(); i++)
            {
                AreaTurrets[i].Update();
                if (AreaTurrets[i].isColliding(player.Position, 16))
                    AreaTurrets[i].Effect(player);
            }

            if (curKey.IsKeyDown(Keys.Z) && prevKey.IsKeyUp(Keys.Z) && (salvage >= turretCost))
            {
                Vector2 spawnLocation = new Vector2((int)(player.Position.X / 32) * 32 + 16, (int)(player.Position.Y / 32) * 32 + 16);

                if (IsEmpty(spawnLocation))
                {
                    HealTurret newTurret = new HealTurret();
                    newTurret.Position = spawnLocation;
                    AreaTurrets.Add(newTurret);
                    salvage -= turretCost;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White); // Clears the screen, preparing for new draws
            spriteBatch.Begin(); // Begins the spritebatch                
            base.Draw(gameTime);
            spriteBatch.Draw(background, ViewPort, Color.White);
            player.Draw(spriteBatch); // Draws player
            foreach (Sprite bullet in Bullets)
                bullet.Draw(spriteBatch, Color.White); // Draws each bullet
            foreach (Zombie zombie in Zombies)
                zombie.Draw(spriteBatch); // Draws all zombies
            spriteBatch.DrawString(font, "Salvage: " + salvage, new Vector2(0, 48), Color.Red); // Displays amount of texture
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(0, 35), Color.Red);
            foreach (Message message in messages)
                message.Draw(spriteBatch); // Draws
            foreach (Turret turret in Turrets)
                turret.Draw(spriteBatch); // Draws every turret
            foreach (Powerup powerUp in powerUps)
                powerUp.Draw(spriteBatch); // Draw each powerup
            foreach (AoETurret turret in AreaTurrets)
                turret.Draw(spriteBatch);
            if (player.IsDead)
            {
                zombieSpawnChance = 0;
                Zombies.Clear();
                
                
                spriteBatch.Draw(t2dGameScreen, new Rectangle(0, 0, 640, 640), Color.White);
                spriteBatch.DrawString(font, "Score: " + score, new Vector2(300, 300), Color.Black);
            }
                
                
            spriteBatch.End(); // Ends spritebatch
            base.Draw(gameTime);
        }
    }
}

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KKMono1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameBubbles : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D imageKK;

        public GameBubbles()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.HardwareModeSwitch = false;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            Window.Title = "Bubbles";
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            var path = @"Images\Ball";
            imageKK = Content.Load<Texture2D>(path);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private KeyboardState keysLast = new KeyboardState();

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //count++;

            var keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.LeftAlt) && keys.IsKeyDown(Keys.Enter) && keysLast.IsKeyUp(Keys.Enter))
                graphics.ToggleFullScreen();
            keysLast = keys;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private int count = 0;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            count++;

            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            /*spriteBatch.Draw(imageKK, Vector2.Zero, Color.White);
            spriteBatch.Draw(imageKK, position: new Vector2(500, 0), effects: SpriteEffects.FlipHorizontally);
            spriteBatch.Draw(imageKK, position: new Vector2(500, 0), rotation: 1, scale: new Vector2(0.1f, 0.1f));*/

            var randpos = new Random(0);
            for (int i = 0; i < 10000; i++)
            {
                var color = new Color((float)randpos.NextDouble(), (float)randpos.NextDouble(), (float)randpos.NextDouble());
                var position = new Vector2((float)randpos.NextDouble() * 1920, (float)randpos.NextDouble() * 1080);
                position += new Vector2((float)(20 * Math.Cos((count + randpos.NextDouble() * 1000) / 15f)), 
                    (float)(20 * Math.Sin((count + randpos.NextDouble() * 1000) / 25f)));
                var size = (float)(0.8 + 0.4 * randpos.NextDouble());
                var rotation = (randpos.NextDouble() + (double)count / 100) * 2 * Math.PI;

                //spriteBatch.Draw(imageKK, position: position, rotation: (float)rotation, scale: new Vector2(size, size), origin: new Vector2(32, 32), color: color);
                spriteBatch.Draw(imageKK, position: position, rotation: (float)rotation, scale: new Vector2(size, size), origin: new Vector2(32, 32), color: new Color(color, 0.5f));
            }

            spriteBatch.End();

            // 3D
            //graphics.GraphicsDevice.DrawUserPrimitives

            base.Draw(gameTime);
        }
    }
}

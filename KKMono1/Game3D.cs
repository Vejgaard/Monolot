using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KKMono1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game3D : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        BasicEffect effect;

        Texture2D imageKK;
        DrawableModel _drawableKK;
        
        Texture2D imageBox;
        DrawableModel _drawableBox;

        Texture2D imageBall;
        DrawableModel _drawableBall;

        public Game3D()
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
            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;

            // TODO: Add your initialization logic here

            effect = new BasicEffect(GraphicsDevice);
            effect.EnableDefaultLighting();
            effect.PreferPerPixelLighting = true;

            Window.Title = "Pila 2 (Alt + Enter for full screen)";

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
            imageKK = Content.Load<Texture2D>(@"Images\KK");
            imageBox = Content.Load<Texture2D>(@"Images\BoxSides");
            imageBall = Content.Load<Texture2D>(@"Images\football");

            var model = new KModel();
            model.TexCoordsAreInPixels = false;
            for (int i = 3; i < 100; i = i+2)
            {
                var z = -i / 2;
                model.AddQuad(
                    model.AddVertex(new Vector3(-i, -i, z)),
                    model.AddVertex(new Vector3(i, -i, z)),
                    model.AddVertex(new Vector3(-i, i, z)),
                    model.AddVertex(new Vector3(i, i, z)),
                    new Rectangle(0, 0, 1, 1));
            }
            _drawableKK = new DrawableModel(model, imageKK, GraphicsDevice);

            model = new KModel();
            model.AddBox(new BoundingBox(new Vector3(-5, -5, 1), new Vector3(-3, -3, 3)), new Rectangle(0, 0, 64, 64));
            model.AddBox(new BoundingBox(new Vector3(-5, 3, 1), new Vector3(-3, 5, 3)), new Rectangle(0, 64, 64, 64));
            model.AddBox(new BoundingBox(new Vector3(3, -5, 1), new Vector3(5, -3, 3)), new Rectangle(0, 64, 64, 64));
            model.AddBox(new BoundingBox(new Vector3(3, 3, 1), new Vector3(5, 5, 3)), new Rectangle(0, 0, 64, 64));
            _drawableBox = new DrawableModel(model, imageBox, GraphicsDevice);

            model = GeometricalPrimitives.CreateBall(2, new Rectangle(0, 0, 1, 1));
            model.TexCoordsAreInPixels = false;
/*            model.Transform(Matrix.CreateTranslation(1, 0, 0));
            var model2 = GeometricalPrimitives.CreateBall(3, new Rectangle(0, 0, 1, 1));
            model2.Transform(Matrix.CreateScale(0.5f));
            model2.Transform(Matrix.CreateTranslation(-1, 0, 0));
            model.Append(model2);*/
            _drawableBall = new DrawableModel(model, imageBall, GraphicsDevice);

            // Matrix test
            var mat = Matrix.CreateTranslation(1, 2, 3);
            Console.WriteLine(mat);
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

        private int count = 0;

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

            base.Update(gameTime);
        }

        public Ball _ball1 = new Ball { Center = new Vector3(2, 0, 0), Radius = 1, Orientation = Matrix.Identity };
        public Ball _ball2 = new Ball { Center = new Vector3(-2, 0, 0), Radius = 1, Orientation = Matrix.Identity };

        void DrawStuff()
        {
            _ball1.Orientation = Matrix.CreateFromAxisAngle(new Vector3(2, 1, 0).Normalized(), count / 50f);
            _ball2.Radius = 1 + 0.3f * (float)Math.Sin(count / 10f);


            // The assignment of effect.View and effect.Projection
            // are nearly identical to the code in the Model drawing code.
            var cameraPosition = new Vector3((float)(6 * Math.Cos(count / 30f)), (float)(6 * Math.Sin(count / 37f)), 10);
            var cameraLookAtVector = Vector3.Zero;
            var cameraUpVector = Vector3.UnitY;

            effect.View = Matrix.CreateLookAt(
                cameraPosition, cameraLookAtVector, cameraUpVector);

            //float aspect = graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height;

            float aspect =
                graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
            float nearClipPlane = 1;
            float farClipPlane = 200;

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspect, nearClipPlane, farClipPlane);



            effect.World = Matrix.Identity;

            // Draw!
            _drawableKK.Draw(effect);
            _drawableBox.Draw(effect);

            effect.World = _ball1.WorldTransform();
            _drawableBall.Draw(effect);

            effect.World = _ball2.WorldTransform();
            _drawableBall.Draw(effect);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            count++;

            GraphicsDevice.Clear(Color.Black);

            DrawStuff();

            base.Draw(gameTime);
        }
    }
}

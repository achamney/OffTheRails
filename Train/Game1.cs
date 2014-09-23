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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Train
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        VertexDeclaration vd;
        Matrix view;
        Matrix perspective;
        public static Random rand;
        int curtex;
        Vector3 camPos;
        Crosshair cross;
        MainTrain mainTrain;
        SpriteFont mainFont;
        SpriteFont titleFont;
        SpriteFont largeFont;
        FlatPlane ground;
        Camera camera;
        TrackSet trackSet;
        int secondsPassed;
        int secondsPassedTillControlsDissapear = 15;
        public static SoundEffect[] sfx;
        public static SoundEffectInstance[] sfxLoop;
        Song bgSong;
        public static Cube helper;


        //STUFFFF
        public static Effect myeffect;
        public static Texture2D[] texlibrary;
        Texture2D perlintex;

        EffectParameter shadertex;
        public static EffectParameter lworld;
        EffectParameter lview;
        EffectParameter time;
        EffectParameter persp;
        EffectParameter pnoise;
        EffectParameter renderParticles;
        EffectParameter light;
        Vector3[] lightpos = new Vector3[2];
        EffectParameter lIntensity;
        public static float[] lightStrength = new float[2];
        EffectParameter eye;
        EffectParameter power;
        //stuffffff

        //MOOORE STUFFFFF
        //ParticleSystem particleSystem;
        //MORREE STUFFFFF

        public Game1()
        {
            rand = new Random();
            graphics = new GraphicsDeviceManager(this);
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
            camPos.Z = 10;
            view = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.Up);
            perspective = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4,
                graphics.GraphicsDevice.Viewport.AspectRatio, 0.5f, 10000000);
            //be = new BasicEffect(graphics.GraphicsDevice, null);
            //be.View = view;
            //be.Projection = perspective;
            graphics.GraphicsDevice.RenderState.CullMode = CullMode.None;
            Projectile.initProj();
            EnemySet.initEnemies();
            base.Initialize();
            cross = new Crosshair(new Vector3(0, 0 , -20));
            vd = new VertexDeclaration(graphics.GraphicsDevice, VertexPositionNormalTexture.VertexElements);
            
            this.graphics.IsFullScreen = true;
            graphics.ApplyChanges(); 
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Weapon.loadModels(Content.Load<Model>("Models/gatGun"));
            mainTrain = new MainTrain(new Vector3(1.00f, -0.75f, 3.5f));
            camera = new Camera(new Vector3(0, 0, 10), new Vector3(0, 0, 0),mainTrain.position);
            ground = new FlatPlane(new Vector3(0,-5,-10),new Vector3(900,1,500));
            mainFont = Content.Load<SpriteFont>("mainFont");
            titleFont = Content.Load<SpriteFont>("TitleFont");
            largeFont = Content.Load<SpriteFont>("Large");
            trackSet = new TrackSet(mainTrain,Content.Load<Model>("Models/cactus"));
            helper = new Cube(new Vector3());

            Particle.setupVerticies(Content.Load<Texture2D>("Textures/smoke"),Content.Load<Texture2D>("Textures/spark"));
            
            //stuff
            texlibrary = new Texture2D[10];
            texlibrary[0] = Content.Load<Texture2D>("Textures/Metal");
            texlibrary[1] = Content.Load<Texture2D>("Textures/darkmetal");
            texlibrary[2] = Content.Load<Texture2D>("Textures/metal2");
            texlibrary[3] = Content.Load<Texture2D>("Textures/grass");
            texlibrary[4] = Content.Load<Texture2D>("Textures/woodtext");
            texlibrary[5] = Content.Load<Texture2D>("Textures/smoke");
            texlibrary[6] = Content.Load<Texture2D>("Textures/grass2");
            texlibrary[7] = Content.Load<Texture2D>("Textures/fish");
            texlibrary[8] = Content.Load<Texture2D>("Textures/hpbar");
            texlibrary[9] = Content.Load<Texture2D>("Textures/energybar");
            perlintex = Content.Load<Texture2D>("Textures/metal2");

            myeffect = Content.Load<Effect>("lights");
            lworld = myeffect.Parameters["World"];
            lview = myeffect.Parameters["View"];
            persp = myeffect.Parameters["Projection"];
            time = myeffect.Parameters["time"];
            shadertex = myeffect.Parameters["face"];
            pnoise = myeffect.Parameters["pnoise"];
            power = myeffect.Parameters["power"];
            renderParticles = myeffect.Parameters["renderParticles"];
            power.SetValue(1);

            pnoise.SetValue(perlintex);

            // illumination:
            light = myeffect.Parameters["light"];
            lIntensity = myeffect.Parameters["lightPower"];
            eye = myeffect.Parameters["eye"];   

            Matrix temp;
            Vector3 eyepos = new Vector3(0, 0, -20);
            
            lightpos[0] = new Vector3(5, 5, -2000);
            lightpos[1] = new Vector3(5, 5, -2000);
            lightStrength[0] = 0f;
            lightStrength[1] = 1;
            temp = Matrix.CreateLookAt(eyepos, new Vector3(0, 0, 0), new Vector3(0, 1, 0));

            eye.SetValue(eyepos);
            lview.SetValue(temp);

            light.SetValue(lightpos);
            lIntensity.SetValue(lightStrength);
            temp = Matrix.Identity;

            lworld.SetValue(temp);

            //            temp = Matrix.CreateOrthographic(40, 30, 0.1f, 600);
            temp = Matrix.CreatePerspectiveFieldOfView(1, 1.33f, 0.1f, 1000);
            persp.SetValue(temp);
            myeffect.CommitChanges();

            bgSong = Content.Load<Song>("SFX/Corneria");
            MediaPlayer.Play(bgSong);
            MediaPlayer.Volume = 0.25f;
            sfx = new SoundEffect[7];
            sfx[0] = Content.Load<SoundEffect>("SFX/mgloop2");
            sfx[1] = Content.Load<SoundEffect>("SFX/mgwind");
            sfx[2] = Content.Load<SoundEffect>("SFX/cactusExplode");
            sfx[3] = Content.Load<SoundEffect>("SFX/explode3");
            sfx[4] = Content.Load<SoundEffect>("SFX/hydraulics");
            sfx[5] = Content.Load<SoundEffect>("SFX/ping");
            sfx[6] = Content.Load<SoundEffect>("SFX/booster");
            sfxLoop = new SoundEffectInstance[7];
            sfxLoop[1] = sfx[1].CreateInstance();
            sfxLoop[6] = sfx[6].CreateInstance();
            //sfxLoop[6].Volume = 0.25f;
            //sfxLoop[1].Volume = 2;
            Game1.sfxLoop[1].IsLooped = true;
            
            //stuff
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            ////Smoke Stack
            Particle.MakeParticle(mainTrain.position - mainTrain.offset + new Vector3(0, 1, 0) ,
                new Vector3((float)rand.NextDouble() * 0.05f - 0.025f, (float)rand.NextDouble() * 0.4f, (float)rand.NextDouble() * 0.05f - 0.025f) +
                mainTrain.getVelocity() / 4,
                new Vector3(0, 0.004f, 0),
                Particle.Texts.SMOKE,
                100,
                1.3f,
                1.01f,
                0.5f);
            ///Sun
            Particle.MakeParticle(new Vector3(100, 150, -600),
                new Vector3((float)rand.NextDouble() * 0.05f - 0.025f, (float)rand.NextDouble() * 0.04f, (float)rand.NextDouble() * 0.05f - 0.025f),
                new Vector3(),
                Particle.Texts.SPARK,
                40, 30,1.01f,0.5f);
            // gameTime.IsRunningSlowly;
            KeyboardState kbs = Keyboard.GetState();
            if (kbs.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            if(kbs.IsKeyDown(Keys.Enter))
            {
                resetGame(gameTime);
            }
            mainTrain.handleInput(kbs);
            handleMouse(Mouse.GetState());
            trackSet.update(mainTrain);
            //specVal.SetValue(mainTrain.specVal);
   
            camera.update(mainTrain.position, mainTrain.getRotation().Y ,mainTrain.getScale(),mainTrain.getVelocity(),mainTrain.getDirection());
    
            
            camera.handleInput(kbs);
            cross.position.Z = -10;
            lview.SetValue( Matrix.CreateLookAt(camera.getPosition(), camera.getLookAt(), Vector3.Up));
            myeffect.CommitChanges();
            if (camera.getState() == Camera.States.FOLLOWTARGET)
            {
                mainTrain.setActive(true);
            }
            else
            {
                mainTrain.setActive(false);
            }
            if (kbs.IsKeyDown(Keys.D4))
            {
                curtex = (curtex + 1) % 4;
                shadertex.SetValue(texlibrary[curtex]);
            }
            //particleSystem.Update(gameTime);
            EnemySet.updateEnemies(mainTrain);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            DrawModels(gameTime);

            
            //particleSystem.setView(lview.GetValueMatrix());
           // particleSystem.setWorld(mainTrain.world);
            //particleSystem.Draw(GraphicsDevice);
            shadertex.SetValue(texlibrary[5]);
            graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;
            renderParticles.SetValue(true);
            Particle.DrawParticles(myeffect,camera, GraphicsDevice);
            renderParticles.SetValue(false);
            graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
            DrawHud(gameTime);
            base.Draw(gameTime);
        }
        protected void DrawModels(GameTime gameTime)
        {
           // be.Begin();
            //be.VertexColorEnabled = true;
           
            graphics.GraphicsDevice.VertexDeclaration = vd;
            graphics.GraphicsDevice.RenderState.CullMode = CullMode.None; // no backface culling
            graphics.GraphicsDevice.RenderState.DepthBufferEnable = true;
            
            graphics.GraphicsDevice.RenderState.DepthBufferFunction = CompareFunction.LessEqual;
            graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
           
           // light.SetValue(train.getPosition());
            myeffect.CommitChanges();
            myeffect.Begin();
            myeffect.Techniques[0].Passes[0].Begin();
            lightpos = new Vector3[2];
            lightpos[1] = new Vector3(300, 1000, -2000);
           // lightpos[0] = new Vector3(mainTrain.position.X, 0, mainTrain.position.Z) /*+ new Vector3((float)Math.Cos(mainTrain.getRotation().Y), 0, (float)Math.Sin(mainTrain.getRotation().Y))*50*/ ;
            lightpos[0] = new Vector3(helper.position.X, helper.position.Y, helper.position.Z);
            myeffect.Parameters["light"].SetValue(lightpos);
            //helper.position = mainTrain.getCurTrack().getPosition();
            //power.SetValue(10);
            //helper.Draw(graphics.GraphicsDevice);
            //power.SetValue(1);
            shadertex.SetValue(texlibrary[2]);
            cross.Draw(graphics.GraphicsDevice, mainTrain.getWorld());
            shadertex.SetValue(texlibrary[3]);
            ground.Draw(graphics.GraphicsDevice);
            shadertex.SetValue(texlibrary[0]);
            trackSet.Draw(graphics.GraphicsDevice,camera.getPosition());
            EnemySet.DrawEnemies(graphics.GraphicsDevice);
            ///Draw non primitive models (ruins VD)///
            mainTrain.Draw(graphics.GraphicsDevice);
            trackSet.drawModels();
            ///Draw non primitive models (ruins VD)///
            myeffect.Techniques[0].Passes[0].End();
            myeffect.End();
  
        }
        protected void DrawHud(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(mainFont, "Health: ", new Vector2(20, 20), Color.White);
            spriteBatch.DrawString(mainFont, "Power: ", new Vector2(20, 40), Color.White);
            float hpRatio = (float)mainTrain.getHealth() / (float)mainTrain.getMaxHealth();
            float energyRatio = (float)mainTrain.getPower()/ (float)mainTrain.getMaxPower();
            spriteBatch.Draw(getTexture("hpbar"), new Rectangle(120, 24,(int)( 600.0f * (hpRatio)), 20),
                new Rectangle(0, 0, (int)(400.0f * hpRatio), 100), Color.White);
            spriteBatch.Draw(getTexture("energybar"), new Rectangle(120, 44, (int)(600.0f * (energyRatio)), 20),
                new Rectangle(0, 0, (int)(400.0f * energyRatio), 100), Color.White);
            spriteBatch.DrawString(largeFont, "Score: " + mainTrain.getScore(), new Vector2(600, 550), Color.Black);
            spriteBatch.DrawString(largeFont, "Score: " + mainTrain.getScore(), new Vector2(598, 548), Color.White);
            
            //spriteBatch.DrawString(mainFont, "" + (new Quaternion(new Vector3(1, 2, 3) * (float)Math.Sin(Math.PI / 8), (float)Math.Cos(Math.PI / 8)) * new Quaternion(new Vector3(-1, -1, 1) * (float)Math.Sin(Math.PI / 12), (float)Math.Cos(Math.PI / 12))), new Vector2(20, 30), Color.White);
            if (camera.getState() == Camera.States.CIRCLETARGET)
            {
                spriteBatch.DrawString(titleFont, "~Off The Rails~", new Vector2(202-80, 202), Color.BlanchedAlmond);
                spriteBatch.DrawString(titleFont, "~Off The Rails~", new Vector2(200-80, 200), Color.Red);
            }
            if ((int)gameTime.TotalGameTime.TotalSeconds != secondsPassed)
                secondsPassed = (int)gameTime.TotalGameTime.TotalSeconds;
            if (secondsPassed < secondsPassedTillControlsDissapear)
            {
                drawControls();
            }
            if (mainTrain.getCurTrack().isEndTrack() && mainTrain.getHealth() >0)
            {
                spriteBatch.DrawString(titleFont, "~Level Complete~", new Vector2(202 - 100, 202), Color.BlanchedAlmond);
                spriteBatch.DrawString(titleFont, "~Level Complete~", new Vector2(200 - 100, 200), Color.Red);
                spriteBatch.DrawString(largeFont, "Press Enter to Reset", new Vector2(202 - 160, 202 + 100), Color.BlanchedAlmond);
                spriteBatch.DrawString(largeFont, "Press Enter to Reset", new Vector2(200 - 160, 200 + 100), Color.Blue);
            }
            if (mainTrain.getHealth() <= 0)
            {
                spriteBatch.DrawString(titleFont, "~Game Over~", new Vector2(202 - 50, 202), Color.BlanchedAlmond);
                spriteBatch.DrawString(titleFont, "~Game Over~", new Vector2(200 - 50, 200), Color.Red);
                spriteBatch.DrawString(largeFont, "Press Enter to Reset", new Vector2(202 - 160, 202 + 100), Color.BlanchedAlmond);
                spriteBatch.DrawString(largeFont, "Press Enter to Reset", new Vector2(200 - 160, 200 + 100), Color.Blue);
            }
          
            spriteBatch.End();
        
            
            // TODO: Add your drawing code here
        }
        public void handleMouse(MouseState ms)
        {
            float mposx = ms.X;
            float mposy = ms.Y;
            cross.position.X = mposx / 15 -20;
            cross.position.Y = -mposy / 15 + 20;
            mainTrain.handleMouse(ms, cross.position);
        }
        public static Texture2D getTexture(String name)
        {
            if (name.Equals("darkmetal"))
                return texlibrary[1];
            if (name.Equals("fish"))
                return texlibrary[7];
            if (name.Equals("grass"))
                return texlibrary[3];
            if (name.Equals("grass2"))
                return texlibrary[6];
            if (name.Equals("Metal"))
                return texlibrary[0];
            if (name.Equals("metal2"))
                return texlibrary[2];
            if (name.Equals("woodtext"))
                return texlibrary[4];
            if (name.Equals("hpbar"))
                return texlibrary[8];
            if (name.Equals("energybar"))
                return texlibrary[9];
            return null;
        }
        private void drawControls()
        {
            spriteBatch.DrawString(largeFont, "Controls:", new Vector2(202 - 160, 202 + 100), Color.BlanchedAlmond);
            spriteBatch.DrawString(largeFont, "Controls:", new Vector2(200 - 160, 200 + 100), Color.Blue);

            spriteBatch.DrawString(largeFont, "Mouse: Aim and Shoot", new Vector2(202 - 160, 202 + 140), Color.BlanchedAlmond);
            spriteBatch.DrawString(largeFont, "Mouse: Aim and Shoot", new Vector2(200 - 160, 200 + 140), Color.Red);
            spriteBatch.DrawString(largeFont, "'W': Booster", new Vector2(202 - 160, 202 + 175), Color.BlanchedAlmond);
            spriteBatch.DrawString(largeFont, "'W': Booster", new Vector2(200 - 160, 200 + 175), Color.Red);
            spriteBatch.DrawString(largeFont, "'S': Break", new Vector2(202 - 160, 202 + 210), Color.BlanchedAlmond);
            spriteBatch.DrawString(largeFont, "'S': Break", new Vector2(200 - 160, 200 + 210), Color.Red);
            spriteBatch.DrawString(largeFont, "Space Bar: Jump", new Vector2(202 - 160, 202 + 245), Color.BlanchedAlmond);
            spriteBatch.DrawString(largeFont, "Space Bar: Jump", new Vector2(200 - 160, 200 + 245), Color.Red);
        }
        public void resetGame(GameTime gt)
        {
            camPos.Z = 10;
            Projectile.initProj();
            EnemySet.initEnemies();
            base.Initialize();
            cross = new Crosshair(new Vector3(0, 0, -20));
            mainTrain = new MainTrain(new Vector3(1.00f, -0.75f, 3.5f));
            camera = new Camera(new Vector3(0, 0, 10), new Vector3(0, 0, 0), mainTrain.position);
            ground = new FlatPlane(new Vector3(0, -5, -10), new Vector3(900, 1, 500));
            trackSet = new TrackSet(mainTrain,Content.Load<Model>("Models/cactus"));
            secondsPassed = (int)gt.TotalGameTime.TotalSeconds;
            secondsPassedTillControlsDissapear = secondsPassed + 15;
        }
    }
}

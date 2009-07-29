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

namespace BoomBoom
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        AudioEngine audioEngine;
        SoundBank soundBank;
        WaveBank waveBank;

        enum KEYS
        {
            SNARE,
            KICK,
            HIHAT_CLOSED,
            HIHAT_OPEN,
            HIHAT_STEPPED,
            HI_TOM,
            MID_TOM,
            LO_TOM,
            CRASH_1,
            CRASH_2,
            CROSS_STICK,

            COUNT
        }

        bool[] m_KeysDown = new bool[(int)KEYS.COUNT];
        Metronome.Zone[] m_KeysInZone = new Metronome.Zone[(int)KEYS.COUNT];
        Keys[] m_Keys = new Keys[(int)KEYS.COUNT];
        string[] m_SoundNames = new string[(int)KEYS.COUNT];

        SpriteFont Font1;
        Vector2 FontPos;

        Metronome m_Metronome;
        bool m_EngageSampleTime = false;
        bool m_SamplerKeyDown = false;
        List<int> m_Samples = new List<int>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            for (int i = 0; i < (int)KEYS.COUNT; i++)
            {
                m_KeysDown[i] = false;
                m_KeysInZone[i] = Metronome.Zone.COUNT;
               
            }
            m_Keys[(int)KEYS.KICK] = Keys.X;
            m_Keys[(int)KEYS.SNARE] = Keys.Z;
            m_Keys[(int)KEYS.HIHAT_CLOSED] = Keys.M;
            m_Keys[(int)KEYS.HIHAT_OPEN] = Keys.K;
            m_Keys[(int)KEYS.HIHAT_STEPPED] = Keys.O;

            m_Keys[(int)KEYS.HI_TOM] = Keys.S;
            m_Keys[(int)KEYS.MID_TOM] = Keys.D;
            m_Keys[(int)KEYS.LO_TOM] = Keys.C;
            
            m_Keys[(int)KEYS.CRASH_1] = Keys.Q;
            m_Keys[(int)KEYS.CRASH_2] = Keys.W;
            m_Keys[(int)KEYS.CROSS_STICK] = Keys.Space;


            m_SoundNames[(int)KEYS.KICK] = "Ambient Kick Hard";
            m_SoundNames[(int)KEYS.SNARE] = "Ambient Snare Hard";
            m_SoundNames[(int)KEYS.HIHAT_CLOSED] = "Ambient Hi-Hat Closed Hard";
            m_SoundNames[(int)KEYS.HIHAT_OPEN] = "Ambient Hi-Hat Open Hard";
            m_SoundNames[(int)KEYS.HIHAT_STEPPED] = "Ambient Hi-Hat Stepped Hard";

            m_SoundNames[(int)KEYS.HI_TOM] = "Ambient Hi-Tom Hard";
            m_SoundNames[(int)KEYS.MID_TOM] = "Ambient Mid-Tom Hard";
            m_SoundNames[(int)KEYS.LO_TOM] = "Ambient Lo-Tom Hard";

            m_SoundNames[(int)KEYS.CRASH_1] = "Ambient Crash 1 Hard";
            m_SoundNames[(int)KEYS.CRASH_2] = "Ambient Crash 2 Hard";
            m_SoundNames[(int)KEYS.CROSS_STICK] = "Ambient Cross Stick Hard";
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
            m_Metronome = new Metronome();
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
            audioEngine = new AudioEngine("Content\\Audio\\Soundage.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Audio\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Audio\\Sound Bank.xsb");

            Font1 = Content.Load<SpriteFont>("Courier New");
            FontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2,
                                  20.0f/*graphics.GraphicsDevice.Viewport.Height / 2*/);

            m_Metronome.Init(this.Content, Font1);
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            audioEngine.Update();

            if (!m_EngageSampleTime && Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                m_Samples.Clear();
                m_EngageSampleTime = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.LeftShift))
            {
                if (m_EngageSampleTime && m_Samples.Count >= 2)
                {
                    m_Metronome.SetRate(m_Samples, false);
                }
                m_EngageSampleTime = false;
            }

            for (int i = 0; i < (int)KEYS.COUNT; i++)
            {
                if (Keyboard.GetState().IsKeyDown(m_Keys[i]) && !m_KeysDown[i])
                {
                    m_KeysDown[i] = true;
                    soundBank.PlayCue(m_SoundNames[i]);
                    m_KeysInZone[i] = m_Metronome.InZone;

                    if (m_EngageSampleTime)
                    {
                        if (!m_SamplerKeyDown)
                        {
                            m_SamplerKeyDown = true;
                            m_Samples.Add((int)gameTime.TotalGameTime.TotalMilliseconds);
                            Console.WriteLine("Sample (" + (m_Samples.Count - 1).ToString() + ") " + m_Samples[m_Samples.Count - 1].ToString());
                        }
                    }
                    
                }
                if (Keyboard.GetState().IsKeyUp(m_Keys[i]))
                {
                    m_KeysDown[i] = false;
                    m_KeysInZone[i] = Metronome.Zone.SLOW;

                    m_SamplerKeyDown = false;
                }
            }

            /*if (m_EngageSampleTime)
            {
                if (!m_SamplerKeyDown && Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    m_SamplerKeyDown = true;
                    m_Samples.Add((int)gameTime.TotalGameTime.TotalMilliseconds);
                    Console.WriteLine("Sample (" + (m_Samples.Count - 1).ToString() + ") " + m_Samples[m_Samples.Count - 1].ToString());
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
            {
                m_SamplerKeyDown = false;
            }*/


            m_Metronome.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            string seperator;


            // Draw the string
            for (int i = 0; i < (int)KEYS.COUNT; i++)
            {

                Color colour;
                if (m_KeysDown[i])
                {
                    switch (m_KeysInZone[i])
                    {
                        case Metronome.Zone.IN:
                            colour = Color.Yellow;
                            seperator = "  !!  ";
                            break;
                        case Metronome.Zone.FAST:
                            colour = Color.Red;
                            seperator = "  /\\  ";
                            break;
                        case Metronome.Zone.SLOW:
                            colour = Color.Blue;
                            seperator = "  \\/  ";
                            break;
                        case Metronome.Zone.MIDDLE:
                            colour = Color.Orange;
                            seperator = "  ||  ";
                            break;
                        default:
                            colour = Color.White;
                            seperator = "  --  ";
                            break;
                    }
                }
                else
                {
                    colour = Color.White;
                    seperator = "  --  ";
                }

                // Draw Hello World
                string output = m_Keys[i].ToString() + seperator + m_SoundNames[i];

                // Find the center of the string
                Vector2 FontOrigin = Font1.MeasureString(m_Keys[i].ToString()); //; Font1.MeasureString(output) / 2;

                Vector2 pos = FontPos;
                pos.Y += 25.0f * i;
                spriteBatch.DrawString(Font1, output, pos, colour,
                                       0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            }

            m_Metronome.Draw(spriteBatch);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}

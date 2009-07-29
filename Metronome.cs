using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BoomBoom
{
    class Metronome
    {
        private TimeSpan m_AccumTime;
        private bool m_MovingRight = true;
        private Sprite m_Arm;
        private SpriteFont m_Font;
        private Vector2 m_Position;

        public enum Zone
        {
            IN,
            FAST,
            SLOW,
            MIDDLE,
            COUNT
        }
        
        private int m_MaxMilliSec = 1000;
        public int MaxMilliSec
        {
            get { return m_MaxMilliSec; }
            set { m_MaxMilliSec = value; }
        }

        private Zone m_InZone = Zone.COUNT;
        public Zone InZone
        {
            get { return m_InZone; }
        }

        public void Init(ContentManager content, SpriteFont font)
        {
            m_Position = new Vector2(400.0f, 550.0f);

            m_Arm = new Sprite();
            m_Arm.Init(content, "Sprites\\Needle");
            m_Arm.Position = m_Position;
            m_Arm.CentrePoint = new Vector2(m_Arm.Size.X / 2, 0.0f);

            m_Font = font;

        }

        public void SetRate(List<int> samples, bool doublespeed)
        {
            if (samples.Count <= 1)
            {
                // fail
                m_MaxMilliSec = 1000;
                return;
            }

            int[] differences = new int[samples.Count - 1];
            for (int i = 0; i < differences.Length; ++i)
            {
                differences[i] = samples[i + 1] - samples[i];
            }

            int total = 0;
            for (int i = 0; i < differences.Length; ++i)
                total += differences[i];

            
            m_MaxMilliSec = total / differences.Length;

            if (doublespeed)
                m_MaxMilliSec /= 2;
        }

        public void Update(GameTime gameTime)
        {
            m_AccumTime += gameTime.ElapsedGameTime;

            if (m_AccumTime.TotalMilliseconds >= m_MaxMilliSec)
            {
                m_MovingRight = !m_MovingRight;
                m_AccumTime = TimeSpan.Zero;
            }

            if (m_AccumTime.TotalMilliseconds > m_MaxMilliSec - (m_MaxMilliSec * 0.1f))
            {
                m_Arm.Colour = Color.Lerp(Color.DarkRed, Color.Red, (float)(m_AccumTime.TotalMilliseconds - (m_MaxMilliSec * 0.9f)) / (m_MaxMilliSec * 0.1f));
                m_InZone = Zone.IN;
            }
            else if (m_AccumTime.TotalMilliseconds < (m_MaxMilliSec * 0.1f))
            {
                m_Arm.Colour = Color.Lerp(Color.Red, Color.DarkRed, (float)(m_MaxMilliSec * 0.1f));
                m_InZone = Zone.IN;
            }
            else
            {
                if (m_AccumTime.TotalMilliseconds < m_MaxMilliSec * 0.25f)
                    m_InZone = Zone.SLOW;
                else if (m_AccumTime.TotalMilliseconds > m_MaxMilliSec * 0.75f)
                    m_InZone = Zone.FAST;
                else
                    m_InZone = Zone.MIDDLE;
            }

            m_Arm.Rotational = (float)(Math.PI) * ((float)m_AccumTime.TotalMilliseconds / m_MaxMilliSec);
            m_Arm.Rotational += (float)(Math.PI / 2);
            if (!m_MovingRight)
                m_Arm.Rotational *= -1.0f;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            m_Arm.Draw(spritebatch);

            int bpm = -1;
            if (m_MaxMilliSec > 0)
                bpm = 1000 * 60 / m_MaxMilliSec;


            string output = "Max: " + m_MaxMilliSec + " (bpm " + bpm + "), " + m_AccumTime.Milliseconds.ToString();
            Vector2 FontOrigin = m_Font.MeasureString("Max: " + m_MaxMilliSec + ",");
            spritebatch.DrawString(m_Font, output, new Vector2(m_Position.X, m_Position.Y + 20.0f), Color.White,
                                       0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
        }
    }
}

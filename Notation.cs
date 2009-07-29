using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BoomBoom
{
    class Notation
    {
        Sprite m_Staff;
        Sprite m_HiHats;
        Sprite m_Kick;
        Sprite m_Snare;

        public void Init()
        {
            m_Staff = new Sprite();
            m_HiHats = new Sprite();
            m_Kick = new Sprite();
            m_Snare = new Sprite();

            m_Staff.Init(content, "Sprites\\Staff");
            m_HiHats.Init(content, "Sprites\\HiHats");
            m_Kick.Init(content, "Sprites\\Kick");
            m_Snare.Init(content, "Sprites\\Snare");

            m_Staff.Position = new Vector2(100.0f, 100.0f);
        }

        public void Update(uint currenttime, uint maxtime, bool movingright)
        {
            if (maxtime > 0)
            {
                int snareX = m_Staff.Position.X (currenttime / maxtime)
                m_Snare.Position = new Vector2(100.0f, 95.0f);
                m_Kick.Position = new Vector2(100.0f, 105.0f);
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            m_Staff.Draw(spritebatch);
            m_Kick.Draw(spritebatch);
            m_Snare.Draw(spritebatch);
        }
    }
}

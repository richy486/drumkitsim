using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace BoomBoom
{
    public class Sprite
    {
        private Texture2D m_Texture;
        private Vector2 m_Position;
        private float m_Rotational;
        private Color m_Colour;
        private Vector2 m_CentrePoint;

        public Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }
        public float Rotational
        {
            get { return m_Rotational; }
            set { m_Rotational = value; }
        }
        public Color Colour
        {
            get { return m_Colour; }
            set { m_Colour = value; }
        }
        public Vector2 CentrePoint
        {
            get { return m_CentrePoint; }
            set { m_CentrePoint = value; }
        }
        public Vector2 Size
        {
            get { return new Vector2(m_Texture.Width, m_Texture.Height); }
        }


        public void Init(ContentManager content, string texture)
        {
            m_Texture = content.Load<Texture2D>(texture);
            m_Position.X = 10.0f;
            m_Position.Y = 10.0f;
            m_Rotational = 0.0f;
            m_Colour = Color.White;
            m_CentrePoint = new Vector2(m_Texture.Width / 2, m_Texture.Height / 2);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(m_Texture,

                 //this is the destination point we want to render at
                 m_Position,

                //this is the texture surface area we want to use when rendering
                new Rectangle(0, 0, m_Texture.Width, m_Texture.Height),

                //By using White we are basically saying just use the texture color
                m_Colour,

                //Here is the rotation in radians
                m_Rotational,

                //This is the center point of the rotation
                m_CentrePoint,

                //This is the scaling of the sprite. I want it a bit larger so you dont
                //see the edges as it spins around
                1.0f,

                //We are not doing any fancy effects
                SpriteEffects.None, 0);

        }

        public void Move(Vector2 offset)
        {
            m_Position += offset;
        }
    }
}

using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Storyboarding.Animation
{
    public class SBSprite : Component, ICloneable
    {
        protected Texture2D _texture;

        public float Opacity { get; set; }

        public Vector2 Origin { get; set; }

        public float Rotation { get; set; }

        public float Scale { get; set; }

        public float Speed = 2f;

        public Vector2 Position;

        public Vector2 Velocity;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)(Position.X - Origin.X), (int)(Position.Y - Origin.Y), (int)(_texture.Width * Scale), (int)(_texture.Height * Scale));
            }
        }

        public bool IsRemoved { get; set; }

        public bool isAdditiveBlend { get; set; }

        public SBSprite(Texture2D texture)
        {
            _texture = texture;

            Opacity = 1f;

            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        }

        public void Update(GameTime gameTime)
        {
            Position -= Velocity;
            Rotation += 0.01f;

            if (Rectangle.Top > BaseGame.ScreenHeight)
                IsRemoved = true;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            /*if (isAdditiveBlend)
            {

            }*/
            if (isAdditiveBlend)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            }
            else
            {
                spriteBatch.Begin();
            }

            spriteBatch.Draw(_texture, Position, null, Color.White * Opacity, Rotation, Origin, Scale, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

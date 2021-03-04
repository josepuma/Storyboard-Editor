using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Storyboarding.Emitters.Effects.SparkleEffect
{
    public class Sparkle : EmitterBase
    {

        protected Texture2D _texture;

        private MouseState lastMouseState = new MouseState();

        private float Opacity { get; set; } = 0;

        private Vector2 Position { get; set; }

        private Vector2 Size { get; set; } = new Vector2(.6f);

        private Vector2 Origin { get; set; }

        private float Rotation { get; set; } = 0;

        public bool IsRemoved = false;

        public int _timer = 0;

        public Sparkle(Texture2D texture, Vector2 position)
        {
            this._texture = texture;
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Position = position;
        }

        public override void Update(GameTime gameTime)
        {

            _timer = (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            var x = Position.X + 20;
            var y = Position.Y + 20;


            Vector2 direction = Position - new Vector2(x, y);
            direction.Normalize();
            Position -= direction;

            Rotation += .05f;

            var sizeX = Size.X - 0.01f;
            var sizeY = Size.Y - 0.01f;

            if (sizeX >= 0 && sizeY >= 0)
            {
                Size = new Vector2(sizeX, sizeY);
            }

            if (_timer <= 50)
            {
                Opacity += .05f;
            }
            else
            {
                _timer = 0;
                Opacity -= .02f;


                if (Opacity <= 0)
                {
                    IsRemoved = true;
                }
            }

            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position,  null, Color.White * Opacity, Rotation, Origin,  Size, SpriteEffects.None, 0);
        }


    }
}

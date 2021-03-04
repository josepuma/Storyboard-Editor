using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storyboarding.Emitters
{
    public abstract class SpriteBase
    {
        public abstract void Update(GameTime gameTime, double audioPosition);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}

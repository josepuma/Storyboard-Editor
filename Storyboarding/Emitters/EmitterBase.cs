using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storyboarding.Emitters
{
    public abstract class EmitterBase
    {
            public abstract void Update(GameTime gameTime);
            public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}

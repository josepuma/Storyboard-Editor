using System;
using Microsoft.Xna.Framework;
using Storyboarding.Animation;
using Storyboarding.Emitters;

namespace Storyboarding.Core
{
    public class Camera
    {
        public Matrix Transform { get; private set; }

        public void Follow(Sprite target)
        {
            var position = Matrix.CreateTranslation(
              -target.Position.X - (target.Rectangle.Width / 2),
              -target.Position.Y - (target.Rectangle.Height / 2),
              0);

            var offset = Matrix.CreateTranslation(
                BaseGame.ScreenWidth / 2,
                BaseGame.ScreenHeight / 2,
                0);

            Transform = position * offset;
        }
    }
}

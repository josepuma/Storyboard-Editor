using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Storyboarding.Animation;

namespace Storyboarding.Emitters
{
    public class SpriteEmitter
    {

        public SpriteEmitter(Sprite particle)
            {

            }

        /*protected override void ApplyGlobalVelocity()
        {
            var xSway = (float)BaseGame.Random.Next(-2, 2);
            foreach (var particle in _particles)
                particle.Velocity.X = (xSway * particle.Scale) / 50;
        }*/

        /*protected override Sprite GenerateParticle()
        {
            var sprite = _particlePrefab.Clone() as Sprite;

            var xPosition = BaseGame.Random.Next(0, BaseGame.ScreenWidth);
            var ySpeed = BaseGame.Random.Next(10, 100) / 100f;

            sprite.Position = new Vector2(xPosition, (sprite.Rectangle.Height) + BaseGame.ScreenHeight + 50);
            sprite.Opacity = (float)BaseGame.Random.NextDouble();
            sprite.Rotation = MathHelper.ToRadians(BaseGame.Random.Next(0, 360));
            sprite.Scale = (float)(BaseGame.Random.NextDouble() * (0.1));
            sprite.isAdditiveBlend = true;
            sprite.Velocity = new Vector2(0, ySpeed);

            return sprite;
        }*/

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Storyboarding.Animation;

namespace Storyboarding.Emitters
{
    public abstract class Emitter : Component
    {

        private float _generateTimer;

        private float _swayTimer;

        protected Sprite _particlePrefab;

        protected List<Sprite> _particles;

        /// <summary>
        /// How often a particle is produced
        /// </summary>
        public float GenerateSpeed = 0.05f;

        /// <summary>
        /// How often we apply the "GlobalVelociy" to our particles
        /// </summary>
        public float GlobalVelocitySpeed = 1;

        public int MaxParticles = 1000;

        public Emitter(Sprite particle)
        {
            _particlePrefab = particle;

            _particles = new List<Sprite>();
        }

        public void Update(GameTime gameTime)
        {
            _generateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _swayTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            AddParticle();

            if (_swayTimer > GlobalVelocitySpeed)
            {
                _swayTimer = 0;

                ApplyGlobalVelocity();
            }

            foreach (var particle in _particles)
                //particle.Update(gameTime);

            RemovedFinishedParticles();
        }

        private void AddParticle()
        {
            if (_generateTimer > GenerateSpeed)
            {
                _generateTimer = 0;

                if (_particles.Count < MaxParticles)
                {
                    _particles.Add(GenerateParticle());
                }
            }
        }

        protected abstract void ApplyGlobalVelocity();

        private void RemovedFinishedParticles()
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                if (_particles[i].IsRemoved)
                {
                    _particles.RemoveAt(i);
                    i--;
                }
            }
        }

        protected abstract Sprite GenerateParticle();

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var particle in _particles)
            {
                //particle.Draw(gameTime, spriteBatch);
            }
                
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storyboarding.Emitters.Effects.SparkleEffect
{
    public class SparkleEmitter : EmitterBase
    {

        private string _spritePath;
        private Texture2D _textureBase;

        private GraphicsDevice graphicsDevice;

        private List<Sparkle> _sparklesList;

        private Vector2 Position { get; set; }

        private int _timer = 0;

        public SparkleEmitter(string spritePath, GraphicsDevice graphicsDevice)
        {
            _spritePath = spritePath;
            this.graphicsDevice = graphicsDevice;
            _sparklesList = new List<Sparkle>();
            LoadTexture();
        }

        void LoadTexture()
        {
            FileStream titleStream = File.OpenRead(Path.GetFullPath(this._spritePath));
            _textureBase = Texture2D.FromStream(graphicsDevice, titleStream);
            titleStream.Close();
            Color[] buffer = new Color[_textureBase.Width * _textureBase.Height];
            _textureBase.GetData(buffer);
            for (int j = 0; j < buffer.Length; j++)
                buffer[j] = Color.FromNonPremultiplied(buffer[j].R, buffer[j].G, buffer[j].B, buffer[j].A);
            _textureBase.SetData(buffer);
        }

        void AddSparkle()
        {
            _sparklesList.Add(
                    new Sparkle(_textureBase, Position));
        }

        public void SetPosition(int x, int y)
        {
            Position = new Vector2(x, y);
        }

        public override void Update(GameTime gameTime)
        {
            _timer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            
            if(_timer > 50)
            {
                AddSparkle();
                _timer = 0;
            }
           

            foreach (var sparkle in _sparklesList.ToArray())
            {
                sparkle.Update(gameTime);
            }

            for(var i = 0; i < _sparklesList.Count; i++)
            {
                if (_sparklesList[i].IsRemoved)
                {
                    _sparklesList.RemoveAt(i);
                    i--;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var sparkle in _sparklesList)
                sparkle.Draw(gameTime, spriteBatch);
        }


    }
}

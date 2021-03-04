using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Storyboarding.Animation;

namespace Storyboarding.Emitters
{
    public class StoryboardEmitter
    {

        protected List<Sprite> _sprites;

        public StoryboardEmitter(){
            _sprites = new List<Sprite>();
        }

        public void LoadSprite(Sprite sprite)
        {
            _sprites.Add(sprite);
        }

        public void LoadSprites(List<Sprite> sprites)
        {
            _sprites.AddRange(sprites);
        }

        public void LoadTextures(GraphicsDevice graphicsDevice)
        {
            Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
            for(var i = 0; i < _sprites.Count; i++)
            {
                var sprite = _sprites[i];
                var file = sprite.SpritePath;

                if (textures.ContainsKey(file))
                {
                    //sprite.LoadTexture(textures[file]);
                    //sprite.SetRectangleTexture(graphicsDevice, textures[file]);
                }
                else
                {
                    Texture2D texture;

                    FileStream titleStream = File.OpenRead(Path.GetFullPath(sprite.SpritePath));
                    texture = Texture2D.FromStream(graphicsDevice, titleStream);
                    titleStream.Close();
                    titleStream.Dispose();
                    Color[] buffer = new Color[texture.Width * texture.Height];
                    texture.GetData(buffer);
                    for (int j = 0; j < buffer.Length; j++)
                        buffer[j] = Color.FromNonPremultiplied(buffer[j].R, buffer[j].G, buffer[j].B, buffer[j].A);
                    texture.SetData(buffer);
                    /*FileStream fileStream = new FileStream(Path.GetFullPath(sprite.SpritePath), FileMode.Open);
                    Texture2D texture = Texture2D.FromStream(graphicsDevice, fileStream);*/
                    textures.Add(file, texture);
                    //fileStream.Dispose();
                    //sprite.LoadTexture(texture);
                    //sprite.SetRectangleTexture(graphicsDevice, texture);
                }
            }
        }

        public void GenerateOsuStoryboard()
        {
            var osbContent = "";
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            osbContent += "[Events]\n";
            osbContent += "//Storyboard Layer 1 (Background)\n";

            foreach (var item in _sprites)
            {
                var filePath = Path.GetFileName(item.SpritePath);
                osbContent += ("Sprite,Background,Centre," + filePath + ",320,240\n");

                if (item.IsAdditiveBlend)
                {
                    osbContent += " P,0," + item.MainStartTime + ",,A\n";
                }

            }

            osbContent += "//";

            File.WriteAllText("/Users/josepuma/Desktop/storyboard.txt", osbContent);

        }

        public void Update(GameTime gameTime, double audioPosition, bool showBorders)
        {
            foreach (var sprite in _sprites.ToArray())
            {
                if (sprite.IsActive(audioPosition))
                {
                    sprite.IsRemoved = false;
                    sprite.VisibleSpriteBorder = Color.Red;
                    sprite.ShowBorders = showBorders;
                    sprite.Update(gameTime, audioPosition);
                }
                else
                {
                    sprite.IsRemoved = true;
                    sprite.VisibleSpriteBorder = Color.White;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, float ratioX, GraphicsDevice graphicsDevice)
        {
            /*foreach (var sprite in _sprites)
            {
                if (!sprite.IsRemoved)
                {
                    if (sprite.IsAdditiveBlend)
                    {
                        graphicsDevice.BlendState = BlendState.Additive;
                    }
                    else
                    {
                        graphicsDevice.BlendState = BlendState.AlphaBlend;
                    }
                    sprite.Draw(gameTime, spriteBatch, ratioX);
                }
            } */             
        }

    }
}

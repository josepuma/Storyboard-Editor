using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storyboarding.Emitters
{
    public static class TextureHandler
    {
        public static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        
        public static Texture2D LoadTexture(string spritePath, GraphicsDevice graphicsDevice)
        {
            if (textures.ContainsKey(spritePath))
            {
                return textures[spritePath];
            }
            else
            {
                Texture2D texture;

                FileStream titleStream = File.OpenRead(Path.GetFullPath(spritePath));
                texture = Texture2D.FromStream(graphicsDevice, titleStream);
                titleStream.Close();
                titleStream.Dispose();
                Color[] buffer = new Color[texture.Width * texture.Height];
                texture.GetData(buffer);
                for (int j = 0; j < buffer.Length; j++)
                    buffer[j] = Color.FromNonPremultiplied(buffer[j].R, buffer[j].G, buffer[j].B, buffer[j].A);
                texture.SetData(buffer);

                textures.Add(spritePath, texture);
                return texture;

            }
        }
    }
}

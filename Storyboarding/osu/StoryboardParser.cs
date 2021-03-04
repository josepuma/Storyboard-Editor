using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Storyboarding.Emitters;

namespace Storyboarding.osu
{
    public class StoryboardParser
    {
        List<string> osbLines;

        public List<Sprite> _spriteList;

        public string AssetsDirectory { get; set; }

        public StoryboardParser(string osbPath, GraphicsDevice graphicsDevice)
        {
            osbLines = new List<string>(File.ReadAllLines(osbPath));
            AssetsDirectory = Path.GetDirectoryName(osbPath);
            //AssetsDirectory = "/Applications/osu!w.app/Contents/Resources/drive_c/osu!/Songs/35701 Lia - Toki wo Kizamu Uta";
            _spriteList = new List<Sprite>();
            ParseLines(graphicsDevice);
        }

        public void ParseLines(GraphicsDevice graphicsDevice)
        {
            Sprite sprite = null;
            var hasLoops = false;
            var isSpriteLoaded = false;

            foreach (var line in osbLines)
            {
                if (!line.StartsWith("//") && !line.StartsWith("[Events]"))
                {
                    var trimmedLine = line.Trim();
                    var values = trimmedLine.Split(',');
                   
                    

                        switch (values[0])
                    {
                        case "Sprite":
                            var path = removePathQuotes(values[3]).Replace(@"\", "/");
                            //Console.WriteLine(path);
                            

                            //Console.WriteLine(path);

                            if (isSpriteLoaded)
                            {
                                if (!hasLoops)
                                {
                                    sprite.End();
                                    _spriteList.Add(sprite);
                                }
                                else
                                {
                                    hasLoops = false;
                                }
                                
                                
                                /*isSpriteLoaded = false;*/
                                sprite = null;
                            }

                            var x = float.Parse(values[4], CultureInfo.InvariantCulture);
                            var y = float.Parse(values[5], CultureInfo.InvariantCulture);
                            var origin = values[2];

                            sprite = new Sprite(TextureHandler.LoadTexture(AssetsDirectory + "/" + path, graphicsDevice));
                            sprite.SetOrigin(origin);
                            sprite.OriginCommand = origin;
                            sprite.Position = new Microsoft.Xna.Framework.Vector2(x + 107, y);
                            isSpriteLoaded = true;
                        break;

                        case "L":
                            hasLoops = true;

                            break;

                        case "T":
                            sprite.Size = new Vector2(0);
                            break;

                        case "Animation":
                            sprite.Size = new Vector2(0);
                            break;

                        default:
                            if (string.IsNullOrEmpty(values[3]))
                                values[3] = values[2];

                            var commandType = values[0];

                            var startTime = double.Parse(values[2], CultureInfo.InvariantCulture);
                            var endTime = double.Parse(values[3], CultureInfo.InvariantCulture);

                            switch (commandType)
                            {
                                case "F":
                                {
                                    var startValue = double.Parse(values[4], CultureInfo.InvariantCulture);
                                    var endValue = values.Length > 5 ? double.Parse(values[5], CultureInfo.InvariantCulture) : startValue;
                                    sprite.Fade(startTime, endTime, startValue, endValue);
                                }
                                break;
                                case "S":
                                {
                                    var startValue = double.Parse(values[4], CultureInfo.InvariantCulture);
                                    var endValue = values.Length > 5 ? double.Parse(values[5], CultureInfo.InvariantCulture) : startValue;
                                    sprite.Scale(startTime, endTime, startValue, endValue);
                                }
                                break;
                                case "V":
                                    {
                                        var startX = double.Parse(values[4], CultureInfo.InvariantCulture);
                                        var startY = double.Parse(values[5], CultureInfo.InvariantCulture);
                                        var endX = values.Length > 6 ? double.Parse(values[6], CultureInfo.InvariantCulture) : startX;
                                        var endY = values.Length > 7 ? double.Parse(values[7], CultureInfo.InvariantCulture) : startY;
                                        sprite.ScaleVec(startTime, endTime, startX, startY, endX, endY);
                                    }
                                    break;
                                case "R":
                                    {
                                        var startValue = double.Parse(values[4], CultureInfo.InvariantCulture);
                                        var endValue = values.Length > 5 ? double.Parse(values[5], CultureInfo.InvariantCulture) : startValue;
                                        sprite.Rotate(startTime, endTime, startValue, endValue);
                                    }
                                    break;
                                case "MX":
                                    {
                                        var startValue = double.Parse(values[4], CultureInfo.InvariantCulture);
                                        var endValue = values.Length > 5 ? double.Parse(values[5], CultureInfo.InvariantCulture) : startValue;
                                        sprite.MoveX(startTime, endTime, startValue + 107, endValue + 107);
                                    }
                                    break;
                                case "MY":
                                    {
                                        var startValue = double.Parse(values[4], CultureInfo.InvariantCulture);
                                        var endValue = values.Length > 5 ? double.Parse(values[5], CultureInfo.InvariantCulture) : startValue;
                                        sprite.MoveY(startTime, endTime, startValue, endValue);
                                    }
                                break;
                                case "M":
                                {
                                    var startX = double.Parse(values[4], CultureInfo.InvariantCulture);
                                    var startY = double.Parse(values[5], CultureInfo.InvariantCulture);
                                    var endX = values.Length > 6 ? double.Parse(values[6], CultureInfo.InvariantCulture) : startX;
                                    var endY = values.Length > 7 ? double.Parse(values[7], CultureInfo.InvariantCulture) : startY;
                                    sprite.MoveX(startTime, endTime, startX + 107, endX + 107);
                                    sprite.MoveY(startTime, endTime, startY, endY);
                                    }
                                break;
                                case "C":
                                {
                                    var startX = double.Parse(values[4], CultureInfo.InvariantCulture);
                                    var startY = double.Parse(values[5], CultureInfo.InvariantCulture);
                                    var startZ = double.Parse(values[6], CultureInfo.InvariantCulture);
                                    var endX = values.Length > 7 ? double.Parse(values[7], CultureInfo.InvariantCulture) : startX;
                                    var endY = values.Length > 8 ? double.Parse(values[8], CultureInfo.InvariantCulture) : startY;
                                    var endZ = values.Length > 9 ? double.Parse(values[9], CultureInfo.InvariantCulture) : startZ;

                                    var color = new Color((int)startX, (int)startY , (int)startZ);
                                    sprite.SpriteColor = color;
                                    //-osbSprite.Color(easing, startTime, endTime, startX / 255f, startY / 255f, startZ / 255f, endX / 255f, endY / 255f, endZ / 255f);
                                }
                                break;
                                case "P":
                                    {
                                        var type = values[4];
                                        switch (type)
                                        {
                                            case "A": sprite.IsAdditiveBlend = true; break;
                                            case "H": sprite.Orientation = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally; break;
                                            case "V": sprite.Orientation = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipVertically; break;
                                        }
                                    }
                                    break;
                            }

                            break;
                    }
                }
            }
        }

        public List<Sprite> GetSprites()
        {
            return _spriteList;             
        }

        private static string removePathQuotes(string path)
        {
            return path.StartsWith("\"") && path.EndsWith("\"") ? path.Substring(1, path.Length - 2) : path;
        }
    }
}

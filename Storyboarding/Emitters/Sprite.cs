using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Storyboarding.Animation;

namespace Storyboarding.Emitters
{
    public class Sprite : SpriteBase
    {
        protected Texture2D _texture;

        protected Texture2D _rectangleTexture;

        public string SpritePath { get; set; }

        public float Opacity { get; set; } = 1;

        public Vector2 Origin { get; set; }

        public string OriginCommand { get; set; } = "Centre";

        public float Rotation { get; set; } = 0;

        public SpriteEffects Orientation { get; set; } = SpriteEffects.None;

        public Vector2 Size { get; set; } = new Vector2(1);

        public bool IsRemoved { get; set; } = false;

        public bool IsAdditiveBlend { get; set; } = false;

        public bool IsHorizontalFlip { get; set; }

        public bool IsVerticalFlip { get; set; }

        public bool isTextureLoaded { get; set; } = false;

        public bool IsTimingCalculated { get; set; }

        public double MainStartTime { get; set; } = 0;

        public double MainEndTime { get; set; } = 0;

        public Color SpriteColor { get; set; } = Color.White;

        public Color VisibleSpriteBorder { get; set; } = Color.Red;

        public Vector2 Position { get; set; } = new Vector2(407, 240);

        public bool ShowBorders { get; set; }

        public List<Command> _commands;

        List<IGrouping<string, Command>> _groupedCommands;

        public Rectangle Rectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height); }
        }

        public float RatioX
        {
            get { return BaseGame.RatioX; }
        }

        public Sprite(Texture2D texture)
        {
            _texture = texture;
            ShowBorders = false;
            _commands = new List<Command>();
            _groupedCommands = new List<IGrouping<string, Command>>();
        }

        public void SetOrigin(string originCommand)
        {
            switch (originCommand)
            {
                case "TopLeft":
                    Origin = new Vector2(0, 0);
                    break;
                case "TopCentre":
                    Origin = new Vector2(_texture.Width / 2, 0);
                    break;
                case "TopRight":
                    Origin = new Vector2(_texture.Width, 0);
                    break;
                case "BottomCentre":
                    Origin = new Vector2(_texture.Width / 2, _texture.Height);
                    break;
                default:
                    Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
                    break;

            }


        }

        /*public Sprite(GraphicsDevice graphicsDevice, Texture2D texture)
      : this(texture)
        {
            //SetRectangleTexture(graphicsDevice, texture);
        }*/

        public void Fade(double startTime, double endTime, double startValue, double endValue)
        {
            _commands.Add(new Command("F", startTime, endTime, startValue, endValue, 0, 0));
        }

        public void Scale(double startTime, double endTime, double startValue, double endValue)
        {
            _commands.Add(new Command("S", startTime, endTime, startValue, endValue, 0, 0));
        }

        public void MoveX(double startTime, double endTime, double startValue, double endValue)
        {
            _commands.Add(new Command("MX", startTime, endTime, startValue, endValue, 0, 0));
        }

        public void MoveY(double startTime, double endTime, double startValue, double endValue)
        {
            _commands.Add(new Command("MY", startTime, endTime, startValue, endValue, 0, 0));
        }

        public void Rotate(double startTime, double endTime, double startValue, double endValue)
        {
            _commands.Add(new Command("R", startTime, endTime, startValue, endValue, 0, 0));
        }

        public void ScaleVec(double startTime, double endTime, double startValueX, double startValueY, double endValueX, double endValueY)
        {
            _commands.Add(new Command("V", startTime, endTime, startValueX, endValueX, startValueY, endValueY));
        }

        public void SetInitialCommands()
        {
            _groupedCommands = _commands
                        .GroupBy(command => command.CommandValue).ToList();

            for (var i = 0; i < _groupedCommands.Count; i++)
            {
                var group = _groupedCommands[i];
                var groupMinValue = group.Min(value => value.StartTime);
                var minCommand = group.Where(startTime => startTime.StartTime == groupMinValue).FirstOrDefault();

                switch (group.Key)
                {
                    case "F":
                        Opacity = (float)(minCommand.StartValue);
                        break;
                    case "S":
                        Size = new Vector2((float)(minCommand.StartValue));
                        break;
                    case "MX":
                        Position = new Vector2((float)(minCommand.StartValue), Position.Y);
                        break;
                    case "MY":
                        Position = new Vector2(Position.X, (float)(minCommand.StartValue));
                        break;
                    case "R":
                        Rotation = (float)(minCommand.StartValue);
                        break;
                    case "V":
                        Size = new Vector2((float)minCommand.StartValue, (float)minCommand.StartValueY);
                        break;
                }

            }
        }

        public bool IsActive(double time) => MainStartTime <= time && time <= MainEndTime;

        public void End()
        {
            if (_commands.Count > 0)
            {
                MainStartTime = _commands.Min(value => value.StartTime);
                MainEndTime = _commands.Max(value => value.EndTime);
                SetInitialCommands();
            }
            IsTimingCalculated = true;
        }

        public override void Update(GameTime gameTime, double audioPosition)
        {

           //VisibleSpriteBorder = Color.Red;

            for (var i = 0; i < _groupedCommands.Count; i++)
            {
                var timing = _groupedCommands[i];
                switch (timing.Key)
                {
                    case "F":
                        foreach (var command in timing)
                        {
                            if (command.IsActive(audioPosition))
                            {
                                Opacity = (float)command.ValueAtTime(audioPosition);
                            }

                        }
                        break;
                    case "S":
                        foreach (var command in timing)
                        {
                            if (command.IsActive(audioPosition))
                            {
                                Size = new Vector2((float)command.ValueAtTime(audioPosition));
                            }
                        }
                        break;
                    case "V":
                        foreach (var command in timing)
                        {
                            if (command.IsActive(audioPosition))
                            {
                                Size = command.ValueAtTime2D(audioPosition);
                            }
                        }
                        break;
                    case "MX":
                        foreach (var command in timing)
                        {
                            if (command.IsActive(audioPosition))
                            {
                                Position = new Vector2((float)command.ValueAtTime(audioPosition), Position.Y);
                            }
                        }
                        break;
                    case "MY":
                        foreach (var command in timing)
                        {
                            if (command.IsActive(audioPosition))
                            {
                                Position = new Vector2(Position.X, (float)command.ValueAtTime(audioPosition));
                            }
                        }
                        break;
                    case "R":
                        foreach (var command in timing)
                        {
                            if (command.IsActive(audioPosition))
                            {
                                Rotation = (float)command.ValueAtTime(audioPosition);
                            }

                        }
                        break;

                }
            }

            if (Opacity <= 0)
            {
                VisibleSpriteBorder = Color.White;
                IsRemoved = true;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position / RatioX, null, SpriteColor * Opacity, Rotation, Origin, Size / RatioX, Orientation, 0);
            //if (ShowBorders)
              //  spriteBatch.Draw(_rectangleTexture, Position / RatioX, null, VisibleSpriteBorder, Rotation, Origin, Size / RatioX, SpriteEffects.None, 0);
        }
    }

    public struct Command
    {
        public string CommandValue { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public double StartValue { get; set; }
        public double EndValue { get; set; }

        public double StartValueY { get; set; }
        public double EndValueY { get; set; }

        public Command(string command, double startTime, double endTime, double startValueX, double endValueX, double startValueY, double endValueY)
        {
            this.CommandValue = command;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.StartValue = startValueX;
            this.EndValue = endValueX;

            this.StartValueY = startValueY;
            this.EndValueY = endValueY;
        }

        public double ValueAtTime(double time)
        {
            if (time <= StartTime) return StartValue;
            if (EndTime <= time) return EndValue;

            var duration = EndTime - StartTime;
            var progress = (time - StartTime) / duration;

            return ValueAtProgress(progress);
        }

        public Vector2 ValueAtTime2D(double time)
        {
            if (time <= StartTime) return new Vector2((float)StartValue, (float)StartValueY);
            if (EndTime <= time) return new Vector2((float)EndValue, (float)EndValueY);

            var duration = EndTime - StartTime;
            var progress = (time - StartTime) / duration;

            return ValueAtProgress2D(progress);
        }

        public Vector2 ValueAtProgress2D(double progress)
        {
            return new Vector2((float)(StartValue + (EndValue - StartValue) * progress), (float)(StartValueY + (EndValueY - StartValueY) * progress));
        }

        public double ValueAtProgress(double progress) => StartValue + (EndValue - StartValue) * progress;

        public bool IsActive(double time) => StartTime <= time && time <= EndTime;
    }

}

using System;
using System.Collections.Generic;
using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Storyboarding.Components;
using Storyboarding.Core;
using Storyboarding.Emitters;
using Storyboarding.Emitters.Effects.SparkleEffect;
using Storyboarding.osu;
using Storyboarding.Sound;

namespace Storyboarding.Animation
{
    public class BaseGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Camera _camera;

        private List<Component> _gameComponents;

        public static int ScreenHeight = 720;
        public static int ScreenWidth = 1280;

        public static float RatioX = 854.0f / ScreenWidth;
        public static float RatioY = 490.0f / ScreenHeight;

        private bool _showBorders = false;

        public static Random Random;

        private Audio _audio;

        public bool IsAudioPlaying = false;

        private StoryboardEmitter _storyboardEmitter;
        private StoryboardParser _storyboardParser;
        private SparkleEmitter _sparkleEmitter;

        private List<Sprite> _spriteList;

        private FontSystem _fontSystem;
        SpriteFontBase font18;

        public BaseGame()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        public GraphicsDevice graphicsDevice()
        {
            return graphics.GraphicsDevice;
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();

            

            Random = new Random();
            _spriteList = new List<Sprite>();
            base.Initialize();
        }

        protected override void LoadContent()
        {

            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _sparkleEmitter = new SparkleEmitter("/Users/josepuma/Downloads/star2.png", graphicsDevice());

            _fontSystem = FontSystemFactory.Create(GraphicsDevice, 300, 300, true);
            _fontSystem.AddFont(File.ReadAllBytes("/Users/josepuma/Library/Fonts/SulphurPoint-Bold.ttf"));
            font18 = _fontSystem.GetFont(14);

            Texture2D texture;

            FileStream titleStream = File.OpenRead("/Users/josepuma/Documents/storyboard/Button.png");
            texture = Texture2D.FromStream(GraphicsDevice, titleStream);
            titleStream.Close();
            Color[] buffer = new Color[texture.Width * texture.Height];
            texture.GetData(buffer);
            for (int j = 0; j < buffer.Length; j++)
                buffer[j] = Color.FromNonPremultiplied(buffer[j].R, buffer[j].G, buffer[j].B, buffer[j].A);
            texture.SetData(buffer);


            var quitButton = new Button(texture, font18)
            {
                Position = new Vector2(0,0),
                Text = "Quit",
            };

            var playButton = new Button(texture, font18)
            {
                Position = new Vector2(130,0),
                Text = "Play"
            };

            var showBorders = new Button(texture, font18)
            {
                Position = new Vector2(260, 0),
                Text = "Show Border"
            };


            quitButton.Click += QuitButton_Click;
            playButton.Click += PlayButton_Click;
            showBorders.Click += ShowBordersButton_Click;

            _gameComponents = new List<Component>()
              {
                quitButton,
                playButton,
                showBorders
              };

            _audio = new Audio("/Applications/osu!w.app/Contents/Resources/drive_c/osu!/Songs/547714 RADWIMPS - Hikari/audio.mp3");

            _storyboardParser = new StoryboardParser("/Applications/osu!w.app/Contents/Resources/drive_c/osu!/Songs/547714 RADWIMPS - Hikari/RADWIMPS - Hikari (Haruto).osb", GraphicsDevice);
            var sprites = _storyboardParser.GetSprites();
            _spriteList.AddRange(sprites);
            //_storyboardEmitter.LoadSprites(sprites);

            var songLength = _audio.GetLength();

            /*var sideBarLeft = new Sprite("/Users/josepuma/Documents/sb/square.jpg");
            sideBarLeft.OriginCommand = "TopLeft";
            sideBarLeft.MoveX(0, songLength, 0, 0);
            sideBarLeft.MoveY(0, songLength, 0, 0);
            sideBarLeft.ScaleVec(0, songLength, 107, 480, 107, 480);
            sideBarLeft.SpriteColor = Color.Black;
            sideBarLeft.Fade(0, songLength, 1, 1);

            var sideBarRight = new Sprite("/Users/josepuma/Documents/sb/square.jpg");
            sideBarRight.OriginCommand = "TopRight";
            sideBarRight.MoveX(0, songLength, 854, 854);
            sideBarRight.MoveY(0, songLength, 0, 0);
            sideBarRight.ScaleVec(0, songLength, 107, 480, 107, 480);
            sideBarRight.SpriteColor = Color.Black;
            sideBarRight.Fade(0, songLength, 1, 1);

            //_storyboardEmitter.LoadSprite(sideBarLeft);
            //_storyboardEmitter.LoadSprite(sideBarRight);

            var bar = new Sprite("/Users/josepuma/Documents/sb/bar.png");
            bar.MoveY(offset, songLength, 440, 440);
            bar.MoveX(offset, songLength, 0, 854);
            bar.Scale(offset, songLength, .3, .3);
            bar.Fade(offset, songLength, 1, 1);
            bar.IsAdditiveBlend = true;




            var startPositionX = 0.0;
            var widthArea = 854.0;
            var barsQ = songLength / (beat * 4);
            var barDistance = widthArea / (barsQ);
            for (var i = 0; i < barsQ; i ++)
            {
                var bar2 = new Sprite("/Users/josepuma/Documents/sb/bar.png");
                bar2.MoveY(offset + beat * 3 * i, offset + beat * 4 * i, 380, 440);
                bar2.MoveY(offset + beat * 4 * i, songLength, 440, 440);
                bar2.MoveX(offset + beat * 3 * i, songLength, startPositionX, startPositionX);
                bar2.Scale(offset + beat * 3 * i, songLength, .1, .1);
                bar2.Fade(offset + beat * 3 * i, offset + beat * 4 * i, 0, 1);
                bar2.Fade(offset + beat * 4 * i, songLength, 1, 1);
                bar2.IsAdditiveBlend = true;

                startPositionX += barDistance;

                _storyboardEmitter.LoadSprite(bar2);
            }

            _storyboardEmitter.LoadSprite(bar);*/

            Console.WriteLine("Sprites:" + TextureHandler.textures.Count);

        }

        private void QuitButton_Click(object sender, System.EventArgs e)
        {
            Exit();
        }

        private void ShowBordersButton_Click(object sender, System.EventArgs e)
        {
            _showBorders = !_showBorders;
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            _audio.Play();
        }

        protected override void Update(GameTime gameTime)
        {

            MouseState currentState = Mouse.GetState(); //Get the state

            if(Mouse.GetState().LeftButton == ButtonState.Released)
            {
                _sparkleEmitter.SetPosition(currentState.X, currentState.Y);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _audio.SetPosition(_audio.GetPosition() + 480);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if(_audio.GetPosition() > 480)
                {
                    _audio.SetPosition(_audio.GetPosition() - 480);
                }
                else
                {
                    _audio.SetPosition(0);
                }
            }

            var audioPosition = _audio.GetPosition();

            if (_audio.IsPlaying)
            {
                foreach (var sprite in _spriteList.ToArray())
                {
                    if (sprite.IsActive(audioPosition))
                    {
                        sprite.IsRemoved = false;
                        sprite.VisibleSpriteBorder = Color.Red;
                        sprite.ShowBorders = _showBorders;

                        sprite.Update(gameTime, audioPosition); 
                    }
                    else
                    {
                        sprite.IsRemoved = true;
                        sprite.VisibleSpriteBorder = Color.White;
                    }
                }
            }

            _sparkleEmitter.Update(gameTime);


            foreach (var component in _gameComponents)
                component.Update(gameTime);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (_audio.GetPosition() > 0)
            {

                foreach (var sprite in _spriteList)
                {
                    if (!sprite.IsRemoved)
                    {
                        if (sprite.IsAdditiveBlend)
                        {
                            GraphicsDevice.BlendState = BlendState.Additive;
                        }
                        else
                        {
                            GraphicsDevice.BlendState = BlendState.AlphaBlend;
                        }
                        sprite.Draw(gameTime, spriteBatch);
                    }
                }
            }

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
            foreach (var component in _gameComponents)
                component.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(font18, "FPS: " +(1000 / gameTime.ElapsedGameTime.Milliseconds), new Vector2(ScreenWidth - 50, ScreenHeight - 50), Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            _sparkleEmitter.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

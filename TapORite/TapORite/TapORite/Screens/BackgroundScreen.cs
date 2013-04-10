using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TapORite
{
    class BackgroundScreen : GameScreen
    {
        Texture2D Background;

        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            Background = Load<Texture2D>("Textures/Backgrounds/background");
            base.LoadContent();
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(Background,Vector2.Zero,new Color(255,255,255,TransitionAlpha));
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}

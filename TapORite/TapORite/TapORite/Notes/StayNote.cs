using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;

namespace TapORite
{
    public class StayNote : DrawableGameComponent
    {
        TapORite curGame = null; const float speed = 2.5f;
        public Vector2 NotePositon; Texture2D NoteTexture; SpriteBatch spriteBatch; Texture2D flashNote;
        string[] color; int textureIndex; float opacity = 0.01f;
        bool flash = false;
        public StayNote(Game game)
            : base(game)
        {
        }
        public StayNote(Game game, SpriteBatch screenSpriteBatch,int x, int y,int textureindex)
            : this(game)
        {
            NotePositon.X = (x-1)*80 +10;
            NotePositon.Y = (y) * 120-20;
            color = new string[] { "blue", "green", "pink", "red", "yellow" };
            curGame = (TapORite)game;
            textureIndex = textureindex;
            spriteBatch = screenSpriteBatch;
        }
        public void handleInput(Vector2 position)
        {
            if (position.X >= NotePositon.X && position.Y >= NotePositon.Y && position.X <= NotePositon.X + NoteTexture.Width && position.Y <= NotePositon.Y + NoteTexture.Height)
            {
                NoteTexture = flashNote; flash = true;
            }
        }
        protected override void LoadContent()
        {
            Random random = new Random();
            int TxtNum = random.Next(0, 4);
            NoteTexture = curGame.Content.Load<Texture2D>("Textures/Notes/note_" + color[textureIndex] + "1");
            flashNote = curGame.Content.Load<Texture2D>("Textures/Notes/note_" + color[textureIndex] + "3");
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            if (flash)
            {
                if (opacity <= 0)
                    curGame.Components.Remove(this);
                opacity -= 0.05f;
            }
            opacity = opacity>=1f ? 1f:opacity + 0.01f;
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(NoteTexture, NotePositon, Color.White*opacity);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

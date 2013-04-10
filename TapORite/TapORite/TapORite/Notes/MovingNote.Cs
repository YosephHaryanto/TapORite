using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;
using Microsoft.Xna.Framework.Input.Touch;
namespace TapORite
{
    public class MovingNote:DrawableGameComponent 
    {
        TapORite curGame = null; const float speed = 2.5f;
        public Vector2 NotePositon; public Texture2D NoteTexture;SpriteBatch spriteBatch;
        public Vector2 tapPoint; 
        string[] color;
        int textureIndex;
        public MovingNote(Game game)
            : base(game)
        {
        }
        
        public MovingNote( Game game, SpriteBatch screenSpriteBatch, int x ,int y,int textureindex)
            : this(game)
        {
            NotePositon.X = 0;
            NotePositon.Y = (y) * 120 -20;
            color = new string[]{"blue","green","pink","red","yellow"};
            curGame = (TapORite)game;
            spriteBatch = screenSpriteBatch;
            textureIndex = textureindex;
            tapPoint = new Vector2( (x-1)*80+10 ,y*120-20);
        }
        
        protected override void LoadContent()
        {
            
            NoteTexture = curGame.Content.Load<Texture2D>("Textures/Notes/note_" + color[textureIndex] + "2");
            NotePositon.X = 0 - NoteTexture.Width;
            base.LoadContent();
        }
        public int HandleInput(Vector2 pos)
        {
            if (CheckCollision() && checkTap(pos))
            {
                return 250 - (int)(Vector2.DistanceSquared(NotePositon, tapPoint) / 20*1.5);
            }
            return 0;
        }
        public override void Update(GameTime gameTime)
        {
            NotePositon.X += 1*speed;
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(NoteTexture, NotePositon, Color.White);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
        private bool CheckCollision()
        {
            if (Vector2.DistanceSquared(NotePositon,tapPoint) < 50*50)
            {
                return true;
            }
            else
                return false;
        }
        private bool checkTap(Vector2 tapPos)
        {
            if (tapPos.X > tapPoint.X && tapPos.Y > tapPoint.Y && tapPos.X < tapPoint.X + NoteTexture.Width && tapPos.Y < tapPoint.Y + NoteTexture.Height)
            {
                return true;
            }
            else
                return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input.Touch;
using TapORite;
namespace TapORite
{
    class GamePlayScreen:GameScreen
    {
        SpriteBatch spriteBatch; TapORite curGame = null; Texture2D VLine;
        Vector2 NotePosition; Texture2D line; int index;
        List<MovingNote> MovingNotes;
        List<StayNote> StayNotes; SpriteFont HUDFont;
        int score = 0;
        private int[,] map;
        double sekon; int PreSekon;
        Texture2D Ready; Texture2D set; Texture2D Tap;Texture2D Menu;
        bool Prepare;

        public GamePlayScreen( Game game,SpriteBatch screenSpriteBatch)
        {
            Prepare = true;
            map = new int[200, 2];
            GenerateLevel(map);
            AudioManager.Initialize(game);
            sekon = 0;
            PreSekon = 0;
            spriteBatch = screenSpriteBatch;
            curGame = (TapORite)game;
            NotePosition.X = 800;
            NotePosition.X = 0;
            
            MovingNotes =  new List<MovingNote>();
            StayNotes = new List<StayNote>();
            EnabledGestures = GestureType.Tap;
        }
        void GenerateLevel(int[,] map)
        {
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                map[i,0] =0;
                map[i, 1] = 0;

            }
            for (int i = 10; i < 200; i++)
            {
                map[i, 0] = rand.Next(2, 8);
                map[i, 0] = map[i, 0] == 2 ? 0 : map[i, 0];
                map[i, 1] = rand.Next(1, 3);
            }
        }
        public override void LoadContent()
        {
            AudioManager.LoadSounds(); 
            line = curGame.Content.Load<Texture2D>("Textures/line_white");
            HUDFont = curGame.Content.Load<SpriteFont>("Fonts/HUDFont");
            VLine = curGame.Content.Load<Texture2D>("Textures/line_white2");
            Ready = curGame.Content.Load<Texture2D>("Textures/Menu/Ready");
            set = curGame.Content.Load<Texture2D>("Textures/Menu/set");
            Tap = curGame.Content.Load<Texture2D>("Textures/Menu/tap");
            Menu = Ready;
            base.LoadContent();
        }
        void DrawString(SpriteFont font, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, text,
            new Vector2(position.X + 1, position.Y + 1), Color.Black);
            spriteBatch.DrawString(font, text, position, color);
        }
        void drawHUD()
        {
            DrawString(HUDFont,"Tap O Rite",new Vector2(400- (HUDFont.MeasureString("Tap O Rite").X)/2,0),Color.OrangeRed);
            DrawString(HUDFont, "Score = ", new Vector2(400 + (HUDFont.MeasureString("Tap O Rite").X) / 2 + 50, 0), Color.OrangeRed);
            DrawString(HUDFont, score.ToString(), new Vector2(400 + (HUDFont.MeasureString("Tap O Rite").X) / 2 + 50 + HUDFont.MeasureString("Score = ").X, 0), Color.Orange);
        }
        /// <summary>
        /// A simple helper to draw shadowed text.
        /// </summary>
        void DrawString(SpriteFont font, string text, Vector2 position, Color color, float fontScale)
        {
            spriteBatch.DrawString(font, text, new Vector2(position.X + 1,
                position.Y + 1), Color.Black, 0, new Vector2(0, font.LineSpacing / 2),
                fontScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, text, position, color, 0,
                new Vector2(0, font.LineSpacing / 2), fontScale, SpriteEffects.None, 0);
        }
        public override void HandleInput(InputState input)
        {
            foreach (GestureSample gestureSample in input.Gestures)
            {
                if (gestureSample.GestureType == GestureType.Tap)
                {
                    foreach (MovingNote movingNote in MovingNotes)
                    {
                        int s = movingNote.HandleInput(gestureSample.Position);
                        score += s;
                        if (s > 0)
                        {
                            foreach (StayNote staynote in StayNotes)
                            {
                                if (Vector2.Distance(staynote.NotePositon, movingNote.tapPoint) == 0f)
                                {
                                    staynote.handleInput(staynote.NotePositon);
                                    StayNotes.Remove(staynote);
                                    break;
                                }
                            }
                            MovingNotes.Remove(movingNote);
                            curGame.Components.Remove(movingNote);
                            break;
                        }
                    }
                }
            }
            base.HandleInput(input);
        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (Prepare)
            {
                if (PreSekon < 2000)
                    Menu = Ready;
                else
                    if (PreSekon < 4000)
                        Menu = set;
                    else
                        if (PreSekon < 6000)
                            Menu = Tap;
                        else
                        {
                            Prepare = false; AudioManager.PlayMusic("Song1");
                        }
                PreSekon += gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                sekon += gameTime.ElapsedGameTime.Milliseconds;
                if (sekon > 1500)
                {
                    if (map[index, 0] > 0)
                    {
                        Random random = new Random();
                        int TxtNum = random.Next(0, 4);
                        MovingNote movingNote = new MovingNote(curGame, spriteBatch, map[index, 0], map[index, 1], TxtNum);
                        curGame.Components.Add(movingNote);
                        MovingNotes.Add(movingNote);
                        StayNote stayNote = new StayNote(curGame, spriteBatch, map[index, 0], map[index, 1], TxtNum);
                        curGame.Components.Add(stayNote);
                        StayNotes.Add(stayNote);
                        sekon -= 1500;
                    }
                    if (map[index, 0] != -1)
                        index++;
                }
                for (int i = 0; i < MovingNotes.Count; i++)
                {
                    if (MovingNotes[i].NotePositon.X > 800)
                    {
                        for (int j = 0; j < StayNotes.Count; j++)
                        {
                            if (Vector2.Distance(StayNotes[j].NotePositon, MovingNotes[i].tapPoint) == 0f)
                            {
                                StayNotes[j].handleInput(StayNotes[j].NotePositon);
                                StayNotes.RemoveAt(j);
                            }
                        }
                        curGame.Components.Remove(MovingNotes[i]);
                        MovingNotes.RemoveAt(i);

                    }
                }
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            

            spriteBatch.Begin();
            //DrawLines
            spriteBatch.Draw(line, new Vector2(0, 140), Color.White);
            spriteBatch.Draw(line, new Vector2(0, 260), Color.White);
            spriteBatch.Draw(line, new Vector2(0, 380), Color.White);
            spriteBatch.Draw(VLine, new Vector2(200, 0), Color.White);
            //Draw HUD
            drawHUD();

            if (Prepare)
                spriteBatch.Draw(Menu, new Vector2(0, 150),null, Color.White,0f,Vector2.Zero,2.2f,SpriteEffects.None,0);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        
    }
}

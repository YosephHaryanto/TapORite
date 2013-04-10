using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameStateManagement;

namespace TapORite
{
    class MainMenuScreen: MenuScreen
    {
        public MainMenuScreen():base (String.Empty)
        {
            IsPopup = true;
            MenuEntry playMenuEntry = new MenuEntry("Play");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");
            
            playMenuEntry.Selected += PlayMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(playMenuEntry);
            MenuEntries.Add(exitMenuEntry);
           

        }
        protected override void UpdateMenuEntryLocations()
        {
            base.UpdateMenuEntryLocations();
            foreach (var entry in MenuEntries)
            {
                var position = entry.Position;
                position.Y += 80;
                entry.Position = position;
            }
        }
        void PlayMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new GamePlayScreen(ScreenManager.Game,ScreenManager.SpriteBatch), null);

        }
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            ScreenManager.Game.Exit();
            base.OnCancel(playerIndex);
        }
    }
}

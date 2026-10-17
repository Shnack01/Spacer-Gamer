using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacerGamer.Collisions;

namespace SpacerGamer
{
    public class ShipUpgrade : GameScreen
    {
        private PlayerData playerData;
        private GamePadState priorGamePadState;
        private Texture2D buttonTexture;
        private Rectangle splitButtonBounds = new Rectangle(300, 150, 200, 75);
        private Rectangle rayButtonBounds = new Rectangle(300, 250, 200, 75);
        private Rectangle singleButtonBounds = new Rectangle(300, 350, 200, 75);
        private Rectangle backgroundBounds = new Rectangle(180, 50, 460, 75);
        private MouseState mouseState = new MouseState();
        private GamePadState gamePadState = new GamePadState();
        private KeyboardState keyboardState = new KeyboardState();
        private KeyboardState priorKeyboardState;
        private ShipBase _ship;
        private SingleShot singleShot;
        private SplitShot splitShot;
        private DeathRay deathRay;


        public ShipUpgrade(Game game, ShipBase ship, PlayerData p, HashSet<WeaponBase> w)
        {
            _ship = ship;
            playerData = p;
            foreach(WeaponBase b in w) 
            {
                if(b is SingleShot) singleShot = (SingleShot)b;
                else if(b is SplitShot) splitShot = (SplitShot)b;
                else if(b is DeathRay) deathRay = (DeathRay)b;
            }
        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            priorKeyboardState = keyboardState;
            priorGamePadState = gamePadState;
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(0);
            mouseState = Mouse.GetState();
            if((splitButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed) || (gamePadState.IsButtonDown(Buttons.Start) && priorGamePadState.IsButtonUp(Buttons.Start)))
            {
                if(splitShot.unlocked) _ship.weapon = splitShot;
                else if(playerData.Money >= splitShot.cost)
                {
                    splitShot.unlocked = true;
                    playerData.Money -= splitShot.cost;
                }
                
                
            }
            if(((singleButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed) || (gamePadState.IsButtonDown(Buttons.Start) && priorGamePadState.IsButtonUp(Buttons.Start))) && singleShot.unlocked)
            {
                _ship.weapon = singleShot;
                
            }
            if((rayButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed) || (gamePadState.IsButtonDown(Buttons.Start) && priorGamePadState.IsButtonUp(Buttons.Start)))
            {
                if(deathRay.unlocked) _ship.weapon = deathRay;
                else if(playerData.Money >= deathRay.cost)
                {
                    deathRay.unlocked = true;
                    playerData.Money -= deathRay.cost;
                }
                
            }
            if((keyboardState.IsKeyUp(Keys.Escape) && priorKeyboardState.IsKeyDown(Keys.Escape)) || (gamePadState.IsButtonDown(Buttons.Start) && priorGamePadState.IsButtonUp(Buttons.Start)))
            {
                ExitScreen();
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            
        }

        public override void Draw(GameTime gameTime)
        {
            buttonTexture = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            buttonTexture.SetData(new[] { Color.White }); 
            
            var sb = ScreenManager.SpriteBatch;

            sb.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp);
            string m = $"${playerData.Money}";

            sb.DrawString(ScreenManager.MenuFont, m, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - ScreenManager.MenuFont.MeasureString(m).X - 20, 20), Color.White);
            sb.Draw(buttonTexture, backgroundBounds, Color.DarkCyan);
            
            
            sb.Draw(buttonTexture, singleButtonBounds, Color.BlueViolet);
            sb.DrawString(ScreenManager.MenuFont, $"Current Weapon: {_ship.weapon.name}", new Vector2(200, 70), Color.White);
            if(splitShot.unlocked)
            {
                sb.Draw(buttonTexture, splitButtonBounds, Color.BlueViolet);
                sb.DrawString(ScreenManager.MenuFont, $"Split Shot", new Vector2(330, 170), Color.White); 
            }
            else
            { 
                sb.Draw(buttonTexture, splitButtonBounds, Color.DarkGray);
                sb.DrawString(ScreenManager.MenuFont, $"Split Shot     - ${splitShot.cost}", new Vector2(330, 170), Color.White);
            }
            
            if(deathRay.unlocked)
            {
                sb.Draw(buttonTexture, rayButtonBounds, Color.BlueViolet);
                sb.DrawString(ScreenManager.MenuFont, $"Death Ray", new Vector2(320, 270), Color.White);
            }
            else
            {
                sb.Draw(buttonTexture, rayButtonBounds, Color.DarkGray);
                sb.DrawString(ScreenManager.MenuFont, $"Death Ray    - ${deathRay.cost}", new Vector2(320, 270), Color.White);
            }
            sb.DrawString(ScreenManager.MenuFont, $"Single Shot", new Vector2(320, 370), Color.White);

            sb.End();
            base.Draw(gameTime);
        }

        

    }
}
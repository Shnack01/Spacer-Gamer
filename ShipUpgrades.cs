using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacerGamer.Collisions;

namespace SpacerGamer
{
    public class ShipUpgrade : GameScreen
    {       
        private HashSet<WeaponBase> unlockedWeapons = new HashSet<WeaponBase>{new SingleShot()};
        private GamePadState priorGamePadState;
        private Texture2D buttonTexture;
        private Rectangle splitButtonBounds = new Rectangle(300, 150, 200, 75);
        private Rectangle rayButtonBounds = new Rectangle(300, 250, 200, 75);
        private Rectangle singleButtonBounds = new Rectangle(300, 350, 200, 75);
        private MouseState mouseState = new MouseState();
        private GamePadState gamePadState = new GamePadState();
        private KeyboardState keyboardState = new KeyboardState();
        private KeyboardState priorKeyboardState;
        private ShipBase _ship;


        public ShipUpgrade(Game game, ShipBase ship)
        {
            _ship = ship;
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

                _ship.weapon = new SplitShot();
                
            }
            if((singleButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed) || (gamePadState.IsButtonDown(Buttons.Start) && priorGamePadState.IsButtonUp(Buttons.Start)))
            {
                _ship.weapon = unlockedWeapons[0];
                
            }
            if((rayButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed) || (gamePadState.IsButtonDown(Buttons.Start) && priorGamePadState.IsButtonUp(Buttons.Start)))
            {
                _ship.weapon = new DeathRay();
                
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
            sb.Draw(buttonTexture, splitButtonBounds, Color.BlueViolet);   
            sb.Draw(buttonTexture, rayButtonBounds, Color.BlueViolet);
            sb.Draw(buttonTexture, singleButtonBounds, Color.BlueViolet);
            sb.DrawString(ScreenManager.MenuFont, $"Current Weapon: {_ship.weapon.ToString()}", new Vector2(100, 70), Color.White);
            sb.DrawString(ScreenManager.MenuFont, $"Split Shot", new Vector2(330, 170), Color.White);
            sb.DrawString(ScreenManager.MenuFont, $"Death Ray", new Vector2(320, 270), Color.White);
            sb.DrawString(ScreenManager.MenuFont, $"Single Shot", new Vector2(320, 370), Color.White);

            sb.End();
            base.Draw(gameTime);
        }

        

    }
}
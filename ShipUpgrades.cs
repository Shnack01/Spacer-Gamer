using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpacerGamer.Collisions;

namespace SpacerGamer
{
    public class ShipUpgrade : GameScreen
    {
        private Texture2D buttonTexture;
        private Rectangle splitButtonBounds = new Rectangle(300, 250, 200, 75);
        private Rectangle rayButtonBounds = new Rectangle(300, 250, 200, 75);
        private Rectangle singleButtonBounds = new Rectangle(300, 250, 200, 75);


        private SpriteFont _spriteFont;
        private ShipBase _ship;


        public ShipUpgrade(Game game, ShipBase ship)
        {
            _ship = ship;
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
            sb.Draw(buttonTexture, singleButtonBounds, Color.BlueViolet);
            sb.DrawString(ScreenManager.MenuFont, $"{_ship.weapon.ToString()}", new Vector2(200, 200), Color.White);
            sb.End();
            base.Draw(gameTime);
        }

        

    }
}
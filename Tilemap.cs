using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tilemaps;
using MonoGame.Extended.Tilemaps.Rendering;
namespace SpacerGamer
{
    public class TileMap : GameScreen
    {
        private Tilemap _map;
        private TilemapSpriteBatchRenderer _renderer;
        private OrthographicCamera _camera;
        private SpriteBatch _spriteBatch;
        private KeyboardState keyboardState;
        private KeyboardState priorKeyboardState;
        private GamePadState gamePadState;
        private GamePadState priorGamePadState;
        private SpaceShipSprite ship;

        public TileMap(Game game)
        {
            _map = game.Content.Load<Tilemap>("FirstTileSet");

            _renderer = new TilemapSpriteBatchRenderer();
            _renderer.BlendState = BlendState.AlphaBlend;   
            _renderer.LoadTilemap(_map);

            _camera = new OrthographicCamera(game.GraphicsDevice);
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
            
            
        }
       

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            priorKeyboardState = keyboardState;
            priorGamePadState = gamePadState;
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(0);

            _renderer.Update(gameTime);  

            if ((keyboardState.IsKeyUp(Keys.Escape) && priorKeyboardState.IsKeyDown(Keys.Escape)) ||
                (gamePadState.IsButtonDown(Buttons.B) && priorGamePadState.IsButtonUp(Buttons.B)))
            {
                ExitScreen();
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(new Color(28, 28, 28));
            _renderer.Draw(_spriteBatch, _camera);
            base.Draw(gameTime);
        }
    }
}
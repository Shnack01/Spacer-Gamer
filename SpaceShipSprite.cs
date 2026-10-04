using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpacerGamer
{
    public class SpaceShipSprite : ShipBase
    {
        private GamePadState gamePadState;
        private GamePadState priorGamePadState;
        private KeyboardState keyboardState;
        private KeyboardState priorKeyboardState;
        private bool lastDirectionLeft = true;

        public SpaceShipSprite()
            : base(new Vector2(400, 400), maxHealth: 3, textureName: "SpaceShip", scale: 3f, hitboxScale: 0.6f)
        {
            boundsOffset = new Vector2(3f, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            priorGamePadState = gamePadState;
            gamePadState = GamePad.GetState(0);
            priorKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            position += gamePadState.ThumbSticks.Left * new Vector2(3, -3);
            if (gamePadState.ThumbSticks.Left.X < 0) lastDirectionLeft = true;
            if (gamePadState.ThumbSticks.Left.X > 0) lastDirectionLeft = false;

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) position += new Vector2(0, -3);
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) position += new Vector2(0, 3);
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                position += new Vector2(-3, 0);
                lastDirectionLeft = true;
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                position += new Vector2(3, 0);
                lastDirectionLeft = false;
            }

            if ((keyboardState.IsKeyDown(Keys.Space) && priorKeyboardState.IsKeyUp(Keys.Space)) ||
                (gamePadState.IsButtonDown(Buttons.A) && priorGamePadState.IsButtonUp(Buttons.A)))
            {
                Fire();
            }

            if ((keyboardState.IsKeyDown(Keys.E) && priorKeyboardState.IsKeyUp(Keys.E)) ||
                (gamePadState.IsButtonDown(Buttons.RightShoulder) && priorGamePadState.IsButtonUp(Buttons.RightShoulder)))
            {
                lastDirectionLeft = false;
                Dash();
            }
            if ((keyboardState.IsKeyDown(Keys.Q) && priorKeyboardState.IsKeyUp(Keys.Q)) ||
                (gamePadState.IsButtonDown(Buttons.LeftShoulder) && priorGamePadState.IsButtonUp(Buttons.LeftShoulder)))
            {
                lastDirectionLeft = true;
                Dash();
            }

            boundsOffset.Y = texture.Height * scale * 0.1f;
            UpdateBounds();
        }

        public void Dash()
        {
            position += new Vector2(lastDirectionLeft ? -50 : 50, 0);
        }
    }
}
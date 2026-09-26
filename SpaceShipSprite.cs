using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SpacerGamer.Collisions;
namespace SpacerGamer
{
    
    public class SpaceShipSprite
    {

        private GamePadState gamePadState;
        private GamePadState priorGamePadState;

        public int Health {get; set;} = 3;
        private bool alive = true;

        private KeyboardState keyboardState;
        private KeyboardState priorKeyboardState;
        private Texture2D texture;

        private Vector2 position = new Vector2(400, 400);
        private bool lastDirectionLeft = true;
        public Vector2 Posision => position;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(200-16,200-16), 20, 20);

        public BoundingRectangle Bounds => bounds;
        public event Action<Vector2> LaserFired;
        
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("SpaceShip");
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            priorGamePadState = gamePadState;
            gamePadState = GamePad.GetState(0);
            priorKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            
            //Vector2 laserOffset = new Vector2(17f, -24f);

            // Apply the gamepad movement with inverted Y axis
            position += gamePadState.ThumbSticks.Left * new Vector2(3, -3);
            if (gamePadState.ThumbSticks.Left.X < 0) lastDirectionLeft = true;
            if (gamePadState.ThumbSticks.Left.X > 0) lastDirectionLeft = false;

            // Apply keyboard movement
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
            if (keyboardState.IsKeyDown(Keys.Space) && priorKeyboardState.IsKeyUp(Keys.Space) || (gamePadState.IsButtonDown(Buttons.A) && priorGamePadState.IsButtonUp(Buttons.A))){
                
                Vector2 spawnPosition = position + new Vector2(0, -24f);//give it a litle offset so that the laser comes from the nose of the ship
                LaserFired?.Invoke(spawnPosition);
            }
            if(keyboardState.IsKeyDown(Keys.E) && priorKeyboardState.IsKeyUp(Keys.E)|| (gamePadState.IsButtonDown(Buttons.RightShoulder) && priorGamePadState.IsButtonUp(Buttons.RightShoulder)))
            {

                lastDirectionLeft = false;
                Dash();
            }
            if(keyboardState.IsKeyDown(Keys.Q) && priorKeyboardState.IsKeyUp(Keys.Q)|| (gamePadState.IsButtonDown(Buttons.LeftShoulder) && priorGamePadState.IsButtonUp(Buttons.LeftShoulder)))
            {
                lastDirectionLeft = true;
                Dash();
            }


            //update the bounds
            float hitboxScale = .6f; // tune by eye until it feels fair
            float scaledWidth = texture.Width * 3f * hitboxScale;
            float scaledHeight = texture.Height * 2f * hitboxScale;

            float verticalOffset = texture.Height * 3f * 0.1f;

            bounds.X = position.X - (scaledWidth / 2f) + 3f;
            bounds.Y = position.Y - (scaledHeight / 2f) + verticalOffset;
            bounds.Width = scaledWidth;
            bounds.Height = scaledHeight;
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) 
        {
            if(!alive) return;
            SpriteEffects spriteEffects = SpriteEffects.None;
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            spriteBatch.Draw(texture, position, null, Color, 0, origin, 3f, spriteEffects, 0);
        }
        public void Dash()
        {
            if(lastDirectionLeft)
            {
                position += new Vector2(-50, 0);
            }
            else
            {
                position += new Vector2(50, 0);
            }
        }
        public void Explode()
        {
            alive = false;
        }
    }
}

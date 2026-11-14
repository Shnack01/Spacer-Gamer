using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SpacerGamer.Collisions;

namespace SpacerGamer{


    public class EnemyLaser
    {
        private const float DrawScale = 3f;
        private const int SrcWidth = 2;
        private const int SrcHeight = 16;
        private Color color = Color.White;
        private Rectangle laserBounds;
        private MouseState mouseState = new MouseState();

        public float velocity = 8f;
        public float direction = 0f;
        private Texture2D texture;
        private Vector2 position;
        public bool used;
        public Vector2 Posision => position;
        public float sizeMult = 1f;

        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;
        public EnemyLaser(Vector2 shipLocation)
        {
            position = shipLocation;
            bounds = new BoundingRectangle(position, 0, 0);
            UpdateBounds();
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Laser");
            
        }
        private void UpdateBounds()
        {
            float w = SrcWidth * DrawScale * sizeMult;
            float h = SrcHeight * DrawScale * sizeMult;
            bounds.Width = w;
            bounds.Height = h;
            bounds.X = position.X - w / 2f;
            bounds.Y = position.Y - h / 2f;
        }
        public void Update(GameTime gameTime)
        {
            position.X += direction;
            position.Y += velocity;
            UpdateBounds();
        }


        /// <summary>
        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (used) return;
            var source = new Rectangle(16, 0, SrcWidth, SrcHeight);
            Vector2 origin = new Vector2(SrcWidth / 2f, SrcHeight / 2f);
            float rotation = (float)Math.Atan2(-direction, velocity);   // 0 when straight down
            spriteBatch.Draw(texture, position, source, color, rotation, origin, DrawScale * sizeMult, SpriteEffects.None, 0);
        }
    }
}
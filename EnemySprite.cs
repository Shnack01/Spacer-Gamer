using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SpacerGamer.Collisions;
using System.Threading;
using System.Net.Http.Headers;


namespace SpacerGamer
{

    public class EnemySprite
    {
       
        private const float ANIMATION_SPEED = 0.1f;
        private double animationTimer;
        private int animationFrame; 
        private Vector2 position;
        
        public Vector2 Position => position;

        private Texture2D texture;

        private BoundingCircle bounds;
        public bool hit = false;
        public BoundingCircle Bounds => bounds;
        public bool Collected {get; set;} = false;
        public Color color = Color.White;
        public float spawnTimer;



        public EnemySprite(float spawnTimer, Vector2 position)
        {
            this.position = position;
            this.bounds = new BoundingCircle(Position, 24);
            this.spawnTimer = spawnTimer;
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("EnemyShipAnimated");
        }
        public void Update(GameTime gameTime, int wave)
        {

            if (spawnTimer > 0)
            {
            
                spawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds; 
                return; 
            } 
            int speed = 0;
            if(wave < 2) speed = 1;
            else if(wave < 5) speed = 3;
            else if(wave < 10)speed = 5;
            position += new Vector2(0, 3 + speed);
            bounds.Center = position;
        }

        /// <summary>
        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(hit) return;
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if(animationTimer > ANIMATION_SPEED)
            {
                animationFrame++;
                if (animationFrame > 7) animationFrame = 0;
                animationTimer -= ANIMATION_SPEED;
            }
            var source = new Rectangle(animationFrame * 32, 0, 32, 32);
            Vector2 origin = new Vector2(source.Width / 2f, source.Height / 2f);
            spriteBatch.Draw(texture, position, source, color, 0, origin, 3f, SpriteEffects.None, 0);
        }
    }
}

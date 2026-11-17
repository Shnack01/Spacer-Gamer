using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;



namespace SpacerGamer
{
    public enum TargetState
    {
        Looking,
        Found,
        Fire
    }
    public class Target
    {
        private TargetState state = TargetState.Looking;
        protected Texture2D texture;
        
        private EnemyState enemyState = EnemyState.Spawning;
        private const float ANIMATION_SPEED = 0.1f;
        private double animationTimer;
        private int animationFrame;
        private float moveTimer = 0.33f;
        private const float SHOOT_TIMER = 3f;
        private float shootTimer = SHOOT_TIMER;
        public bool readyToFire = false;
        public Vector2 position;
        public Game game;
        public Vector2 playerPosition = new Vector2(300, 300);
        
        
        
        public Target(float spawnTimer, Vector2 position, Game game)
        {
            this.game = game;
            this.position = position;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Target");
        }

        public void Update(GameTime gameTime)
        {
            if(state == TargetState.Fire)
            {
                readyToFire = true;
            }
            if(playerPosition != position && state == TargetState.Looking)
            {
                position += (playerPosition - position)/50;
            }
            if(playerPosition == position && state == TargetState.Looking)
            {
                state = TargetState.Found;
            }
            if(state == TargetState.Found)
            {
                if(shootTimer > 0)
                {
                    shootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    state = TargetState.Fire;
                }
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 3f, SpriteEffects.None, 0f);
        }
    }
}
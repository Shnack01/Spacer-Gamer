using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
enum EnemyState
{
    Spawning,
    Moving,
    Stopped
}


namespace SpacerGamer
{
    public class Sentry : EnemyBase
    {
        private EnemyState enemyState = EnemyState.Spawning;
        private const float ANIMATION_SPEED = 0.1f;
        private double animationTimer;
        private int animationFrame;
        public override int Health { get; set; } = 2;
        private float moveTimer = 0.33f;
        private const float SHOOT_TIMER = 1f;
        private float shootTimer = SHOOT_TIMER;
        public override int points { get; set; } = 200;
        public bool shoot = false;
        
        public Sentry(float spawnTimer, Vector2 position): base(position, spawnTimer, 24)
        {
            
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("ShootingEnemyAnimated");
        }

        public override void Update(GameTime gameTime, int wave)
        {
            if(enemyState == EnemyState.Spawning)
            {
                if (spawnTimer > 0)
                {
                    spawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    return;
                }
                else enemyState = EnemyState.Moving;
            }
            else if(enemyState == EnemyState.Moving)
            {
                if(moveTimer > 0)
                {
                    moveTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    int speed = 1;
                    position += new Vector2(0, 3 + speed);
                    bounds.Center = position;
                    return;
                }
                else enemyState = EnemyState.Stopped;
            }
            else
            {
                if(shootTimer > 0)
                {
                    shootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    return;
                }
                else
                {
                    Shoot();
                    shootTimer = SHOOT_TIMER;
                }
            }

            
        }

        public void Shoot()
        {
            shoot = true;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Destroyed) return;

            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTimer > ANIMATION_SPEED)
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpacerGamer
{
    public class EnemySprite : EnemyBase
    {
        private const float ANIMATION_SPEED = 0.1f;
        private double animationTimer;
        private int animationFrame;


        public EnemySprite(float spawnTimer, Vector2 position)
            : base(position, spawnTimer, 24)
        {
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("EnemyShipAnimated");
        }

        public override void Update(GameTime gameTime, int wave)
        {
            if (spawnTimer > 0)
            {
                spawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }

            int speed = 0;
            if (wave < 2) speed = 1;
            else if (wave < 5) speed = 3;
            else if (wave < 10) speed = 5;

            position += new Vector2(0, 3 + speed);
            bounds.Center = position;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (hit) return;

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
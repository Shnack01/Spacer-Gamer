using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpacerGamer.Collisions;

namespace SpacerGamer
{
    public abstract class EnemyBase : IEnemy
    {
        
        protected Vector2 position;
        protected Texture2D texture;
        protected BoundingCircle bounds;

        public Vector2 Position => position;
        public BoundingCircle Bounds => bounds;
        public virtual int Health { get; set; } = 1;
        public bool Destroyed { get; set; } = false;
        public Color color { get; set; } = Color.White;
        public float spawnTimer { get; set; }

        public virtual int points {get; set;} = 100;

        protected EnemyBase(Vector2 position, float spawnTimer, float radius)
        {
            this.position = position;
            this.spawnTimer = spawnTimer;
            this.bounds = new BoundingCircle(position, radius);
        }

        public abstract void LoadContent(ContentManager content);
        public abstract void Update(GameTime gameTime, int wave);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
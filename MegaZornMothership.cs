using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpacerGamer.Collisions;

namespace SpacerGamer
{
    public class MegaZornMothership : EnemyBase
    {
        public bool spraying = false;
        private int attackNum = 0;
        public bool movingBackHome = false;
        public bool Attacking = false;
        private EnemyState enemyState = EnemyState.Spawning;
        private const float ANIMATION_SPEED = 0.1f;
        private double animationTimer;
        private int animationFrame;
        public override int Health { get; set; } = 10;
        private float moveTimer = 0.33f;
        private const float SHOOT_TIMER = 2f;
        private const float DOUBLE_TIMER = .1f;
        private float shootTimer = SHOOT_TIMER;
        public override int points { get; set; } = 200;
        public bool shoot = false;
        private const float HIT_WIDTH = 480f;
        private const float HIT_HEIGHT = 80f; 
        private BoundingRectangle _hitBox;
        public BoundingRectangle HitBox => _hitBox;
        private GameTime gt;
        public float doubleShotTime = DOUBLE_TIMER;
        private int shotCount = 30;
        private float startPositionX;
        private float age = 0f;
        private float direction = 0f;
        private Vector2 home;
                
        public MegaZornMothership(float spawnTimer, Vector2 position): base(position, spawnTimer, 24)
        {
           _hitBox = new BoundingRectangle(position, HIT_WIDTH, HIT_HEIGHT);
           _hitBox.X = position.X - HIT_WIDTH / 2f;
            _hitBox.Y = position.Y - HIT_HEIGHT / 2f;
            startPositionX = position.X;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("MothershipSprite");
        }

        public override void Update(GameTime gameTime, int wave)
        {
            gt = gameTime;
            age += (float)gameTime.ElapsedGameTime.TotalSeconds;
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (movingBackHome)
            {
                position = Vector2.Lerp(position, home, 1f - (float)Math.Exp(-5f * dt));
                if (Vector2.Distance(position, home) < 0.5f)
                {
                    position = home;
                    movingBackHome = false;
                }
                bounds.Center = position;
                _hitBox.X = position.X - HIT_WIDTH / 2f;
                _hitBox.Y = position.Y - HIT_HEIGHT / 2f;
            }
            
            if(Attacking) position.X = startPositionX + (float)Math.Sin(age * 6f) * 30f;
            if(spraying)
            {
                direction = startPositionX + (float)Math.Sin(age * 6f) * 30f;
            }
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
                    _hitBox.X = position.X - HIT_WIDTH / 2f;
                    _hitBox.Y = position.Y - HIT_HEIGHT / 2f;
                    return;
                }
                else
                {
                    enemyState = EnemyState.Stopped;
                    home = position;
                } 
            }
            else
            {
                if (Attacking || movingBackHome) return;  

                shootTimer -= dt;
                if (shootTimer <= 0)
                {
                    Shoot();
                    shootTimer = SHOOT_TIMER;
                }
            }

            
        }
        public List<EnemyLaser> doubleShootAndMove()
        {
            List<EnemyLaser> lasers = new List<EnemyLaser>();
            Vector2 leftP = new Vector2(position.X - 75,position.Y + 50);
            Vector2 rightP = new Vector2(position.X + 75,position.Y + 50);
            var leftLaser = new EnemyLaser(leftP);
            var rightLaser = new EnemyLaser(rightP);
            leftLaser.velocity = 10f;
            leftLaser.sizeMult = 1.5f;
            rightLaser.velocity = 10f;
            rightLaser.sizeMult = 1.5f;
            lasers.Add(leftLaser);
            lasers.Add(rightLaser);

            return lasers;
        }
        public List<EnemyLaser> spray()
        {
            List<EnemyLaser> lasers = new List<EnemyLaser>();
            Vector2 leftP = new Vector2(position.X - 75,position.Y + 50);
            Vector2 rightP = new Vector2(position.X + 75,position.Y + 50);
            var leftLaser = new EnemyLaser(leftP);
            var rightLaser = new EnemyLaser(rightP);
            leftLaser.velocity = 10f;
            leftLaser.sizeMult = 1.5f;
            rightLaser.velocity = 10f;
            rightLaser.sizeMult = 1.5f;
            lasers.Add(leftLaser);
            lasers.Add(rightLaser);

            return lasers;
        }


        public List<EnemyLaser> Attack()
        {
            List<EnemyLaser> lasers = new List<EnemyLaser>();
            if(attackNum == 0) attackNum = Random.Shared.Next(1, 3);
            switch (attackNum)
            {
                case 1:
                        if(doubleShotTime > 0)
                        {
                            doubleShotTime -= (float)gt.ElapsedGameTime.TotalSeconds;
                            
                        }
                        else
                        {
                            lasers = doubleShootAndMove();
                            shotCount--;
                            doubleShotTime = DOUBLE_TIMER;
                            if (shotCount == 0)
                            {
                                shotCount = 30;
                                movingBackHome = true;
                                Attacking = false;
                                shoot = false;    
                            }
                        }
                        break;
                case 2:
                        if(doubleShotTime > 0)
                        {
                            doubleShotTime -= (float)gt.ElapsedGameTime.TotalSeconds;
                            
                        }
                        else
                        {
                            lasers = doubleShootAndMove();
                            shotCount--;
                            doubleShotTime = DOUBLE_TIMER;
                            if (shotCount == 0)
                            {
                                shotCount = 30;
                                movingBackHome = true;
                                Attacking = false;
                                shoot = false;    
                            }
                        }
                        break;
                default:
                        break;
            }
            
            
            return lasers;
        }
        public void Shoot()
        {
            shoot = true;
            Attacking = true;
            age = 0f;
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
            spriteBatch.Draw(texture, position, null, color, 0, texture.Bounds.Center.ToVector2(), 5f, SpriteEffects.None, 0);
        }
    }
}
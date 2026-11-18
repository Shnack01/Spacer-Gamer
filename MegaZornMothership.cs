using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpacerGamer.Collisions;

public enum AttackingState
{
    Waiting = 0,
    DoubleShoot = 1,
    SprayShoot,
    SniperShoot,
    Moving,
    Attack
}

namespace SpacerGamer
{
    public class MegaZornMothership : EnemyBase
    {
        
        private Target target;
        public AttackingState state = AttackingState.Waiting;
        public Game game;
        private int attackNum = 0;
        private EnemyState enemyState = EnemyState.Spawning;
        private const float ANIMATION_SPEED = 0.1f;
        private double animationTimer;
        private int animationFrame;
        public override int Health { get; set; } = 10;
        private float moveTimer = 0.33f;
        private const float SHOOT_TIMER = 2f;
        private const float DOUBLE_TIMER = .1f;
        private const float SPRAY_TIMER = .04f;
        private float shootTimer = SHOOT_TIMER;
        public override int points { get; set; } = 200;
        public bool shoot = false;
        private const float HIT_WIDTH = 480f;
        private const float HIT_HEIGHT = 80f; 
        private BoundingRectangle _hitBox;
        public BoundingRectangle HitBox => _hitBox;
        public bool rightHit = false;
        public bool centerHit = false;
        public bool leftHit = false;
        private static Texture2D pixel;
        public BoundingCircle centerScreen;
        public BoundingCircle rightScreen;
        public BoundingCircle leftScreen;
        private GameTime gt;
        public float doubleShotTime = DOUBLE_TIMER;
        public float sprayShotTime = SPRAY_TIMER;
        private int shotCount = 30;
        private float startPositionX;
        private float age = 0f;
        private float direction = 0f;
        private Vector2 home;
        public bool doubleShooting = false;
        private const int   SPRAY_SHOTS  = 50;   
        
        private const float SPRAY_SPEED  = 7f;    
        private const float SPRAY_MAX_ANGLE = 60f;  

        private int  sprayShotsFired = 0;
        private bool sweepLeftToRight = true;
        private Vector2 circleOffset = new Vector2(0f, 60f);
        private Vector2 rightCircleOffset = new Vector2(140f, 26f);
        private Vector2 leftCircleOffset = new Vector2(-140f, 26f);
                
        public MegaZornMothership(float spawnTimer, Vector2 position): base(position, spawnTimer, 24)
        {
           _hitBox = new BoundingRectangle(position, HIT_WIDTH, HIT_HEIGHT);
           _hitBox.X = position.X - HIT_WIDTH / 2f;
            _hitBox.Y = position.Y - HIT_HEIGHT / 2f;
            centerScreen = new BoundingCircle(position + circleOffset, 45f);
            leftScreen = new BoundingCircle(position + leftCircleOffset, 38f);
            rightScreen = new BoundingCircle(position + rightCircleOffset, 38f);
            startPositionX = position.X;
        }
        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("MothershipSprite");
        }
        public void Hit()
        {
            
        }
        public bool CollidesWith(BoundingRectangle other)
        {
            centerHit = other.CollidesWith(centerScreen);
            if(centerHit) Hit();
            leftHit   = other.CollidesWith(leftScreen);
            rightHit  = other.CollidesWith(rightScreen);
            return centerHit || leftHit || rightHit;
        }
        public override void Update(GameTime gameTime, int wave)
        {
            gt = gameTime;
            age += (float)gameTime.ElapsedGameTime.TotalSeconds;
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (state == AttackingState.Moving)
            {
                position = Vector2.Lerp(position, home, 1f - (float)Math.Exp(-5f * dt));
                if (Vector2.Distance(position, home) < 0.5f)
                {
                    position = home;
                    state = AttackingState.Waiting;
                }
                bounds.Center = position;
                _hitBox.X = position.X - HIT_WIDTH / 2f;
                _hitBox.Y = position.Y - HIT_HEIGHT / 2f;
            }
            
            if(state == AttackingState.DoubleShoot) position.X = startPositionX + (float)Math.Sin(age * 6f) * 30f;
            if (state == AttackingState.SprayShoot) direction = (float)Math.Sin(age * 6f) * 10f; 
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
                if (state != AttackingState.Waiting) return;

                shootTimer -= dt;
                if (shootTimer <= 0)
                {
                    Shoot();
                    shootTimer = SHOOT_TIMER;
                }
            }
            centerScreen.Center = position + circleOffset;
            leftScreen.Center = position + leftCircleOffset;
            rightScreen.Center = position + rightCircleOffset;
            
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
            var lasers = new List<EnemyLaser>();

            // t goes 0 -> 1 across the whole sweep
            float t = sprayShotsFired / (float)(SPRAY_SHOTS - 1);
            if (!sweepLeftToRight) t = 1f - t;
            float max = MathHelper.ToRadians(SPRAY_MAX_ANGLE);
            float angle = MathHelper.Lerp(-max, max, t);      // -60 deg (left) ... +60 deg (right)

            foreach (float offset in new[] { -75f, 75f })
            {
                var laser = new EnemyLaser(new Vector2(position.X + offset, position.Y + 50));
                laser.direction = (float)Math.Sin(angle) * SPRAY_SPEED;   // sideways
                laser.velocity  = (float)Math.Cos(angle) * SPRAY_SPEED;   // downward
                laser.sizeMult  = 1.5f;
                lasers.Add(laser);
            }
            return lasers;
        }
        public List<EnemyLaser> Snipe()
        {
            float speed = 15f;
            List<EnemyLaser> lasers = new List<EnemyLaser>();
            Vector2 leftP = new Vector2(position.X - 75,position.Y + 50);
            Vector2 leftAim = target.position - leftP;
            leftAim.Normalize();
            Vector2 rightP = new Vector2(position.X + 75,position.Y + 50);
            Vector2 rightAim = target.position - rightP;
            var leftLaser = new EnemyLaser(leftP);
            var rightLaser = new EnemyLaser(rightP);
            rightAim.Normalize();
            leftLaser.direction = leftAim.X * speed;
            leftLaser.velocity = leftAim.Y * speed;

            leftLaser.sizeMult = 2.5f;

            rightLaser.direction = rightAim.X * speed;
            rightLaser.velocity = rightAim.Y * speed;

            rightLaser.sizeMult = 2.5f;
            lasers.Add(leftLaser);
            lasers.Add(rightLaser);
            return lasers;
        }


        public List<EnemyLaser> Attack()
        {
            List<EnemyLaser> lasers = new List<EnemyLaser>();
            if(attackNum == 0) {
                attackNum = Random.Shared.Next(1,4);
                state = (AttackingState)attackNum;
            }
            switch (state)
            {
                case AttackingState.DoubleShoot:
                        
                        if(doubleShotTime > 0)
                        {
                            doubleShotTime -= (float)gt.ElapsedGameTime.TotalSeconds;
                            
                        }
                        else
                        {
                            lasers = doubleShootAndMove();
                            shotCount--;
                            doubleShotTime = DOUBLE_TIMER;
                            if(shotCount == 0)
                            {
                                shotCount = 30;
                                shoot = false;   
                                state = AttackingState.Moving;
                                attackNum = 0; 
                            }
                        }
                        break;

                case AttackingState.SprayShoot:
                        
                        if (sprayShotTime > 0)
                        sprayShotTime -= (float)gt.ElapsedGameTime.TotalSeconds;
                        else
                        {
                            lasers = spray();
                            sprayShotsFired++;
                            sprayShotTime = SPRAY_TIMER;
                            if (sprayShotsFired >= SPRAY_SHOTS)
                            {
                                sprayShotsFired = 0;
                                sweepLeftToRight = !sweepLeftToRight;
                                state = AttackingState.Moving;
                                shoot = false;
                                attackNum = 0;
                            }
                        }

                        break;

                case AttackingState.SniperShoot: 
                        if(target == null)
                        {
                            target = new Target(0f, new Vector2(200, 400), game);
                            target.LoadContent(game.Content);
                            if (game is SpaceGame spaceGame)
                            {
                                spaceGame.targets.Add(target);
                            }
                        }
                        if(target.readyToFire)
                        {
                            lasers = Snipe();
                            target = null;
                            shoot = false;
                            state = AttackingState.Moving;
                            attackNum = 0;
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
            state = AttackingState.Attack;
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

            if (true)
            DrawCircle(spriteBatch, centerScreen.Center, centerScreen.Radius, Color.Red);
            if (true)
            DrawCircle(spriteBatch, rightScreen.Center, rightScreen.Radius, Color.Red);
            if (true)
            DrawCircle(spriteBatch, leftScreen.Center, leftScreen.Radius, Color.Red);
        }
        private void DrawCircle(SpriteBatch spriteBatch, Vector2 center, float radius, Color color, int segments = 48)
        {
            if (pixel == null)
            {
                pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                pixel.SetData(new[] { Color.White });
            }

            float step = MathHelper.TwoPi / segments;
            Vector2 prev = center + new Vector2(radius, 0f);

            for (int i = 1; i <= segments; i++)
            {
                float a = i * step;
                Vector2 next = center + new Vector2((float)Math.Cos(a), (float)Math.Sin(a)) * radius;

                Vector2 edge = next - prev;
                float angle = (float)Math.Atan2(edge.Y, edge.X);
                spriteBatch.Draw(pixel, prev, null, color, angle, Vector2.Zero,
                    new Vector2(edge.Length(), 1f), SpriteEffects.None, 0f);

                prev = next;
            }
        }
    }
}
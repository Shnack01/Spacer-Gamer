using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using SpacerGamer.Collisions;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using SpacerGamer.ParticleStuff;

enum GameState 
{ 
    MainMenu, 
    Playing, 
    GameOver 
}

namespace SpacerGamer{

public class SpaceGame : Game, IParticleEmitter
{
    EngineParticle _engineParticle;
    public Vector2 Position {get; set;}
    public Vector2 Velocity {get; set;}
    private SoundEffect hit;
    private SoundEffect laserFired;
    private SoundEffect boom;
    private float bgScroll = 0f;
    private const float BgSpeed = 60f;
    private float blinkTimer = 0f;
    private const float BlinkDuration = 0.6f;
    private const float BlinkInterval = 0.1f;
    private float waveTransitionTimer = 2f;
    private bool waveCompleted = true;
    private GamePadState gamePadState;
    private GamePadState priorGamePadState;
    GameState currentState = GameState.MainMenu;
    private Spawning spawn;
    private GraphicsDeviceManager _graphics;
    public int screenWidth;
    public int screenHeight;
    private SpriteBatch _spriteBatch;
    private SpaceShipSprite ship;
    private SpriteFont _spriteFont;
    public List<IEnemy> enemys = new List<IEnemy>();
    private List<Laser> lasers = new List<Laser>();
    private List<EnemyLaser> enemyLasers = new List<EnemyLaser>();
    private Texture2D buttonTexture;
    private Rectangle playButtonBounds = new Rectangle(300, 250, 200, 75);
    private Rectangle playAgainButtonBounds;
    private Rectangle mainMenuButtonBounds;
    private MouseState mouseState = new MouseState();
    private Texture2D staticShip;
    private Texture2D staticEnemy;
    private Texture2D _background;
    private Song backGroundMisic;
    
     
    private int score = 0;

    public SpaceGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        screenWidth = _graphics.PreferredBackBufferWidth;
        screenHeight = _graphics.PreferredBackBufferHeight;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
    }

    protected override void Initialize()
    {
        
        ship = new SpaceShipSprite();
        spawn = new Spawning(screenWidth);
        ship.LaserFired += LaserFired;
        _engineParticle = new EngineParticle(this, this);
        Components.Add(_engineParticle);
        
        base.Initialize();
    }
    private void LaserFired(Vector2 shipPosition)
    {
        
        var laser = new Laser(shipPosition);
        laser.LoadContent(Content);
        laserFired.Play();
        lasers.Add(laser);
    }
    private void EnemyLaserFired(Vector2 enemyPosition)
    {
        var laser = new EnemyLaser(enemyPosition);
        laser.LoadContent(Content);
        laserFired.Play();
        enemyLasers.Add(laser);
    }
    private void Restart()
    {
        enemyLasers = new List<EnemyLaser>();
        enemys = new List<IEnemy>();
        lasers = new List<Laser>();
        ship = new SpaceShipSprite();
        ship.LoadContent(Content);
        ship.LaserFired += LaserFired;
        spawn = new Spawning(screenWidth);
        waveCompleted = true;
        waveTransitionTimer = 2f;
        score = 0;
        blinkTimer = 0f;
        MediaPlayer.Play(backGroundMisic);
        _engineParticle = new EngineParticle(this, this);
        Components.Add(_engineParticle);
        currentState = GameState.Playing;

        
    }

    protected override void LoadContent()
    {
        
        _background = Content.Load<Texture2D>("SpaceBackground-export");
        int buttonWidth = 200;
        int buttonHeight = 75;
        playAgainButtonBounds = new Rectangle(
        (GraphicsDevice.Viewport.Width - buttonWidth) / 2, (GraphicsDevice.Viewport.Height - buttonHeight) / 2, buttonWidth, buttonHeight);
        mainMenuButtonBounds = new Rectangle((GraphicsDevice.Viewport.Width - buttonWidth) / 2, (GraphicsDevice.Viewport.Height - buttonHeight + 200) / 2, buttonWidth, buttonHeight);
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        buttonTexture = new Texture2D(GraphicsDevice, 1, 1);
        staticEnemy = Content.Load<Texture2D>("EnemyShip");
        staticShip = Content.Load<Texture2D>("SpaceShip");
        buttonTexture.SetData(new[] { Color.White }); 
        _spriteFont = Content.Load<SpriteFont>("arial");
        ship.LoadContent(Content);
        hit = Content.Load<SoundEffect>("Hit1");
        boom = Content.Load<SoundEffect>("Boom7");
        laserFired = Content.Load<SoundEffect>("LaserFired");
        backGroundMisic = Content.Load<Song>("BeepBox-Song");
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.3f;
        
    }

    protected override void Update(GameTime gameTime)
    {
    if(currentState == GameState.MainMenu)
    {
        _engineParticle.Enabled = false; 
        _engineParticle.Visible = false;
        priorGamePadState = gamePadState;
        gamePadState = GamePad.GetState(0);
        mouseState = Mouse.GetState();
        if((playButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed) || (gamePadState.IsButtonDown(Buttons.Start) && priorGamePadState.IsButtonUp(Buttons.Start))) {currentState = GameState.Playing; Restart();}
        base.Update(gameTime);
        return;
    }
    else if (currentState == GameState.GameOver)
    {
        _engineParticle.Enabled = false; 
        _engineParticle.Visible = false;
        MediaPlayer.Stop();
        priorGamePadState = gamePadState;
        gamePadState = GamePad.GetState(0);
        mouseState = Mouse.GetState();
        if((playAgainButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed) || (gamePadState.IsButtonDown(Buttons.Start) && priorGamePadState.IsButtonUp(Buttons.Start))) 
        {
            Restart();
            currentState = GameState.Playing;
        }
        priorGamePadState = gamePadState;
        gamePadState = GamePad.GetState(0);
        mouseState = Mouse.GetState();
        if((mainMenuButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed) || (gamePadState.IsButtonDown(Buttons.Start) && priorGamePadState.IsButtonUp(Buttons.Start))) currentState = GameState.MainMenu;

        base.Update(gameTime);
        
        return; 
    }
        
        
        ship.Color = Color.White;
        bgScroll += BgSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (bgScroll >= _background.Height) bgScroll = 0f;
        if (blinkTimer > 0) blinkTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (enemys.Count == 0)
        {
            if (!waveCompleted) 
            {
                spawn.wave++;
                waveTransitionTimer = 2f;
                waveCompleted = true;
            }
            else
            {
                waveTransitionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (waveTransitionTimer <= 0)
                {
                    enemys = spawn.MakeEnemies(enemys);
                    waveCompleted = false;
                }
            }
        }
        foreach(IEnemy enemy in enemys)
        {
            enemy.LoadContent(Content);
            enemy.Update(gameTime, spawn.wave);
            enemy.color = Color.White;
        }
        
        ship.Update(gameTime);
        
        Velocity = new Vector2(0, 1); 
        Position = new Vector2(ship.Posision.X + 1, ship.Bounds.Bottom - 10);
        foreach(var laser in enemyLasers)
        {
            laser.Update(gameTime);
            if (laser.used) continue;
                if (laser.Bounds.CollidesWith(ship.Bounds))
                {
                    ship.Health--;
                    blinkTimer = BlinkDuration;
                    laser.used = true;
                    if(ship.Health == 0)
                    {
                        ship.Explode();
                        currentState = GameState.GameOver;
                    }
                }
        }
        foreach (var laser in lasers)
        {
            laser.Update(gameTime);
            if (laser.used) continue;

            foreach(IEnemy enemy in enemys)
            {
                if (enemy.Health == 0 || enemy.spawnTimer > 0) continue;
                if (laser.Bounds.CollidesWith(enemy.Bounds)) 
                {
                    enemy.color = Color.Red;
                    enemy.Health--;
                    if(enemy.Health == 0) 
                    {enemy.Destroyed = true;
                    boom.Play();
                    }

                    laser.used = true;
                    
                    
                    score += enemy.points;
                    break;
                }
            }
            
            
        }
        foreach(IEnemy enemy in enemys)
        {
            if(enemy is Sentry sEnemy)
                {
                    if(sEnemy.shoot)
                    {
                        EnemyLaserFired(sEnemy.Position);
                        sEnemy.shoot = false;
                    }
                }
            if (enemy.Position.Y > screenHeight)
            {
                enemy.Destroyed = true;
            }
            if (enemy.Bounds.CollidesWith(ship.Bounds))
            {
                
                ship.Health--;
                
                blinkTimer = BlinkDuration;
                enemy.Destroyed = true;
                if(ship.Health == 0)
                {
                    boom.Play();
                    ship.Explode();
                    currentState = GameState.GameOver;
                }
                hit.Play();
                ship.Color = Color.Red;
                
            }
        }
        
        
        enemys.RemoveAll(enemy => enemy.Destroyed);
        lasers.RemoveAll(laser => laser.used);

        base.Update(gameTime);
        
    }
    
    //Make a class that takes a string and a Y position, and returns the vector2 where the text should be drawn to be perfectly in the middle of the screen, x wise.
    private Vector2 CenterText(string text, float y) {return new Vector2();}
    protected override void Draw(GameTime gameTime)
    {
        Color space = new Color(28, 28, 28);
        GraphicsDevice.Clear(space);
        
        _spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp);


        if(currentState == GameState.MainMenu)
        {
            _spriteBatch.End();
            Matrix transform = Matrix.CreateTranslation(0, 0, 0);
            _spriteBatch.Begin(transformMatrix: transform, samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            _spriteBatch.Draw(_background, new Vector2(0, _background.Height), Color.White);
            _spriteBatch.End();
            _spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(buttonTexture, playButtonBounds, Color.BlueViolet);
            _spriteBatch.DrawString(_spriteFont, "Play/START", new Vector2(312, 268), Color.White);
            _spriteBatch.DrawString(_spriteFont, "Spacer Gamer", new Vector2(290, 2), Color.SkyBlue);
            _spriteBatch.DrawString(_spriteFont, "Space/A button to Shoot", new Vector2(225, 40), Color.White);
            _spriteBatch.DrawString(_spriteFont, "Q/LB to Dash Left", new Vector2(270, 80), Color.White);
            _spriteBatch.DrawString(_spriteFont, "E/RB to Dash Right", new Vector2(270, 120), Color.White);
            _spriteBatch.Draw(staticShip, new Vector2(250, 200), null, Color.White, MathHelper.ToRadians(60), new Vector2(), 7f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(staticEnemy, new Vector2(630, 50), null, Color.White, MathHelper.ToRadians(60), new Vector2(), 7f, SpriteEffects.None, 0f);
        }
        else if (currentState == GameState.GameOver)
        {
            _spriteBatch.Draw(buttonTexture, playAgainButtonBounds, Color.BlueViolet);
            _spriteBatch.Draw(buttonTexture, mainMenuButtonBounds, Color.BlueViolet);
            string lableMain = "Main Menu";
            Vector2 textMainSize = _spriteFont.MeasureString(lableMain);
            Vector2 textMainPos = new Vector2(
                mainMenuButtonBounds.Center.X - textMainSize.X / 2,
                mainMenuButtonBounds.Center.Y - textMainSize.Y / 2);
            string label = "Play Again";
            Vector2 textPlayAgainSize = _spriteFont.MeasureString(label);
            Vector2 textPlayAgainPos = new Vector2(
                playAgainButtonBounds.Center.X - textPlayAgainSize.X / 2,
                playAgainButtonBounds.Center.Y - textPlayAgainSize.Y / 2);
            string labelWRS = $"Wave Reached: {spawn.wave}     Score: {score}";
            Vector2 textWRSSize = _spriteFont.MeasureString(labelWRS);
            Vector2 textWRSPos = new Vector2(
                (GraphicsDevice.Viewport.Width - textWRSSize.X) / 2,
                130);
            string lableOver = "Game Over";
            Vector2 textOverize = _spriteFont.MeasureString(lableOver);
            Vector2 textOverPos = new Vector2(
                (GraphicsDevice.Viewport.Width - textOverize.X) / 2,
                80);
            _spriteBatch.DrawString(_spriteFont, lableMain, textMainPos, Color.White);
            _spriteBatch.DrawString(_spriteFont, lableOver, textOverPos, Color.Blue);
            _spriteBatch.DrawString(_spriteFont, labelWRS, textWRSPos, Color.Blue);
            _spriteBatch.DrawString(_spriteFont, label, textPlayAgainPos, Color.White);
        }
        else 
        {
            _spriteBatch.End();

            Matrix transform = Matrix.CreateTranslation(0, bgScroll, 0);
            _spriteBatch.Begin(transformMatrix: transform, samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            _spriteBatch.Draw(_background, new Vector2(0, -_background.Height), Color.White);
            _spriteBatch.End();

            _spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp);
            foreach(IEnemy enemy in enemys)
            {
                enemy.Draw(gameTime, _spriteBatch);
            }
            foreach (var laser in lasers)
            {
                laser.Draw(gameTime, _spriteBatch);
            }
            foreach(var laser in enemyLasers)
            {
                laser.Draw(gameTime, _spriteBatch); 
            }
            if(waveCompleted)
            {
                _spriteBatch.DrawString(_spriteFont, $"Wave {spawn.wave}", new Vector2(325,200), Color.Blue);
            }
            _spriteBatch.DrawString(_spriteFont, $"Ship Health: {ship.Health}", new Vector2(2,2), Color.Blue);
            _spriteBatch.DrawString(_spriteFont, $"Wave: {spawn.wave}", new Vector2(650,2), Color.Blue);
            _spriteBatch.DrawString(_spriteFont, $"Score: {score}", new Vector2(325,2), Color.Blue);
            bool visible = blinkTimer <= 0 || (int)(blinkTimer / BlinkInterval) % 2 == 0;
            if (visible) ship.Draw(gameTime, _spriteBatch);
        }
        _spriteBatch.End(); 

        base.Draw(gameTime);
    }
}
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using SpacerGamer.Collisions;
using System.Collections.Generic;
using System.Linq;

enum GameState 
{ 
    MainMenu, 
    Playing, 
    GameOver 
}

namespace SpacerGamer{

public class SpaceGame : Game
{
    GameState currentState = GameState.MainMenu;
    private Spawning spawn;
    private GraphicsDeviceManager _graphics;
    public int screenWidth;
    public int screenHeight;
    private SpriteBatch _spriteBatch;
    private SpaceShipSprite ship;
    private SpriteFont _spriteFont;
    public List<EnemySprite> enemys = new List<EnemySprite>();
    private List<Laser> lasers = new List<Laser>();
    private Texture2D buttonTexture;
    private Rectangle playButtonBounds = new Rectangle(300, 250, 200, 75);
    private MouseState mouseState = new MouseState();
    private Texture2D staticShip;
    private Texture2D staticEnemy;
    
     
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
        base.Initialize();
    }
    private void LaserFired(Vector2 shipPosition)
    {
        var laser = new Laser(shipPosition);
        laser.LoadContent(Content);
        lasers.Add(laser);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        buttonTexture = new Texture2D(GraphicsDevice, 1, 1);
        staticEnemy = Content.Load<Texture2D>("EnemyShip");
        staticShip = Content.Load<Texture2D>("SpaceShip");
        buttonTexture.SetData(new[] { Color.White }); 
        _spriteFont = Content.Load<SpriteFont>("arial");
        ship.LoadContent(Content);
        
    }

    protected override void Update(GameTime gameTime)
    {
    if(currentState == GameState.MainMenu)
    {
        mouseState = Mouse.GetState();
        if(playButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed) currentState = GameState.Playing;
        base.Update(gameTime);
        return;
    }
    else if (currentState == GameState.GameOver)
    {
        base.Update(gameTime);
        return; 
    }
        if (enemys.Count == 0)
        {

            enemys = spawn.MakeEnemies(enemys);
            
        }
        foreach(EnemySprite enemy in enemys)
        {
            enemy.LoadContent(Content);
            enemy.Update(gameTime, spawn.wave);
        }
        
        ship.Update(gameTime);
        foreach (var laser in lasers)
        {
            laser.Update(gameTime);
            if (laser.used) continue;

            foreach(EnemySprite enemy in enemys)
            {
                if (enemy.hit || enemy.spawnTimer > 0) continue;
                if (laser.Bounds.CollidesWith(enemy.Bounds)) 
                {
                    enemy.hit = true;
                    laser.used = true;
                    score += 100;
                    break;
                }
            }
            
            
        }
        foreach(EnemySprite enemy in enemys)
        {
            if (enemy.Position.Y > screenHeight)
            {
                enemy.hit = true;
            }
            if (enemy.Bounds.CollidesWith(ship.Bounds))
            {
                ship.Health--;
                enemy.hit= true;
                if(ship.Health == 0)
                {
                    ship.Explode();
                    currentState = GameState.GameOver;
                }
                
                
            }
        }
        
        
        enemys.RemoveAll(enemy => enemy.hit);
        lasers.RemoveAll(laser => laser.used);
        if(enemys.Count == 0)
        {
            spawn.wave++;
        }
        base.Update(gameTime);
        
    }
    

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
            _spriteBatch.Draw(buttonTexture, playButtonBounds, Color.BlueViolet);
            _spriteBatch.DrawString(_spriteFont, "Play", new Vector2(365, 265), Color.White);
            _spriteBatch.DrawString(_spriteFont, "Spacer Gamer", new Vector2(290, 2), Color.SkyBlue);
            _spriteBatch.DrawString(_spriteFont, "Space/A button to Shoot", new Vector2(225, 40), Color.White);
            _spriteBatch.DrawString(_spriteFont, "L Ctrl/B button to Blink", new Vector2(240, 80), Color.White);
            _spriteBatch.Draw(staticShip, new Vector2(250, 200), null, Color.White, MathHelper.ToRadians(60), new Vector2(), 7f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(staticEnemy, new Vector2(630, 50), null, Color.White, MathHelper.ToRadians(60), new Vector2(), 7f, SpriteEffects.None, 0f);
        }
        else if(currentState == GameState.GameOver)
        {
            _spriteBatch.DrawString(_spriteFont, $"Game Over", new Vector2(300,200), Color.Blue);
            _spriteBatch.DrawString(_spriteFont, $"Wave Reached: {spawn.wave}     Score: {score}", new Vector2(150,250), Color.Blue);
        }
        else 
        {
            _spriteBatch.DrawString(_spriteFont, $"Ship Health: {ship.Health}", new Vector2(2,2), Color.Blue);
            _spriteBatch.DrawString(_spriteFont, $"Wave: {spawn.wave}", new Vector2(650,2), Color.Blue);
            _spriteBatch.DrawString(_spriteFont, $"Score: {score}", new Vector2(325,2), Color.Blue);
            ship.Draw(gameTime, _spriteBatch);
            foreach(EnemySprite enemy in enemys)
            {
                enemy.Draw(gameTime, _spriteBatch);
            }
            foreach (var laser in lasers)
            {
                laser.Draw(gameTime, _spriteBatch);
            }
        }
        _spriteBatch.End(); 

        base.Draw(gameTime);
    }
}
}
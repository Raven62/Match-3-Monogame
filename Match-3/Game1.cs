using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Match_3
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private MainMenu mainMenu;
        private GameOver gameOver;
        private BoardController boardController;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.PreferredBackBufferHeight = 920;
            _graphics.ApplyChanges();

            mainMenu = new MainMenu();
            mainMenu.spritePlay = Content.Load<Texture2D>("ButtonPlay");
            mainMenu.Initialize(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            gameOver = new GameOver();
            gameOver.spriteOk = Content.Load<Texture2D>("ButtonOk");
            gameOver.spriteFont = Content.Load<SpriteFont>("GameOverText");
            gameOver.Initialize(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);



           

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(mainMenu.inputPlay == false && gameOver.inputOk == false) {
                boardController = new BoardController();
                boardController.spriteCell = Content.Load<Texture2D>("Cell");
                boardController.spritePurpleCristal = Content.Load<Texture2D>("PurpleCristal");
                boardController.spriteRedCristal = Content.Load<Texture2D>("RedCristal");
                boardController.spriteGreenCristal = Content.Load<Texture2D>("GreenCristal");
                boardController.spriteBlueCristal = Content.Load<Texture2D>("BlueCristal");
                boardController.spriteYellowCristal = Content.Load<Texture2D>("YellowCristal");
                boardController.spriteFont = Content.Load<SpriteFont>("BoardText");
                boardController.Initialize();
            }

            if(mainMenu.inputPlay == true && boardController.time > 0 && gameOver.inputOk == false)
            {
                boardController.Update();
                boardController.Animation(gameTime);
            }

            if(boardController.time <= 0)
            {
                gameOver.inputOk = true;
                boardController.time = 60;
                mainMenu.inputPlay = false;
            }
            
            if(gameOver.inputOk == false)
            {
                mainMenu.Update();
                
            }
            
            if(gameOver.inputOk == true)
            {
                gameOver.score = boardController.score;
                
                gameOver.Update();

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(139,0,255,255));

            _spriteBatch.Begin();

            if(mainMenu.inputPlay == true || gameOver.inputOk == true)
            {
                boardController.Draw(_spriteBatch);
            }
            
            if(gameOver.inputOk == false && mainMenu.inputPlay == false)
            {
                mainMenu.Draw(_spriteBatch);
            }
            
            if(gameOver.inputOk == true)
            {
                gameOver.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Match_3
{
    class GameOver
    {
        private MouseState mouse;
        public bool inputOk = false;
        public Texture2D spriteOk;
        public SpriteFont spriteFont;
        private Color inputColor = Color.White;
        private int width = 200;
        private int height = 100;
        private Point playPosition;
        public int score;


        public void Initialize(int screenWidth, int screenHeight)
        {
            playPosition.X = (screenWidth / 2) - (width / 2);
            playPosition.Y = (screenHeight / 2) - (height / 2);
        }


        public void Update()
        {
            MouseController();
        }

        private void MouseController()
        {
            mouse = Mouse.GetState();



            if (mouse.Position.X > playPosition.X && mouse.Position.X < playPosition.X + width && mouse.Position.Y > playPosition.Y && mouse.Position.Y < playPosition.Y + height)
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    inputColor = new Color(100, 100, 100, 255);


                }
                else
                {
                    if (inputColor == new Color(100, 100, 100, 255))
                        inputOk = false;
                    inputColor = Color.White;

                }
            }

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (inputOk == true)
            {
                spriteBatch.DrawString(spriteFont, "Game over", new Vector2(400, 10), Color.White);
                spriteBatch.Draw(spriteOk, new Rectangle(playPosition.X, playPosition.Y, width, height), inputColor);
            }

        }

    }





}


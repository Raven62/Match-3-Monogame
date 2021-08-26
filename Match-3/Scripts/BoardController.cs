using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Match_3
{
    public class BoardController
    {

        bool game = false;

        bool waiting;
        bool move = true;
        MouseState mouse;

        int speed = 10000;
        int x = 0;
        Random random;
        bool test = false;
        private int cristalCount = 0;

        private int xSize = 8;
        private int ySize = 8;
        
        public Texture2D spriteCell;
        private Cell cell;
        private Cell[,] cells;
     

        public Texture2D spritePurpleCristal;
        public Texture2D spriteRedCristal;
        public Texture2D spriteGreenCristal;
        public Texture2D spriteBlueCristal;
        public Texture2D spriteYellowCristal;
        private Cristal cristal;
        private Cristal[,] cristals;

       private Cell firstSelectCell;
       private Cell secondSelectCell;

       public SpriteFont spriteFont;
       public int score = 0;
       public int time = 60;
       bool timerOn = false;
       
        public void Initialize()
        {

            
            random = new Random();
            cells = new Cell[xSize, ySize];
            cristals = new Cristal[xSize, ySize];
           
            CreateCells();
            
 

        }





        public void Update()
        {
            
            

            MoveDownCristals();
            CreateCristals();

            if (cristalCount == xSize * ySize)
            {
                speed = 10;
            }

            CheckRow();
            CheckColumn();

            if(cristalCount == xSize * ySize)
            {
                if (timerOn == false)
                {
                    TimeTick();
                }

                MouseController();
            }




        }

        private async void Wait()
        {

            waiting = true;

            await Task.Delay(1000);
            waiting = false;

        }

        private async void TimeTick()
        {
            timerOn = true;

            while (time > 0)
            {
                await Task.Delay(1000);
                time -= 1;
            }
                
           
            


        }

        private void CreateCells()
        {
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    
                    cell = new Cell();
                    cell.cristal = null;
                    cell.spriteCell = spriteCell;
                    cell.x = x;
                    cell.y = y;
                    cell.position.X = x * spriteCell.Width;
                    cell.position.Y = y * spriteCell.Height + 40;
                    cell.color = Color.White;
                    cell.rectangle = new Rectangle(x, y, spriteCell.Width, spriteCell.Height);
                    cells[x, y] = cell;
                    
                }
            }
        }

        private void CreateCristals()
        {
            if (cristalCount < xSize * ySize && x >= 8)
            {
                x = 0;
            }

            if (x < xSize && test == false)
            {
                if (cells[x, 0].cristal == null && cristals[x, 0] == null)
                {
                   
                    cristal = new Cristal();

                    switch (random.Next(1, 5))
                    {
                        case 1:
                            cristal.spriteCristal = spritePurpleCristal;
                            cristal.id = 1;
                            break;

                        case 2:
                            cristal.spriteCristal = spriteRedCristal;
                            cristal.id = 2;
                            break;
                        case 3:
                            cristal.spriteCristal = spriteGreenCristal;
                            cristal.id = 3;
                            break;
                        case 4:
                            cristal.spriteCristal = spriteBlueCristal;
                            cristal.id = 4;
                            break;
                        case 5:
                            cristal.spriteCristal = spriteYellowCristal;
                            cristal.id = 5;
                            break;
                    }

                    cristal.x = x;
                    cristal.y = 0;


                    cristals[x, 0] = cristal;
                    cells[x, 0].cristal = cristal;
                    cristals[x, 0].position.X = cells[x, 0].position.X + 4;
                    cristals[x, 0].position.Y = cells[x, 0].position.Y + 4;

                    x++;
                    cristalCount++;

                }
                else
                {
                    x++;
                }

            }
            

            
        }

        private void MoveDownCristals()
        {
            foreach (var item in cristals)
            {

                if (item != null)
                {

                    if (item.y + 1 < ySize)
                    {
                        if (cells[item.x, item.y + 1].cristal == null && cristals[item.x , item.y + 1] == null && item.move == false)
                        {


                            item.move = true;
                            test = true;


                        }

                        if (item.move == true)
                        {
                            if (item.position.Y < cells[item.x, item.y + 1].position.Y)
                            {

                                item.position.Y += speed;

                            }
                            else
                            {

                               

                                cells[item.x, item.y].cristal = null;
                                cristals[item.x, item.y] = null;
                                item.y++;
                                cristals[item.x, item.y] = item;
                                cells[item.x, item.y].cristal = item;
                                item.position.Y = cells[item.x, item.y].position.Y + 4;
                                item.position.X = cells[item.x, item.y].position.X + 4;
                                item.move = false;
                                test = false;
                            }

                        }

                    }

                }



            }
            
            

        }

        private bool CheckRow()
        {

            if (cristalCount == xSize * ySize && test == false)
            {
               
                foreach (var itemCristal in cristals)
                {

                    bool chek = false;
                    int count = 1;
                    Cristal[] similarCristal = new Cristal[8];

                    similarCristal[0] = itemCristal;

                    if (itemCristal != null)
                    {
                        for (int x = itemCristal.x; x < xSize; x++)
                        {
                            chek = false;

                            if (cristals[x, itemCristal.y] != null)
                            {

                                if (itemCristal.id == cristals[x, itemCristal.y].id)
                                {
                                    foreach (var itemSimilarCristal in similarCristal)
                                    {
                                        if (itemSimilarCristal != null)
                                        {
                                            if (itemSimilarCristal.x != cristals[x, itemCristal.y].x) chek = false;
                                            else chek = true;
                                        }

                                    }
                                    if (chek == false)
                                    {
                                        count++;
                                        similarCristal[count] = cristals[x, itemCristal.y];

                                    }

                                }
                                else
                                {
                                    break;
                                }
                            }

                           
                        }
                    }

                    if (count >= 3)
                    {
                        if(game)
                        {
                            switch (count)
                            {
                                case 3:
                                    score += 10;
                                    break;

                                case 4:
                                    score += 15;
                                    break;
                                case 5:
                                    score += 20;
                                    break;
                                case 6:
                                    score += 30;
                                    break;
                                case 7:
                                    score += 40;
                                    break;
                                case 8:
                                    score += 50;
                                    break;
                            }
                        }
                        
                        
                        foreach (var itemSimilarCristal in similarCristal)
                        {
                            if (itemSimilarCristal != null)
                            {

                                
                                cells[itemSimilarCristal.x, itemSimilarCristal.y].cristal = null;
                                cristals[itemSimilarCristal.x, itemSimilarCristal.y] = null;
                                //MoveDownCristals();
                            }


                        }
                        cristalCount -= count;
                        return true;
                    }
                    
                    

                }


            }
            return false;

        }

        private bool CheckColumn()
        {

            if (cristalCount == xSize * ySize && test == false)
            {

                foreach (var itemCristal in cristals)
                {

                    bool chek = false;
                    int count = 1;
                    Cristal[] similarCristal = new Cristal[8];

                    similarCristal[0] = itemCristal;

                    if (itemCristal != null)
                    {
                        for (int y = itemCristal.y; y < xSize; y++)
                        {
                            chek = false;

                            if (cristals[itemCristal.x, y] != null)
                            {

                                if (itemCristal.id == cristals[itemCristal.x, y].id)
                                {
                                    foreach (var itemSimilarCristal in similarCristal)
                                    {
                                        if (itemSimilarCristal != null)
                                        {
                                            if (itemSimilarCristal.y != cristals[itemCristal.x, y].y) chek = false;
                                            else chek = true;
                                        }

                                    }
                                    if (chek == false)
                                    {
                                        count++;
                                        similarCristal[count] = cristals[itemCristal.x, y];

                                    }

                                }
                                else
                                {
                                    break;
                                }
                            }


                        }
                    }

                    if (count >= 3)
                    {
                        if(game)
                        {
                            switch (count)
                            {
                                case 3:
                                    score += 10;
                                    break;

                                case 4:
                                    score += 15;
                                    break;
                                case 5:
                                    score += 20;
                                    break;
                                case 6:
                                    score += 30;
                                    break;
                                case 7:
                                    score += 40;
                                    break;
                                case 8:
                                    score += 50;
                                    break;
                            }
                        }
                       

                        foreach (var itemSimilarCristal in similarCristal)
                        {
                            if (itemSimilarCristal != null)
                            {

                                
                                cells[itemSimilarCristal.x, itemSimilarCristal.y].cristal = null;
                                cristals[itemSimilarCristal.x, itemSimilarCristal.y] = null;
                                //MoveDownCristals();
                            }


                        }
                        cristalCount -= count;
                        return true;

                    }
                    

                }


            }
            return false;
        }

        private void Permutation()
        {
            int idFirstCristal = 0;
            int idSecondCristal = 0;

            Texture2D spriteFirstCristal;
            Texture2D spriteSecondCristal;



            if (firstSelectCell.x + 1 == secondSelectCell.x && firstSelectCell.y == secondSelectCell.y)
            {



                Wait();

                if (waiting == true)
                {
                   

                    if (cristals[secondSelectCell.x, secondSelectCell.y].position.X - 4 > cells[firstSelectCell.x, firstSelectCell.y].position.X && move == true)
                    {
                        cristals[secondSelectCell.x, secondSelectCell.y].position.X -= 5;
                        cristals[firstSelectCell.x, firstSelectCell.y].position.X += 5;
                       

                    }
                    else 
                    {
                        move = false;
                        if (cristals[secondSelectCell.x, secondSelectCell.y].position.X - 4 > cells[secondSelectCell.x, secondSelectCell.y].position.X)
                        {
                            cristals[secondSelectCell.x, secondSelectCell.y].position.X += 5;
                            cristals[firstSelectCell.x, firstSelectCell.y].position.X -= 5;
                        }
                        else 
                        {
                            move = true;
                            waiting = false;
                        }
                        
                    }
                    

                }
                
                


                if (waiting == false)
                {



                    cristals[secondSelectCell.x, secondSelectCell.y].position.X = cells[secondSelectCell.x, secondSelectCell.y].position.X + 4;
                    cristals[firstSelectCell.x, firstSelectCell.y].position.X = cells[firstSelectCell.x, firstSelectCell.y].position.X + 4;

                    idFirstCristal = firstSelectCell.cristal.id;
                    idSecondCristal = secondSelectCell.cristal.id;

                    spriteFirstCristal = firstSelectCell.cristal.spriteCristal;
                    spriteSecondCristal = secondSelectCell.cristal.spriteCristal;

                    cristals[secondSelectCell.x, secondSelectCell.y].id = idFirstCristal;
                    cristals[secondSelectCell.x, secondSelectCell.y].spriteCristal = spriteFirstCristal;

                    cristals[firstSelectCell.x, firstSelectCell.y].id = idSecondCristal;
                    cristals[firstSelectCell.x, firstSelectCell.y].spriteCristal = spriteSecondCristal;




                    if (CheckRow() == false && CheckColumn() == false)
                    {
                        cristals[secondSelectCell.x, secondSelectCell.y].id = idSecondCristal;
                        cristals[secondSelectCell.x, secondSelectCell.y].spriteCristal = spriteSecondCristal;

                        cristals[firstSelectCell.x, firstSelectCell.y].id = idFirstCristal;
                        cristals[firstSelectCell.x, firstSelectCell.y].spriteCristal = spriteFirstCristal;
                    }

                    firstSelectCell.color = Color.White;
                    secondSelectCell.color = Color.White;
                    secondSelectCell = null;
                    firstSelectCell = null;
                }
                

            }
            else if (firstSelectCell.x - 1 == secondSelectCell.x && firstSelectCell.y == secondSelectCell.y)
            {

                Wait();

                if (waiting == true)
                {


                    if (cristals[secondSelectCell.x, secondSelectCell.y].position.X - 4 < cells[firstSelectCell.x, firstSelectCell.y].position.X && move == true)
                    {
                        cristals[secondSelectCell.x, secondSelectCell.y].position.X += 5;
                        cristals[firstSelectCell.x, firstSelectCell.y].position.X -= 5;


                    }
                    else
                    {
                        move = false;
                        if (cristals[secondSelectCell.x, secondSelectCell.y].position.X - 4 < cells[secondSelectCell.x, secondSelectCell.y].position.X)
                        {
                            cristals[secondSelectCell.x, secondSelectCell.y].position.X -= 5;
                            cristals[firstSelectCell.x, firstSelectCell.y].position.X += 5;
                        }
                        else
                        {
                            move = true;
                            waiting = false;
                        }

                    }


                }


                if (waiting == false)
                {
                    cristals[secondSelectCell.x, secondSelectCell.y].position.X = cells[secondSelectCell.x, secondSelectCell.y].position.X + 4;
                    cristals[firstSelectCell.x, firstSelectCell.y].position.X = cells[firstSelectCell.x, firstSelectCell.y].position.X + 4;

                    idFirstCristal = firstSelectCell.cristal.id;
                    idSecondCristal = secondSelectCell.cristal.id;

                    spriteFirstCristal = firstSelectCell.cristal.spriteCristal;
                    spriteSecondCristal = secondSelectCell.cristal.spriteCristal;


                    cristals[secondSelectCell.x, secondSelectCell.y].id = idFirstCristal;
                    cristals[secondSelectCell.x, secondSelectCell.y].spriteCristal = spriteFirstCristal;

                    cristals[firstSelectCell.x, firstSelectCell.y].id = idSecondCristal;
                    cristals[firstSelectCell.x, firstSelectCell.y].spriteCristal = spriteSecondCristal;

                    if (CheckRow() == false && CheckColumn() == false)
                    {
                        cristals[secondSelectCell.x, secondSelectCell.y].id = idSecondCristal;
                        cristals[secondSelectCell.x, secondSelectCell.y].spriteCristal = spriteSecondCristal;

                        cristals[firstSelectCell.x, firstSelectCell.y].id = idFirstCristal;
                        cristals[firstSelectCell.x, firstSelectCell.y].spriteCristal = spriteFirstCristal;
                    }

                    firstSelectCell.color = Color.White;
                    secondSelectCell.color = Color.White;
                    secondSelectCell = null;
                    firstSelectCell = null;

                }  




            }
            else if (firstSelectCell.y - 1 == secondSelectCell.y && firstSelectCell.x == secondSelectCell.x)
            {

                Wait();

                if (waiting == true)
                {


                    if (cristals[secondSelectCell.x, secondSelectCell.y].position.Y - 4 < cells[firstSelectCell.x, firstSelectCell.y].position.Y && move == true)
                    {
                        cristals[secondSelectCell.x, secondSelectCell.y].position.Y += 5;
                        cristals[firstSelectCell.x, firstSelectCell.y].position.Y -= 5;


                    }
                    else
                    {
                        move = false;
                        if (cristals[secondSelectCell.x, secondSelectCell.y].position.Y - 4 < cells[secondSelectCell.x, secondSelectCell.y].position.Y)
                        {
                            cristals[secondSelectCell.x, secondSelectCell.y].position.Y -= 5;
                            cristals[firstSelectCell.x, firstSelectCell.y].position.Y += 5;
                        }
                        else
                        {
                            move = true;
                            waiting = false;
                        }

                    }


                }

                if (waiting == false)
                {
                    cristals[secondSelectCell.x, secondSelectCell.y].position.Y = cells[secondSelectCell.x, secondSelectCell.y].position.Y + 4;
                    cristals[firstSelectCell.x, firstSelectCell.y].position.Y = cells[firstSelectCell.x, firstSelectCell.y].position.Y + 4;

                    idFirstCristal = firstSelectCell.cristal.id;
                    idSecondCristal = secondSelectCell.cristal.id;

                    spriteFirstCristal = firstSelectCell.cristal.spriteCristal;
                    spriteSecondCristal = secondSelectCell.cristal.spriteCristal;


                    cristals[secondSelectCell.x, secondSelectCell.y].id = idFirstCristal;
                    cristals[secondSelectCell.x, secondSelectCell.y].spriteCristal = spriteFirstCristal;

                    cristals[firstSelectCell.x, firstSelectCell.y].id = idSecondCristal;
                    cristals[firstSelectCell.x, firstSelectCell.y].spriteCristal = spriteSecondCristal;

                    if (CheckRow() == false && CheckColumn() == false)
                    {
                        cristals[secondSelectCell.x, secondSelectCell.y].id = idSecondCristal;
                        cristals[secondSelectCell.x, secondSelectCell.y].spriteCristal = spriteSecondCristal;

                        cristals[firstSelectCell.x, firstSelectCell.y].id = idFirstCristal;
                        cristals[firstSelectCell.x, firstSelectCell.y].spriteCristal = spriteFirstCristal;
                    }

                    firstSelectCell.color = Color.White;
                    secondSelectCell.color = Color.White;
                    secondSelectCell = null;
                    firstSelectCell = null;
                }

                
            }
            else if (firstSelectCell.y + 1 == secondSelectCell.y && firstSelectCell.x == secondSelectCell.x)
            {

                Wait();

                if (waiting == true)
                {


                    if (cristals[secondSelectCell.x, secondSelectCell.y].position.Y - 4 > cells[firstSelectCell.x, firstSelectCell.y].position.Y && move == true)
                    {
                        cristals[secondSelectCell.x, secondSelectCell.y].position.Y -= 5;
                        cristals[firstSelectCell.x, firstSelectCell.y].position.Y += 5;


                    }
                    else
                    {
                        move = false;
                        if (cristals[secondSelectCell.x, secondSelectCell.y].position.Y - 4 > cells[secondSelectCell.x, secondSelectCell.y].position.Y)
                        {
                            cristals[secondSelectCell.x, secondSelectCell.y].position.Y += 5;
                            cristals[firstSelectCell.x, firstSelectCell.y].position.Y -= 5;
                        }
                        else
                        {
                            move = true;
                            waiting = false;
                        }

                    }


                }

                if (waiting == false)
                {

                    cristals[secondSelectCell.x, secondSelectCell.y].position.Y = cells[secondSelectCell.x, secondSelectCell.y].position.Y + 4;
                    cristals[firstSelectCell.x, firstSelectCell.y].position.Y = cells[firstSelectCell.x, firstSelectCell.y].position.Y + 4;

                    idFirstCristal = firstSelectCell.cristal.id;
                    idSecondCristal = secondSelectCell.cristal.id;

                    spriteFirstCristal = firstSelectCell.cristal.spriteCristal;
                    spriteSecondCristal = secondSelectCell.cristal.spriteCristal;


                    cristals[secondSelectCell.x, secondSelectCell.y].id = idFirstCristal;
                    cristals[secondSelectCell.x, secondSelectCell.y].spriteCristal = spriteFirstCristal;

                    cristals[firstSelectCell.x, firstSelectCell.y].id = idSecondCristal;
                    cristals[firstSelectCell.x, firstSelectCell.y].spriteCristal = spriteSecondCristal;

                    if (CheckRow() == false && CheckColumn() == false)
                    {
                        cristals[secondSelectCell.x, secondSelectCell.y].id = idSecondCristal;
                        cristals[secondSelectCell.x, secondSelectCell.y].spriteCristal = spriteSecondCristal;

                        cristals[firstSelectCell.x, firstSelectCell.y].id = idFirstCristal;
                        cristals[firstSelectCell.x, firstSelectCell.y].spriteCristal = spriteFirstCristal;
                    }


                    firstSelectCell.color = Color.White;
                    secondSelectCell.color = Color.White;
                    secondSelectCell = null;
                    firstSelectCell = null;
                }

                
            }
            else
            {
                firstSelectCell.color = Color.White;
                secondSelectCell.color = Color.White;
                firstSelectCell = secondSelectCell;
                secondSelectCell = null;
                firstSelectCell.color = Color.Red;
            }

        }

        private void MouseController()
        {
            
           

            mouse = Mouse.GetState();

                int mouseX = 0;
                int mouseY = 0;


                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    game = true;
                    mouseX = mouse.X / spriteCell.Width;
                    mouseY = (mouse.Y - 40) / spriteCell.Height;

                    if(mouseX >= 0 && mouseY >= 0 && mouseX < xSize  && mouseY < ySize)
                    {

                    

                            if (secondSelectCell == null && firstSelectCell != null)
                            {
                               if(firstSelectCell.x != cells[mouseX, mouseY].x || firstSelectCell.y != cells[mouseX, mouseY].y)
                               {
                                     Console.WriteLine(mouseX);
                                     Console.WriteLine(mouseY);
                                     secondSelectCell = new Cell();
                                     secondSelectCell = cells[mouseX, mouseY];
                                     secondSelectCell.color = Color.Blue;
                           
                               }

                            }

                       if (firstSelectCell == null)
                       {
                        Console.WriteLine(mouseX);
                        Console.WriteLine(mouseY);
                        firstSelectCell = new Cell();
                        firstSelectCell = cells[mouseX, mouseY];
                        firstSelectCell.color = Color.Red;


                       }

                    


                    }

                   


                }



            if (firstSelectCell != null && secondSelectCell != null)
            {
                Permutation();
            }

        }

        public void Animation(GameTime gameTime)
        {
            foreach (var itemCristal in cristals)
            {
                if (itemCristal != null)
                {
                    itemCristal.currentTime += gameTime.ElapsedGameTime.Milliseconds;
                    if (itemCristal.currentTime > itemCristal.period)
                    {
                        itemCristal.currentTime -= itemCristal.period;
                        itemCristal.currentFrame.X++; 
                        if (itemCristal.currentFrame.X >= itemCristal.spriteSize.X)
                        {
                            itemCristal.currentFrame.X = 0;
                           
                            if (itemCristal.currentFrame.Y >= itemCristal.spriteSize.Y)
                                itemCristal.currentFrame.Y = 0;
                        }
                    }
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var itemCell in cells)
            {
                if(itemCell != null)
                {
                     spriteBatch.Draw(itemCell.spriteCell, itemCell.position, itemCell.color);
                }
               
            }
            foreach (var itemCristal in cristals)
            {
                if(itemCristal != null)
                {
                    spriteBatch.Draw(itemCristal.spriteCristal, itemCristal.position, 
                        new Rectangle(itemCristal.currentFrame.X * itemCristal.frameWidth, itemCristal.currentFrame.Y * itemCristal.frameHeight, itemCristal.frameWidth, itemCristal.frameHeight)
                        , Color.White);
                }
               

            }

            spriteBatch.DrawString(spriteFont, "Timer:", new Vector2(960, 50), Color.White);
            spriteBatch.DrawString(spriteFont, time.ToString(), new Vector2(1010, 50), Color.White);
            spriteBatch.DrawString(spriteFont, "Score:", new Vector2(960, 100), Color.White);
            spriteBatch.DrawString(spriteFont, score.ToString(), new Vector2(960, 130), Color.White);


        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Match_3
{
    public class Cristal 
    {
        public bool move = false;
        public int id;
        public Texture2D spriteCristal;
        public Vector2 position;
        public int x;
        public int y;

        public int frameWidth = 106;
        public int frameHeight = 100;

        public Point currentFrame = new Point(0, 0);
        public Point spriteSize = new Point(5, 0);

        public int currentTime = 0;
        public int period = 90;



        

    }
}

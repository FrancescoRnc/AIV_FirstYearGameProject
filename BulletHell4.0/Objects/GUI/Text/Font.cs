using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    public class Font
    {
        public Texture Texture { get; protected set; }
        public string TextureName { get; protected set; }
        public int FirstVal { get; protected set; }
        public int NumCol { get; protected set; } 
        public float CharW { get; protected set; }
        public float CharH { get; protected set; }
        public float Scale { get; protected set; }


        public Font(string textureName, string texturePath, int numCols, int firstASCIIval, float charWidth, float charHeight, float scale = 1)
        {
            NumCol = numCols;
            FirstVal = firstASCIIval;
            CharW = charWidth;
            CharH = charHeight;
            Texture = new Texture(texturePath);
            TextureName = textureName;
            Scale = scale;
        }

        //costruisce e posiziona il carattere con sprite
        protected Vector2 ComputeOffset(char c) 
        {
            int cVal = c;
            int firstVal = 0;
            int numCol = (int)(Texture.Width / CharW);
            int numRow = (int)(Texture.Height / CharH);

            int delta = cVal - firstVal;
            int x = delta % numCol;
            int y = delta / numCol;

            //animation.SetStartPosition((int)(x * Sprite.Width), (int)(y * Sprite.Height));
            return new Vector2(x * CharW, y * CharH);
        }
    }
}

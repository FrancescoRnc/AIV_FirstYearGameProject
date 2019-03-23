using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    public class TextChar : GameObject
    {
        protected char character;
        protected Vector2 textureOffset;
        protected Font font;
        protected float charW;
        protected float charH;
        protected float scale;

        public char Character { get { return character; } set { character = value; ComputeOffset(); } }   


        public TextChar(Vector2 pos, char ch, Font font) : base(pos, font.TextureName, DrawMgr.Layer.GUI)
        {                    
            this.font = font;
            charW = this.font.CharW;
            charH = this.font.CharH;
            scale = font.Scale;

            Sprite = new Sprite(charW * scale, charH * scale);            
            Position = pos;
            Sprite.pivot = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
            Sprite.Camera = CameraMgr.GetCamera("GUI");
            
            Character = ch;
            UpdateMgr.Add(this);
            DrawMgr.Add(this);
        }

        protected void ComputeOffset()
        {
            int cVal = character;
            int firstVal = font.FirstVal;
            int numCol = (int)(texture.Width / charW);
            int numRow = (int)(texture.Height / charH);

            int delta = cVal - firstVal;
            int x = delta % numCol;
            int y = delta / numCol;

            //animation.SetStartPosition((int)(x * charW), (int)(y * charH));
            textureOffset = new Vector2(x * charW, y * charH);
        }


        public override void Draw()
        {
            if (IsActive)
                Sprite.DrawTexture(texture, (int)textureOffset.X, (int)textureOffset.Y, (int)charW, (int)charH);
                //base.Draw();
        }
    }
}

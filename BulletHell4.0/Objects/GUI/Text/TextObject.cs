using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    public class TextObject : IActivable
    {
        protected List<TextChar> sprites;
        protected bool isActive;
        protected string text;

        public Font Font { get; protected set; }

        public Vector2 Position;
        public bool IsActive { get { return isActive; } set { isActive = value; UpdateCharStatus(value); } }
        public string Text { get { return text; } set { SetText(value); } }

        public float TextWidth
        {
            get
            {
                float width = 0;
                for (int i = 0; i < sprites.Count; i++)
                {
                    width += sprites[i].Width;
                }
                return width;
            }
        }


        public TextObject(Vector2 Pos, string textString = "", Font font = null)
        {
            Position = Pos;
            sprites = new List<TextChar>();
            if (font == null)
                font = GfxMgr.GetFont("standard");
            Font = font;
            if (textString != "")
                SetText(textString);

            isActive = true;
        }


        private void SetText(string newText)
        {
            if (newText != text)
            {
                text = newText;
                int numChars = text.Length;
                float charX = Position.X;
                float charY = Position.Y;

                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];

                    if (i > sprites.Count - 1) //add a char
                        sprites.Add(new TextChar(new Vector2(charX, charY), c, Font));
                    else if (c != sprites[i].Character)
                    {
                        sprites[i].Character = c;
                    }

                    charX += sprites[i].Width;
                }
                if (sprites.Count > text.Length) //remove old characters
                {
                    int count = sprites.Count - text.Length;
                    int from = text.Length;

                    //List<TextChar> overflowChars = sprites.GetRange(from, count);
                    //foreach (var chars in overflowChars) 
                    //{
                    //    chars.Destroy();
                    //} //lento

                    for (int i = from; i < sprites.Count; i++)
                    {
                        sprites[i].Destroy();
                    } //veloce

                    sprites.RemoveRange(from, count); //più veloce
                }
            }
        }

        protected virtual void UpdateCharStatus(bool status)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].IsActive = status;
            }
        }
    }
}

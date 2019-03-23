using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace BulletHell4_0
{
    class Button : GameObject
    {
        Vector2 startPos;
        Vector2 endPos;

        public Button(Vector2 pos, string textureName) : base(pos, textureName, DrawMgr.Layer.GUI)
        {
            startPos = new Vector2(Position.X - Width * 0.5f, Position.Y - Height * 0.5f);
            endPos = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);

            Create();
        }

        public bool OnMouseClick()
        {
            if (InputMgr.IsMouseOnArea(startPos, endPos))
            {
                Sprite.scale = new Vector2(1.25f);
                if (InputMgr.IsReleased("MouseLeft"))
                {
                    return true;
                }
            }
            else
            {
                Sprite.scale = new Vector2(1f);
            }
            return false;
        }
    }
}

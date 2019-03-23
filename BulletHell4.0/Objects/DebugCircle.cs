using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    public class DebugCircle : GameObject
    {
        public float Radius;
        public bool Visible;

        public DebugCircle(Vector2 pos, float radius, bool visible) : base(pos, "debug_circle", DrawMgr.Layer.Foreground)
        {
            RescaleCircle(radius);

            Visible = visible;

            Sprite.SetMultiplyTint(1, 1, 1, 0.7f);

            if (Visible)
                Create();
            else
                IsActive = false;
        }

        public void RescaleCircle(float radius)
        {
            Radius = radius;
            Sprite.scale = new Vector2((Radius * 2) / Sprite.Width, (Radius * 2) / Sprite.Height);
        }

        public void Translate(Vector2 newPos)
        {
            Position = newPos;
        }
    }
}

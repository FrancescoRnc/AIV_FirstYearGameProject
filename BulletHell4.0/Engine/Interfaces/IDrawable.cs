using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHell4_0
{
    public interface IDrawable
    {
        DrawMgr.Layer Layer { get; }

        void Draw();
    }
}

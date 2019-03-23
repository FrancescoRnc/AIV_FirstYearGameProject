using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    public struct Collision
    {
        public enum CollisionType { None, RectsIntersection, CircleIntersection, Touch }

        public CollisionType Type;
        public Vector2 Delta;
        public GameObject Collider;
    }
}

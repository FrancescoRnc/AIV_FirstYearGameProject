using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    public class Circle : Collider
    {
        public float Radius { get; set; }
        public RigidBody rigidbody;
        public Vector2 Position { get { return rigidbody.Position + relativePosition; } }
        public Vector2 Center { get { return Position; } }
        public DebugCircle DCircle { get; protected set; }
        
        protected Vector2 relativePosition;


        public Circle(Vector2 offset, float radius, RigidBody owner, bool visible = false)
        {
            relativePosition = offset;
            Radius = radius;
            rigidbody = owner;

            DCircle = new DebugCircle(offset, Radius, visible);
        }


        public bool Contains(Vector2 point)
        {
            Vector2 distance = point - Position;
            return (distance.Length <= Radius);
        }

        public bool Collides(Circle other)
        {
            Vector2 distance = other.Position - Position;
            return (distance.Length <= Radius + other.Radius);
        }

        public void DebugCircleTranslate()
        {
            DCircle.Position = Position;
        }
    }
}

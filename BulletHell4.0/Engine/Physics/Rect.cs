 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    public class Rect : Collider
    {
        public Vector2 RelativePosition;
        public RigidBody rigidbody;
        public Vector2 Position { get { return rigidbody.Position + RelativePosition; } }
        public float HalfWidth { get; protected set; }
        public float HalfHeight { get; protected set; }
        public Vector2 Min { get { return new Vector2(Position.X - HalfWidth, Position.Y - HalfHeight); } }
        public Vector2 Max { get { return new Vector2(Position.X + HalfWidth, Position.Y + HalfHeight); } }

        public Rect(Vector2 offset, RigidBody owner, float width, float height)
        {
            RelativePosition = offset;
            rigidbody = owner;
            HalfWidth = width / 2;
            HalfHeight = height / 2;
        }

        public bool Collides(Rect rect, ref Collision collInfo)
        {
            Vector2 distance = rect.Position - Position;

            float deltaX = Math.Abs(distance.X) - (HalfWidth + rect.HalfWidth);
            float deltaY = Math.Abs(distance.Y) - (HalfHeight + rect.HalfHeight);

            if (deltaX <= 0 && deltaY <= 0)
            {
                collInfo.Type = Collision.CollisionType.RectsIntersection;
                collInfo.Delta = new Vector2(-deltaX, -deltaY);
                return true;
            }
            collInfo.Type = Collision.CollisionType.None;
            return false;
        }

        public bool Collides(Circle circle)
        {
            float left = rigidbody.Position.X - HalfWidth;
            float right = rigidbody.Position.X + HalfWidth;
            float top = rigidbody.Position.Y - HalfHeight;
            float bottom = rigidbody.Position.Y + HalfHeight;

            float nearestX = Math.Max(left, Math.Min(circle.Position.X, right));
            float nearestY = Math.Max(top, Math.Min(circle.Position.Y, bottom));

            float deltaX = circle.Position.X - nearestX;
            float deltaY = circle.Position.Y - nearestY;

            return (deltaX * deltaX + deltaY * deltaY <= circle.Radius * circle.Radius);
        }
    }
}

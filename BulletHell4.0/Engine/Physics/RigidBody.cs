using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    public class RigidBody : IUpdatable
    {
        public GameObject Owner { get; protected set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; set; }
        public Circle BoundingCircle { get; set; }
        public Rect BoundingBox { get; set; }
        public List<Collider> Colliders;
        public float HalfHeight { get { return BoundingBox != null ? BoundingBox.HalfHeight : BoundingCircle.Radius; } }
        public float HalfWidth { get { return BoundingBox != null ? BoundingBox.HalfWidth : BoundingCircle.Radius; } }
        public bool GravityAffection { get; set; }
        public bool CollisionAffection { get; set; }
        public uint Type;

        protected uint CollisionMask;

        public RigidBody(Vector2 pos, GameObject owner, Circle bCircle = null, Rect bBox = null, bool createbBox = true)
        {
            Position = pos;
            Owner = owner;
            AddCollider(bCircle, bBox, createbBox);
            CollisionAffection = true;
            PhysicsMgr.Add(this);
        }
        public RigidBody(Vector2 pos, GameObject owner)
        {
            Position = pos;
            Owner = owner;

            CollisionAffection = true;
            PhysicsMgr.Add(this);
        }


        public void SetBCircleRadius(float newRadius)
        {
            BoundingCircle.Radius = newRadius;
            BoundingCircle.DCircle.RescaleCircle(newRadius);
        }

        public void SetDebugCircle(bool condition)
        {
            BoundingCircle.DCircle.Visible = condition;
            BoundingCircle.DCircle.IsActive = condition;
        }

        public void Destroy()
        {
            if (BoundingBox != null)
            {
                BoundingBox.rigidbody = null;
                BoundingBox = null;
            }
            if (BoundingCircle != null)
            {
                BoundingCircle.rigidbody = null;
                BoundingCircle = null;
            }
            Owner = null;
        }

        void AddCollider(Circle c, Rect b, bool create)
        {
            if (c == null)
            {
                float ray = (float)Math.Sqrt(Owner.Width * Owner.Width + Owner.Height * Owner.Height) / 2;
                BoundingCircle = new Circle(Vector2.Zero, ray, this);
            }
            else
            {
                BoundingCircle = c;
                BoundingCircle.rigidbody = this;
            }
            if (b == null)
            {
                if (create)
                {
                    BoundingBox = new Rect(Vector2.Zero, this, Owner.Width, Owner.Height);
                }
            }
            else
            {
                BoundingBox = b;
                BoundingBox.rigidbody = this;
            }
        }
        void AddCollider(Collider c)
        {
            Colliders.Add(c);
        }

        public bool Collides(RigidBody other, ref Collision collInfo)
        {
            if (BoundingCircle.Collides(other.BoundingCircle))
            {
                if (BoundingBox != null && other.BoundingBox != null)
                {
                    return BoundingBox.Collides(other.BoundingBox, ref collInfo);
                }
                else
                {
                    if (BoundingBox != null)
                    {
                        return BoundingBox.Collides(other.BoundingCircle);
                    }
                    else if (other.BoundingBox != null)
                    {
                        return other.BoundingBox.Collides(BoundingCircle);
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckCollisionWith(RigidBody rb)
        {
            return (CollisionMask & rb.Type) != 0;
        }

        public void SetCollisionMask(uint mask)
        {
            CollisionMask = mask;
        }

        public void AddCollision(uint mask)
        {
            CollisionMask |= mask;
        }


        public void Update()
        {
            if (GravityAffection)
                Velocity += new Vector2(0, PhysicsMgr.Gravity) * Game.window.deltaTime;
            Position += Velocity * Game.window.deltaTime;
            BoundingCircle.DebugCircleTranslate();
            
        }
    }
}

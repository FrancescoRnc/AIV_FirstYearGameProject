using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace BulletHell4_0
{
    class Bullet : GameObject
    {
        public BulletType Type;
        public Ship Owner;

        public Bullet(string textureName) : base(Vector2.Zero, textureName, DrawMgr.Layer.Midground)
        {
            IsActive = false;

            Circle bCircle = new Circle(Vector2.Zero, 8, null);
            Rigidbody = new RigidBody(Position, this, bCircle, null, false);
        }


        public virtual void Spawn(Ship owner, Vector2 pos, Vector2 offset, Vector2 dir, float vel)
        {
            Owner = owner;
            Rotation = owner.Rotation;
            Position = pos + offset;
            Velocity = vel * dir;
            IsActive = true;
            Create();
            PhysicsMgr.Add(Rigidbody);
            Rigidbody.SetDebugCircle(true);
        }

        public override void OnDie()
        {
            IsActive = false;
            UpdateMgr.Remove(this);
            DrawMgr.Remove(this);
            //RemoveRigidbody();
            Rigidbody.SetDebugCircle(false);
            //PhysicsMgr.Remove(Rigidbody);
            BulletMgr.RecoverBullet(this);
            Owner = null;
        }

        void CheckOffScreen()
        {
            if ((Position.X < -50 || Position.X > Game.window.Width + 50) || (Position.Y < -50 || Position.Y > Game.window.Height + 50))
            {
                OnDie();
            }
        }


        public override void Update()
        {
            base.Update();

            CheckOffScreen();
        }
    }
}

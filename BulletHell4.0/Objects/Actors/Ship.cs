using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Audio;
using OpenTK;

namespace BulletHell4_0
{
    public abstract class Ship : GameObject
    {
        public int TotalLifePoints;
        public int LifePoints;

        protected Timer shootTime;
        protected BulletType bulletType;
        protected float shootSpeed;

        protected AudioSource source;
        protected AudioClip shootClip;
        

        public Ship(Vector2 pos, string textureName) : base(pos, textureName, DrawMgr.Layer.Playground)
        {
            Circle bCircle = new Circle(Vector2.Zero, Width / 2, null);
            Rigidbody = new RigidBody(Sprite.position, this, bCircle, null, false); 

            Physics.AddCollider(bCircle);

            source = new AudioSource();
            source.Volume = 0.5f;
            shootClip = AudioMgr.GetClip("laser");

            Create();
        }


        public void PlayShootSound()
        {
            source.Play(shootClip);
        }

        public virtual void GetHitted(int amount)
        {
            LifePoints -= amount;
            if (LifePoints <= 0)
            {
                OnDie();
            }
        } 

        public override void OnDie()
        {
            base.OnDie();
            Rigidbody.SetDebugCircle(false);
            Explosion explode = new Explosion(Position);
            explode.PlayExplodeSound(source);
        }

        public virtual void OnShooting()
        {

        }

        public virtual void Shoot(Vector2 offset)
        {
            //BulletMgr.PrintBulletQuantity();
            PlayShootSound();
        }


        public override void Update()
        {
            BaseUpdate();
        }
    }
}

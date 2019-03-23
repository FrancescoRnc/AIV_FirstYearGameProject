using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace BulletHell4_0
{
    class PlayerBullet : Bullet
    {
        public PlayerBullet() : base("bullet_player")
        {
            Sprite.pivot += new Vector2(11, 0);
            Rigidbody.Type = (uint)ColliderType.PlayerBullet;
            Rigidbody.SetCollisionMask((uint)ColliderType.Enemy | (uint)ColliderType.EnemyBullet);

            Type = BulletType.Player;
        }

        public override void OnCollide(Collision collInfo)
        {
            if (collInfo.Collider is Enemy)
            {
                OnDie();
            }
        }
    }
}

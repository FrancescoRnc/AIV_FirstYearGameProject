using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace BulletHell4_0
{
    class EnemyBullet : Bullet
    {
        public EnemyBullet() : base("bullet_enemy")
        {
            Sprite.pivot += new Vector2(11, 0);
            Rigidbody.Type = (uint)ColliderType.EnemyBullet;
            Rigidbody.SetCollisionMask((uint)ColliderType.Player | (uint)ColliderType.PlayerBullet);

            Type = BulletType.Enemy;
        }

        public override void OnCollide(Collision collInfo)
        {
            if (collInfo.Collider is Player)
            {
                OnDie();
            }
        }
    }
}

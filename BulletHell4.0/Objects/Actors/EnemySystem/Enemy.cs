using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace BulletHell4_0
{
    public class Enemy : Ship
    {
        public EnemyType Type;
        public int DirectionToGo;

        protected Player target;
        protected float deltaMove;
        float moveTime;
        float maxT;
        protected int scoreValue;
        protected int scoreMultiply;

        public Enemy(Vector2 pos, string textureName, int dirToGo = 1) : base(pos, textureName)
        {
            Rotation = MathHelper.DegreesToRadians(90);

            Rigidbody.SetBCircleRadius(25);
            Rigidbody.Type = (uint)ColliderType.Enemy;
            Rigidbody.SetCollisionMask((uint)ColliderType.Player | (uint)ColliderType.PlayerBullet);
            DirectionToGo = dirToGo;

            shootTime = new Timer(0, 0.5f);
            maxT = (RNG.GetRNG(0, 10) * 0.1f) + 6;
            bulletType = BulletType.Enemy;
            shootSpeed = 500;
            
            scoreValue = 100;
            scoreMultiply = ((int)(Type) + 1);
            Type = EnemyType.Normal;

            LifePoints = TotalLifePoints = 10;

            target = ((PlayScene)Game.CurrentScene).Player;
        }


        public virtual void Movement()
        {
            moveTime += Game.window.deltaTime;
            deltaMove += Game.window.deltaTime * 9;
            float x = (float)Math.Cos(deltaMove);
            float y = (float)Math.Sin(deltaMove);

            Vector2 bonusVelocity = new Vector2(100 * DirectionToGo, 0);

            if (moveTime > maxT)
            {
                bonusVelocity = Vector2.Zero;
                maxT = (RNG.GetRNG(0, 10) * 0.1f) + 6.5f;
            }
                                    
            LookAt(target.Position);
            
            Velocity = (new Vector2(x, y) * DirectionToGo * 300) - bonusVelocity;
        }

        public override void OnShooting()
        {
            if (target.IsActive)
                shootTime.Update(Game.window.deltaTime);

            if (shootTime.EndTime())
            {
                Shoot((Right * -15) + (Up * 40));
                Shoot((Right * 13) + (Up * 40));

                shootTime.Reset();
            }
        }

        public override void Shoot(Vector2 offset)
        {
            EnemyBullet bullet = (EnemyBullet)BulletMgr.GetBullet(bulletType);

            if (bullet != null)
            {
                bullet.Spawn(this, Position, offset, Up, shootSpeed);
                base.Shoot(offset);
            }
        }

        public override void OnCollide(Collision collInfo)
        {
            if (collInfo.Collider is PlayerBullet)
            {
                GetHitted(1);
                ScorePointsSystem.IncreaseScore(1);
            }

            if (collInfo.Collider is Player)
            {
                if (!target.Invincibility)
                {
                    GetHitted(LifePoints);
                }
                //ScorePointsSystem.IncreaseScore(1);
            }
        }

        public override void OnDie()
        {
            base.OnDie();
            EnemyMgr.RemoveEnemy(this);
            ScorePointsSystem.IncreaseScore(scoreValue * scoreMultiply);
        }


        public override void Update()
        {
            Movement();

            BaseUpdate();

            OnShooting();
        }
    }
}

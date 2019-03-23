using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace BulletHell4_0
{
    public class EnemyBoss : Enemy
    {
        LifeBar lifebar;


        public EnemyBoss(Vector2 pos, string textureName) : base(pos, textureName)
        {
            Rigidbody.SetBCircleRadius(55);

            shootTime = new Timer(0, 0.7f);
            scoreMultiply = ((int)(Type) + 1) * 5;
            Type = EnemyType.Boss;

            lifebar = new LifeBar(this, Position - new Vector2(0, -80));
            LifePoints = TotalLifePoints = 200;
        }


        public override void Movement()
        {
            if (Position.Y >= 110)
            {
                deltaMove += Game.window.deltaTime;
                float moveDir = (float)Math.Cos(deltaMove);

                Velocity = new Vector2(moveDir * 500f, 0);
            }
            else
            {
                Velocity = new Vector2(0, 50);
            }
        }

        public override void OnShooting()
        {
            if (target.IsActive)
                shootTime.Update(Game.window.deltaTime);

            if (shootTime.EndTime())
            {
                Shoot((Up * 38.5f) + (Right * 89));
                //Shoot((Up * 42.5f) + (Right * 73));
                Shoot((Up * 46.5f) + (Right * 57));
                //Shoot((Up * 50.5f) + (Right * 41));
                Shoot((Up * 54.5f) + (Right * 25));
                Shoot((Up * 54.5f) + (Right * -25));
                //Shoot((Up * 50.5f) + (Right * -41));
                Shoot((Up * 46.5f) + (Right * -57));
                //Shoot((Up * 42.5f) + (Right * -73));
                Shoot((Up * 38.5f) + (Right * -89));

                shootTime.Reset();
            }
        }

        public override void GetHitted(int amount)
        {
            base.GetHitted(amount);
            lifebar.UpdateLife();
        }

        public override void OnDie()
        {
            base.OnDie();
            lifebar.IsActive = false;
        }


        public override void Update()
        {
            Movement();

            BaseUpdate();

            OnShooting();
        }
    }
}

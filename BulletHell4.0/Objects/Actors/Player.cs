using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using Aiv.Audio;
using OpenTK;

namespace BulletHell4_0
{
    public class Player : Ship
    {
        public int PlayerId { get; protected set; }
        public bool Invincibility;

        //Mouse Movement Input
        protected float rotationSpeed = 3f;
        //Mouse Following Input
        protected float currentSpeed = 300;
        protected float currentHorizontalSpeed;
        protected float currentVerticalSpeed;
                
        protected float additiveShootSpeed = 1;
        //public TextObject Name;
        private GameObject lifeHUD;
        private const float invTimeMax = 2;
        private Timer invTime;

        protected BlinkingEffect effect;
        protected AudioSource damageSource;


        public Player(Vector2 pos, int id = 0) : base(pos, "player")
        {
            PlayerId = id;

            //Sprite.scale = new Vector2(0.7f);

            Rotation = MathHelper.DegreesToRadians(-90);

            Rigidbody.SetBCircleRadius(28);
            Rigidbody.Type = (uint)ColliderType.Player;
            Rigidbody.SetCollisionMask((uint)ColliderType.Enemy | (uint)ColliderType.EnemyBullet);

            shootTime = new Timer(0, 0.1f);
            bulletType = BulletType.Player;
            shootSpeed = 900;

            invTime = new Timer(0, invTimeMax);

            LifePoints = TotalLifePoints = 3;
            lifeHUD = new GameObject(new Vector2(), "player_life_HUD", DrawMgr.Layer.GUI);
            lifeHUD.Create();

            effect = new BlinkingEffect(this, invTimeMax, 12);
            damageSource = new AudioSource();
        }


        public void CheckRotation()
        {
            if (Sprite.Rotation < -Math.PI / 2)
            {
                Sprite.Rotation += rotationSpeed * 1.5f * Game.window.deltaTime;
                if (Sprite.Rotation > -Math.PI / 2)
                    Sprite.Rotation = (float)-Math.PI / 2;
            }
            if (Sprite.Rotation > -Math.PI / 2)
            {
                Sprite.Rotation -= rotationSpeed * 1.5f * Game.window.deltaTime;
                if (Sprite.Rotation < -Math.PI / 2)
                    Sprite.Rotation = (float)-Math.PI / 2;
            }
        }

        public void Input_NormalMovement()
        {
            #region Left&Right
            if (InputMgr.IsPressing("Left"))
            {
                currentHorizontalSpeed = -currentSpeed * 2f;
            }
            else if (InputMgr.IsPressing("Right"))
            {
                currentHorizontalSpeed = currentSpeed * 2f;
            }
            else
            {
                currentHorizontalSpeed = 0;
            }
            #endregion

            #region Down&UP
            if (InputMgr.IsPressing("Up"))
            {
                currentVerticalSpeed = -currentSpeed;
            }
            else if (InputMgr.IsPressing("Down"))
            {
                currentVerticalSpeed = currentSpeed;
            }
            else
            {
                currentVerticalSpeed = 0;
            }
            #endregion

            Velocity = (Game.GlobalRight * currentHorizontalSpeed) + (Game.GlobalUp * currentVerticalSpeed);
        }

        public void Input_MouseFollowing()
        {
            float slowVar = InputMgr.IsPressing("SlowMove") ? 0.5f : 1;

            #region Left&Right
            //if (Game.window.GetKey(KeyCode.A))
            if (InputMgr.IsPressed("Left"))
            {
                currentHorizontalSpeed = -currentSpeed * 2f;
            }
            else if (InputMgr.IsPressed("Right"))
            {
                currentHorizontalSpeed = currentSpeed * 2f;
            }
            else
            {
                currentHorizontalSpeed = 0;
            }
            #endregion

            #region Down&UP
            if (InputMgr.IsPressed("Up"))
            {
                currentVerticalSpeed = -currentSpeed;
            }
            else if (InputMgr.IsPressed("Down"))
            {
                currentVerticalSpeed = currentSpeed;
            }
            else
            {
                currentVerticalSpeed = 0;
            }
            #endregion

            LookAt(InputMgr.MousePosition);

            Velocity = ((Game.GlobalRight * currentHorizontalSpeed) + (Game.GlobalUp * currentVerticalSpeed)) * slowVar;
        }

        public void Input_MouseMovement()
        {
            if (InputMgr.IsPressing("Left"))
            {
                Sprite.Rotation -= rotationSpeed * Game.window.deltaTime;
            }
            else if (InputMgr.IsPressing("Right"))
            {
                Sprite.Rotation += rotationSpeed * Game.window.deltaTime;
            }
            else
            {
                CheckRotation();
            }

            Position = Game.window.mousePosition;
        }

        public override void OnShooting()
        {
            shootTime.Update(Game.window.deltaTime * additiveShootSpeed);
            if (InputMgr.IsPressing("MouseLeft"))
            {
                if (shootTime.EndTime())
                {
                    Shoot((Up * 10) + (Right * -27));
                    Shoot((Up * 10) + (Right * 27));
                    shootTime.Reset();
                }
            }
        }

        public override void Shoot(Vector2 offset)
        {
            PlayerBullet bullet = (PlayerBullet)BulletMgr.GetBullet(bulletType);

            if (bullet != null)
            {
                bullet.Spawn(this, Position, offset, Up, shootSpeed);
                base.Shoot(offset);
            }
        }

        public override void OnCollide(Collision collInfo)
        {
            if (!Invincibility)
            {
                if (collInfo.Collider is EnemyBullet)
                {
                    GetHitted(1);
                }
                if (collInfo.Collider is Enemy /*|| collInfo.Collider is EnemyBoss*/)
                {
                    GetHitted(LifePoints);
                }
            }
        }

        public override void GetHitted(int amount)
        {
            if (!Invincibility)
            {
                base.GetHitted(amount);
                Invincibility = true;
                if (LifePoints > 0)
                {
                    damageSource.Play(AudioMgr.GetClip("crash"));
                }
                effect.BeginEffect();
            }
        }

        public override void OnDie()
        {
            base.OnDie();
            lifeHUD.Destroy();
            AudioMgr.StopBGMClip();
            ((PlayScene)Game.CurrentScene).GameOverScreen();
        }


        public void Input()
        {
            if (IsActive)
            {
                if (Game.numJS > 0)
                {

                }
                else
                {
                    //Input_NormalMovement();
                    //Input_MouseMovement();
                    Input_MouseFollowing();

                    OnShooting();
                }
            }
        }

        public override void Update()
        {
            base.Update();

            if (Invincibility)
            {
                invTime.Update(Game.window.deltaTime);
                if (invTime.EndTime())
                {
                    Invincibility = false;
                    invTime.Reset();
                }
            }
        }

        public override void Draw()
        {
            base.Draw();

            for (int i = 0; i < LifePoints; i++)
            {
                lifeHUD.Position = new Vector2(1230 - (48 * i), Game.WindowHeight - 35);
                lifeHUD.Draw();
            }
        }
    }
}

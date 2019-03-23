using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    public class GameObject : IDrawable, IUpdatable, IActivable
    {
        public Sprite Sprite { get; protected set; }
        public RigidBody Rigidbody { get; protected set; }
        public float Width { get { return Sprite.Width * Sprite.scale.X; } }
        public float Height { get { return Sprite.Height * Sprite.scale.Y; } }

        public float Rotation { get { return Sprite.Rotation; } set { Sprite.Rotation = value; } }
        public virtual Vector2 Position
        {
            get
            {
                if (Rigidbody != null)
                    return Rigidbody.Position;
                return Sprite.position;
            }
            set
            {
                Sprite.position = value;
                if (Rigidbody != null)
                    Rigidbody.Position = value;
            }
        }
        public virtual Vector2 Velocity
        {
            get
            {
                if (Rigidbody != null)
                    return Rigidbody.Velocity;
                return Vector2.Zero;
            }
            set
            {
                if (Rigidbody != null)
                    Rigidbody.Velocity = value;
            }
        }
        public Vector2 Up
        {
            get
            {
                float right = (float)Math.Cos(Rotation);
                float up = (float)Math.Sin(Rotation);
                return new Vector2(right, up);
            }
        }
        public Vector2 Right
        {
            get
            {
                float right = (float)Math.Sin(Rotation);
                float up = (float)Math.Cos(Rotation);
                //float right = (float)Math.Cos(Rotation);
                return new Vector2(right, -up);
            }
        }
        public Vector2 Direction { get { return new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)); } set { Rotation = value.Length; } }

        public DrawMgr.Layer Layer { get { return layer; } }
        public bool IsActive { get; set; }

        protected Texture texture;
        protected DrawMgr.Layer layer;
        protected Animation currentAnimation;
        protected List<Animation> animations;


        public GameObject(Vector2 pos, string textureName, DrawMgr.Layer currLayer = DrawMgr.Layer.Playground)
        {
            Tuple<Texture, List<Animation>> spritesheet = GfxMgr.GetSpritesheet(textureName);
            if (texture == null)
                texture = GfxMgr.GetSpritesheet(textureName).Item1;
            texture = spritesheet.Item1;
            animations = spritesheet.Item2;
            currentAnimation = animations[0];
            Sprite = new Sprite(currentAnimation.FrameWidth, currentAnimation.FrameHeight);
            Sprite.pivot = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
            Sprite.position = pos;
            IsActive = true;
            layer = currLayer;
        }


        protected void Rescale(Vector2 scale)
        {
            Sprite.scale.X = scale.X;
            Sprite.scale.Y = scale.Y;
        }

        public void LookAt(Vector2 pos)
        {
            Vector2 direction = (pos - Position).Normalized();
            Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }

        public virtual void SetCamera(Camera camera)
        {
            Sprite.Camera = camera;
        }

        public virtual void OnCollide(Collision collInfo)
        {

        }

        public virtual void OnDie()
        {
            Destroy();
        }

        public virtual void Create()
        {            
            UpdateMgr.Add(this);
            DrawMgr.Add(this);
        }

        public virtual void Destroy()
        {
            IsActive = false;
            UpdateMgr.Remove(this);
            DrawMgr.Remove(this);
            //if (Rigidbody != null)
            //    PhysicsMgr.Remove(Rigidbody);
        }

        public virtual void RemoveRigidbody()
        {
            if (Rigidbody != null)
                PhysicsMgr.Remove(Rigidbody);
        }

        public virtual void BaseUpdate()
        {
            if (IsActive)
            {
                currentAnimation.Update();
                if (Rigidbody != null)
                    Sprite.position = Rigidbody.Position;
            }
        }


        public virtual void Update()
        {
            BaseUpdate();
        }

        public virtual void Draw()
        {
            if (IsActive)
                Sprite.DrawTexture(texture, (int)currentAnimation.OffsetX, (int)currentAnimation.OffsetY, (int)Sprite.Width, (int)Sprite.Height);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    public class Background : GameObject
    {
        protected Sprite bottomSprite;

        protected Vector2 downScroll;

        public Background(string fileName) : base(Vector2.Zero, fileName, DrawMgr.Layer.Background)
        {
            downScroll = new Vector2(0, 100);
            Sprite.pivot = Vector2.Zero;
            bottomSprite = new Sprite(texture.Width, texture.Height);

            Create();
        }


        public override void SetCamera(Camera camera)
        {
            base.SetCamera(camera);
            bottomSprite.Camera = camera;
        }

        public void ScrollBackground()
        {
            Sprite.position += downScroll * Game.window.deltaTime;

            bottomSprite.position.Y = Sprite.position.Y - Height;
            bottomSprite.position += downScroll * Game.window.deltaTime;

            if (Sprite.position.Y >= Height)
            {
                Sprite.position.Y = bottomSprite.position.Y - Height;

                Sprite first = Sprite;
                Sprite = bottomSprite;
                bottomSprite = first;
            }
        }
        public void DrawOtherBackground()
        {
            bottomSprite.DrawTexture(texture);
        }


        public override void Update()
        {
            ScrollBackground();
        }

        public override void Draw()
        {
            base.Draw();
            DrawOtherBackground();
        }
    }
}

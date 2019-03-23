using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    class LogoScene : Scene
    {
        GameObject background;
        FadingEffect effect;
        float currentTime;
        float totalTime;


        public override void Start()
        {
            base.Start();
            Type = SceneType.Title;
            background = new GameObject(Game.ScreenCenter, "logo", DrawMgr.Layer.Background);
            background.Create();

            totalTime = 6;

            effect = new FadingEffect(background, totalTime);
            effect.BeginEffect();
        }

        public override void OnExit()
        {
            DrawMgr.RemoveAll();
            IsPlaying = false;
        }


        public override void Input()
        {
            if (currentTime < effect.TotalFrameTime)
            {
                if (Game.window.GetKey(KeyCode.Space) || Game.window.mouseLeft)
                {
                    currentTime = 3.1f;
                    effect.CurrentTime = currentTime;
                }
            }
        }

        public override void Update()
        {
            currentTime += Game.window.deltaTime;

            UpdateMgr.Update();

            if (currentTime >= totalTime)
                OnExit();
        }

        public override void Draw()
        {
            DrawMgr.Draw();
        }
    }
}

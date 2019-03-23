using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;


namespace BulletHell4_0
{
    public abstract class Scene
    {
        public SceneType Type;
        public bool IsPlaying { get; set; }
        public Scene PreviousScene { get; set; }
        public Scene NextScene { get; set; }


        public virtual void Start()
        {
            IsPlaying = true;
        }

        public virtual void OnExit()
        {
            IsPlaying = false;
        }

        public virtual void Reset()
        {
            OnExit();
            Start();
        }

        public virtual void QuitGame()
        {
            NextScene = null;
            OnExit();
        }


        public abstract void Input();

        public abstract void Update();

        public abstract void Draw();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace BulletHell4_0
{
    public class MenuScene : Scene
    {
        GameObject background;

        Button playButton;
        Button quitButton;


        public override void Start()
        {
            base.Start();
            Type = SceneType.Menu;
            playButton = new Button(new Vector2(350, 600), "play_button");
            quitButton = new Button(new Vector2(Game.window.Width - 350, 600), "quit_button");
            background = new GameObject(Game.ScreenCenter, "menu_Background", DrawMgr.Layer.Background);
            background.Create();
        }


        public override void OnExit()
        {
            UpdateMgr.RemoveAll();
            DrawMgr.RemoveAll();
            AudioMgr.StopBGMClip();
            IsPlaying = false;
        }

        public void PlayGame()
        {
            OnExit();
        }
        
        
        public override void Input()
        {
            if (playButton.OnMouseClick())
            {
                PlayGame();
            }
            if (quitButton.OnMouseClick())
            {
                QuitGame();
            }
        }

        public override void Update()
        {
            AudioMgr.Update("Menu_ST");
        }

        public override void Draw()
        {
            DrawMgr.Draw();
        }
    }
}

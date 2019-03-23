using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    class GameOverScene : Scene
    {
        GameObject gameoverText;

        Button quitButton;


        public GameOverScene()
        {
            gameoverText = new GameObject(Game.ScreenCenter, "gameover", DrawMgr.Layer.GUI);
        }


        public override void Start()
        {
            base.Start();
            
            gameoverText.Sprite.scale = new Vector2(2f);
            gameoverText.Create();

            quitButton = new Button(new Vector2(Game.ScreenCenter.X, 600), "quit_button");

        }


        public override void Input()
        {
            if (quitButton.OnMouseClick())
            {
                QuitGame();
            }
        }

        public override void Update()
        {
            UpdateMgr.Update();
        }

        public override void Draw()
        {
            DrawMgr.Draw();
            //gameoverSpr.DrawTexture(gameoverTex);
        }
    }
}

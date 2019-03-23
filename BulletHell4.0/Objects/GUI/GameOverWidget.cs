using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace BulletHell4_0
{
    public class GameOverWidget : GameObject
    {
        Button retryButton;
        Button quitButton;
        

        public GameOverWidget() : base(Game.ScreenCenter, "gameover", DrawMgr.Layer.GUI)
        {
            retryButton = new Button(new Vector2(350, 600), "retry_button");
            quitButton = new Button(new Vector2(Game.window.Width - 350, 600), "quit_button");

            Create();
        }

        public override void Update()
        {
            if (retryButton.OnMouseClick())
            {
                ScorePointsSystem.ResetScore();
                Game.ToPrevScene();
            }

            if (quitButton.OnMouseClick())
            {
                ScorePointsSystem.SaveRecordScore();
                Game.Quit();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;


namespace BulletHell4_0
{
    public class PlayScene : Scene
    {
        public bool IsPlayerAlive { get { return Player.IsActive; } }

        public Player Player { get; private set; }
        Background background;
        GameOverWidget goWidget;


        public override void Start()
        {
            base.Start();
            InitializingComponents();
        }

        public override void OnExit()
        {
            DrawMgr.RemoveAll();
            //UpdateMgr.RemoveAll();
            AudioMgr.StopBGMClip();
            base.OnExit();
        }

        

        void InitializingComponents()
        {
            CameraMgr.Init(Game.ScreenCenter, Game.ScreenCenter);

            ScorePointsSystem.Init();
            ScorePointsSystem.LoadRecordScore();

            EnemyMgr.Init();
            BulletMgr.Init();

            background = new Background("background");

            Player = new Player(new Vector2(Game.ScreenCenter.X, 620));
        }
        
        public void GameOverScreen()
        {
            goWidget = new GameOverWidget();
        }


        public override void Input()
        {
            Player.Input();
        }

        public override void Update()
        {
            ScorePointsSystem.GameTimeUpdate(Game.window.deltaTime);
            EnemyMgr.Update();

            if (IsPlayerAlive)
                AudioMgr.Update("Game_ST");
            UpdateMgr.Update();
            PhysicsMgr.Update();
            PhysicsMgr.CheckCollision();
            CameraMgr.Update();

           
        }

        public override void Draw()
        {
            DrawMgr.Draw();
        }
    }
}

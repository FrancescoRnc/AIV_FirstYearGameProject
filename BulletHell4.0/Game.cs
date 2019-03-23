using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    public enum SceneType { Title, Menu, Play }

    static class Game
    {
        static public Window window { get; private set; }

        static public Vector2 ScreenCenter;
        static public Vector2 GlobalUp { get { return new Vector2(0, 1); } }
        static public Vector2 GlobalRight { get { return new Vector2(1, 0); } }

        static public float WindowWidth { get { return PixelToUnit(window.Width); } }
        static public float WindowHeight { get { return PixelToUnit(window.Height); } }
        static public float UnitSize
        {
            get
            {
                if (window.CurrentOrthoGraphicSize == 0)
                    return 1;
                return window.Height / window.CurrentOrthoGraphicSize;
            }
        }

        static public Scene[] AllScenes;
        static public Scene CurrentScene;

        static public int numJS;


        //Inizializzazione
        static Game()
        {
            GameSetting();

            GfxMgr.LoadAll();
            TimeMgr.Init();

            AllScenes = new Scene[]
            {
                new LogoScene(),
                new MenuScene(),
                new PlayScene(),
            };

            for (int i = 0; i < AllScenes.Length; i++)
            {
                if (i - 1 >= 0)
                    AllScenes[i].PreviousScene = AllScenes[i - 1];
                if (i + 1 < AllScenes.Length)
                    AllScenes[i].NextScene = AllScenes[i + 1];
            }

            CurrentScene = AllScenes[(int)SceneType.Title];
            CurrentScene.Start();

            #region Old Scenes
            // titlescene = new TitleScene();
            // menuscene = new MenuScene();
            // playscene = new PlayScene();
            // //gameoverscene = new GameOverScene();
            // 
            // CurrentScene = titlescene;
            // //CurrentScene = playscene;
            // //titlescene.NextScene = playscene;
            // titlescene.NextScene = menuscene;
            // menuscene.PreviousScene = titlescene;
            // menuscene.NextScene = playscene;
            // playscene.PreviousScene = menuscene;
            // //playscene.PreviousScene = titlescene;
            // playscene.NextScene = null;
            // //playscene.NextScene = gameoverscene;
            // //gameoverscene.PreviousScene = playscene;
            // //gameoverscene.NextScene = null;
            // titlescene.Start();
            // //playscene.Start();
            #endregion

            string[] joysticks = window.Joysticks;
            for (int i = 0; i < joysticks.Length; i++)
            {
                if (joysticks[i] != null && joysticks[i] != "Unmapped Controller")
                    numJS++;
            }

            GamepadSetting();
        }


        static public float PixelToUnit(float size)
        {
            return size / UnitSize;
        }

        static void Swap<T>(T a, T b)
        {

        }

        static void GameSetting()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Assets/XML/ScreenSettings.xml");
            XmlNode root = xmlDoc.DocumentElement;

            XmlNode currNode = root.FirstChild;
            int width = int.Parse(currNode.InnerText);
            currNode = currNode.NextSibling;
            int height = int.Parse(currNode.InnerText);
            currNode = currNode.NextSibling;
            string title = currNode.InnerText;
            currNode = currNode.NextSibling;
            bool fs = bool.Parse(currNode.InnerText);
            currNode = currNode.NextSibling;
            float ortho = float.Parse(currNode.InnerText);

            window = new Window(width, height, title, fs);
            window.SetDefaultOrthographicSize(ortho);
            ScreenCenter = new Vector2(window.Width / 2, window.Height / 2);

            AudioMgr.SetVolume(0.35f);
        }

        static void GamepadSetting()
        {
            string[] pads = window.Joysticks;
            for (int i = 0; i < pads.Length; i++)
            {
                if (pads[i] != null && pads[i] != "Unmapped Controller")
                    numJS++;
            }
        }
        
        static public void ToNextScene()
        {
            CurrentScene.OnExit();
            CurrentScene = CurrentScene.NextScene;
            CurrentScene.Start();
        }
        static public void ToPrevScene()
        {
            if (CurrentScene.PreviousScene != null)
            {
                CurrentScene.OnExit();
                CurrentScene = CurrentScene.PreviousScene;
                CurrentScene.Start();
            }
        }
        static public void ToSelectedScene(SceneType index)
        {
            CurrentScene.OnExit();
            CurrentScene = AllScenes[(int)index];
            CurrentScene.Start();
        }
        static public Scene GetScene(SceneType index)
        {
            return AllScenes[(int)index];
        }

        static public void Quit()
        {
            CurrentScene.NextScene = null;
            CurrentScene.OnExit();
        }


        //Metodi
        static void Input()
        {
            CurrentScene.Input();
        }
        static void Update()
        {
            CurrentScene.Update();
        }
        static void Draw()
        {
            CurrentScene.Draw();
        }

        //Metodi principali
        static public void Play()
        {
            while (window.opened)
            {
                #region Exit Conditions
                if (window.GetKey(KeyCode.Esc))
                    return;
                if (!CurrentScene.IsPlaying)
                {
                    if (CurrentScene.NextScene != null)
                    {
                        ToNextScene();
                    }
                    else
                        return;
                }
                #endregion


                Input();

                Update();

                Draw();


                window.Update();
            }
        }
    }
}

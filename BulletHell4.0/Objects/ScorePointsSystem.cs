using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    static public class ScorePointsSystem
    {
        static int totalPoints;
        static int recordPoints;

        static public float GameTime { get; private set; }

        static public TextObject ScorePointsText;
        static public TextObject RecordPointsText;

        static public void Init()
        {
            ScorePointsText = new TextObject(new Vector2(30, Game.window.Height - 20), "Points: " + 0);
            RecordPointsText = new TextObject(new Vector2(Game.ScreenCenter.X - 120, Game.window.Height - 20), "Record: " + recordPoints.ToString());
            //RecordPointsText.IsActive = false;
        }


        static public void GameTimeUpdate(float deltatime)
        {
            if (((PlayScene)Game.CurrentScene).IsPlayerAlive)
                GameTime += deltatime;
        }

        static public void IncreaseScore(int amount)
        {
            totalPoints += amount;
            ScoreTextUpdate(totalPoints);
            if (totalPoints > recordPoints)
                RecordTextUpdate(totalPoints);
        }

        static void ScoreTextUpdate(int amount)
        {
            totalPoints = amount;
            ScorePointsText.Text = "Points: " + totalPoints;
        }

        static public void ResetScore()
        {
            ScoreTextUpdate(0);
        }

        static void RecordTextUpdate(int amount)
        {
            recordPoints = amount;
            RecordPointsText.Text = "Record: " + recordPoints;
        }

        static public void ResetRecord()
        {
            recordPoints = 0;
            SaveRecordScore();
        }

        static public void SaveRecordScore(string file = @"Score_Results.txt")
        {
            using (StreamWriter sw = File.CreateText(file))
            {
                sw.Write(recordPoints);
            }
        }

        static public void LoadRecordScore(string file = @"Score_Results.txt")
        {
            if (File.Exists(file))
            {
                using (StreamReader sr = File.OpenText(file))
                {
                    string s = "";
                    if ((s = sr.ReadLine()) != null)
                    {
                        RecordTextUpdate(int.Parse(s));
                    }
                }
            }
        }
    }
}

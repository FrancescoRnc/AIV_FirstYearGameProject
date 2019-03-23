using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace BulletHell4_0
{
    public enum EnemyType { Normal, Boss }

    static public class EnemyMgr
    {
        #region Score Info
        static int enemyDefeatedCounter;
        static int bossDefeatedCounter;
        #endregion

        static List<Enemy>[] totalEnemies;
        static int enemyDefeatedCounterToBoss;
        static bool isBossPresent { get { return totalEnemies[1].Count == 1; } }
        static Timer spawnTime;
        //static Timer2 spawntime;

        static EnemyAlert[] alerts;


        static public void Init()
        {
            totalEnemies = new List<Enemy>[2];

            for (int i = 0; i < totalEnemies.Length; i++)
            {
                switch ((EnemyType)i)
                {
                    case EnemyType.Normal:
                        totalEnemies[i] = new List<Enemy>();
                        break;
                    case EnemyType.Boss:
                        totalEnemies[i] = new List<Enemy>();
                        break;
                    default:
                        break;
                }
            }

            alerts = new EnemyAlert[2]
            {
                new EnemyAlert(Game.ScreenCenter - new Vector2(625, 0), "alertSx"),
                new EnemyAlert(Game.ScreenCenter + new Vector2(625, 0), "alertDx")
            };

            spawnTime = new Timer(0, 5);
            //spawntime = new Timer2(5);

            //spawntime.Start();

            enemyDefeatedCounterToBoss = 0;
        }


        static public void SpawnEnemy()
        {
            int direction = 0;
            int side = 0;
            while (direction == 0)
            {
                direction = Math.Sign(RNG.GetRNG(0, 2) - 1);
            }
            side = direction == -1 ? 0 : 1;

            float randomY = 100 * (RNG.GetRNG(0, 10) * 0.12f) + 200;

            Vector2 pos = new Vector2(Game.WindowWidth * 0.5f + (900 * direction), randomY);
            totalEnemies[0].Add(new Enemy(pos, "enemy_1", direction));

            alerts[side].StartAlert();
        }
        static public void SpawnBoss(Vector2 pos)
        {
            totalEnemies[1].Add(new EnemyBoss(pos, "boss"));
        }

        static public void RemoveEnemy(Enemy enemy)
        {
            EnemyType index = enemy.Type;
            if (index == EnemyType.Normal)
            {
                enemyDefeatedCounter++;
                if (!isBossPresent)
                {
                    enemyDefeatedCounterToBoss++;
                }
            }
            else if (index == EnemyType.Boss)
            {
                bossDefeatedCounter++;
                enemyDefeatedCounterToBoss = 0;
                spawnTime.MaxTime -= (bossDefeatedCounter * 0.5f);
                Console.WriteLine(spawnTime.MaxTime);
            }
            totalEnemies[(int)index].Remove(enemy);
        }


        static public void Update()
        {
            spawnTime.Update(Game.window.deltaTime);
            //Console.WriteLine(spawntime.CurrentTime); 
            if (totalEnemies[0].Count < 4)
            {
                if (spawnTime.EndTime())/*(spawntime.OnEndTime)*/
                {
                    if (((PlayScene)Game.CurrentScene).IsPlayerAlive)
                    {
                        SpawnEnemy();
                        spawnTime.Reset();
                        //spawntime.Reset();
                    }
                }
            }

            if (totalEnemies[1].Count < 1)
            {
                if (enemyDefeatedCounterToBoss == 5)
                {
                    SpawnBoss(new Vector2(Game.ScreenCenter.X, -50));
                }
            }
        }
    }
}

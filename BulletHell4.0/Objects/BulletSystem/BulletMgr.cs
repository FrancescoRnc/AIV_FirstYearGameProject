using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHell4_0
{
    public enum BulletType { Player, Enemy }

    static class BulletMgr
    {
        static Queue<Bullet>[] allBullets;
        static int typeQuantity = 2;
        static int usedBullets;


        static public void Init()
        {
            allBullets = new Queue<Bullet>[typeQuantity];
            DefiningBullets();
        }


        static void DefiningBullets()
        {
            for (int i = 0; i < typeQuantity; i++)
            {
                allBullets[i] = new Queue<Bullet>();
                switch((BulletType)i)
                {
                    case BulletType.Player:
                        for (int j = 0; j < 150; j++)
                        {
                            allBullets[i].Enqueue(new PlayerBullet());
                        }
                        break;
                    case BulletType.Enemy:
                        for (int j = 0; j < 150; j++)
                        {
                            allBullets[i].Enqueue(new EnemyBullet());
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        static public Bullet GetBullet(BulletType type)
        {
            Bullet b = null;
            int index = (int)type;
            if (allBullets[index].Count > 0)
                b = allBullets[index].Dequeue();
            return b;
        }

        static public void RecoverBullet(Bullet bullet)
        {
            allBullets[(int)bullet.Type].Enqueue(bullet);
            //PrintBulletQuantity();
        }

        static public void IncreaseUsedBullets()
        {
            usedBullets++;
        }

        //static public void PrintBulletQuantity()
        //{
        //    Console.Clear();
        //    Console.WriteLine("Player: " + allBullets[(int)BulletType.Player].Count);
        //    Console.WriteLine("Enemy: " + allBullets[(int)BulletType.Enemy].Count);
        //    //Console.WriteLine(usedBullets);
        //}
    }
}

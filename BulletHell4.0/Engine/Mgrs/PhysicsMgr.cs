using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHell4_0
{
    enum ColliderType : uint { Player = 1, PlayerBullet = 2, Enemy = 4, EnemyBullet = 8, Tile = 99 }

    public static class PhysicsMgr
    {
        static List<RigidBody> items;
        static List<RigidBody> itemsToRemove;
        static public float Gravity = 400;
        static Collision collInfo;

        static PhysicsMgr()
        {
            items = new List<RigidBody>();
            itemsToRemove = new List<RigidBody>();
            collInfo = new Collision();
        }

        static public void Add(RigidBody rb)
        {
            if (!items.Contains(rb))
            items.Add(rb);            
        }

        static public void Remove(RigidBody rb)
        {
            items.Remove(rb);
            itemsToRemove.Add(rb);
        }

        static private void DeleteItemsToRemove()
        {
            if (itemsToRemove.Count > 0)
            {
                for (int i = 0; i < itemsToRemove.Count; i++)
                {
                    items.Remove(itemsToRemove[i]);
                    itemsToRemove[i].Destroy();
                }
                itemsToRemove.Clear();
            }
        }


        static public void Update()
        {
            DeleteItemsToRemove();

            for (int i = 0; i < items.Count; i++)
                if (items[i].Owner.IsActive)
                    items[i].Update();
        }

        static public void CheckCollision()
        {
            DeleteItemsToRemove();

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Owner.IsActive && items[i].CollisionAffection)
                {
                    for (int j = i + 1; j < items.Count; j++)
                    {
                        if (items[j].Owner.IsActive && items[i].CollisionAffection)
                        {
                            bool checkFirst = items[i].CheckCollisionWith(items[j]);
                            bool checkSecond = items[j].CheckCollisionWith(items[i]);

                            if (items[j].Owner.IsActive && items[j].CollisionAffection && items[i].Collides(items[j], ref collInfo))
                            {
                                if (checkFirst)
                                {
                                    collInfo.Collider = items[j].Owner;
                                    items[i].Owner.OnCollide(collInfo);

                                }
                                if (checkSecond)
                                {
                                    collInfo.Collider = items[i].Owner;
                                    items[j].Owner.OnCollide(collInfo);

                                }
                            }
                        }
                    }
                }
            }
        }                
    }
}

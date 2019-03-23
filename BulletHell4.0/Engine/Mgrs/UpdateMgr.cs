using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHell4_0
{
    public static class UpdateMgr
    {
        static List<IUpdatable> items;
        static List<IUpdatable> itemsToRemove;

        static UpdateMgr()
        {
            items = new List<IUpdatable>();
            itemsToRemove = new List<IUpdatable>();
        }


        static public void Add(IUpdatable item)
        {
            items.Add(item);
        }

        static public void Remove(IUpdatable item)
        {
            itemsToRemove.Add(item);
        }

        static public void RemoveAll()
        {
            for (int i = 0; i < items.Count; i++)
            {
                itemsToRemove.Add(items[i]);
            }
            items.Clear();
        }


        static public void Update()
        {
            TimeMgr.Update();

            if (itemsToRemove.Count > 0)
            {
                for (int i = 0; i < itemsToRemove.Count; i++)
                {
                    items.Remove(itemsToRemove[i]);
                }
                itemsToRemove.Clear();
            }

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Update();
            }
        }
    }
}

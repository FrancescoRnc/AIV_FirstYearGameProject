using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHell4_0
{
    public static class DrawMgr
    {
        public enum Layer { Background, Midground, Playground, Foreground, GUI }

        static List<IDrawable>[] items;
        static List<IDrawable> itemsToRemove;


        static DrawMgr()
        {
            items = new List<IDrawable>[5];
            for (int i = 0; i < items.Length; i++)
                items[i] = new List<IDrawable>();
            itemsToRemove = new List<IDrawable>();
        }

        static public void Add(IDrawable item)
        {
            if (!items[(int)item.Layer].Contains(item))
                items[(int)item.Layer].Add(item);
        }

        static public void Remove(IDrawable item)
        {
            if (items[(int)item.Layer].Contains(item))
                //items[(int)item.Layer].Remove(item);
                itemsToRemove.Add(item);
        }

        static public void Draw()
        {
            if (itemsToRemove.Count > 0)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    for (int j = 0; j < itemsToRemove.Count; j++)
                    {
                        items[i].Remove(itemsToRemove[j]);
                    }
                }
                itemsToRemove.Clear();
            }

            for (int i = 0; i < items.Length; i++)
                for (int j = 0; j < items[i].Count; j++)
                    items[i][j].Draw();
        }

        static public void RemoveAll()
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].Clear();
            }
        }
    }
}

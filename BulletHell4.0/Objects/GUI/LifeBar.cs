using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    public class LifeBar : GameObject
    {
        public Ship Owner;

        //Texture pointsTex;
        //Sprite pointsSpr;
        GameObject bar;

        float totalLife { get { return Owner.TotalLifePoints; } }
        float currentLife { get { return Owner.LifePoints; } }

        public LifeBar(Ship owner, Vector2 pos, bool follow = false) : base(pos, "lifebar_bg_enemy", DrawMgr.Layer.GUI)
        {
            Owner = owner;

            //pointsTex = GfxMgr.GetTexture("lifebar_point_enemy");
            //pointsSpr = new Sprite(pointsTex.Width, pointsTex.Height);
            //pointsSpr.pivot = new Vector2(0, pointsSpr.Height / 2);
            //pointsSpr.position = Position - new Vector2(200, 0);

            bar = new GameObject(pos, "lifebar_point_enemy", DrawMgr.Layer.GUI);
            bar.Sprite.pivot = new Vector2(0, bar.Height / 2);
            bar.Position = pos - new Vector2(200, 0);
            bar.Create();

            Create();
        }

        public void UpdateLife()
        {            
            float perc = currentLife / totalLife;
            //pointsSpr.scale = new Vector2(perc, 1);
            bar.Sprite.scale = new Vector2(perc, 1);
        }

        public override void Destroy()
        {
            base.Destroy();
            bar.Destroy();
        }


        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
            //pointsSpr.DrawTexture(pointsTex);
            bar.Draw();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{    
    public class Ray : IDrawable
    {
        public Vector2 Origin;
        //public Vector2 Position { get { return Origin + OffsetPos; } }

        public DrawMgr.Layer Layer { get { return layer; } }

        public Vector2 Direction;
        public bool Confirm;

        protected GameObject InteractableObj;

        public Sprite Redpoint;
        protected DrawMgr.Layer layer;


        public Ray(Vector2 origin, Vector2 direction)
        {
            Origin = origin;
            Direction = direction;

            layer = DrawMgr.Layer.Foreground;

            Redpoint = new Sprite(5, 5);
            Redpoint.pivot = new Vector2(2.5f, 2.5f);
            Redpoint.position = Origin;

            DrawMgr.Add(this);
        }

        public bool Casting(Collision.CollisionType type)
        {
            if (type == Collision.CollisionType.CircleIntersection)
                Confirm = Physics.Raycast_C(Origin, Direction);
            else if (type == Collision.CollisionType.RectsIntersection)
                Confirm = Physics.Raycast_R(Origin, Direction);
            else
                Confirm = false;
            return Confirm;
        }

        public void Draw()
        {
            for (float i = 0; i < 100; i += 6f)
            {
                if (Confirm)
                    Redpoint.DrawSolidColor(1f, 0, 0);
                else
                    Redpoint.DrawSolidColor(0, 0, 0);
                Redpoint.position += 6f * Direction;
            }

        }
    }
}

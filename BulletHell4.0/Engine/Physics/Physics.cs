using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    public class RaycastResults
    {
        public Vector2 NearestPoint;
        public Vector2 FarthestPoint;
        public Circle CircleCollider;
        public Rect RectCollider;
        public bool IsIntersected;

        public RaycastResults(Vector2 nPoint, Vector2 fPoint, Circle circle, Rect rect, bool inters)
        {
            NearestPoint = nPoint;
            FarthestPoint = fPoint;
            CircleCollider = circle;
            RectCollider = rect;
            IsIntersected = inters;
        }

    }

    public static class Physics
    {
        static private List<Circle> circleColliders;
        static private List<Rect> rectColliders;


        static Physics()
        {
            circleColliders = new List<Circle>();
            rectColliders = new List<Rect>();
        }

        static public RaycastResults Raycast(Vector2 origin, Vector2 direction, Collision.CollisionType type)
        {
            RaycastResults result = null;

            if (type == Collision.CollisionType.CircleIntersection)
            {
                foreach (Circle collider in circleColliders)
                {
                    Vector2 l = collider.Center - origin; //lunghezza da origine ray a centro coll
                    float tca = Vector2.Dot(l, direction); //prodotto scalare tra lunghezza e direzione (deve essere positiva)
                    if (tca < 0)
                        continue;
                    float d = (float)Math.Sqrt(l.Length * l.Length - tca * tca);
                    if (d > collider.Radius)
                        continue;
                    float thc = (float)Math.Sqrt(collider.Radius * collider.Radius - d * d);
                    float p = tca - thc;
                    float p1 = tca + thc;
                    Vector2 pCoords = origin + direction * p;
                    Vector2 p1Coords = origin + direction * p1;

                    return result = new RaycastResults(pCoords, p1Coords, collider, null, true);
                }
            }
            else if (type == Collision.CollisionType.RectsIntersection)
            {
                foreach (Rect collider in rectColliders)
                {
                    float tmin = float.MinValue;
                    float tmax = float.MaxValue;

                    float tx1 = (collider.Min.X - origin.X) * (1 / direction.X);
                    float tx2 = (collider.Max.X - origin.X) * (1 / direction.X);

                    tmin = Math.Max(tmin, Math.Min(tx1, tx2));
                    tmax = Math.Min(tmax, Math.Max(tx1, tx2));

                    float ty1 = (collider.Min.Y - origin.Y) * (1 / direction.Y);
                    float ty2 = (collider.Max.Y - origin.Y) * (1 / direction.Y);

                    tmin = Math.Max(tmin, Math.Min(ty1, ty2));
                    tmax = Math.Min(tmax, Math.Max(ty1, ty2));

                    Vector2 pCoords = origin + direction * tmin;
                    Vector2 p1Coords = origin + direction * tmax;

                    if (tmax >= tmin)
                        return result = new RaycastResults(pCoords, p1Coords, null, collider, true);
                }
            }

            return null;
        }

        static public bool Raycast_C(Vector2 origin, Vector2 direction)
        {
            foreach (Circle collider in circleColliders)
            {
                Vector2 l = collider.Center - origin; //lunghezza da origine ray a centro coll
                float tca = Vector2.Dot(l, direction); //prodotto scalare tra lunghezza e direzione (deve essere positiva)
                if (tca < 0)
                    continue;
                float d = (float)Math.Sqrt(l.Length * l.Length - tca * tca);
                if (d > collider.Radius)
                    continue;
                float thc = (float)Math.Sqrt(collider.Radius * collider.Radius - d * d);
                float p = tca - thc;
                float p1 = tca + thc;
                Vector2 pCoords = origin + direction * p;
                Vector2 p1Coords = origin + direction * p1;

                return true;
            }
            return false;
        }

        static public bool Raycast_R(Vector2 origin, Vector2 direction)
        {
            foreach (Rect collider in rectColliders)
            {
                float tmin = float.MinValue;
                float tmax = float.MaxValue;

                if (direction.X != 0)
                {
                    float tx1 = (collider.Min.X - origin.X) / direction.X;
                    float tx2 = (collider.Max.X - origin.X) / direction.X;

                    tmin = Math.Max(tmin, Math.Min(tx1, tx2));
                    tmax = Math.Min(tmax, Math.Max(tx1, tx2));
                }
                if (direction.Y != 0)
                {
                    float ty1 = (collider.Min.Y - origin.Y) / direction.Y;
                    float ty2 = (collider.Max.Y - origin.Y) / direction.Y;

                    tmin = Math.Max(tmin, Math.Min(ty1, ty2));
                    tmax = Math.Min(tmax, Math.Max(ty1, ty2));
                }

                Vector2 pCoords = origin + direction * tmin;
                Vector2 p1Coords = origin + direction * tmax;

                return tmax >= tmin;
            }
            return false;
        }

        static public Circle Raycast_Circle(Vector2 origin, Vector2 direction, Circle myself, float maxDistance = float.MaxValue)
        {
            Circle nearestColl = null;
            float nearestPoint = maxDistance;

            foreach (Circle collider in circleColliders)
            {
                if (collider == myself)
                    continue;
                Vector2 l = collider.Center - origin; //lunghezza da origine ray a centro coll
                float tca = Vector2.Dot(l, direction); //prodotto scalare tra lunghezza e direzione (deve essere positiva)
                if (tca < 0)
                    continue;
                float d = (float)Math.Sqrt(l.Length * l.Length - tca * tca);
                if (d > collider.Radius)
                    continue;
                float thc = (float)Math.Sqrt(collider.Radius * collider.Radius - d * d);
                float p = tca - thc;
                float p1 = tca + thc;
                Vector2 pCoords = origin + direction * p;
                Vector2 p1Coords = origin + direction * p1;
                if (p < nearestPoint || p1 < nearestPoint)
                {
                    nearestColl = collider;
                    nearestPoint = Math.Min(p, p1);
                }
            }
            return nearestColl;
        }

        static public Rect Raycast_Rect(Vector2 origin, Vector2 direction, Rect myself, float maxDistance = float.MaxValue)
        {
            Rect nearestColl = null;
            float nearestPoint = maxDistance;

            foreach (Rect collider in rectColliders)
            {
                float tmin = float.MinValue;
                float tmax = float.MaxValue;

                if (direction.X != 0)
                {
                    float tx1 = (collider.Min.X - origin.X) / direction.X;
                    float tx2 = (collider.Max.X - origin.X) / direction.X;

                    tmin = Math.Max(tmin, Math.Min(tx1, tx2));
                    tmax = Math.Min(tmax, Math.Max(tx1, tx2));
                }
                if (direction.Y != 0)
                {
                    float ty1 = (collider.Min.Y - origin.Y) / direction.Y;
                    float ty2 = (collider.Max.Y - origin.Y) / direction.Y;

                    tmin = Math.Max(tmin, Math.Min(ty1, ty2));
                    tmax = Math.Min(tmax, Math.Max(ty1, ty2));
                }

                Vector2 pCoords = origin + direction * tmin;
                Vector2 p1Coords = origin + direction * tmax;

                nearestColl = collider;
                nearestPoint = Math.Min(tmax, tmin);
            }
            return nearestColl;
        }

        static public Vector2 Raycast_Point(Vector2 origin, Vector2 direction, Circle myself, float maxDistance = float.MaxValue)
        {
            Circle nearestColl = null;
            float nearestPoint = maxDistance;
            Vector2 nearestVector = origin;

            foreach (Circle collider in circleColliders)
            {
                if (collider == myself)
                    continue;
                Vector2 l = collider.Center - origin; //lunghezza da origine ray a centro coll
                float tca = Vector2.Dot(l, direction); //prodotto scalare tra lunghezza e direzione (deve essere positiva)
                if (tca < 0)
                    continue;
                float d = (float)Math.Sqrt(l.Length * l.Length - tca * tca);
                if (d > collider.Radius)
                    continue;
                float thc = (float)Math.Sqrt(collider.Radius * collider.Radius - d * d);
                float p = tca - thc;
                float p1 = tca + thc;
                Vector2 pCoords = origin + direction * p;
                Vector2 p1Coords = origin + direction * p1;
                if (p < nearestPoint || p1 < nearestPoint)
                {
                    nearestColl = collider;
                    nearestPoint = Math.Min(p, p1);
                    nearestVector = pCoords;
                }
            }
            return nearestVector;
        }

        public static void AddCollider(Circle collider)
        {
            if (!circleColliders.Contains(collider))
                circleColliders.Add(collider);
        }

        public static void RemoveCollider(Circle collider)
        {
            if (circleColliders.Contains(collider))
                circleColliders.Remove(collider);
        }

        public static void AddCollider(Rect collider)
        {
            if (!rectColliders.Contains(collider))
                rectColliders.Add(collider);
        }

        public static void RemoveCollider(Rect collider)
        {
            if (rectColliders.Contains(collider))
                rectColliders.Remove(collider);
        }
    }
}

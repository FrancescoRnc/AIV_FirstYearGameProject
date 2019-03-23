using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    public static class CameraMgr //fai MoveCameraTo ->setta target null, nuova posizione, tempo di blend, muove la camera da ultimo punto al prossimo target
    {
        static Camera mainCamera;
        static GameObject target;
        static Dictionary<string, Tuple<Camera, float>> cameraList;

        static Vector2 startCameraPos;
        static Vector2 endCameraPos;
        static float delay;
        static float moveCounter;
        static bool isUpdating;

        static float minX;
        static float minY;
        static float maxX;
        static float maxY;


        static public void Init(Vector2 pivot, Vector2 position)
        {
            mainCamera = new Camera(position.X, position.Y);
            mainCamera.pivot = pivot;
            cameraList = new Dictionary<string, Tuple<Camera, float>>();
            minX = 0;//Game.window.Width / 2;
            minY = 0;
            maxX = Game.window.Width;
            maxY = Game.window.Height;
        }


        static public void Add(string cameraIndex, float speedMul = 1f, Camera c = null)
        {
            if (c == null)
            {
                c = new Camera(mainCamera.position.X, mainCamera.position.Y);
                c.pivot = mainCamera.pivot;
            }

            cameraList.Add(cameraIndex, new Tuple<Camera, float>(c, speedMul));
        }

        static public Camera GetCamera(string cameraIndex)
        {
            if (cameraList.ContainsKey(cameraIndex))
                return cameraList[cameraIndex].Item1;
            return null;
        }

        static public void SetTarget(GameObject newTarget)
        {
            target = newTarget;
        }

        static public void Reset()
        {
            mainCamera.pivot = Vector2.Zero;
            mainCamera.position = Vector2.Zero;
        }

        static void CheckLimits()
        {
            if (mainCamera.position.X > maxX)
                mainCamera.position.X = maxX;
            else if (mainCamera.position.X < minX)
                mainCamera.position.X = minX;
            
            if (mainCamera.position.Y > maxY)
                mainCamera.position.Y = maxY;
            else if (mainCamera.position.Y < minY)
                mainCamera.position.Y = minY;
        }

        static public void MoveCameraTo(Vector2 newPos, float time = 1.0f)
        {
            target = null;
            isUpdating = true;
            startCameraPos = mainCamera.position;
            endCameraPos = newPos;
            delay = time;
            moveCounter = 0;
        }        


        static public void Update()
        {
            Vector2 deltaCamera = mainCamera.position;

            if (isUpdating)
            {
                moveCounter += Game.window.deltaTime;
                if (moveCounter >= delay)
                {
                    isUpdating = false;
                    mainCamera.position = endCameraPos;
                }
                else
                {
                    mainCamera.position = Vector2.Lerp(startCameraPos, endCameraPos, moveCounter / delay);
                }
            }
            else if (target != null)
            {
                mainCamera.position = Vector2.Lerp(mainCamera.position, target.Position, Game.window.deltaTime * 4);                
            }
            //CheckLimits();
            deltaCamera = mainCamera.position - deltaCamera;
            foreach (var item in cameraList)
            {
                item.Value.Item1.position += deltaCamera * item.Value.Item2;
            }
        }
    }
}

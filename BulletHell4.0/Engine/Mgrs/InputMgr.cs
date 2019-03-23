using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    public enum InputType { Mouse, Keyboard, Gamepad }

    static public class InputMgr
    {
        static public Vector2 MousePosition { get { return Game.window.mousePosition; } }
        static public bool Left { get { return Game.window.mouseLeft; } }
        static public bool Right { get { return Game.window.mouseRight; } }

        static Dictionary<string, InputComponent> inputButtons;


        static InputMgr()
        {
            inputButtons = new Dictionary<string, InputComponent>();
            AddInput("Up");
            AddInput("Down");
            AddInput("Left");
            AddInput("Right");
            AddInput("SlowMove");
            AddInput("MouseLeft");
            AddInput("MouseRight");
        }


        static void AddInput(string comand)
        {
            inputButtons.Add(comand, new InputComponent());
        }
        static void RemoveInput(string comand)
        {
            if (inputButtons.ContainsKey(comand))
                inputButtons.Remove(comand);
        }
        

        static public bool GetKeyCode(KeyCode code)
        {
            return Game.window.GetKey(code);
        }

        static public bool GetComand(string comand)
        {
            switch (comand)
            {
                case "Up":
                    return GetKeyCode(KeyCode.W);
                case "Down":
                    return GetKeyCode(KeyCode.S);
                case "Left":
                    return GetKeyCode(KeyCode.A);
                case "Right":
                    return GetKeyCode(KeyCode.D);
                case "SlowMove":
                    return GetKeyCode(KeyCode.ShiftLeft);
                case "MouseLeft":
                    return Left;
                case "MouseRight":
                    return Right;
                default:
                    return false;
            }
            
        }

        static public bool IsMouseOnArea(Vector2 start, Vector2 end)
        {
            return (MousePosition.X >= start.X && MousePosition.Y >= start.Y) &&
                   (MousePosition.X <= end.X && MousePosition.Y <= end.Y);
        }

        static public bool IsPressing(string comand)
        {
            return inputButtons[comand].OnPressing(GetComand(comand));
        }
        static public bool IsPressed(string comand)
        {
            return inputButtons[comand].OnPressed(GetComand(comand));
        }
        static public bool IsReleased(string comand)
        {
            return inputButtons[comand].OnReleased(GetComand(comand));
        }
    }


    public class InputComponent
    {
        public string Name;
        public bool IsPressed;
        public bool IsReleased;

        public InputType Type { get; private set; }
        public KeyCode Code { get; private set; }

        public bool Comand { get { return InputMgr.GetComand(Name);} }


        public InputComponent()
        {

        }
        public InputComponent(string name, InputType type, KeyCode code = KeyCode.Keypad0)
        {
            Name = name;
            Type = type;
            Code = code;
        }

        public bool OnPressing(bool comand)
        {
            return comand;
        }

        public bool OnPressed(bool comand)
        {
            if (comand)
            {
                if (!IsPressed)
                {
                    IsPressed = true;
                }
            }
            else
            {
                IsPressed = false;
            }

            return IsPressed;
        }

        public bool OnReleased(bool comand)
        {
            IsReleased = false;
            if (comand)
            {
                if (!IsPressed)
                {
                    IsPressed = true;
                }
            }
            else
            {
                if (IsPressed)
                {
                    IsReleased = true;
                }
                IsPressed = false;
            }

            return IsReleased;
        }
    }
}

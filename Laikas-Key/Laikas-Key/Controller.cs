using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MiUtil;

namespace Laikas_Key
{
    class Controller : MiController<MiStandardControllerState>
    {
        public static MiControl LEFT = new MiControl();
        public static MiControl RIGHT = new MiControl();
        public static MiControl UP = new MiControl();
        public static MiControl DOWN = new MiControl();
        public static MiControl A = new MiControl();
        public static MiControl B = new MiControl();
        public static MiControl START = new MiControl();

        public static readonly MiControl[] controls = { LEFT, RIGHT, UP, DOWN, A, B, START };
        
        new public static MiStandardControllerState GetState()
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            controllerState = new MiStandardControllerState();
            if (gamePadState.IsConnected)
            {
                controllerState[A] = gamePadState.Buttons.A;
                controllerState[B] = gamePadState.Buttons.B;
                controllerState[LEFT] = gamePadState.DPad.Left;
                controllerState[RIGHT] = gamePadState.DPad.Right;
                controllerState[UP] = gamePadState.DPad.Up;
                controllerState[DOWN] = gamePadState.DPad.Down;
                controllerState[START] = gamePadState.Buttons.Start;

                Vector2 ts_direction = gamePadState.ThumbSticks.Left;
                if (ts_direction.X > 0) controllerState[RIGHT] = ButtonState.Pressed;
                if (ts_direction.X < 0) controllerState[LEFT] = ButtonState.Pressed;
                if (ts_direction.Y > 0) controllerState[UP] = ButtonState.Pressed;
                if (ts_direction.Y < 0) controllerState[DOWN] = ButtonState.Pressed;

            }
            else
            {
                controllerState[A] = (ButtonState)keyboardState[Keys.Space];
                controllerState[B] = (ButtonState)keyboardState[Keys.LeftShift];
                controllerState[LEFT] = (ButtonState)keyboardState[Keys.Left];
                controllerState[RIGHT] = (ButtonState)keyboardState[Keys.Right];
                controllerState[UP] = (ButtonState)keyboardState[Keys.Up];
                controllerState[DOWN] = (ButtonState)keyboardState[Keys.Down];
                controllerState[START] = (ButtonState)keyboardState[Keys.Escape];
            }
            return controllerState;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
using Microsoft.Xna.Framework;

namespace Laikas_Key
{
    class InputHandler : MiInputHandler
    {
        private const int HOLD_REPEAT_INTERVAL = 30;
        private Dictionary<MiControl, int> holdTimer;
        public InputHandler(MiGame game)
            : base(game)
        {
            oldState = Controller.GetState();
            holdTimer = new Dictionary<MiControl, int>();
            foreach (MiControl control in Controller.controls)
            {
                holdTimer[control] = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                MiStandardControllerState newState = Controller.GetState();
                foreach (MiControl control in Controller.controls)
                {
                    if (oldState.IsReleased(control) && newState.IsPressed(control))
                    {
                        Game.ScriptEngine.ExecuteScript(Focused.RespondToInput(control));
                    }

                    if (oldState.IsPressed(control) && newState.IsPressed(control))
                    {
                        holdTimer[control]++;
                        if (holdTimer[control] > HOLD_REPEAT_INTERVAL)
                        {
                            holdTimer[control] = 0;
                            Game.ScriptEngine.ExecuteScript(Focused.RespondToInput(control));
                        }
                    }

                    if (newState.IsReleased(control))
                    {
                        holdTimer[control] = 0;
                    }
                }

                oldState = newState;
            }
        }
    }
}

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
        int frame = 0;
        public InputHandler(MiGame game)
            : base(game)
        {
            oldState = Controller.GetState();
        }

        public override void Update(GameTime gameTime)
        {
            MiStandardControllerState newState = Controller.GetState();
            if (oldState.IsReleased(Controller.UP) && newState.IsPressed(Controller.UP))
            {
                Game.ScriptEngine.ExecuteScript(Focused.Upped);
            }

            if (oldState.IsReleased(Controller.DOWN) && newState.IsPressed(Controller.DOWN))
            {
                Game.ScriptEngine.ExecuteScript(Focused.Downed);
            }

            if (oldState.IsReleased(Controller.LEFT) && newState.IsPressed(Controller.LEFT))
            {
                Game.ScriptEngine.ExecuteScript(Focused.Lefted);
            }

            if (oldState.IsReleased(Controller.RIGHT) && newState.IsPressed(Controller.RIGHT))
            {
                Game.ScriptEngine.ExecuteScript(Focused.Righted);
            }

            if (oldState.IsReleased(Controller.A) && newState.IsPressed(Controller.A))
            {
                Game.ScriptEngine.ExecuteScript(Focused.Pressed);
            }

            if (oldState.IsReleased(Controller.B) && newState.IsPressed(Controller.B))
            {
                Game.ScriptEngine.ExecuteScript(Focused.Cancelled);
            }

            if (oldState.IsReleased(Controller.START) && newState.IsPressed(Controller.START))
            {
                Game.ScriptEngine.ExecuteScript(Focused.Escaped);
            }

            oldState = newState;
        }
    }
}

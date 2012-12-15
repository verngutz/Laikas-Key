using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace MiUtil
{
    public class MiStandardControllerState : MiControllerState
    {
        private Dictionary<MiControl, ButtonState> controlStates;
        public MiStandardControllerState()
        {
            controlStates = new Dictionary<MiControl, ButtonState>();
        }

        public override ButtonState this[MiControl control]
        {
            get { return controlStates[control]; }
            set { controlStates[control] = value; }
        }

        public override bool IsReleased(MiControl control)
        {
            if (controlStates[control] == ButtonState.Released) return true;
            return false;
        }

        public override bool IsPressed(MiControl control)
        {
            return !IsReleased(control);
        }
    }
}

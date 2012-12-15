using System.Collections.Generic;
namespace MiUtil
{
    public abstract class MiInputHandler : MiComponent
    {
        protected MiControllerState oldState;
        public MiScreen Focused;

        public MiInputHandler(MiGame game) : base(game) { }
    }
}

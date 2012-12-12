using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace MiUtil
{
    public abstract class MiScreen : MiDrawableComponent
    {
        protected Dictionary<MiControl, MiScript> inputResponses;

        public MiButton ActiveButton { get; set; }

        protected bool exitSequenceMutex;
        protected bool entrySequenceMutex;

        public MiScreen(MiGame game)
            : base(game)
        {
            Enabled = false;
            Visible = false;
            inputResponses = new Dictionary<MiControl, MiScript>();
        }

        public virtual IEnumerator<ulong> EntrySequence() { yield break; }
        public virtual IEnumerator<ulong> ExitSequence() { yield break; }

        public MiScript RespondToInput(MiControl control)
        {
            if (inputResponses.ContainsKey(control))
                return inputResponses[control];
            else
                return DoNothing;
        }

        protected static IEnumerator<ulong> DoNothing()
        {
            yield break;
        }
    }
}

using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace MiUtil
{
    public abstract class MiScreen : MiDrawableComponent
    {
        private static readonly MiScript DO_NOTHING = new MiScript(DoNothing);
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

        public MiScript RespondToInput(MiControl control)
        {
            if (inputResponses.ContainsKey(control))
                return inputResponses[control];
            else
                return DO_NOTHING;
        }

        private static IEnumerator<ulong> DoNothing()
        {
            yield break;
        }
    }
}

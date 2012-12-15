using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Choice = System.Collections.Generic.KeyValuePair<string, MiUtil.MiScript>;
namespace Laikas_Key
{
    static class Scripts
    {
      
        public static IEnumerator<ulong> ChooseYourFate()
        {
            TownScreen.Instance.PlayerMoveEnabled = false;
            ChoiceScreen.Show("Choose your fate.",
                new Choice("Traditionalist",
                    delegate
                    {
                        MessageScreen.Show("Your backwardness is preventing equality for all.");
                        return null;
                    }),
                new Choice("Futurist",
                    delegate
                    {
                        MessageScreen.Show("Your technology is destroying the earth.");
                        return null;
                    })
                );
            while (TownScreen.Instance.Game.InputHandler.Focused is DialogScreen)
                yield return 5;
            TownScreen.Instance.PlayerMoveEnabled = true;
            yield break;
        }

        public static IEnumerator<ulong> AboutTheWar()
        {
            TownScreen.Instance.PlayerMoveEnabled = false;
            ChoiceScreen.Show("Would you like to know more about the war?",
                new Choice("Yes",
                    delegate
                    {
                        MessageScreen.Show("This war is rooted in the differences of Traditionalists and Futurists.");
                        return null;
                    }),
                new Choice("No",
                    delegate
                    {
                        MessageScreen.Show("You don't really care do you?");
                        return null;
                    })
                );
            while (TownScreen.Instance.Game.InputHandler.Focused is DialogScreen)
                yield return 5;
            TownScreen.Instance.PlayerMoveEnabled = true;
            yield break;
        }

        public static IEnumerator<ulong> FightOrFlee()
        {
            TownScreen.Instance.PlayerMoveEnabled = false;
            ChoiceScreen.Show("Would you rather fight or flee?",
                new Choice("Fight",
                    delegate
                    {
                        MessageScreen.Show("Careful, don't forget about your team");
                        return null;
                    }),
                new Choice("Flee",
                    delegate
                    {
                        MessageScreen.Show("You can't always run...");
                        return null;
                    })
                );
            while (TownScreen.Instance.Game.InputHandler.Focused is DialogScreen)
                yield return 5;
            TownScreen.Instance.PlayerMoveEnabled = true;
            yield break;
        }
    }
}

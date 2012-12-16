using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
using Choice = System.Collections.Generic.KeyValuePair<string, MiUtil.MiScript>;

namespace Laikas_Key
{
    static class Scripts
    {
        private static MiGame game;
        public static void Init(MiGame game)
        {
            Scripts.game = game;
        }

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
            while (game.InputHandler.Focused is DialogScreen)
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
            while (game.InputHandler.Focused is DialogScreen)
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
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            TownScreen.Instance.PlayerMoveEnabled = true;
            yield break;
        }

        private static bool runWorldScreenTutorial = true;
        public static IEnumerator<ulong> Tutorial()
        {
            if(runWorldScreenTutorial)
            {
                // Wait for first time player encounters world screen
                while (!(game.InputHandler.Focused is WorldScreen))
                    yield return 5;

                MessageScreen.Show("Locations with green markers are controlled by your faction.");
                while (game.InputHandler.Focused is DialogScreen)
                    yield return 5;
                MessageScreen.Show("Locations with red markers are controlled by the enemy faction.");
                while (game.InputHandler.Focused is DialogScreen)
                    yield return 5;
                MessageScreen.Show("Locations with gray markers are not controlled by either faction.");
                while (game.InputHandler.Focused is DialogScreen)
                    yield return 5;

                runWorldScreenTutorial = false;
            }

            // Add the "self" to party
            Character self = new Character("You", 5, 5, 5, 5, 5);
            self.KnownAttacks.Add(Attack.shootGun);
            self.KnownAttacks.Add(Attack.swingSword);
            Player.Party.Add(self);

            yield break;
        }
    }
}

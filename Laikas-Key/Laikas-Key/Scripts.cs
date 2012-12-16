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

        public static IEnumerator<ulong> FriendlyConversation()
        {
            TownScreen.Instance.PlayerMoveEnabled = false;
            MessageScreen.Show("Schneider:	But you know, if those rumors are real and the guy’s not a total lunatic, we’d have awesome gadgets and stuff. Imagine how much the poor would be helped if we had the right tools to do so.");
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            MessageScreen.Show("Andres:	Now wait just a darn minute! You know how it is with all those inventions. If they’re not carefully made, everyone breathes the poison produced along with them. Leave alone the fact that my race is slowly losing its once beautiful ability to dance with nature because of the crappy waste your modernization has brought. This environmental mental guy, on the other hand, even if he’s crazy, he makes a good point.");
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            MessageScreen.Show("Schneider:	I don’t know about you, but I think preserving those elitist arts is a lost cause. Don’t you desire change, justice, equality?");
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            MessageScreen.Show("Andres:		Not if it means we all die of cancer. Yeah, there you go. Death for all. Equality for all. Schneider, you’re too much of an idealist, but have too much pollution covering your eyes from reality.");
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            MessageScreen.Show("Schneider:	Hey hey now! No need to attack my person ‘cause you can’t hold your emotions. We’re all rational thinkers here. Or are we?");
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            MessageScreen.Show("Andres:		Mark my words Schneider. One of these days, the treacherousness of technology will be revealed, and we will have nothing to rely on but what our ancestors have developed over millennia.");
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            MessageScreen.Show("Schneider:	Well, we’ll just have to see about that. I want to hear what this Industrial Revolutionist has to say. You guys better go with me. I will be at Gadgets class if you need me.");
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            MessageScreen.Show("Andres:		No, you BETTER go with me! The entire world is worth more than all mankind. I will be at Will Defense class.");
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            TownScreen.Instance.PlayerMoveEnabled = true;
            yield break;
        }

        public static IEnumerator<ulong> SchneiderMail()
        {
            TownScreen.Instance.PlayerMoveEnabled = false;
            MessageScreen.Show("Schneider:	Hey, have you heard? There’s a man to the West claiming to have come from the future. He’s telling everyone about technological acceleration and stuff. Freaking insane right? We’ve got to see this! Let’s meet later at the Academe (school) cafe. I’ll tell Andres.");
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            TownScreen.Instance.PlayerMoveEnabled = true;
            yield break;
        }

        public static IEnumerator<ulong> AndresMail()
        {
            TownScreen.Instance.PlayerMoveEnabled = false;
            MessageScreen.Show("Andres:		Hey lazy bones! Get up and get your arse up here to school cafe! We need to talk. It’s about this crazy guy who says he’s from the future and he goes “down with technology” and what-not and wants to protect our environment. This I gotta see. And so should you. I’ll tell Schneider.");
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            TownScreen.Instance.PlayerMoveEnabled = true;
            yield break;
        }

        public static IEnumerator<ulong> PASystem()
        {
            TownScreen.Instance.PlayerMoveEnabled = false;
            MessageScreen.Show("ATTENTION EVERYONE! THIS IS NOT A DRILL. USE THE EMERGENCY EXITS TO ESCAPE THE BUILDING! AGAIN, THIS IS NOT A DRILL.");
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            TownScreen.Instance.PlayerMoveEnabled = true;
            yield break;
        }

        public static IEnumerator<ulong> FoundAndres()
        {
            TownScreen.Instance.PlayerMoveEnabled = false;
            MessageScreen.Show("Andres:		Finally found you. Have you seen Schneider? Last time I saw him was at the school cafe when the three of us were talking. I just saw a number of students captured by a few Futurists, I hope he’s not among them. Let’s go find out. I assume you can fight?");
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            TownScreen.Instance.PlayerMoveEnabled = true;
            yield break;
        }

        public static IEnumerator<ulong> CourtYard()
        {
            TownScreen.Instance.PlayerMoveEnabled = false;
            MessageScreen.Show("Engineer: 	Attack me one more time or these students die.");
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            MessageScreen.Show("Andres: 	Let them go!");
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            MessageScreen.Show("Engineer: 	Why should I? So you Traditionalist scums can brainwash these students? Under no circumstances will I give them back to you. Come join us; you two look like you have potential.");
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            MessageScreen.Show("Schneider: 	It’s gonna be fine. I chose to be captured. I will not waste one more day with people who do not want change. Change is necessary for this world to be better. Will you come with me?");
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            MessageScreen.Show("Andres: 	I want no part in this war. Come with me.");
            while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            TownScreen.Instance.PlayerMoveEnabled = true;
            yield break;
        }

        public static IEnumerator<ulong> PubShootOut()
        {
            TownScreen.Instance.PlayerMoveEnabled = false;
            MessageScreen.Show("Schneider: 	You guys wait for me outside, I’m gonna go alone.");
             while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            MessageScreen.Show("Gunner: 	Roger that.");
             while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            MessageScreen.Show("Schneider: 	Black Sage! I waited fifteen years for this day of retribution. My parents were massacred by you! I suffered long enough!");
             while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            MessageScreen.Show("Black Sage: 	Oh, but did I really massacre them? Or did they allow themselves to be massacred? Maybe they just couldn’t swallow the possibility of all being one again and thus succumbed to the weakness of only half of One. I hope you do not follow their folly.");
             while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            MessageScreen.Show("Schneider: 	SHUT UP!!");
             while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
             MessageScreen.Show("Black Sage: 	What’s the matter young boy? Analysis paralysis? Most people act without thinking, but I see you think very well but cannot act. Even you need a drug to strengthen your will, or you cannot live happily on this planet. I’ve desired nothing but the happiness of all. Pray tell me where I’ve erred.");
             while (game.InputHandler.Focused is DialogScreen)
                yield return 5;
            MessageScreen.Show("Andres: 	I naively thought that I could live in a world at war and make myself not a part of it. Schneider, I am going to destroy you and your stupid ambition.");
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

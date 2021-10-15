using System;
using System.Collections.Generic;

namespace LcrGame.Simulation
{
    public class Player
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="playerPosition"></param>
        public Player(int playerPosition)
        {
            PlayerPosition = playerPosition;

            for (int i = 0; i < Game.INITIAL_NUMBER_OF_DICE; i++)
            {
                Dice.Add(new Die());
            }
        }

        /// <summary>
        /// This player's index position
        /// </summary>
        public int PlayerPosition { get; private set; } = 0;

        /// <summary>
        /// The number of chips this player has
        /// </summary>
        public int NumberOfChips { get; set; } = Game.INITIAL_NUMBER_OF_CHIPS;

        /// <summary>
        /// The dice rolled by the player
        /// </summary>
        public List<Die> Dice { get; private set; } = new List<Die>();

        /// <summary>
        /// Called when the player rolls the dice
        /// </summary>
        /// <param name="random"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        public bool RollDice(Random random, List<Player> players)
        {
            AddRemoveDice();

            if (Dice.Count == 0) return false;

            foreach (var die in Dice)
            {
                die.Roll(random);

                switch (die.RolledValue)
                {
                    // Left
                    case 0:
                        {
                            int playerIndex = PlayerPosition - 1;

                            if (playerIndex < 0)
                            {
                                playerIndex = players.Count - 1;
                            }

                            NumberOfChips--;

                            players[playerIndex].NumberOfChips++;
                        }
                        break;
                    // Center
                    case 1:
                        {
                            NumberOfChips--;
                        }
                        break;
                    // Right
                    case 2:
                        {
                            int playerIndex = PlayerPosition + 1;

                            if (playerIndex >= players.Count)
                            {
                                playerIndex = 0;
                            }

                            NumberOfChips--;

                            players[playerIndex].NumberOfChips++;
                        }
                        break;
                    // Dot
                    case 3:
                    // Dot
                    case 4:
                    // Dot
                    case 5:
                        break;
                    default:
                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// Add or remove dice before rolling, if the player has less than 3 chips the number of dice decreases to the number of chips they have
        /// If the player has less than 3 dice before rolling, increase to the number of chips they have up to 3
        /// </summary>
        private void AddRemoveDice()
        {
            if (Dice.Count > NumberOfChips)
            {
                // Remove dice
                Dice.RemoveRange(0, Dice.Count - NumberOfChips);
            }
            else if (Dice.Count < 3 && NumberOfChips > Dice.Count)
            {
                // Add dice
                int diceToAdd = Math.Min(NumberOfChips - Dice.Count, 3);

                for (int i = 0; i < diceToAdd; i++)
                {
                    Dice.Add(new Die());
                }
            }
        }
    }
}

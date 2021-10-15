using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LcrGame.Simulation
{
    public class Game
    {
        /// <summary>
        /// The number of dice used in the game
        /// </summary>
        public static int INITIAL_NUMBER_OF_DICE { get; private set; } = 3;

        /// <summary>
        /// The initial number of chips given to each player
        /// </summary>
        public static int INITIAL_NUMBER_OF_CHIPS { get; private set; } = 3;

        /// <summary>
        /// The random number generator used to roll the dice
        /// </summary>
        public static Random Random { get; private set; } = new Random();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="numberOfPlayers"></param>
        public Game(int numberOfPlayers)
        {
            NumberOfPlayers = numberOfPlayers;

            for (int i = 0; i < NumberOfPlayers; i++)
            {
                Players.Add(new Player(i));
            }
        }

        /// <summary>
        /// The players in the game
        /// </summary>
        public List<Player> Players { get; private set; } = new List<Player>();

        /// <summary>
        /// The number of players in the game
        /// </summary>
        public int NumberOfPlayers { get; private set; } = 0;

        /// <summary>
        /// The number of turns it took to win the game
        /// </summary>
        public int NumberOfTurns { get; private set; } = 0;

        /// <summary>
        /// Run a game turn
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RunTurnAsync()
        {
            bool keepGoing = true;

            await Task.Run(() =>
            {
                foreach (var player in Players)
                {
                    if (!IsGameOver())
                    {
                        player.RollDice(Random, Players);

                        ++NumberOfTurns;
                    }
                    else
                    {
                        keepGoing = false;
                    }
                }
            });

            return keepGoing;
        }

        /// <summary>
        /// Get the winning player index
        /// </summary>
        /// <returns></returns>
        public int? GetWinner()
        {
            return Players.FirstOrDefault(x => x.NumberOfChips > 0)?.PlayerPosition;
        }

        /// <summary>
        /// Check to see if the game is over
        /// </summary>
        /// <returns></returns>
        public bool IsGameOver()
        {
            return Players.Where(x => x.NumberOfChips > 0).Count() <= 1;
        }
    }
}

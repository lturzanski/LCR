using LcrGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LcrGame.Simulation
{
    public class LcrSimulation : ILcrSimulation
    {
        /// <summary>
        /// The shortest game length
        /// </summary>
        public int ShortestGameLength { get; private set; } = int.MaxValue;

        /// <summary>
        /// The shortest game number
        /// </summary>
        public int ShortestGameNumber { get; private set; } = 0;

        /// <summary>
        /// The longest game length
        /// </summary>
        public int LongestGameLength { get; private set; } = 0;

        /// <summary>
        /// The longest game number
        /// </summary>
        public int LongestGameNumber { get; private set; } = 0;

        /// <summary>
        /// The average game length
        /// </summary>
        public int AverageGameLength { get; private set; } = 0;

        /// <summary>
        /// The number of games to play
        /// </summary>
        public int NumberOfGames { get; private set; } = 0;

        /// <summary>
        /// The number of players in the game
        /// </summary>
        public int NumberOfPlayers { get; private set; } = 0;

        /// <summary>
        /// The game lengths
        /// </summary>
        public List<double> AllGameLengths { get; private set; } = new List<double>();

        /// <summary>
        /// The game indexes
        /// </summary>
        public List<double> AllGameIndexes { get; private set; } = new List<double>();

        /// <summary>
        /// Initializes the simulation
        /// </summary>
        /// <param name="numberOfGames"></param>
        /// <param name="numberOfPlayers"></param>
        public void InitializeSimulation(int numberOfGames, int numberOfPlayers)
        {
            NumberOfGames = numberOfGames;

            NumberOfPlayers = numberOfPlayers;
        }

        /// <summary>
        /// Runs the simulation async, updates the winning player through the player view models
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="playerViewModels"></param>
        /// <returns></returns>
        public async Task RunSimulationAsync(CancellationTokenSource cancellationToken, ObservableCollection<PlayerViewModel> playerViewModels)
        {
            long averageGame = 0;

            int gameNumber = 1;

            // Run each game
            for (int i = 0; i < NumberOfGames; i++)
            {
                // Create a new game simulation
                Game game = new Game(NumberOfPlayers);

                // Run the game simulation
                while (game.IsGameOver() == false)
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await game.RunTurnAsync();
                    }
                    else
                    {
                        break;
                    }
                }

                // Stop if game is cancelled
                if (cancellationToken.IsCancellationRequested) break;

                // Set the shortest and logest games
                if (game.NumberOfTurns < ShortestGameLength)
                {
                    ShortestGameLength = game.NumberOfTurns;
                    ShortestGameNumber = gameNumber;
                }

                if (game.NumberOfTurns > LongestGameLength)
                {
                    LongestGameLength = game.NumberOfTurns;
                    LongestGameNumber = gameNumber;
                }

                AllGameLengths.Add(game.NumberOfTurns);
                AllGameIndexes.Add(gameNumber++);

                // Update the winning player
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    foreach (var item in playerViewModels.Where(x => x.PlayerWon == true))
                    {
                        item.PlayerWon = false;
                    }

                    playerViewModels[game.GetWinner().Value].PlayerWon = true;
                }));

                // Add to the average
                averageGame += (long)game.NumberOfTurns;
            }

            AverageGameLength = (int)(averageGame / (long)NumberOfGames);
        }
    }
}

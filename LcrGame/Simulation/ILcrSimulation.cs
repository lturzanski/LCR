using LcrGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LcrGame.Simulation
{
    public interface ILcrSimulation
    {
        void InitializeSimulation(int numberOfGames, int numberOfPlayers);
        Task RunSimulationAsync(CancellationTokenSource cancellationToken, ObservableCollection<PlayerViewModel> playerViewModels);
        int ShortestGameLength { get; }
        int ShortestGameNumber { get; }
        int LongestGameLength { get; }
        int LongestGameNumber { get; }
        int AverageGameLength { get; }
        List<double> AllGameLengths { get; }
        List<double> AllGameIndexes { get; }
    }
}

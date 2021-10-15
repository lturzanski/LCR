using LcrGame.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ScottPlot;
using System.Collections.ObjectModel;

namespace LcrGame.ViewModels
{
    public class ApplicationViewModel : ViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationViewModel()
        {
            PlotControl.Plot.Palette = ScottPlot.Palette.Dark;
            PlotControl.Plot.Style(ScottPlot.Style.Black);
        }

        protected List<string> _presets = new List<string>() {
            "3 players x 100 games",
            "4 players x 100 games",
            "5 players x 100 games",
            "5 players x 1,000 games",
            "5 players x 10,000 games",
            "5 players x 100,000 games",
            "6 players x 100 games",
            "7 players x 100 games",
        };

        /// <summary>
        /// The list of presets
        /// </summary>
        public List<string> Presets
        {
            get
            {
                return _presets;
            }
            set
            {
                if (value != _presets)
                {
                    _presets = value;
                    NotifyPropertyChanged("Presets");
                }
            }
        }

        protected int _selectedPreset = 0;
        /// <summary>
        /// The selected preset index
        /// </summary>
        public int SelectedPreset
        {
            get
            {
                return _selectedPreset;
            }
            set
            {
                if (value != _selectedPreset)
                {
                    switch (value)
                    {
                        // 3 Players x 100 Games
                        case 0:
                            NumberOfPlayers = 3;
                            NumberOfGames = 100;
                            break;
                        // 4 Players x 100 Games
                        case 1:
                            NumberOfPlayers = 4;
                            NumberOfGames = 100;
                            break;
                        // 5 Players x 100 Games
                        case 2:
                            NumberOfPlayers = 5;
                            NumberOfGames = 100;
                            break;
                        // 5 Players x 1,000 Games
                        case 3:
                            NumberOfPlayers = 5;
                            NumberOfGames = 1000;
                            break;
                        // 5 Players x 10,000 Games
                        case 4:
                            NumberOfPlayers = 5;
                            NumberOfGames = 10000;
                            break;
                        // 5 Players x 100,000 Games
                        case 5:
                            NumberOfPlayers = 5;
                            NumberOfGames = 100000;
                            break;
                        // 6 Players x 100 Games
                        case 6:
                            NumberOfPlayers = 6;
                            NumberOfGames = 100;
                            break;
                        // 7 Players x 100 Games
                        case 7:
                            NumberOfPlayers = 7;
                            NumberOfGames = 100;
                            break;
                        default:
                            break;
                    }
                    _selectedPreset = value;
                    NotifyPropertyChanged("SelectedPreset");
                }
            }
        }

        protected int _numberOfPlayers = 3;
        /// <summary>
        /// The number of players in the game
        /// </summary>
        public int NumberOfPlayers
        {
            get
            {
                return _numberOfPlayers;
            }
            set
            {
                if (value != _numberOfPlayers)
                {
                    if (value < 3) value = 3;

                    _numberOfPlayers = value;
                    NotifyPropertyChanged("NumberOfPlayers");

                    ResetGame();
                }
            }
        }

        protected int _numberOfGames = 100;
        /// <summary>
        /// The number of games to play
        /// </summary>
        public int NumberOfGames
        {
            get
            {
                return _numberOfGames;
            }
            set
            {
                if (value != _numberOfGames)
                {
                    if (value < 1) value = 1;

                    _numberOfGames = value;
                    NotifyPropertyChanged("NumberOfGames");
                }
            }
        }

        protected bool _playEnabled = true;
        /// <summary>
        /// Is the play button enabled
        /// </summary>
        public bool PlayEnabled
        {
            get
            {
                return _playEnabled;
            }
            set
            {
                if (value != _playEnabled)
                {
                    _playEnabled = value;
                    NotifyPropertyChanged("PlayEnabled");
                }
            }
        }

        protected WpfPlot _plotControl = new WpfPlot();
        /// <summary>
        /// The line chart plot control
        /// </summary>
        public WpfPlot PlotControl
        {
            get
            {
                return _plotControl;
            }
            set
            {
                if (value != _plotControl)
                {
                    _plotControl = value;
                    NotifyPropertyChanged("PlotControl");
                }
            }
        }

        /// <summary>
        /// The cancellation token for use after the game has started then canceled
        /// </summary>
        CancellationTokenSource CancellationToken { get; set; } = null;

        /// <summary>
        /// Called when Play is pressed
        /// </summary>
        public void OnPlay()
        {
            // Disable the play button
            PlayEnabled = false;

            if (CancellationToken != null)
            {
                OnCancel();
            }

            // Create the cancellation token
            CancellationToken = new CancellationTokenSource();

            // Reset the game players and plot
            ResetGame();

            // Run game async
            Task.Run(async () =>
            {
                // Create the simulation
                ILcrSimulation simulation = new LcrSimulation();

                // Initialize the simulation
                simulation.InitializeSimulation(NumberOfGames, NumberOfPlayers);

                // Run the simulation
                await simulation.RunSimulationAsync(CancellationToken, Players);

                CancellationToken.Cancel(false);

                CancellationToken = null;

                PlayEnabled = true;

                // Update the plot
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    PlotControl.Plot.AddScatter(simulation.AllGameIndexes.ToArray(), simulation.AllGameLengths.ToArray(), color: System.Drawing.Color.FromArgb(255, 0, 56), label: "Game");
                    PlotControl.Plot.XAxis.Label("GAMES");
                    PlotControl.Plot.YAxis.Label("TURNS");

                    // Legend
                    var legend = PlotControl.Plot.Legend(true, Alignment.UpperRight);
                    legend.FillColor = System.Drawing.Color.Black;
                    legend.FontColor = System.Drawing.Color.FromArgb(236, 236, 236);
                    legend.FontBold = true;

                    // Shortest game tooltip
                    var shortTooltip = PlotControl.Plot.AddTooltip($"Shortest ({simulation.ShortestGameLength})", simulation.ShortestGameNumber, simulation.ShortestGameLength);
                    shortTooltip.FillColor = System.Drawing.Color.Black;
                    shortTooltip.BorderColor = System.Drawing.Color.MediumPurple;
                    shortTooltip.Font.Color = System.Drawing.Color.MediumPurple;
                    shortTooltip.Font.Bold = true;
                    shortTooltip.LabelPadding = 5;

                    // Longest game tooltip
                    var longTooltip = PlotControl.Plot.AddTooltip($"Longest ({simulation.LongestGameLength})", simulation.LongestGameNumber, simulation.LongestGameLength);
                    longTooltip.FillColor = System.Drawing.Color.Black;
                    longTooltip.BorderColor = System.Drawing.Color.Yellow;
                    longTooltip.Font.Color = System.Drawing.Color.Yellow;
                    longTooltip.Font.Bold = true;
                    longTooltip.LabelPadding = 5;

                    // Average horizontal line
                    var hline = PlotControl.Plot.AddHorizontalLine(simulation.AverageGameLength, label: "Average");
                    hline.LineWidth = 2;
                    hline.Color = System.Drawing.Color.Green;
                    hline.PositionLabel = true;
                    hline.PositionLabelBackground = hline.Color;
                    hline.DragEnabled = false;

                    PlotControl.Refresh();
                }));

            }, CancellationToken.Token);
        }

        /// <summary>
        /// Reset game players and plot
        /// </summary>
        private void ResetGame()
        {
            PlotControl.Plot.Clear();

            Players.Clear();

            PlotControl.Refresh();

            for (int i = 0; i < NumberOfPlayers; i++)
            {
                Players.Add(new PlayerViewModel() { PlayerNumber = i + 1 });
            }
        }

        /// <summary>
        /// Called when Cancel is pressed
        /// </summary>
        public void OnCancel()
        {
            if (CancellationToken != null)
            {
                CancellationToken.Cancel(false);
            }
        }

        protected ObservableCollection<PlayerViewModel> _players = new ObservableCollection<PlayerViewModel>() { 
            new PlayerViewModel() { PlayerNumber = 1 }, 
            new PlayerViewModel() { PlayerNumber = 2 }, 
            new PlayerViewModel() { PlayerNumber = 3 }
        };

        /// <summary>
        /// The players in the game
        /// </summary>
        public ObservableCollection<PlayerViewModel> Players
        {
            get
            {
                return _players;
            }
            set
            {
                if (value != _players)
                {
                    _players = value;
                    NotifyPropertyChanged("Players");
                }
            }
        }
    }
}

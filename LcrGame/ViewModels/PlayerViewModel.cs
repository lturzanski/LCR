namespace LcrGame.ViewModels
{
    public class PlayerViewModel : ViewModel
    {
        protected int _playerNumber;
        /// <summary>
        /// The player index number
        /// </summary>
        public int PlayerNumber
        {
            get
            {
                return _playerNumber;
            }
            set
            {
                if (value != _playerNumber)
                {
                    _playerNumber = value;
                    NotifyPropertyChanged("PlayerNumber");
                }
            }
        }

        protected bool _playerWon;
        /// <summary>
        /// Has this player won the game
        /// </summary>
        public bool PlayerWon
        {
            get
            {
                return _playerWon;
            }
            set
            {
                if (value != _playerWon)
                {
                    _playerWon = value;
                    NotifyPropertyChanged("PlayerWon");
                }
            }
        }
    }
}

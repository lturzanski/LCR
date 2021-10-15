using LcrGame.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LcrGame.ViewModels
{
    public abstract class ViewModel : DependencyObject, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Notifiable"/> class.
        /// </summary>
        public ViewModel()
        {
            _callbackCommand = new CallbackCommand(this);
        }

        private CallbackCommand _callbackCommand;

        /// <summary>
        /// Gets or sets the callback command.
        /// </summary>
        /// <value>
        /// The callback command.
        /// </value>
        public CallbackCommand CallbackCommand
        {
            get
            {
                return _callbackCommand;
            }
            set
            {
                if (value != _callbackCommand)
                {
                    _callbackCommand = value;
                    NotifyPropertyChanged("CallbackCommand");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies when the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

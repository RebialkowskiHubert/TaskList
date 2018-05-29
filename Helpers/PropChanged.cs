using System.ComponentModel;

namespace ToDoList.Helpers
{
    /// <summary>
    /// Klasa odpowiedzialna za implementację interfejsu INotifyPropertyChanged,
    /// który odpowiada za rozgłoszenie czy dane pole modelu zostało zmienione.
    /// </summary>
    public class PropChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}

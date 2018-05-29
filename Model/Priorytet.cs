using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDoList.Helpers;

namespace ToDoList.Model
{
    /// <summary>
    /// Reprezentuje priorytet zadania
    /// </summary>
    [Table("Priorytety", Schema = "dbo")]
    public class Priorytet : PropChanged
    {
        /// <summary>
        /// Identyfikator priorytetu
        /// </summary>
        private int id_priorytet;
        [Key]
        public int Id_priorytet
        {
            get { return id_priorytet; }
            set
            {
                if(id_priorytet != value)
                {
                    id_priorytet = value;
                    RaisePropertyChanged("Id_priorytet");
                }
            }
        }

        /// <summary>
        /// Nazwa priorytetu
        /// </summary>
        private string nazwa_priorytet;
        public string Nazwa_priorytet
        {
            get { return nazwa_priorytet; }
            set
            {
                if(nazwa_priorytet != value)
                {
                    nazwa_priorytet = value;
                    RaisePropertyChanged("Nazwa_priorytet");
                }
            }
        }

        public ObservableCollection<Zadanie> Zadania { get; set; }

        public Priorytet()
        {
            Zadania = new ObservableCollection<Zadanie>();
        }
    }
}

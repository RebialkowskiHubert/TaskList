using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDoList.Helpers;

namespace ToDoList.Model
{
    /// <summary>
    /// Reprezentuje status zadania
    /// </summary>
    [Table("Statusy", Schema = "dbo")]
    public class Status : PropChanged
    {
        /// <summary>
        /// Identyfikator statusu
        /// </summary>
        private int id_status;
        [Key]
        public int Id_status
        {
            get { return id_status; }
            set
            {
                if(id_status != value)
                {
                    id_status = value;
                    RaisePropertyChanged("Id_status");
                }
            }
        }

        /// <summary>
        /// Nazwa statusu zadania
        /// </summary>
        private string nazwa_status;
        public string Nazwa_status
        {
            get { return nazwa_status; }
            set
            {
                if(nazwa_status != value)
                {
                    nazwa_status = value;
                    RaisePropertyChanged("Nazwa_status");
                }
            }
        }

        public ObservableCollection<Zadanie> Zadania { get; set; }

        public Status()
        {
            Zadania = new ObservableCollection<Zadanie>();
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDoList.Helpers;

namespace ToDoList.Model
{
    /// <summary>
    /// Reprezentuje pojedyncze zadanie
    /// </summary>
    [Table("Zadania", Schema = "dbo")]
    public partial class Zadanie : PropChanged
    {
        /// <summary>
        /// Identyfikator zadania
        /// </summary>
        private int id_zadanie;
        [Key]
        public int Id_zadanie
        {
            get { return id_zadanie; }
            set
            {
                if (id_zadanie != value)
                {
                    id_zadanie = value;
                    RaisePropertyChanged("Id_zadanie");
                }
            }
        }

        /// <summary>
        /// Temat (lub skrócony opis) zadania
        /// </summary>
        private string temat;
        [StringLength(50)]
        public string Temat
        {
            get { return temat; }
            set
            {
                if (temat != value)
                {
                    temat = value;
                    RaisePropertyChanged("Temat");
                }
            }
        }

        /// <summary>
        /// Opis zadania
        /// </summary>
        private string opis;
        public string Opis
        {
            get { return opis; }
            set
            {
                if (opis != value)
                {
                    opis = value;
                    RaisePropertyChanged("Opis");
                }
            }
        }

        /// <summary>
        /// Data wpisu zadania
        /// </summary>
        private DateTime wpisano;
        public DateTime Wpisano
        {
            get { return wpisano; }
            set
            {
                if (wpisano != value)
                {
                    wpisano = value;
                    RaisePropertyChanged("Wpisano");
                }
            }
        }

        /// <summary>
        /// Klucz obcy priorytetu zadania
        /// </summary>
        [ForeignKey("Priorytet")]
        public int Id_priorytet { get; set; }

        /// <summary>
        /// Pole reprezentuj¹ce priorytet zadania
        /// </summary>
        private Priorytet priorytet;
        [ForeignKey("Priorytet")]
        public Priorytet Priorytet
        {
            get { return priorytet; }
            set
            {
                if (priorytet != value)
                {
                    priorytet = value;
                    RaisePropertyChanged("Priorytet");
                }
            }
        }

        /// <summary>
        /// Docelowa data realizacji
        /// </summary>
        private DateTime data;
        public DateTime Data
        {
            get { return data; }
            set
            {
                if (data != value)
                {
                    data = value;
                    RaisePropertyChanged("Data");
                }
            }
        }

        /// <summary>
        /// Klucz obcy statusu zadania
        /// </summary>
        [ForeignKey("Status")]
        public int Id_status { get; set; }

        /// <summary>
        /// Pole reprezentuj¹ce status zadania
        /// </summary>
        private Status status;
        [ForeignKey("Status")]
        public Status Status
        {
            get { return status; }
            set
            {
                if(status != value)
                {
                    status = value;
                    RaisePropertyChanged("Status");
                }
            }
        }
    }
}
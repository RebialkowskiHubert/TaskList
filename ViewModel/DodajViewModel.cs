using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ToDoList.DatabaseCtx;
using ToDoList.Helpers;
using ToDoList.Model;

namespace ToDoList.ViewModel
{
    /// <summary>
    /// Klasa ViewModel odpowiedzialna za okno modyfikacji/dodania zadania
    /// </summary>
    public class DodajViewModel : PropChanged
    {
        /// <summary>
        /// Pole kontekstu bazy danych
        /// </summary>
        private BazaKontekst DbContext;

        /// <summary>
        /// Pole zbindowane z przyciskiem Anuluj
        /// </summary>
        public MyICommand<Window> AnulujCommand { get; set; }

        /// <summary>
        /// Pole zbindowane z przyciskiem Dodaj
        /// </summary>
        public MyICommand<Window> DodajCommand { get; set; }

        /// <summary>
        /// Pole zbindowane z przyciskiem Edytuj
        /// </summary>
        public MyICommand<Window> EdytujCommand { get; set; }

        /// <summary>
        /// Wskazuje czy okno jest przeznczone do modyfikacji zadania (true) lub do dodawania (false)
        /// Niezbędne do ukrycia/pokazania elementów widoku, służących do modyfikacji
        /// </summary>
        public bool Edycja { get; set; }

        /// <summary>
        /// Wskazuje czy okno jest przeznczone do modyfikacji zadania (false) lub do dodawania (true)
        /// Niezbędne do ukrycia/pokazania elementów widoku, służących do dodawania
        /// </summary>
        public bool Dodawanie { get; set; }

        /// <summary>
        /// Przechowuje edytowane zadanie
        /// </summary>
        public Zadanie NoweZadanie { get; set; }

        /// <summary>
        /// Lista przechowująca wszystkie priorytety
        /// </summary>
        public List<Priorytet> Priorytety { get; set; }

        /// <summary>
        /// Lista przechowująca wszystkie statusy
        /// </summary>
        public List<Status> Statusy { get; set; }

        /// <summary>
        /// Lista przechowująca godziny doby
        /// </summary>
        public List<int> Godziny { get; set; }

        /// <summary>
        /// Lista przechowująca minuty danej godziny
        /// </summary>
        public List<int> Minuty { get; set; }

        /// <summary>
        /// Godzina zadania
        /// </summary>
        public int Godzina { get; set; }

        /// <summary>
        /// Minuta zadania
        /// </summary>
        public int Minuta { get; set; }

        /// <summary>
        /// Właściwość przechowująca tytuł okna
        /// </summary>
        public string Tytul { get; set; }

        /// <summary>
        /// Nazwa priorytetu zadania
        /// </summary>
        private string priorytetZad;
        public string PriorytetZad
        {
            get { return priorytetZad; }
            set
            {
                if(priorytetZad != value)
                {
                    priorytetZad = value;
                    RaisePropertyChanged("PriorytetZad");
                }
            }
        }

        /// <summary>
        /// Nazwa statusu zadania
        /// </summary>
        private string statusZad;
        public string StatusZad
        {
            get { return statusZad; }
            set
            {
                if(statusZad != value)
                {
                    statusZad = value;
                    RaisePropertyChanged("StatusZad");
                }
            }
        }


        public DodajViewModel()
        {
            Godziny = new List<int>();
            Minuty = new List<int>();

            for (int i = 0; i < 24; i++)
                Godziny.Add(i);

            for (int i = 0; i < 60; i++)
                Minuty.Add(i);
            
            AnulujCommand = new MyICommand<Window>(Zamknij);
            DodajCommand = new MyICommand<Window>(DodajAsync);
            EdytujCommand = new MyICommand<Window>(EdytujAsync);

            DbContext = new BazaKontekst();
            Priorytety = new List<Priorytet>();
            Statusy = new List<Status>();

            Priorytety = DbContext.Priorytety.ToList();
            Priorytety.Reverse();
            Statusy = DbContext.Statusy.ToList();
        }

        /// <summary>
        /// Odpowiada za wskazanie trybu (edycji lub dodawania zadania)
        /// </summary>
        /// <param name="zadanie">W trybie edycji Zadanie do edycji</param>
        /// <param name="data">W trybie dodawania data realizacji zadania</param>
        public void Tryb(Zadanie zadanie, DateTime? data = null)
        {
            if (zadanie == null)
            {
                Tytul = "Dodaj zadanie";
                NoweZadanie = new Zadanie
                {
                    Wpisano = DateTime.Now,
                    Data = (DateTime)data
                };
                Edycja = false;
            }
            else
            {
                Tytul = "Edytuj zadanie";
                NoweZadanie = zadanie;
                PriorytetZad = NoweZadanie.Priorytet.Nazwa_priorytet;
                StatusZad = NoweZadanie.Status.Nazwa_status;
                Godzina = NoweZadanie.Data.Hour;
                Minuta = NoweZadanie.Data.Minute;
                Edycja = true;
            }

            Dodawanie = !Edycja;
        }

        /// <summary>
        /// Funkcja zamykająca okno
        /// </summary>
        /// <param name="win">Okno</param>
        private void Zamknij(Window win)
        {
            win.Close();
        }

        /// <summary>
        /// Funkcja odpowiedzialna za dodanie Zadanie do bazy danych
        /// </summary>
        /// <param name="win">Okno do zamknięcia po zapisie</param>
        private async void DodajAsync(Window win)
        {
            if (!ScalIWaliduj())
                return;

            NoweZadanie.Status = Statusy.Where(s => s.Nazwa_status == "Do realizacji").First();

            DbContext.Zadania.Add(NoweZadanie);
            if (await DbContext.SaveChangesAsync() != 0)
                MessageBox.Show("Zadanie zostało dodane");
            else
            {
                MessageBox.Show("Wystąpił problem z zapisem");
                throw new Exception();
            }

            Zamknij(win);
        }

        /// <summary>
        /// Funkcja odpowiedzialna za zapisanie edytowanego zadania w bazie
        /// </summary>
        /// <param name="win">Okno do zamknięcia</param>
        private async void EdytujAsync(Window win)
        {
            if (!ScalIWaliduj())
                return;

            NoweZadanie.Status = Statusy.Where(s => s.Nazwa_status == StatusZad).First();

            Zadanie zadaniaDb = DbContext.Zadania.Where(z => z.Id_zadanie == NoweZadanie.Id_zadanie).First();
            zadaniaDb.Temat = NoweZadanie.Temat;
            zadaniaDb.Opis = NoweZadanie.Opis;
            zadaniaDb.Priorytet = NoweZadanie.Priorytet;
            zadaniaDb.Status = NoweZadanie.Status;
            zadaniaDb.Data = NoweZadanie.Data;

            await DbContext.SaveChangesAsync();
            MessageBox.Show("Zadanie zostało zmodyfikowane");
            
            Zamknij(win);
        }

        /// <summary>
        /// Funkcja odpowiedzialna za scalenie wszystkich informacji oraz za przeprowadzenie walidacji
        /// </summary>
        /// <returns>Czy zadanie zostało zwalidowane</returns>
        private bool ScalIWaliduj()
        {
            TimeSpan ts = new TimeSpan(Godzina, Minuta, 0);
            NoweZadanie.Data = new DateTime(NoweZadanie.Data.Year, NoweZadanie.Data.Month, NoweZadanie.Data.Day, Godzina, Minuta, 0, 0, NoweZadanie.Data.Kind);

            if (PriorytetZad == null)
            {
                MessageBox.Show("Wybierz priorytet zadania.");
                return false;
            }
            NoweZadanie.Priorytet = Priorytety.Where(p => p.Nazwa_priorytet == PriorytetZad).First();

            if (NoweZadanie.Temat == null)
            {
                MessageBox.Show("Wypełnij temat zadania.");
                return false;
            }

            if (NoweZadanie.Opis == null)
            {
                MessageBox.Show("Wypełnij opis zadania.");
                return false;
            }


            if (NoweZadanie.Data < DateTime.Now)
            {
                MessageBox.Show("Wpisano datę z przeszłości.");
                return false;
            }

            return true;
        }
    }
}

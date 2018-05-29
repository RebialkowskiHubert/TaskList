using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using ToDoList.DatabaseCtx;
using ToDoList.Helpers;
using ToDoList.Model;
using ToDoList.Views;
using Tulpep.NotificationWindow;

namespace ToDoList.ViewModel
{
    /// <summary>
    /// Klasa odpowiedzialna za obsługę listy zadań
    /// </summary>
    public class ZadaniaViewModel : PropChanged
    {
        /// <summary>
        /// Kontekst bazy danych
        /// </summary>
        private BazaKontekst context;

        /// <summary>
        /// Ilość wszystkich zadań z danego dnia
        /// </summary>
        private string razem = "0";
        public string Razem
        {
            get { return razem; }
            set
            {
                if(razem != value)
                {
                    razem = value;
                    RaisePropertyChanged("Razem");
                }
            }
        }


        /// <summary>
        /// Data wybrana przez użytkownika w datepicker
        /// </summary>
        private DateTime wybranaData = DateTime.Now;
        public DateTime WybranaData
        {
            get { return wybranaData; }
            set
            {
                wybranaData = value;
                LadujZadania(WybranaData);
            }
        }

        /// <summary>
        /// Lista przechowująca wszystkie zadania z danego dnia
        /// </summary>
        private ObservableCollection<Zadanie> zadaniaLista;
        public ObservableCollection<Zadanie> ZadaniaLista
        {
            get { return zadaniaLista; }
            set
            {
                if(zadaniaLista != value)
                {
                    zadaniaLista = value;
                    RaisePropertyChanged("ZadaniaLista");
                }
            }
        }

        /// <summary>
        /// Zadanie wybrane przez użytkownika
        /// </summary>
        private Zadanie zaznaczoneZadanie;
        public Zadanie ZaznaczoneZadanie
        {
            get { return zaznaczoneZadanie; }
            set
            {
                zaznaczoneZadanie = value;
                UsunCommand.RaiseCanExecuteChanged();
                PokazCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Pole zbindowane z przyciskiem Dodaj
        /// </summary>
        public MyICommand<Zadanie> DodajCommand { get; set; }

        /// <summary>
        /// Pole zbindowane z przyciskiem Pokaż
        /// </summary>
        public MyICommand PokazCommand { get; set; }

        /// <summary>
        /// Pole zbindowane z przyciskiem Usuń
        /// </summary>
        public MyICommand UsunCommand { get; set; }

        public ZadaniaViewModel()
        {
            context = new BazaKontekst();
            DodajCommand = new MyICommand<Zadanie>(Dodaj);
            PokazCommand = new MyICommand(Pokaz, CzyZaznaczony);
            UsunCommand = new MyICommand(UsunAsync, CzyZaznaczony);

            LadujZadania(WybranaData);
            PokazPowiadomienie();
        }

        /// <summary>
        /// Funkcja odpowiedzialna za otwarcie okna pozwalającego na dodanie zadania
        /// </summary>
        /// <param name="zad">null</param>
        private void Dodaj(Zadanie zad = null)
        {
            DodajView dv = new DodajView();
            DodajViewModel dvm = new DodajViewModel();
            if (zad == null)
                dvm.Tryb(zad, WybranaData);
            else
                dvm.Tryb(zad);

            dv.DataContext = dvm;
            dv.Show();

            dv.Closed += (d, e) =>
            {
                LadujZadania(WybranaData);
            };
        }

        /// <summary>
        /// Zwraca czy zadanie zostało wybrane przez użytkownika
        /// </summary>
        /// <returns>Czy zostało wybrane</returns>
        private bool CzyZaznaczony()
        {
            return ZaznaczoneZadanie != null;
        }

        /// <summary>
        /// Funkcja odpowiedzialna za otwarcie okna pozwalającego na podgląd i ewentualną modyfikację zadania
        /// </summary>
        private void Pokaz()
        {
            Dodaj(ZaznaczoneZadanie);
        }

        /// <summary>
        /// Funkcja odpowiedzialna za usunięcie zadania
        /// </summary>
        private async void UsunAsync()
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć zadanie?", "Usuń", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {
                int id = context.Zadania.Where(z => z.Id_zadanie == ZaznaczoneZadanie.Id_zadanie).First().Id_zadanie;
                if (await context.Database.ExecuteSqlCommandAsync("DELETE FROM Zadania WHERE Id_zadanie=" + id.ToString()) != 0)
                {
                    ZadaniaLista.Remove(ZaznaczoneZadanie);
                    MessageBox.Show("Zadanie zostało usunięte.");
                }
            }
        }

        /// <summary>
        /// Funkcja odpowiedzialna za załadowanie zadań z wybranego dnia
        /// </summary>
        /// <param name="dzien">Wybrany dzień</param>
        private void LadujZadania(DateTime dzien)
        {
            ZadaniaLista = ToObservable(context.Zadania
                .Include(z => z.Status)
                .Include(z => z.Priorytet)
                .Where(z => DbFunctions.TruncateTime(z.Data) == dzien.Date)
                .OrderBy(z => z.Data));

            Razem = ZadaniaLista.Count.ToString();
        }

        /// <summary>
        /// Funkcja konwertująca List<Zadania> na ObservableCollection<Zadania>
        /// </summary>
        /// <param name="lista">Lista zadań</param>
        /// <returns>ObservableCollection<Zadania></returns>
        private ObservableCollection<Zadanie> ToObservable(IEnumerable<Zadanie> lista)
        {
            return new ObservableCollection<Zadanie>(lista);
        }

        /// <summary>
        /// Funkcja odpoowiedzialna za pokazane powiadomienia o zbliżającym się zadaniu
        /// </summary>
        private void PokazPowiadomienie()
        {
            if(ZadaniaLista.Where(z => z.Data >= DateTime.Now).FirstOrDefault() != null)
            {
                Zadanie pierwsze = ZadaniaLista.Where(z => z.Data >= DateTime.Now).FirstOrDefault();
                string tresc = pierwsze.Data.Hour + ":" + pierwsze.Data.Minute + " - " + pierwsze.Temat;

                PopupNotifier popup = new PopupNotifier
                {
                    TitleText = "Lista zadań - przypomnienie",
                    ContentText = tresc
                };
                popup.Popup();
            }
        }
    }
}

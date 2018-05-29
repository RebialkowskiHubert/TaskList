using System.Data.Entity;
using System.Data.Entity.Migrations;
using ToDoList.Model;

namespace ToDoList.DatabaseCtx
{
    /// <summary>
    /// Klasa odpowiedzialna za tworzenie kontekstu bazy danych
    /// </summary>
    public class BazaKontekst : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            DbMigrationsConfiguration dmc = new DbMigrationsConfiguration();
            dmc.AutomaticMigrationsEnabled = true;
        }

        public virtual DbSet<Zadanie> Zadania { get; set; }
        public virtual DbSet<Priorytet> Priorytety { get; set; }
        public virtual DbSet<Status> Statusy { get; set; }
    }
}

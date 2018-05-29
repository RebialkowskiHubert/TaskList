namespace ToDoList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Zadania", "Priorytet_Id_priorytet", "dbo.Priorytety");
            DropForeignKey("dbo.Zadania", "Status_Id_status", "dbo.Statusy");
            DropIndex("dbo.Zadania", new[] { "Priorytet_Id_priorytet" });
            DropIndex("dbo.Zadania", new[] { "Status_Id_status" });
            RenameColumn(table: "dbo.Zadania", name: "Priorytet_Id_priorytet", newName: "Id_priorytet");
            RenameColumn(table: "dbo.Zadania", name: "Status_Id_status", newName: "Id_status");
            AlterColumn("dbo.Zadania", "Id_priorytet", c => c.Int(nullable: false));
            AlterColumn("dbo.Zadania", "Id_status", c => c.Int(nullable: false));
            CreateIndex("dbo.Zadania", "Id_priorytet");
            CreateIndex("dbo.Zadania", "Id_status");
            AddForeignKey("dbo.Zadania", "Id_priorytet", "dbo.Priorytety", "Id_priorytet");
            AddForeignKey("dbo.Zadania", "Id_status", "dbo.Statusy", "Id_status");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Zadania", "Id_status", "dbo.Statusy");
            DropForeignKey("dbo.Zadania", "Id_priorytet", "dbo.Priorytety");
            DropIndex("dbo.Zadania", new[] { "Id_status" });
            DropIndex("dbo.Zadania", new[] { "Id_priorytet" });
            AlterColumn("dbo.Zadania", "Id_status", c => c.Int());
            AlterColumn("dbo.Zadania", "Id_priorytet", c => c.Int());
            RenameColumn(table: "dbo.Zadania", name: "Id_status", newName: "Status_Id_status");
            RenameColumn(table: "dbo.Zadania", name: "Id_priorytet", newName: "Priorytet_Id_priorytet");
            CreateIndex("dbo.Zadania", "Status_Id_status");
            CreateIndex("dbo.Zadania", "Priorytet_Id_priorytet");
            AddForeignKey("dbo.Zadania", "Status_Id_status", "dbo.Statusy", "Id_status");
            AddForeignKey("dbo.Zadania", "Priorytet_Id_priorytet", "dbo.Priorytety", "Id_priorytet");
        }
    }
}

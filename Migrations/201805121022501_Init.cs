namespace ToDoList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Priorytety",
                c => new
                    {
                        Id_priorytet = c.Int(nullable: false, identity: true),
                        Nazwa_priorytet = c.String(),
                    })
                .PrimaryKey(t => t.Id_priorytet);
            
            CreateTable(
                "dbo.Statusy",
                c => new
                    {
                        Id_status = c.Int(nullable: false, identity: true),
                        Nazwa_status = c.String(),
                    })
                .PrimaryKey(t => t.Id_status);
            
            CreateTable(
                "dbo.Zadania",
                c => new
                    {
                        Id_zadanie = c.Int(nullable: false, identity: true),
                        Temat = c.String(maxLength: 50),
                        Opis = c.String(),
                        Wpisano = c.DateTime(nullable: false),
                        Data = c.DateTime(nullable: false),
                        Priorytet_Id_priorytet = c.Int(),
                        Status_Id_status = c.Int(),
                    })
                .PrimaryKey(t => t.Id_zadanie)
                .ForeignKey("dbo.Priorytety", t => t.Priorytet_Id_priorytet)
                .ForeignKey("dbo.Statusy", t => t.Status_Id_status)
                .Index(t => t.Priorytet_Id_priorytet)
                .Index(t => t.Status_Id_status);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Zadania", "Status_Id_status", "dbo.Statusy");
            DropForeignKey("dbo.Zadania", "Priorytet_Id_priorytet", "dbo.Priorytety");
            DropIndex("dbo.Zadania", new[] { "Status_Id_status" });
            DropIndex("dbo.Zadania", new[] { "Priorytet_Id_priorytet" });
            DropTable("dbo.Zadania");
            DropTable("dbo.Statusy");
            DropTable("dbo.Priorytety");
        }
    }
}

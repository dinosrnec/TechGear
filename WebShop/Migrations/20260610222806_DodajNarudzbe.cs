using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class DodajNarudzbe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "korisnik",
                columns: table => new
                {
                    KorisnikID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    Lozinka = table.Column<string>(type: "longtext", nullable: false),
                    Ime = table.Column<string>(type: "longtext", nullable: true),
                    Prezime = table.Column<string>(type: "longtext", nullable: true),
                    Uloga = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_korisnik", x => x.KorisnikID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "proizvod",
                columns: table => new
                {
                    proizvodID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    naziv = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    opis = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    cijena = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    kategorija = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    slikaUrl = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Lager = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_proizvod", x => x.proizvodID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "recenzija",
                columns: table => new
                {
                    recenzijaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    korisnikIme = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    tekst = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    ocjena = table.Column<int>(type: "int", nullable: false),
                    datumKreiranja = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recenzija", x => x.recenzijaID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "narudzba",
                columns: table => new
                {
                    NarudzbaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    KorisnikID = table.Column<int>(type: "int", nullable: false),
                    DatumNarudžbe = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UkupnaCijena = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_narudzba", x => x.NarudzbaID);
                    table.ForeignKey(
                        name: "FK_narudzba_korisnik_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "korisnik",
                        principalColumn: "KorisnikID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "wishlist",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    korisnikID = table.Column<int>(type: "int", nullable: false),
                    proizvodID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wishlist", x => x.id);
                    table.ForeignKey(
                        name: "FK_wishlist_proizvod_proizvodID",
                        column: x => x.proizvodID,
                        principalTable: "proizvod",
                        principalColumn: "proizvodID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "narudzba_stavka",
                columns: table => new
                {
                    NarudzbaStavkaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    NarudzbaID = table.Column<int>(type: "int", nullable: false),
                    ProizvodID = table.Column<int>(type: "int", nullable: false),
                    Naziv = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Cijena = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_narudzba_stavka", x => x.NarudzbaStavkaID);
                    table.ForeignKey(
                        name: "FK_narudzba_stavka_narudzba_NarudzbaID",
                        column: x => x.NarudzbaID,
                        principalTable: "narudzba",
                        principalColumn: "NarudzbaID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_narudzba_KorisnikID",
                table: "narudzba",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_narudzba_stavka_NarudzbaID",
                table: "narudzba_stavka",
                column: "NarudzbaID");

            migrationBuilder.CreateIndex(
                name: "IX_wishlist_proizvodID",
                table: "wishlist",
                column: "proizvodID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "narudzba_stavka");

            migrationBuilder.DropTable(
                name: "recenzija");

            migrationBuilder.DropTable(
                name: "wishlist");

            migrationBuilder.DropTable(
                name: "narudzba");

            migrationBuilder.DropTable(
                name: "proizvod");

            migrationBuilder.DropTable(
                name: "korisnik");
        }
    }
}

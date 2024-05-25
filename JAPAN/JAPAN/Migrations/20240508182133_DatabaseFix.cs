using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JAPAN.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pitanje",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tekst = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pitanje_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tezina",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    naziv = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tezina_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tip_sadrzaj",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    naziv = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tip_sadrzaj_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "uloga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    naziv = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("uloga_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "odgovor",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tekst = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tocno = table.Column<int>(type: "integer", nullable: false),
                    idpitanje = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("odgovor_pkey", x => x.id);
                    table.ForeignKey(
                        name: "odgovor_idpitanje_fkey",
                        column: x => x.idpitanje,
                        principalTable: "pitanje",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ispit",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    naziv = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    opis = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    pozicija = table.Column<double>(type: "double precision", nullable: false),
                    idtezina = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ispit_pkey", x => x.id);
                    table.ForeignKey(
                        name: "ispit_idtezina_fkey",
                        column: x => x.idtezina,
                        principalTable: "tezina",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tecaj",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    naziv = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    opis = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    sadrzaj = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    kreirano = table.Column<DateOnly>(type: "date", nullable: false),
                    pozicija = table.Column<double>(type: "double precision", nullable: false),
                    idtipsadrzaj = table.Column<int>(type: "integer", nullable: false),
                    idtezina = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tecaj_pkey", x => x.id);
                    table.ForeignKey(
                        name: "tecaj_idtezina_fkey",
                        column: x => x.idtezina,
                        principalTable: "tezina",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "tecaj_idtipsadrzaj_fkey",
                        column: x => x.idtipsadrzaj,
                        principalTable: "tip_sadrzaj",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "korisnik",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    korisnickoime = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    identifikator = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ime = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    prezime = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    datumrodenja = table.Column<DateOnly>(type: "date", nullable: true),
                    preporuka = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    iduloga = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("korisnik_pkey", x => x.id);
                    table.ForeignKey(
                        name: "korisnik_iduloga_fkey",
                        column: x => x.iduloga,
                        principalTable: "uloga",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ispitpitanje",
                columns: table => new
                {
                    idispit = table.Column<int>(type: "integer", nullable: false),
                    idpitanje = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ispitpitanje_pkey", x => new { x.idispit, x.idpitanje });
                    table.ForeignKey(
                        name: "ispitpitanje_idispit_fkey",
                        column: x => x.idispit,
                        principalTable: "ispit",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ispitpitanje_idpitanje_fkey",
                        column: x => x.idpitanje,
                        principalTable: "pitanje",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "forum_pitanje",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    naslov = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    sadrzaj = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    kreirano = table.Column<DateOnly>(type: "date", nullable: false),
                    idkorisnik = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("forum_pitanje_pkey", x => x.id);
                    table.ForeignKey(
                        name: "forum_pitanje_idkorisnik_fkey",
                        column: x => x.idkorisnik,
                        principalTable: "korisnik",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "statistika",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rezultat = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    zavrseno = table.Column<DateOnly>(type: "date", nullable: false),
                    idkorisnik = table.Column<int>(type: "integer", nullable: false),
                    idtecaj = table.Column<int>(type: "integer", nullable: true),
                    idispit = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("statistika_pkey", x => x.id);
                    table.ForeignKey(
                        name: "statistika_idispit_fkey",
                        column: x => x.idispit,
                        principalTable: "ispit",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "statistika_idkorisnik_fkey",
                        column: x => x.idkorisnik,
                        principalTable: "korisnik",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "statistika_idtecaj_fkey",
                        column: x => x.idtecaj,
                        principalTable: "tecaj",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "forum_odgovor",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sadrzaj = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    kreirano = table.Column<DateOnly>(type: "date", nullable: false),
                    idkorisnik = table.Column<int>(type: "integer", nullable: false),
                    idpitanje = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("forum_odgovor_pkey", x => x.id);
                    table.ForeignKey(
                        name: "forum_odgovor_idkorisnik_fkey",
                        column: x => x.idkorisnik,
                        principalTable: "korisnik",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "forum_odgovor_idpitanje_fkey",
                        column: x => x.idpitanje,
                        principalTable: "forum_pitanje",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_forum_odgovor_idkorisnik",
                table: "forum_odgovor",
                column: "idkorisnik");

            migrationBuilder.CreateIndex(
                name: "IX_forum_odgovor_idpitanje",
                table: "forum_odgovor",
                column: "idpitanje");

            migrationBuilder.CreateIndex(
                name: "IX_forum_pitanje_idkorisnik",
                table: "forum_pitanje",
                column: "idkorisnik");

            migrationBuilder.CreateIndex(
                name: "IX_ispit_idtezina",
                table: "ispit",
                column: "idtezina");

            migrationBuilder.CreateIndex(
                name: "IX_ispitpitanje_idpitanje",
                table: "ispitpitanje",
                column: "idpitanje");

            migrationBuilder.CreateIndex(
                name: "IX_korisnik_id",
                table: "korisnik",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_korisnik_iduloga",
                table: "korisnik",
                column: "iduloga");

            migrationBuilder.CreateIndex(
                name: "IX_korisnik_korisnickoime",
                table: "korisnik",
                column: "korisnickoime",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_odgovor_idpitanje",
                table: "odgovor",
                column: "idpitanje");

            migrationBuilder.CreateIndex(
                name: "IX_statistika_idispit",
                table: "statistika",
                column: "idispit");

            migrationBuilder.CreateIndex(
                name: "IX_statistika_idkorisnik",
                table: "statistika",
                column: "idkorisnik");

            migrationBuilder.CreateIndex(
                name: "IX_statistika_idtecaj",
                table: "statistika",
                column: "idtecaj");

            migrationBuilder.CreateIndex(
                name: "IX_tecaj_idtezina",
                table: "tecaj",
                column: "idtezina");

            migrationBuilder.CreateIndex(
                name: "IX_tecaj_idtipsadrzaj",
                table: "tecaj",
                column: "idtipsadrzaj");

            // Seed podaci za tablicu Uloga
            migrationBuilder.InsertData(
                table: "uloga",
                columns: new[] { "id", "naziv" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Moderator" },
                    { 3, "Korisnik" }
                });

            // Seed podaci za tablicu Tip_Sadrzaj
            migrationBuilder.InsertData(
                table: "tip_sadrzaj",
                columns: new[] { "id", "naziv" },
                values: new object[,]
                {
                    { 1, "Tekst" },
                    { 2, "Markdown" },
                    { 3, "HTML" },
                    { 4, "Audio" },
                    { 5, "Video" }
                });

            // Seed podaci za tablicu Tezina
            migrationBuilder.InsertData(
                table: "tezina",
                columns: new[] { "id", "naziv" },
                values: new object[,]
                {
                    { 1, "Početnik" },
                    { 2, "Srednje znanje" },
                    { 3, "Napredno" }
                });

            // Seed podaci za tablicu Pitanje
            migrationBuilder.InsertData(
                table: "pitanje",
                columns: new[] { "id", "tekst" },
                values: new object[,]
                {
                    { 1, "Kako se kaže 'zdravo' na japanskom?" },
                    { 2, "Što znači 'arigatou'?" },
                    { 3, "Kako se piše 'japan' na japanskom pismu?" }
                });

            // Seed podaci za tablicu Odgovor
            migrationBuilder.InsertData(
                table: "odgovor",
                columns: new[] { "id", "tekst", "tocno", "idpitanje" },
                values: new object[,]
                {
                    { 1, "こんにちは (Konnichiwa)", 1, 1 },
                    { 2, "ありがとう (Arigatou)", 0, 1 },
                    { 3, "じゃね (Ja ne)", 0, 1 },
                    { 4, "Hvala", 1, 2 },
                    { 5, "Japanski", 0, 3 },
                    { 6, "日本", 1, 3 }
                });

            // Seed podaci za tablicu Korisnik
            migrationBuilder.InsertData(
                table: "korisnik",
                columns: new[] { "korisnickoime", "identifikator", "id", "iduloga" },
                values: new object[,]
                {
                    { "admin", "auth0|663a2956a2d3c01179d78ead", 1, 1 },
                    { "moderator", "auth0|663a29d048650c09beb04c47", 2, 2 },
                    { "korisnik1", "auth0|663a2a1bce64ccdb7d4f1560", 3, 3 },
                    { "korisnik2", "auth0|663a2a64505c4c1b0c843839", 4, 3 }
                });

            // Seed podaci za tablicu Tecaj
            migrationBuilder.InsertData(
                table: "tecaj",
                columns: new[] { "id", "naziv", "opis", "sadrzaj", "kreirano", "pozicija", "idtipsadrzaj", "idtezina" },
                values: new object[,]
                {
                    { 1, "Osnove japanskog jezika", "Tecaj za pocetnike koji zele nauciti osnove japanskog jezika.", "Ovo je sadrzaj tecaja...", new DateOnly(2024, 4, 14), 10, 1, 1 },
                    { 2, "Više o japanskom", "Tecaj za one koji vec imaju osnovno znanje japanskog jezika i zele ga poboljsati.", "Ovo je sadrzaj tecaja...", new DateOnly(2024, 4, 14), 20, 1, 2 }
                });

            // Seed podaci za tablicu Ispit
            migrationBuilder.InsertData(
                table: "ispit",
                columns: new[] { "id", "naziv", "opis", "pozicija", "idtezina" },
                values: new object[,]
                {
                    { 1, "Testiranje osnova", "Test za provjeru osnovnog znanja japanskog jezika.", 10, 1 },
                    { 2, "Testiranje naprednih vjestina", "Test za provjeru naprednih vjestina u japanskom jeziku.", 20, 3 }
                });

            // Seed podaci za tablicu Statistika
            migrationBuilder.InsertData(
                table: "statistika",
                columns: new[] { "id", "rezultat", "zavrseno", "idkorisnik", "idtecaj", "idispit" },
                values: new object[,]
                {
                    { 1, "Završeno", new DateOnly(2024, 4, 14), 3, 1, null },
                    { 2, "Završeno", new DateOnly(2024, 4, 14), 4, 1, null },
                    { 3, "1/2", new DateOnly(2024, 4, 14), 4, null, 1 }
                });

            // Seed podaci za tablicu Forum_Pitanje
            migrationBuilder.InsertData(
                table: "forum_pitanje",
                columns: new[] { "id", "naslov", "sadrzaj", "kreirano", "idkorisnik" },
                values: new object[,]
                {
                    { 1, "Kako koristiti japanske brojeve?", "Molim vas da mi pomognete oko koristenja japanskih brojeva.", new DateOnly(2024, 4, 14), 3 },
                    { 2, "Pomoc oko glagola", "Imam problema s konjugacijom glagola u proslosti.", new DateOnly(2024, 4, 14), 4 }
                });

            // Seed podaci za tablicu Forum_Odgovor
            migrationBuilder.InsertData(
                table: "forum_odgovor",
                columns: new[] { "id", "sadrzaj", "kreirano", "idkorisnik", "idpitanje" },
                values: new object[,]
                {
                    { 1, "Japanski brojevi se koriste na različite načine...", new DateOnly(2024, 4, 14), 4, 1 },
                    { 2, "Evo nekoliko primjera korištenja japanskih brojeva...", new DateOnly(2024, 4, 14), 3, 1 },
                    { 3, "Za konjugaciju glagola u prošlosti koristite prošlo vrijeme...", new DateOnly(2024, 4, 14), 3, 2 },
                    { 4, "Evo primjera kako se konjugiraju glagoli u prošlosti...", new DateOnly(2024, 4, 14), 4, 2 }
                });

            // Seed podaci za tablicu IspitPitanje
            migrationBuilder.InsertData(
                table: "ispitpitanje",
                columns: new[] { "idispit", "idpitanje" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "forum_odgovor");

            migrationBuilder.DropTable(
                name: "ispitpitanje");

            migrationBuilder.DropTable(
                name: "odgovor");

            migrationBuilder.DropTable(
                name: "statistika");

            migrationBuilder.DropTable(
                name: "forum_pitanje");

            migrationBuilder.DropTable(
                name: "pitanje");

            migrationBuilder.DropTable(
                name: "ispit");

            migrationBuilder.DropTable(
                name: "tecaj");

            migrationBuilder.DropTable(
                name: "korisnik");

            migrationBuilder.DropTable(
                name: "tezina");

            migrationBuilder.DropTable(
                name: "tip_sadrzaj");

            migrationBuilder.DropTable(
                name: "uloga");
        }
    }
}

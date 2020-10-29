using Microsoft.EntityFrameworkCore.Migrations;

namespace RigantiGraphQlDemo.Dal.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    SecretPiggyBankLocation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Farms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    PersonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Farms_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Species = table.Column<string>(nullable: true),
                    FarmId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animals_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Name", "SecretPiggyBankLocation" },
                values: new object[] { 1, "Mr. Jones", "In a dark cave." });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Name", "SecretPiggyBankLocation" },
                values: new object[] { 2, "Mr. Whymper", "Does not have a piggy bank." });

            migrationBuilder.InsertData(
                table: "Farms",
                columns: new[] { "Id", "Name", "PersonId" },
                values: new object[] { 1, "Manor Farm", 1 });

            migrationBuilder.InsertData(
                table: "Farms",
                columns: new[] { "Id", "Name", "PersonId" },
                values: new object[] { 2, "AnimalFarm", 2 });

            migrationBuilder.InsertData(
                table: "Animals",
                columns: new[] { "Id", "FarmId", "Name", "Species" },
                values: new object[] { 1, 1, "Napoleon", "Pig" });

            migrationBuilder.InsertData(
                table: "Animals",
                columns: new[] { "Id", "FarmId", "Name", "Species" },
                values: new object[] { 2, 1, "Snowball", "Pig" });

            migrationBuilder.InsertData(
                table: "Animals",
                columns: new[] { "Id", "FarmId", "Name", "Species" },
                values: new object[] { 3, 1, "Boxer", "Horse" });

            migrationBuilder.InsertData(
                table: "Animals",
                columns: new[] { "Id", "FarmId", "Name", "Species" },
                values: new object[] { 4, 1, "Moses", "Raven" });

            migrationBuilder.InsertData(
                table: "Animals",
                columns: new[] { "Id", "FarmId", "Name", "Species" },
                values: new object[] { 5, 1, "Benjamin", "Donkey" });

            migrationBuilder.InsertData(
                table: "Animals",
                columns: new[] { "Id", "FarmId", "Name", "Species" },
                values: new object[] { 6, 2, "AnonymousCat", "Cat" });

            migrationBuilder.InsertData(
                table: "Animals",
                columns: new[] { "Id", "FarmId", "Name", "Species" },
                values: new object[] { 7, 2, "AnonymousGoat", "Goat" });

            migrationBuilder.CreateIndex(
                name: "IX_Animals_FarmId",
                table: "Animals",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_PersonId",
                table: "Farms",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "Farms");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}

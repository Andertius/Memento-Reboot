using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Memento.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Cards",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Word = table.Column<string>(type: "text", nullable: true),
                Translation = table.Column<string>(type: "text", nullable: true),
                Definition = table.Column<string>(type: "text", nullable: true),
                Hint = table.Column<string>(type: "text", nullable: true),
                Image = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Cards", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Categories",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "text", nullable: true),
                Description = table.Column<string>(type: "text", nullable: true),
                Image = table.Column<string>(type: "text", nullable: true),
                CardEntityId = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Categories", x => x.Id);
                table.ForeignKey(
                    name: "FK_Categories_Cards_CardEntityId",
                    column: x => x.CardEntityId,
                    principalTable: "Cards",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Tags",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "text", nullable: true),
                CardEntityId = table.Column<int>(type: "integer", nullable: true),
                CategoryEntityId = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tags", x => x.Id);
                table.ForeignKey(
                    name: "FK_Tags_Cards_CardEntityId",
                    column: x => x.CardEntityId,
                    principalTable: "Cards",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Tags_Categories_CategoryEntityId",
                    column: x => x.CategoryEntityId,
                    principalTable: "Categories",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Categories_CardEntityId",
            table: "Categories",
            column: "CardEntityId");

        migrationBuilder.CreateIndex(
            name: "IX_Tags_CardEntityId",
            table: "Tags",
            column: "CardEntityId");

        migrationBuilder.CreateIndex(
            name: "IX_Tags_CategoryEntityId",
            table: "Tags",
            column: "CategoryEntityId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Tags");

        migrationBuilder.DropTable(
            name: "Categories");

        migrationBuilder.DropTable(
            name: "Cards");
    }
}

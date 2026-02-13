using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Memento.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Cards_CardEntityId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Cards_CardEntityId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Categories_CategoryEntityId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_CardEntityId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_CategoryEntityId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CardEntityId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CardEntityId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "CategoryEntityId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "CardEntityId",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Word",
                table: "Cards",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Definition",
                table: "Cards",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CardEntityCategoryEntity",
                columns: table => new
                {
                    CardsId = table.Column<int>(type: "integer", nullable: false),
                    CategoriesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardEntityCategoryEntity", x => new { x.CardsId, x.CategoriesId });
                    table.ForeignKey(
                        name: "FK_CardEntityCategoryEntity_Cards_CardsId",
                        column: x => x.CardsId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardEntityCategoryEntity_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardEntityTagEntity",
                columns: table => new
                {
                    CardsId = table.Column<int>(type: "integer", nullable: false),
                    TagsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardEntityTagEntity", x => new { x.CardsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_CardEntityTagEntity_Cards_CardsId",
                        column: x => x.CardsId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardEntityTagEntity_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryEntityTagEntity",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "integer", nullable: false),
                    TagsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryEntityTagEntity", x => new { x.CategoriesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_CategoryEntityTagEntity_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryEntityTagEntity_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardEntityCategoryEntity_CategoriesId",
                table: "CardEntityCategoryEntity",
                column: "CategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_CardEntityTagEntity_TagsId",
                table: "CardEntityTagEntity",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryEntityTagEntity_TagsId",
                table: "CategoryEntityTagEntity",
                column: "TagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardEntityCategoryEntity");

            migrationBuilder.DropTable(
                name: "CardEntityTagEntity");

            migrationBuilder.DropTable(
                name: "CategoryEntityTagEntity");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "CardEntityId",
                table: "Tags",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryEntityId",
                table: "Tags",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "CardEntityId",
                table: "Categories",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Word",
                table: "Cards",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Definition",
                table: "Cards",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_CardEntityId",
                table: "Tags",
                column: "CardEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_CategoryEntityId",
                table: "Tags",
                column: "CategoryEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CardEntityId",
                table: "Categories",
                column: "CardEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Cards_CardEntityId",
                table: "Categories",
                column: "CardEntityId",
                principalTable: "Cards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Cards_CardEntityId",
                table: "Tags",
                column: "CardEntityId",
                principalTable: "Cards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Categories_CategoryEntityId",
                table: "Tags",
                column: "CategoryEntityId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}

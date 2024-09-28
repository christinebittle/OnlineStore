using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class categorydescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryDescription",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryDescription",
                table: "Categories");
        }
    }
}

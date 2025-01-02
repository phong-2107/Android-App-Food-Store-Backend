using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerAPICanteen.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCanteen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Canteens");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Canteens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Canteens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Canteens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

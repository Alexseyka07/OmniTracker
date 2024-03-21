using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmniTracker.Migrations
{
    /// <inheritdoc />
    public partial class TermMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TermEimination",
                table: "Requests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TermEimination",
                table: "Requests");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceDayAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddBannerUrlToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ONLY add the new column
            migrationBuilder.AddColumn<string>(
                name: "BannerUrl",
                table: "Event",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // ONLY drop the column if we roll back
            migrationBuilder.DropColumn(
                name: "BannerUrl",
                table: "Event");
        }
    }
}
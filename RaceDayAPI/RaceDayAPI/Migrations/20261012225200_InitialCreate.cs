using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceDayAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "EventOrganiser",
                columns: table => new
                {
                    OrganiserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventOrganiser", x => x.OrganiserID);
                    table.ForeignKey(
                        name: "FK_EventOrganiser_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participant",
                columns: table => new
                {
                    ParticipantID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participant", x => x.ParticipantID);
                    table.ForeignKey(
                        name: "FK_Participant_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganiserID = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Distance = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventID);
                    table.CheckConstraint("CK_Event_Distance", "Distance > 0");
                    table.CheckConstraint("CK_Event_EventType", "EventType IN ('Walk', 'Run', 'Cycle')");
                    table.ForeignKey(
                        name: "FK_Event_EventOrganiser_OrganiserID",
                        column: x => x.OrganiserID,
                        principalTable: "EventOrganiser",
                        principalColumn: "OrganiserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventCategories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AgeGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DistanceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategories", x => x.CategoryID);
                    table.CheckConstraint("CK_EventCategory_AgeOrDistance", "AgeGroup IS NOT NULL OR DistanceType IS NOT NULL");
                    table.ForeignKey(
                        name: "FK_EventCategories_Event_EventID",
                        column: x => x.EventID,
                        principalTable: "Event",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Route",
                columns: table => new
                {
                    RouteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    RouteDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Distance = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Difficulty = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MapURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Route", x => x.RouteID);
                    table.ForeignKey(
                        name: "FK_Route_Event_EventID",
                        column: x => x.EventID,
                        principalTable: "Event",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventEnrolment",
                columns: table => new
                {
                    EnrolmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParticipantID = table.Column<int>(type: "int", nullable: false),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    EnrolmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventEnrolment", x => x.EnrolmentID);
                    table.CheckConstraint("CK_EventEnrolment_Status", "Status IN ('Confirmed', 'Withdrawn')");
                    table.ForeignKey(
                        name: "FK_EventEnrolment_EventCategories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "EventCategories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventEnrolment_Event_EventID",
                        column: x => x.EventID,
                        principalTable: "Event",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventEnrolment_Participant_ParticipantID",
                        column: x => x.ParticipantID,
                        principalTable: "Participant",
                        principalColumn: "ParticipantID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    ResultID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnrolmentID = table.Column<int>(type: "int", nullable: false),
                    FinishTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    DateOfResults = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.ResultID);
                    table.CheckConstraint("CK_Result_Position", "Position > 0");
                    table.ForeignKey(
                        name: "FK_Results_EventEnrolment_EnrolmentID",
                        column: x => x.EnrolmentID,
                        principalTable: "EventEnrolment",
                        principalColumn: "EnrolmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Event_OrganiserID",
                table: "Event",
                column: "OrganiserID");

            migrationBuilder.CreateIndex(
                name: "IX_EventCategories_EventID",
                table: "EventCategories",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_EventEnrolment_CategoryID",
                table: "EventEnrolment",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_EventEnrolment_EventID",
                table: "EventEnrolment",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_EventEnrolment_ParticipantID_EventID_CategoryID",
                table: "EventEnrolment",
                columns: new[] { "ParticipantID", "EventID", "CategoryID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventOrganiser_UserID",
                table: "EventOrganiser",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participant_UserID",
                table: "Participant",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Results_EnrolmentID",
                table: "Results",
                column: "EnrolmentID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Route_EventID",
                table: "Route",
                column: "EventID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_EmailAddress",
                table: "User",
                column: "EmailAddress",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Route");

            migrationBuilder.DropTable(
                name: "EventEnrolment");

            migrationBuilder.DropTable(
                name: "EventCategories");

            migrationBuilder.DropTable(
                name: "Participant");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "EventOrganiser");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}

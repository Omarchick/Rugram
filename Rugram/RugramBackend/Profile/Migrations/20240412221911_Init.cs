using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileName = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfileUserProfile",
                columns: table => new
                {
                    SubscribedToId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscribersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileUserProfile", x => new { x.SubscribedToId, x.SubscribersId });
                    table.ForeignKey(
                        name: "FK_UserProfileUserProfile_UserProfiles_SubscribedToId",
                        column: x => x.SubscribedToId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfileUserProfile_UserProfiles_SubscribersId",
                        column: x => x.SubscribersId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_ProfileName",
                table: "UserProfiles",
                column: "ProfileName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileUserProfile_SubscribersId",
                table: "UserProfileUserProfile",
                column: "SubscribersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfileUserProfile");

            migrationBuilder.DropTable(
                name: "UserProfiles");
        }
    }
}

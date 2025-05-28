using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestrantApplication.EF.Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPictureTableAndAddRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PictureID",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserPicture",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PictureName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPicture", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PictureID",
                table: "AspNetUsers",
                column: "PictureID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserPicture_PictureID",
                table: "AspNetUsers",
                column: "PictureID",
                principalTable: "UserPicture",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserPicture_PictureID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserPicture");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PictureID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PictureID",
                table: "AspNetUsers");
        }
    }
}

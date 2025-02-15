using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentProject.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStudentThirdPArtyUIdUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_student_third_party_uid",
                schema: "dbo",
                table: "tb_student");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tb_student_third_party_uid",
                schema: "dbo",
                table: "tb_student",
                column: "third_party_uid",
                unique: true,
                filter: "[third_party_uid] IS NOT NULL");
        }
    }
}

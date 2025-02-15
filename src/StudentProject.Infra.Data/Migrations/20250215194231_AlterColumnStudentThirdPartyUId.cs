using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentProject.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterColumnStudentThirdPartyUId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "third_party_student_uid",
                schema: "dbo",
                table: "tb_student",
                newName: "third_party_uid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "third_party_uid",
                schema: "dbo",
                table: "tb_student",
                newName: "third_party_student_uid");
        }
    }
}

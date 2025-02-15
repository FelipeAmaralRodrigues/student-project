using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentProject.Infra.CrossCutting.Bus.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumCreateStudentThirdPartySagaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudentUId",
                schema: "mst",
                table: "tb_student_created_third_party_registration_saga_data",
                newName: "student_uid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "student_uid",
                schema: "mst",
                table: "tb_student_created_third_party_registration_saga_data",
                newName: "StudentUId");
        }
    }
}

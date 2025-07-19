using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentProject.Infra.CrossCutting.Bus.Migrations
{
    /// <inheritdoc />
    public partial class AddTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "mst");

            migrationBuilder.CreateTable(
                name: "tb_student_created_third_party_registration_saga_data",
                schema: "mst",
                columns: table => new
                {
                    correlation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    current_state = table.Column<string>(type: "varchar(64)", nullable: false),
                    saga_init_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    student_uid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    request_uid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    request_create_student_third_party_uid_sended_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    response_create_student_third_party_uid_not_received_retry_count = table.Column<int>(type: "int", nullable: true),
                    response_create_student_third_party_uid_not_received_last_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    response_create_student_third_party_uid_not_received_schedule_token_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    response_create_student_third_party_uid_received_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    student_third_party_uid_updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKstudentcreatedthirdpartyregistrationsagadata", x => x.correlation_id)
                        .Annotation("SqlServer:Clustered", true);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_student_created_third_party_registration_saga_data",
                schema: "mst");
        }
    }
}

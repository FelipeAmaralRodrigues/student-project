using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentProject.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "tb_student",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    first_name = table.Column<string>(type: "varchar(128)", nullable: false),
                    last_name = table.Column<string>(type: "varchar(128)", nullable: false),
                    birth_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    email = table.Column<string>(type: "varchar(128)", nullable: false),
                    third_party_uid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKtbstudent", x => x.id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_student_email",
                schema: "dbo",
                table: "tb_student",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_student_uid",
                schema: "dbo",
                table: "tb_student",
                column: "uid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_student",
                schema: "dbo");
        }
    }
}

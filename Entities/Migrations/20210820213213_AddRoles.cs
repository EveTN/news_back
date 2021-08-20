using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("8d691ee5-2ee9-4534-84c2-8391e9c503f8"), "9ee28384-8710-4902-aabd-38aa18400e7d", "Administrator", "ADMINISTRATOR" },
                    { new Guid("aceb7ed9-4f66-4218-a63b-6593b3fe0b6b"), "fb6c54e9-7e77-47e6-b20e-e84648f510a4", "Correspondent", "CORRESPONDENT" },
                    { new Guid("cf9b7fae-577e-4893-920f-9f008be81479"), "585a3412-a3f3-4a30-a815-7a318cf887f9", "MediaAnalyst", "MEDIAANALYST" },
                    { new Guid("50184fc9-0ede-49fd-80d8-7f77031293e9"), "2a768eae-9e9c-4559-87ca-94d59455f327", "Editor", "EDITOR" },
                    { new Guid("d7461d24-0b4e-43ec-bcdd-f3ef35ce2422"), "7ae935c8-2349-45b4-93bd-e7ce7a3fe7f3", "Corrector", "CORRECTOR" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("50184fc9-0ede-49fd-80d8-7f77031293e9"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d691ee5-2ee9-4534-84c2-8391e9c503f8"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aceb7ed9-4f66-4218-a63b-6593b3fe0b6b"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cf9b7fae-577e-4893-920f-9f008be81479"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d7461d24-0b4e-43ec-bcdd-f3ef35ce2422"));
        }
    }
}

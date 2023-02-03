using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileService.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Length = table.Column<long>(type: "bigint", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastAccessTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastAccessTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastWriteTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastWriteTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Attributes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");
        }
    }
}

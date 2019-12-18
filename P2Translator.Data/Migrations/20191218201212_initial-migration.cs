using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace P2Translator.Data.Migrations
{
    public partial class initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "MessageId",
                startValue: 3L);

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    MessageId = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"MessageId\"')"),
                    Content = table.Column<string>(nullable: true),
                    MessageDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.MessageId);
                });

            migrationBuilder.InsertData(
                table: "Message",
                columns: new[] { "MessageId", "Content", "MessageDateTime" },
                values: new object[,]
                {
                    { 1, "first message", new DateTime(2019, 12, 18, 14, 12, 11, 512, DateTimeKind.Local).AddTicks(750) },
                    { 2, "second message", new DateTime(2019, 12, 18, 14, 12, 11, 542, DateTimeKind.Local).AddTicks(7930) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropSequence(
                name: "MessageId");
        }
    }
}

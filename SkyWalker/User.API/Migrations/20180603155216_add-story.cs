using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace User.API.Migrations
{
    public partial class addstory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(type: "varchar(50)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(50)", nullable: true),
                    UserPassWord = table.Column<string>(type: "varchar(50)", nullable: true),
                    Avatar = table.Column<string>(type: "varchar(200)", nullable: true),
                    Sex = table.Column<int>(nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => new { x.Id, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "book",
                columns: table => new
                {
                    book_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    book_name = table.Column<string>(type: "varchar(100)", nullable: true),
                    preface = table.Column<string>(type: "varchar(500)", nullable: true),
                    status = table.Column<string>(type: "varchar(100)", nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book", x => x.book_id);
                });

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OwnerUserId = table.Column<int>(nullable: false),
                    TargetUserId = table.Column<int>(nullable: false),
                    StoryId = table.Column<int>(nullable: false),
                    content = table.Column<string>(type: "text", nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "story",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    title = table.Column<string>(type: "varchar(200)", nullable: true),
                    content = table.Column<string>(type: "varchar(1000)", nullable: true),
                    conver_photo = table.Column<string>(type: "varchar(200)", nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_story", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropTable(
                name: "book");

            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "story");
        }
    }
}

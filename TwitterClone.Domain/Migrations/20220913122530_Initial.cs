using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TwitterClone.Domain.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "tbl_users",
                schema: "public",
                columns: table => new
                {
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_users", x => x.username);
                });

            migrationBuilder.CreateTable(
                name: "tbl_followings",
                schema: "public",
                columns: table => new
                {
                    follow_by = table.Column<string>(type: "character varying(50)", nullable: false),
                    follow_for = table.Column<string>(type: "character varying(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_followings", x => new { x.follow_by, x.follow_for });
                    table.ForeignKey(
                        name: "FK_tbl_followings_tbl_users_follow_by",
                        column: x => x.follow_by,
                        principalSchema: "public",
                        principalTable: "tbl_users",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_followings_tbl_users_follow_for",
                        column: x => x.follow_for,
                        principalSchema: "public",
                        principalTable: "tbl_users",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_posts",
                schema: "public",
                columns: table => new
                {
                    post_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    author_username = table.Column<string>(type: "character varying(50)", nullable: true),
                    comment_to = table.Column<int>(type: "integer", nullable: true),
                    post_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    message = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_posts", x => x.post_id);
                    table.ForeignKey(
                        name: "FK_tbl_posts_tbl_users_author_username",
                        column: x => x.author_username,
                        principalSchema: "public",
                        principalTable: "tbl_users",
                        principalColumn: "username");
                });

            migrationBuilder.CreateTable(
                name: "tbl_likes",
                schema: "public",
                columns: table => new
                {
                    post_id = table.Column<int>(type: "integer", nullable: false),
                    username = table.Column<string>(type: "character varying(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_likes", x => new { x.post_id, x.username });
                    table.ForeignKey(
                        name: "FK_tbl_likes_tbl_posts_post_id",
                        column: x => x.post_id,
                        principalSchema: "public",
                        principalTable: "tbl_posts",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_likes_tbl_users_username",
                        column: x => x.username,
                        principalSchema: "public",
                        principalTable: "tbl_users",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_followings_follow_for",
                schema: "public",
                table: "tbl_followings",
                column: "follow_for");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_likes_username",
                schema: "public",
                table: "tbl_likes",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_posts_author_username",
                schema: "public",
                table: "tbl_posts",
                column: "author_username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_followings",
                schema: "public");

            migrationBuilder.DropTable(
                name: "tbl_likes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "tbl_posts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "tbl_users",
                schema: "public");
        }
    }
}

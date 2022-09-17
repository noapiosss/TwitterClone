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
                    password = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
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
                    author = table.Column<string>(type: "character varying(50)", nullable: true),
                    comment_to = table.Column<int>(type: "integer", nullable: true),
                    post_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    message = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_posts", x => x.post_id);
                    table.ForeignKey(
                        name: "FK_tbl_posts_tbl_users_author",
                        column: x => x.author,
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
                    liked_by = table.Column<string>(type: "character varying(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_likes", x => new { x.post_id, x.liked_by });
                    table.ForeignKey(
                        name: "FK_tbl_likes_tbl_posts_post_id",
                        column: x => x.post_id,
                        principalSchema: "public",
                        principalTable: "tbl_posts",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_likes_tbl_users_liked_by",
                        column: x => x.liked_by,
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
                name: "IX_tbl_likes_liked_by",
                schema: "public",
                table: "tbl_likes",
                column: "liked_by");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_posts_author",
                schema: "public",
                table: "tbl_posts",
                column: "author");
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

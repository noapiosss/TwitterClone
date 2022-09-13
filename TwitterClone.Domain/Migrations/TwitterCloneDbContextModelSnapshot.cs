﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TwitterClone.Domain.Database;

#nullable disable

namespace TwitterClone.Domain.Migrations
{
    [DbContext(typeof(TwitterCloneDbContext))]
    partial class TwitterCloneDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TwitterClone.Contracts.Database.Following", b =>
                {
                    b.Property<string>("FollowByUsername")
                        .HasColumnType("character varying(50)")
                        .HasColumnName("follow_by");

                    b.Property<string>("FollowForUsername")
                        .HasColumnType("character varying(50)")
                        .HasColumnName("follow_for");

                    b.HasKey("FollowByUsername", "FollowForUsername");

                    b.HasIndex("FollowForUsername");

                    b.ToTable("tbl_followings", "public");
                });

            modelBuilder.Entity("TwitterClone.Contracts.Database.Like", b =>
                {
                    b.Property<int>("PostId")
                        .HasColumnType("integer")
                        .HasColumnName("post_id");

                    b.Property<string>("LikedByUsername")
                        .HasColumnType("character varying(50)")
                        .HasColumnName("liked_by_username");

                    b.HasKey("PostId", "LikedByUsername");

                    b.HasIndex("LikedByUsername");

                    b.ToTable("tbl_likes", "public");
                });

            modelBuilder.Entity("TwitterClone.Contracts.Database.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("post_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PostId"));

                    b.Property<string>("AuthorUsername")
                        .HasColumnType("character varying(50)")
                        .HasColumnName("author_username");

                    b.Property<int?>("CommentTo")
                        .HasColumnType("integer")
                        .HasColumnName("comment_to");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("message");

                    b.Property<DateTime>("PostDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("post_date");

                    b.HasKey("PostId");

                    b.HasIndex("AuthorUsername");

                    b.ToTable("tbl_posts", "public");
                });

            modelBuilder.Entity("TwitterClone.Contracts.Database.User", b =>
                {
                    b.Property<string>("Username")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("username");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password");

                    b.HasKey("Username");

                    b.ToTable("tbl_users", "public");
                });

            modelBuilder.Entity("TwitterClone.Contracts.Database.Following", b =>
                {
                    b.HasOne("TwitterClone.Contracts.Database.User", "FollowBy")
                        .WithMany("Followings")
                        .HasForeignKey("FollowByUsername")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TwitterClone.Contracts.Database.User", "FollowFor")
                        .WithMany("Followers")
                        .HasForeignKey("FollowForUsername")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FollowBy");

                    b.Navigation("FollowFor");
                });

            modelBuilder.Entity("TwitterClone.Contracts.Database.Like", b =>
                {
                    b.HasOne("TwitterClone.Contracts.Database.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("LikedByUsername")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TwitterClone.Contracts.Database.Post", "Post")
                        .WithMany("Likes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TwitterClone.Contracts.Database.Post", b =>
                {
                    b.HasOne("TwitterClone.Contracts.Database.User", "Author")
                        .WithMany("AuthoredPosts")
                        .HasForeignKey("AuthorUsername");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("TwitterClone.Contracts.Database.Post", b =>
                {
                    b.Navigation("Likes");
                });

            modelBuilder.Entity("TwitterClone.Contracts.Database.User", b =>
                {
                    b.Navigation("AuthoredPosts");

                    b.Navigation("Followers");

                    b.Navigation("Followings");

                    b.Navigation("Likes");
                });
#pragma warning restore 612, 618
        }
    }
}

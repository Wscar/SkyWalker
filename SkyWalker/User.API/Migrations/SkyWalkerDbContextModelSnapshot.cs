﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkyWalker.Dal.DBContext;

namespace User.API.Migrations
{
    [DbContext(typeof(SkyWalkerDbContext))]
    partial class SkyWalkerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799");

            modelBuilder.Entity("SkyWalker.Dal.Entities.AppUser", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Avatar")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Phone")
                        .HasColumnType("varchar(20)");

                    b.Property<int>("Sex");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("UserPassWord")
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id", "UserId");

                    b.ToTable("AppUser");
                });

            modelBuilder.Entity("SkyWalker.Dal.Entities.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("book_id");

                    b.Property<string>("BookName")
                        .HasColumnName("book_name")
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Preface")
                        .HasColumnName("preface")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Status")
                        .HasColumnName("status")
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<int>("UserId");

                    b.HasKey("BookId");

                    b.ToTable("book");
                });

            modelBuilder.Entity("SkyWalker.Dal.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .HasColumnName("content")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("OwnerUserId");

                    b.Property<int>("StoryId");

                    b.Property<int>("TargetUserId");

                    b.HasKey("Id");

                    b.ToTable("comment");
                });

            modelBuilder.Entity("SkyWalker.Dal.Entities.Story", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BookId");

                    b.Property<string>("Content")
                        .HasColumnName("content")
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("CoverPhoto")
                        .HasColumnName("conver_photo")
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Title")
                        .HasColumnName("title")
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("story");
                });
#pragma warning restore 612, 618
        }
    }
}

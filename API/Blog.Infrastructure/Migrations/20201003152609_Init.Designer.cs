﻿// <auto-generated />
using System;
using Blog.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Blog.Infrastructure.Migrations
{
    [DbContext(typeof(BlogDbContext))]
    [Migration("20201003152609_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Blog.Domain.Models.Aggregates.Post.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("Blog.Domain.Models.Aggregates.Post.Post", b =>
                {
                    b.OwnsOne("Blog.Domain.Models.Aggregates.Post.Content", "Content", b1 =>
                        {
                            b1.Property<Guid>("PostId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.HasKey("PostId");

                            b1.ToTable("Post");

                            b1.WithOwner()
                                .HasForeignKey("PostId");
                        });

                    b.OwnsOne("Blog.Domain.Models.Aggregates.Post.Title", "Title", b1 =>
                        {
                            b1.Property<Guid>("PostId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.HasKey("PostId");

                            b1.ToTable("Post");

                            b1.WithOwner()
                                .HasForeignKey("PostId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}

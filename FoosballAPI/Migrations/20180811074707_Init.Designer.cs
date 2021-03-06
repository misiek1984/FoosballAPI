﻿// <auto-generated />
using FoosballAPI.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace FoosballAPI.Migrations
{
    [DbContext(typeof(FoosballDbContext))]
    [Migration("20180811074707_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FoosballAPI.Database.Entities.GameEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid?>("FirstTeamId");

                    b.Property<Guid?>("SecondTeamId");

                    b.HasKey("Id");

                    b.HasIndex("FirstTeamId");

                    b.HasIndex("SecondTeamId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("FoosballAPI.Database.Entities.SetEntity", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<Guid>("ParentId");

                    b.Property<int>("FirstTeamResult");

                    b.Property<int>("SecondTeamResult");

                    b.HasKey("Id", "ParentId");

                    b.HasIndex("ParentId");

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("FoosballAPI.Database.Entities.TeamEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("FoosballAPI.Database.Entities.GameEntity", b =>
                {
                    b.HasOne("FoosballAPI.Database.Entities.TeamEntity", "FirstTeam")
                        .WithMany()
                        .HasForeignKey("FirstTeamId");

                    b.HasOne("FoosballAPI.Database.Entities.TeamEntity", "SecondTeam")
                        .WithMany()
                        .HasForeignKey("SecondTeamId");
                });

            modelBuilder.Entity("FoosballAPI.Database.Entities.SetEntity", b =>
                {
                    b.HasOne("FoosballAPI.Database.Entities.GameEntity", "Parent")
                        .WithMany("Sets")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

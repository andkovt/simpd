﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpD;

#nullable disable

namespace SimpD.Migrations
{
    [DbContext(typeof(MainContext))]
    partial class MainContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("SimpD.Entity.Container", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Containers");
                });

            modelBuilder.Entity("SimpD.Entity.EnvironmentVariable", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ContainerId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ContainerId");

                    b.ToTable("EnvironmentVariables");
                });

            modelBuilder.Entity("SimpD.Entity.Mount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ContainerId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Mode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ContainerId");

                    b.ToTable("Mounts");
                });

            modelBuilder.Entity("SimpD.Entity.Port", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<ushort>("Container")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("ContainerId")
                        .HasColumnType("TEXT");

                    b.Property<ushort>("Host")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ContainerId");

                    b.ToTable("Ports");
                });

            modelBuilder.Entity("SimpD.Entity.EnvironmentVariable", b =>
                {
                    b.HasOne("SimpD.Entity.Container", null)
                        .WithMany("EnvironmentVariables")
                        .HasForeignKey("ContainerId");
                });

            modelBuilder.Entity("SimpD.Entity.Mount", b =>
                {
                    b.HasOne("SimpD.Entity.Container", null)
                        .WithMany("Mounts")
                        .HasForeignKey("ContainerId");
                });

            modelBuilder.Entity("SimpD.Entity.Port", b =>
                {
                    b.HasOne("SimpD.Entity.Container", null)
                        .WithMany("Ports")
                        .HasForeignKey("ContainerId");
                });

            modelBuilder.Entity("SimpD.Entity.Container", b =>
                {
                    b.Navigation("EnvironmentVariables");

                    b.Navigation("Mounts");

                    b.Navigation("Ports");
                });
#pragma warning restore 612, 618
        }
    }
}

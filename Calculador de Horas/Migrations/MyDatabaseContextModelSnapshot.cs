﻿// <auto-generated />
using System;
using Calculador_de_Horas.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Calculador_de_Horas.Migrations
{
    [DbContext(typeof(MyDatabaseContext))]
    partial class MyDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Calculador_de_Horas.Entities.BancoDeHoras", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DataRegistro")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("FuncionarioId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("HorasExtras")
                        .HasColumnType("time(6)");

                    b.Property<string>("Justificativa")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("FuncionarioId");

                    b.ToTable("BancoDeHoras");
                });

            modelBuilder.Entity("Calculador_de_Horas.Entities.Empresa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CNPJ")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("DiaFechamento")
                        .HasColumnType("int");

                    b.Property<string>("RazaoSocial")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Empresa");
                });

            modelBuilder.Entity("Calculador_de_Horas.Entities.Funcionario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Funcao")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<TimeSpan>("HoraAlmocoRetorno")
                        .HasColumnType("time(6)");

                    b.Property<TimeSpan>("HoraAlmocoSaida")
                        .HasColumnType("time(6)");

                    b.Property<TimeSpan>("HoraIncio")
                        .HasColumnType("time(6)");

                    b.Property<TimeSpan>("HoraTermino")
                        .HasColumnType("time(6)");

                    b.Property<string>("Nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Registro")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Funcionario");
                });

            modelBuilder.Entity("Calculador_de_Horas.Entities.HorasFuncionario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DataRegistro")
                        .HasColumnType("datetime(6)");

                    b.Property<TimeSpan>("Entrada")
                        .HasColumnType("time(6)");

                    b.Property<TimeSpan>("Extras")
                        .HasColumnType("time(6)");

                    b.Property<int>("FuncionarioId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Saida")
                        .HasColumnType("time(6)");

                    b.HasKey("Id");

                    b.HasIndex("FuncionarioId");

                    b.ToTable("HorasFuncionarios");
                });

            modelBuilder.Entity("Calculador_de_Horas.Entities.BancoDeHoras", b =>
                {
                    b.HasOne("Calculador_de_Horas.Entities.Funcionario", null)
                        .WithMany("BancoDeHoras")
                        .HasForeignKey("FuncionarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Calculador_de_Horas.Entities.HorasFuncionario", b =>
                {
                    b.HasOne("Calculador_de_Horas.Entities.Funcionario", null)
                        .WithMany("CartaoPonto")
                        .HasForeignKey("FuncionarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

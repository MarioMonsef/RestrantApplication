﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RestrantApplication.EF.Entities;

#nullable disable

namespace RestrantApplication.EF.Entities.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20250407160954_AddProductAndCategoryAndPhotoTables")]
    partial class AddProductAndCategoryAndPhotoTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RestrantApplication.Core.Models.Product.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("RestrantApplication.Core.Models.Product.Photo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("PhotoName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("RestrantApplication.Core.Models.Product.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAvilable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PhotoID")
                        .HasColumnType("int");

                    b.Property<decimal>("Prise")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Stock")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("categoryID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("PhotoID")
                        .IsUnique();

                    b.HasIndex("categoryID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("RestrantApplication.Core.Models.Product.Product", b =>
                {
                    b.HasOne("RestrantApplication.Core.Models.Product.Photo", "Photo")
                        .WithOne("Product")
                        .HasForeignKey("RestrantApplication.Core.Models.Product.Product", "PhotoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RestrantApplication.Core.Models.Product.Category", "category")
                        .WithMany("Products")
                        .HasForeignKey("categoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Photo");

                    b.Navigation("category");
                });

            modelBuilder.Entity("RestrantApplication.Core.Models.Product.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("RestrantApplication.Core.Models.Product.Photo", b =>
                {
                    b.Navigation("Product")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

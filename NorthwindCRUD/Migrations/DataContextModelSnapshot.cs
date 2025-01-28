﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NorthwindCRUD;

#nullable disable

namespace NorthwindCRUD.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.22");

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.CategoryDb", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Picture")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.CustomerDb", b =>
                {
                    b.Property<string>("CustomerId")
                        .HasColumnType("TEXT");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.EmployeeDb", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("AvatarUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("BirthDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("HireDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ReportsTo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TitleOfCourtesy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("EmployeeId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.EmployeeTerritoryDb", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TerritoryId")
                        .HasColumnType("TEXT");

                    b.HasKey("EmployeeId", "TerritoryId");

                    b.HasIndex("TerritoryId");

                    b.ToTable("EmployeesTerritories");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.OrderDb", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CustomerId")
                        .HasColumnType("TEXT");

                    b.Property<float>("Discount")
                        .HasColumnType("REAL");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Freight")
                        .HasColumnType("REAL");

                    b.Property<string>("OrderDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RequiredDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ShipName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ShipVia")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ShipperId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("REAL");

                    b.HasKey("OrderId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ShipperId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.OrderDetailDb", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderId")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Discount")
                        .HasColumnType("REAL");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("REAL");

                    b.HasKey("ProductId", "OrderId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.ProductDb", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Discontinued")
                        .HasColumnType("INTEGER");

                    b.Property<string>("QuantityPerUnit")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ReorderLevel")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SupplierId")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("UnitPrice")
                        .HasColumnType("REAL");

                    b.Property<int?>("UnitsInStock")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("UnitsOnOrder")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("SupplierId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.RegionDb", b =>
                {
                    b.Property<int>("RegionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RegionDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("RegionId");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.ShipperDb", b =>
                {
                    b.Property<int>("ShipperId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ShipperId");

                    b.ToTable("Shippers");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.SupplierDb", b =>
                {
                    b.Property<int>("SupplierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<string>("CompanyName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactTitle")
                        .HasColumnType("TEXT");

                    b.Property<string>("Country")
                        .HasColumnType("TEXT");

                    b.Property<string>("Fax")
                        .HasColumnType("TEXT");

                    b.Property<string>("HomePage")
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<string>("PostalCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("Region")
                        .HasColumnType("TEXT");

                    b.HasKey("SupplierId");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.TerritoryDb", b =>
                {
                    b.Property<string>("TerritoryId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("RegionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TerritoryDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TerritoryId");

                    b.HasIndex("RegionId");

                    b.ToTable("Territories");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.UserDb", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.CustomerDb", b =>
                {
                    b.OwnsOne("NorthwindCRUD.Models.DbModels.AddressDb", "Address", b1 =>
                        {
                            b1.Property<string>("CustomerDbCustomerId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Phone")
                                .HasColumnType("TEXT");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Region")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.HasKey("CustomerDbCustomerId");

                            b1.ToTable("Customers");

                            b1.WithOwner()
                                .HasForeignKey("CustomerDbCustomerId");
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.EmployeeDb", b =>
                {
                    b.OwnsOne("NorthwindCRUD.Models.DbModels.AddressDb", "Address", b1 =>
                        {
                            b1.Property<int>("EmployeeDbEmployeeId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Phone")
                                .HasColumnType("TEXT");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Region")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.HasKey("EmployeeDbEmployeeId");

                            b1.ToTable("Employees");

                            b1.WithOwner()
                                .HasForeignKey("EmployeeDbEmployeeId");
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.EmployeeTerritoryDb", b =>
                {
                    b.HasOne("NorthwindCRUD.Models.DbModels.EmployeeDb", "Employee")
                        .WithMany("EmployeesTerritories")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NorthwindCRUD.Models.DbModels.TerritoryDb", "Territory")
                        .WithMany("EmployeesTerritories")
                        .HasForeignKey("TerritoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Territory");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.OrderDb", b =>
                {
                    b.HasOne("NorthwindCRUD.Models.DbModels.CustomerDb", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("NorthwindCRUD.Models.DbModels.EmployeeDb", "Employee")
                        .WithMany("Orders")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("NorthwindCRUD.Models.DbModels.ShipperDb", "Shipper")
                        .WithMany("Orders")
                        .HasForeignKey("ShipperId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.OwnsOne("NorthwindCRUD.Models.DbModels.AddressDb", "ShipAddress", b1 =>
                        {
                            b1.Property<int>("OrderDbOrderId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Phone")
                                .HasColumnType("TEXT");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Region")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.HasKey("OrderDbOrderId");

                            b1.ToTable("Orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderDbOrderId");
                        });

                    b.Navigation("Customer");

                    b.Navigation("Employee");

                    b.Navigation("ShipAddress");

                    b.Navigation("Shipper");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.OrderDetailDb", b =>
                {
                    b.HasOne("NorthwindCRUD.Models.DbModels.OrderDb", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NorthwindCRUD.Models.DbModels.ProductDb", "Product")
                        .WithMany("Details")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.ProductDb", b =>
                {
                    b.HasOne("NorthwindCRUD.Models.DbModels.CategoryDb", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("NorthwindCRUD.Models.DbModels.SupplierDb", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Category");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.TerritoryDb", b =>
                {
                    b.HasOne("NorthwindCRUD.Models.DbModels.RegionDb", "Region")
                        .WithMany("Territories")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Region");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.CategoryDb", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.CustomerDb", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.EmployeeDb", b =>
                {
                    b.Navigation("EmployeesTerritories");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.ProductDb", b =>
                {
                    b.Navigation("Details");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.RegionDb", b =>
                {
                    b.Navigation("Territories");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.ShipperDb", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.SupplierDb", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("NorthwindCRUD.Models.DbModels.TerritoryDb", b =>
                {
                    b.Navigation("EmployeesTerritories");
                });
#pragma warning restore 612, 618
        }
    }
}

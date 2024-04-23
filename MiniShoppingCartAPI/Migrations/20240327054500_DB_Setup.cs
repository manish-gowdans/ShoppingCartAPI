using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniShoppingCartAPI.Migrations
{
    /// <inheritdoc />
    public partial class DB_Setup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Pid",
                keyValue: new Guid("18893b7e-d11a-493b-8906-19028d77942f"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Pid",
                keyValue: new Guid("6743816e-9122-4100-a1f3-4eacb6cc377c"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Pid",
                keyValue: new Guid("8a273cba-a4c7-419e-9fac-f9bcc38ca760"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Pid",
                keyValue: new Guid("d0433985-2fcd-4f19-a832-ea72e63f970b"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Pid",
                keyValue: new Guid("f44e7140-4683-4ddb-b3a4-3a6077ff9d56"));

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "TEXT",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "Users",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<Guid>(
                name: "Uid",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Productname",
                table: "Products",
                type: "TEXT",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "Pid",
                table: "Products",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Carts",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "SetQuantity",
                table: "Carts",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "Carts",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CartId",
                table: "Carts",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Pid", "Price", "Productname", "Quantity" },
                values: new object[,]
                {
                    { new Guid("235a07e3-5c0c-48ca-a9e1-44a27ef0be6d"), 50, "Orange", 20 },
                    { new Guid("3ac87c2d-6d88-4778-8f88-1aea9e3da21f"), 30, "Strawberry", 100 },
                    { new Guid("49358d8c-437d-4cbb-955e-b9df3e115400"), 100, "Apple", 10 },
                    { new Guid("9ec0e6e7-31d3-45d3-abaa-ed4d167314a1"), 10, "Banana", 50 },
                    { new Guid("e4935e46-2cc1-4c12-8e76-e9ca64162a9d"), 200, "Watermelon", 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Pid",
                keyValue: new Guid("235a07e3-5c0c-48ca-a9e1-44a27ef0be6d"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Pid",
                keyValue: new Guid("3ac87c2d-6d88-4778-8f88-1aea9e3da21f"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Pid",
                keyValue: new Guid("49358d8c-437d-4cbb-955e-b9df3e115400"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Pid",
                keyValue: new Guid("9ec0e6e7-31d3-45d3-abaa-ed4d167314a1"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Pid",
                keyValue: new Guid("e4935e46-2cc1-4c12-8e76-e9ca64162a9d"));

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "Users",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<Guid>(
                name: "Uid",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Productname",
                table: "Products",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "Pid",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Carts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "SetQuantity",
                table: "Carts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "Carts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "CartId",
                table: "Carts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Pid", "Price", "Productname", "Quantity" },
                values: new object[,]
                {
                    { new Guid("18893b7e-d11a-493b-8906-19028d77942f"), 30, "Strawberry", 100 },
                    { new Guid("6743816e-9122-4100-a1f3-4eacb6cc377c"), 50, "Orange", 20 },
                    { new Guid("8a273cba-a4c7-419e-9fac-f9bcc38ca760"), 10, "Banana", 50 },
                    { new Guid("d0433985-2fcd-4f19-a832-ea72e63f970b"), 100, "Apple", 10 },
                    { new Guid("f44e7140-4683-4ddb-b3a4-3a6077ff9d56"), 200, "Watermelon", 5 }
                });
        }
    }
}

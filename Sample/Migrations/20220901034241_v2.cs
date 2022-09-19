using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sma_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sma_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sma_companies",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    logo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sma_companies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sma_departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sma_departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "sma_users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    activation_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone = table.Column<int>(type: "int", nullable: false),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    active = table.Column<int>(type: "int", nullable: true),
                    logo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sma_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sma_expenses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    amount = table.Column<double>(type: "float", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    category_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sma_expenses", x => x.id);
                    table.ForeignKey(
                        name: "FK_sma_expenses_sma_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "sma_categories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "sma_products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    barcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cost = table.Column<double>(type: "float", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sma_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_sma_products_sma_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "sma_categories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "sma_students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sma_students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sma_students_sma_departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "sma_departments",
                        principalColumn: "DepartmentId");
                });

            migrationBuilder.CreateTable(
                name: "sma_sales",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    reference_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    total = table.Column<double>(type: "float", nullable: false),
                    discount = table.Column<double>(type: "float", nullable: false),
                    shipping = table.Column<double>(type: "float", nullable: false),
                    grand_total = table.Column<double>(type: "float", nullable: false),
                    sale_status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    payment_status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    payment_method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    customer_id = table.Column<int>(type: "int", nullable: true),
                    biller_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sma_sales", x => x.id);
                    table.ForeignKey(
                        name: "FK_sma_sales_sma_companies_customer_id",
                        column: x => x.customer_id,
                        principalTable: "sma_companies",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_sma_sales_sma_users_biller_id",
                        column: x => x.biller_id,
                        principalTable: "sma_users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_sma_expenses_category_id",
                table: "sma_expenses",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_sma_products_category_id",
                table: "sma_products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_sma_sales_biller_id",
                table: "sma_sales",
                column: "biller_id");

            migrationBuilder.CreateIndex(
                name: "IX_sma_sales_customer_id",
                table: "sma_sales",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_sma_students_DepartmentId",
                table: "sma_students",
                column: "DepartmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sma_expenses");

            migrationBuilder.DropTable(
                name: "sma_products");

            migrationBuilder.DropTable(
                name: "sma_sales");

            migrationBuilder.DropTable(
                name: "sma_students");

            migrationBuilder.DropTable(
                name: "sma_categories");

            migrationBuilder.DropTable(
                name: "sma_companies");

            migrationBuilder.DropTable(
                name: "sma_users");

            migrationBuilder.DropTable(
                name: "sma_departments");
        }
    }
}

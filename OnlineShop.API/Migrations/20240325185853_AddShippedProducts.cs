using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.API.Migrations
{
    /// <inheritdoc />
    public partial class AddShippedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShippedProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SellerID = table.Column<int>(type: "int", nullable: false),
                    SellerKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AverageRating = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippedProducts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShippedProducts");
        }
    }
}

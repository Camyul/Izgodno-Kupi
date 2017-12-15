using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace IzgodnoKupi.Data.Migrations
{
    public partial class FixFKOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FullContactInfo_AspNetUsers_UserID",
                table: "FullContactInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Orders_OrderId",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Products_ProductId",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_FullContactInfo_FullContactInfoId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShortContactInfo_ShortContactInfoId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShortContactInfo",
                table: "ShortContactInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FullContactInfo",
                table: "FullContactInfo");

            migrationBuilder.RenameTable(
                name: "ShortContactInfo",
                newName: "ShortContactInfos");

            migrationBuilder.RenameTable(
                name: "OrderItem",
                newName: "OrderItems");

            migrationBuilder.RenameTable(
                name: "FullContactInfo",
                newName: "FullContactInfos");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_ProductId",
                table: "OrderItems",
                newName: "IX_OrderItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_OrderId",
                table: "OrderItems",
                newName: "IX_OrderItems_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_FullContactInfo_UserID",
                table: "FullContactInfos",
                newName: "IX_FullContactInfos_UserID");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShortContactInfoId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "FullContactInfoId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShortContactInfos",
                table: "ShortContactInfos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FullContactInfos",
                table: "FullContactInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FullContactInfos_AspNetUsers_UserID",
                table: "FullContactInfos",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_FullContactInfos_FullContactInfoId",
                table: "Orders",
                column: "FullContactInfoId",
                principalTable: "FullContactInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShortContactInfos_ShortContactInfoId",
                table: "Orders",
                column: "ShortContactInfoId",
                principalTable: "ShortContactInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FullContactInfos_AspNetUsers_UserID",
                table: "FullContactInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_FullContactInfos_FullContactInfoId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShortContactInfos_ShortContactInfoId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShortContactInfos",
                table: "ShortContactInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FullContactInfos",
                table: "FullContactInfos");

            migrationBuilder.RenameTable(
                name: "ShortContactInfos",
                newName: "ShortContactInfo");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                newName: "OrderItem");

            migrationBuilder.RenameTable(
                name: "FullContactInfos",
                newName: "FullContactInfo");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItem",
                newName: "IX_OrderItem_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItem",
                newName: "IX_OrderItem_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_FullContactInfos_UserID",
                table: "FullContactInfo",
                newName: "IX_FullContactInfo_UserID");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShortContactInfoId",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "FullContactInfoId",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShortContactInfo",
                table: "ShortContactInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FullContactInfo",
                table: "FullContactInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FullContactInfo_AspNetUsers_UserID",
                table: "FullContactInfo",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Orders_OrderId",
                table: "OrderItem",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Products_ProductId",
                table: "OrderItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_FullContactInfo_FullContactInfoId",
                table: "Orders",
                column: "FullContactInfoId",
                principalTable: "FullContactInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShortContactInfo_ShortContactInfoId",
                table: "Orders",
                column: "ShortContactInfoId",
                principalTable: "ShortContactInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

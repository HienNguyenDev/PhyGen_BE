using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhyGen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamSets_Users_CreatedBy",
                schema: "public",
                table: "ExamSets");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamSets_Users_ReviewedBy",
                schema: "public",
                table: "ExamSets");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamSets_Users_UserId",
                schema: "public",
                table: "ExamSets");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamSets_Users_UserId1",
                schema: "public",
                table: "ExamSets");

            migrationBuilder.DropIndex(
                name: "IX_ExamSets_UserId",
                schema: "public",
                table: "ExamSets");

            migrationBuilder.DropIndex(
                name: "IX_ExamSets_UserId1",
                schema: "public",
                table: "ExamSets");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "public",
                table: "ExamSets");

            migrationBuilder.DropColumn(
                name: "UserId1",
                schema: "public",
                table: "ExamSets");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "public",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<bool>(
                name: "IsProvider",
                schema: "public",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSets_Users_CreatedBy",
                schema: "public",
                table: "ExamSets",
                column: "CreatedBy",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSets_Users_ReviewedBy",
                schema: "public",
                table: "ExamSets",
                column: "ReviewedBy",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamSets_Users_CreatedBy",
                schema: "public",
                table: "ExamSets");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamSets_Users_ReviewedBy",
                schema: "public",
                table: "ExamSets");

            migrationBuilder.DropColumn(
                name: "IsProvider",
                schema: "public",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "public",
                table: "Users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "public",
                table: "ExamSets",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                schema: "public",
                table: "ExamSets",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamSets_UserId",
                schema: "public",
                table: "ExamSets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSets_UserId1",
                schema: "public",
                table: "ExamSets",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSets_Users_CreatedBy",
                schema: "public",
                table: "ExamSets",
                column: "CreatedBy",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSets_Users_ReviewedBy",
                schema: "public",
                table: "ExamSets",
                column: "ReviewedBy",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSets_Users_UserId",
                schema: "public",
                table: "ExamSets",
                column: "UserId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSets_Users_UserId1",
                schema: "public",
                table: "ExamSets",
                column: "UserId1",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}

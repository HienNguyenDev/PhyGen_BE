using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhyGen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixExamSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamSets_Users_ReviewedBy",
                schema: "public",
                table: "ExamSets");

            migrationBuilder.DropIndex(
                name: "IX_ExamSets_ReviewedBy",
                schema: "public",
                table: "ExamSets");

            migrationBuilder.DropColumn(
                name: "ReviewNote",
                schema: "public",
                table: "ExamSets");

            migrationBuilder.DropColumn(
                name: "ReviewedAt",
                schema: "public",
                table: "ExamSets");

            migrationBuilder.DropColumn(
                name: "ReviewedBy",
                schema: "public",
                table: "ExamSets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReviewNote",
                schema: "public",
                table: "ExamSets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewedAt",
                schema: "public",
                table: "ExamSets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReviewedBy",
                schema: "public",
                table: "ExamSets",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamSets_ReviewedBy",
                schema: "public",
                table: "ExamSets",
                column: "ReviewedBy");

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
    }
}

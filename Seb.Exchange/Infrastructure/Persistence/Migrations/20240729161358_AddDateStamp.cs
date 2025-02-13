﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Seb.Server.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDateStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateStamp",
                table: "Currencies",
                type: "datetime2",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateStamp",
                table: "Currencies");
        }
    }
}

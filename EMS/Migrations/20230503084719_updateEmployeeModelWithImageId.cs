using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.Migrations
{
    /// <inheritdoc />
    public partial class updateEmployeeModelWithImageId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE dbo.AspNetUsers ADD ImageId UNIQUEIDENTIFIER NULL;");
            migrationBuilder.Sql("ALTER TABLE dbo.AspNetUsers ADD CONSTRAINT FK_AspNetUsers_EmployeeImages_ImageId FOREIGN KEY (ImageId) REFERENCES dbo.EmployeeImages(Id);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE dbo.AspNetUsers ADD ImageId UNIQUEIDENTIFIER NULL;");
            migrationBuilder.Sql("ALTER TABLE dbo.AspNetUsers ADD CONSTRAINT FK_AspNetUsers_EmployeeImages_ImageId FOREIGN KEY (ImageId) REFERENCES dbo.EmployeeImages(Id);");
        }
    }
}

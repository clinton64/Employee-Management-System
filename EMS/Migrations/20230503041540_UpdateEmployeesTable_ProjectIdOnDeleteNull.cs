using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeesTable_ProjectIdOnDeleteNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE dbo.AspNetUsers DROP CONSTRAINT FK_AspNetUsers_Projects_ProjectId");
            migrationBuilder.Sql("ALTER TABLE dbo.AspNetUsers ADD CONSTRAINT FK_AspNetUsers_Projects_ProjectId FOREIGN KEY (ProjectId) REFERENCES dbo.Projects (ProjectId) ON DELETE SET NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE dbo.AspNetUsers DROP CONSTRAINT FK_AspNetUsers_Projects_ProjectId");
            migrationBuilder.Sql("ALTER TABLE dbo.AspNetUsers ADD CONSTRAINT FK_AspNetUsers_Projects_ProjectId FOREIGN KEY (ProjectId) REFERENCES dbo.Projects (ProjectId) ON DELETE SET NULL");
        }
    }
}
